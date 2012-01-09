using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.ComponentModel;

namespace MusicPlayerWindow
{
    /// <summary>
    ///  Parses iTunes's xml library and creates local playlists
    /// </summary>
    public class PlaylistParser
    {
        #region Properties
        
        private XmlTextReader reader; //for reading the iTunes XML Library
        private String songsFile = "iTunes Songs.xml"; //temporary song index used when creating playlists
        private String outputDir; //where the playlists are stored
        private HashSet<String> excludedPlaylists = new HashSet<string>(new String[] { "Books", "iTunes DJ", "iTunes U", "Library", "Movies", "Music Videos", "Purchased", "Recently Added", "Recently Played", "Top 25 Most Played", });
        
        private System.Xml.XPath.XPathDocument playlistDoc;  //for reading "iTunes Song.xml" once it has been created
        private System.Xml.XPath.XPathNavigator playlistNav; //for navigating "iTunes Song.xml"
        
        private Progress_Bar bar;           //display a progress bar when parsing
        private BackgroundWorker bgWorker;  //do the parsing in a background worker thread
        private String status = "";         //the status of the parsing; displayed in the Progress Bar form
        
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
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(_createPlaylists);
            bar = new Progress_Bar();
        }

        #endregion

        #region Interface

        /// <summary>
        /// Creates a song index, builds the permanent playlist "cache", and deletes the song index
        /// </summary>
        public void createPlaylists()
        {
            //show the bard and run the parsing in the background
            bar.Show();
            bgWorker.RunWorkerAsync();

            //update the progress bar and the status label while the parsing continues in the background
            int count = 0;
            while (reader.ReadState != ReadState.Closed)
            {
                count = (count + 4) % 999;
                bar.setValue(count);
                if (status != "") {
                    bar.setLabel(status);
                    status = "";
                }
                bar.Update();
                System.Threading.Thread.Sleep(10);
            }

            //parsing finished, so close the progress bar
            bar.Close();
        }

        #endregion

        #region Background Interface
        /// <summary>
        /// Creates a song index, and then grabs the iTunes playlists and matches iTunes ids to the corresponding songs
        /// </summary>
        /// <param name="sender">background thread argument</param>
        /// <param name="e">background thread argument</param>
        private void _createPlaylists(object sender, DoWorkEventArgs e)
        {
            //read the rest of the iTunes Library file
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        if (reader.Value == "Tracks") {
                            //make the song index
                            status = "Creating song index...";
                            getSongs();
                            //make the playlists
                            status = "Creating playlists...";
                            getPlaylists();
                        }
                        break;
                    default:
                        break;
                }
            }
            status = "Finishing up parsing...";
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
            //flags for xml parsing
            Boolean nextTextIsID = false;
            Boolean nextTextIsLoc = false;
            String currentID = "";
            String currentLoc = "";

            //open the temporary song index and write the xml DTD for it
            TextWriter w = new StreamWriter(songsFile);
            writeDTD(w);

            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.Text) {
                    //the current node has an iTunes id
                    if (nextTextIsID) {
                        nextTextIsID = false;
                        currentID = reader.Value;
                    }
                    //the current node has a location for a song
                    else if (nextTextIsLoc) {
                        nextTextIsLoc = false;
                        currentLoc = reader.Value;
                        writeXML(w, currentID, currentLoc);
                    }
                    //the next node has an iTunes id
                    else if (reader.Value == "Track ID")
                        nextTextIsID = true;
                    //the next node has a location for a song
                    else if (reader.Value == "Location")
                        nextTextIsLoc = true;
                    //the song index is complete; we should start parsing the playlists
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

        /// <summary>
        /// Writes the DTD for the given file, referenced by the argument
        /// </summary>
        /// <param name="w">file handle for a given file</param>
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
            //flags for reading xml
            Boolean nextNodeIsPlaylistName = false;
            String currentPlaylist = "";

            String songsPath = Directory.GetCurrentDirectory() + @"\" + songsFile; //song index
            playlistDoc = new System.Xml.XPath.XPathDocument(songsPath);           //xml doc of song index
            playlistNav = playlistDoc.CreateNavigator();
            reader.ReadToFollowing("array");
            
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        //next node is the name of a playlist
                        if (reader.Value == "Name")
                            nextNodeIsPlaylistName = true;
                        //the current node is the name of a playlist
                        else if (nextNodeIsPlaylistName) {
                            currentPlaylist = reader.Value;
                            nextNodeIsPlaylistName = false;
                        }
                        break;
                    case XmlNodeType.Element:
                        //the current node marks the beginning of the playlist specified by currentPlaylist
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
            //skip excluded playlists
            if (excludedPlaylists.Contains(playlistName))
                return;

            //status for the progress bar
            status = "Creating the "+playlistName+" playlist...";
            
            int count = 0; //song id
            Boolean nextElementIsID = false; //flag for getting the next song id
            String loc = ""; //location of song

            //create the next playlist file
            playlistName = playlistName.Replace("/", " and ");
            playlistName = playlistName.Replace(@"\u201b", "'");
            String playlist = outputDir + "/" + playlistName + ".xml";
            
            TextWriter w = new StreamWriter(playlist);
            writePlaylistDTD(w);
            Boolean finishedGettingPlaylist = false;
            
            while (reader.Read() && !finishedGettingPlaylist) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        //the next node is an iTunes id
                        if (reader.Value == "Track ID")
                            nextElementIsID = true;
                        //the current node is an iTunes id; get the location of the song associated with that iTunes id and write it to the playlist file
                        else if (nextElementIsID) {
                            count++;
                            nextElementIsID = false;
                            loc = getLocationFromID(reader.Value);
                            writePlaylistXML(w, count, loc);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        //marks the end of a playlist
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
            loc = Uri.UnescapeDataString(loc);
            
            playlistWriter.WriteLine("  <song id=\'{0:000000}\'>{1}</song>", id, loc);
        }

        /// <summary>
        /// Writes the DTD for the given file, referenced by the argument
        /// </summary>
        /// <param name="w">file handle for the given file</param>
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
