using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;

namespace MusicPlayerWindow
{
    /// <summary>
    ///  Parses iTunes's xml library and creates local playlists
    /// </summary>
    public class PlaylistParser
    {
        #region Properties
        private XmlTextReader reader;
        private String songsFile = "iTunes Songs.xml";
        private String outputDir;
        private System.Xml.XPath.XPathDocument playlistDoc;
        private System.Xml.XPath.XPathNavigator playlistNav;
        private HashSet<String> excludedPlaylists = new HashSet<string>(new String[] { "Books", "iTunes DJ", "iTunes U", "Library", "Movies", "Music Videos", "Purchased", "Recently Added", "Recently Played", "Top 25 Most Played", });
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes PlaylistParser, including a reader for creating the song index and the location of the music library
        /// </summary>
        /// <param name="path">The path to "iTunes Music Library.xml"</param>
        /// <param name="dir">The directory where the playlists should be dumped</param>
        public PlaylistParser(String path, String dir)
        {
            reader = new XmlTextReader(path);
            outputDir = dir;
        }

        #endregion

        #region Interface
        /// <summary>
        /// Creates a song index, builds the permanent playlist "cache", and deletes the song index
        /// </summary>
        public void createPlaylists()
        {
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        if (reader.Value == "Tracks") {
                            getSongs();
                            getPlaylists();
                        }
                        break;
                    default:
                        break;
                }
            }
            reader.Close();
            File.Delete(songsFile);
        }
        #endregion

        #region Song Helpers
        /// <summary>
        /// Builds the song index, which is used for creating the playlists
        /// </summary>
        private void getSongs()
        {
            Boolean nextTextIsID = false;
            Boolean nextTextIsLoc = false;
            String currentID = "";
            String currentLoc = "";

            TextWriter w = new StreamWriter(songsFile);
            writeDTD(w);

            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.Text) {
                    if (nextTextIsID) {
                        nextTextIsID = false;
                        currentID = reader.Value;
                    } else if (nextTextIsLoc) {
                        nextTextIsLoc = false;
                        currentLoc = reader.Value;
                        writeXML(w, currentID, currentLoc);
                    } else if (reader.Value == "Track ID")
                        nextTextIsID = true;
                    else if (reader.Value == "Location")
                        nextTextIsLoc = true;
                    else if (reader.Value == "Playlists")
                        break;
                }
            }
            w.WriteLine("</tracklist>");
            w.Close();
        }

        /// <summary>
        /// Wrapper for writing XML for the song index; a song consists of an id and its location
        /// </summary>
        /// <param name="writer">File handle for writing to the song index</param>
        /// <param name="id">The (iTunes-assigned) id of the song</param>
        /// <param name="loc">The location of a song in the filesystem</param>
        private void writeXML(TextWriter writer, String id, String loc)
        {
            loc = loc.Replace("&", "&amp;");
            loc = loc.Replace(@"file://localhost/", "");
            loc = loc.Replace("%20", " ");
            writer.WriteLine("  <song id=\'{0:000000}\'>{1}</song>", id, loc);
        }

        private void writeDTD(TextWriter w)
        {
            w.WriteLine("<?xml version='1.0' ?>");
            w.WriteLine("<!DOCTYPE tracklist [");
            w.WriteLine("  <!ELEMENT tracklist (song)>");
            w.WriteLine("  <!ELEMENT song     (#PCDATA)>");
            w.WriteLine("  <!ATTLIST song id ID #REQUIRED>");
            w.WriteLine("]>");
            w.WriteLine("<tracklist>");
        }

        #endregion

        #region Playlist Helpers

        /// <summary>
        /// Creates all of the playlists.  Delegates creation of individual playlists to getOnePlaylist <see cref="getOnePlaylist"/>
        /// </summary>
        private void getPlaylists()
        {
            Boolean nextTextIsPlaylistName = false;
            String currentPlaylist = "";
            String songsPath = Directory.GetCurrentDirectory() + @"\" + songsFile;
            playlistDoc = new System.Xml.XPath.XPathDocument(songsPath);
            playlistNav = playlistDoc.CreateNavigator();
            reader.ReadToFollowing("array");
            
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        if (reader.Value == "Name")
                            nextTextIsPlaylistName = true;
                        else if (nextTextIsPlaylistName) {
                            currentPlaylist = reader.Value;
                            nextTextIsPlaylistName = false;
                        }
                        break;
                    case XmlNodeType.Element:
                        if (reader.Name == "array")
                            getOnePlaylist(currentPlaylist);
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// Creates an individual playlist, specified by the parameter "playlistName"
        /// </summary>
        /// <param name="playlistName">The name of the playlist and what the file where the playlist is stored will be called</param>
        private void getOnePlaylist(String playlistName)
        {
            if (excludedPlaylists.Contains(playlistName))
                return;

            int count = 0;
            Boolean nextElementIsID = false;
            String loc = "";
            playlistName = playlistName.Replace("/", " and ");
            playlistName = playlistName.Replace(@"\u201b", "'");
            String playlist = outputDir + "/" + playlistName + ".xml";
            
            TextWriter w = new StreamWriter(playlist);
            writePlaylistDTD(w);
            Boolean finishedGettingPlaylist = false;
            
            while (reader.Read() && !finishedGettingPlaylist)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                        if (reader.Value == "Track ID")
                            nextElementIsID = true;
                        else if (nextElementIsID) {
                            count++;
                            nextElementIsID = false;
                            loc = getLocationFromID(reader.Value);
                            writePlaylistXML(w, count, loc);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "array")
                            finishedGettingPlaylist = true;
                        break;
                    default: break;
                }
            }
            w.WriteLine("</playlist>");
            w.Close();
        }

        /// <summary>
        /// Returns the location of a song given the iTunes assigned id for that song
        /// </summary>
        /// <param name="id">The iTunes assigned id for a song</param>
        private String getLocationFromID(String id)
        {
            playlistNav.MoveToId(id);
            return playlistNav.Value;
        }

        /// <summary>
        /// Wrapper for writing XML for an individual playlist
        /// </summary>
        /// <param name="playlistWriter">File handle for a given playlist</param>
        /// <param name="id">The playlist-specific id of a song (usually its numeric position within the playlist)</param>
        /// <param name="loc">The location of the song associated by the id</param>
        private void writePlaylistXML(TextWriter playlistWriter, int id, String loc)
        {
            loc = loc.Replace("&", "&amp;");
            loc = loc.Replace("%C3%B6", @"\u00F6"); //umlaut o
            loc = loc.Replace("%C3%BC", @"\u00FC"); //umlaut u
            loc = loc.Replace("%5B", "[").Replace("%5D", "]");
            loc = loc.Replace("%23", "#");
            loc = loc.Replace("%7B", "{");
            loc = loc.Replace("%60", "`");
            loc = loc.Replace("%C3%A9", "\u00EA"); //e with accent*/
            //loc = Uri.UnescapeDataString(loc);
            
            playlistWriter.WriteLine("  <song id=\'{0:000000}\'>{1}</song>", id, loc);
        }

        private void writePlaylistDTD(TextWriter w)
        {
            w.WriteLine("<?xml version='1.0' ?>");
            w.WriteLine("<!DOCTYPE playlist [");
            w.WriteLine("  <!ELEMENT playlist (song)>");
            w.WriteLine("  <!ELEMENT song     (#PCDATA)>");
            w.WriteLine("  <!ATTLIST song id ID #REQUIRED>");
            w.WriteLine("]>");
            w.WriteLine("<playlist>");
        }
        #endregion
    }
}
