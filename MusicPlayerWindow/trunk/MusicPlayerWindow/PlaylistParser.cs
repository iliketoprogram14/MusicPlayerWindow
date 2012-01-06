using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MusicPlayerWindow
{
    /// <summary>
    ///  Parses iTunes's xml library and creates local playlists
    /// </summary>
    public class PlaylistParser
    {
        private XmlTextReader reader;
        private String songsFile = "iTunes Songs.xml";

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public PlaylistParser(String path)
        {
            reader = new XmlTextReader(path);
        }

        #endregion

        #region Interface
        /// <summary>
        /// 
        /// </summary>
        public void createPlaylists()
        {
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        if (reader.Value == "Tracks")
                            getSongs();
                        else if (reader.Value == "Playlists")
                            getPlaylists();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        private void getSongs()
        {
            Boolean nextTextIsID = false;
            Boolean nextTextIsLoc = false;
            String currentID = "";
            String currentLoc;

            TextWriter w = new StreamWriter(songsFile);
            w.WriteLine("<?xml version='1.0' ?>");
            w.WriteLine("<!DOCTYPE tracklist [");
            w.WriteLine("  <!ELEMENT tracklist (song)>");
            w.WriteLine("  <!ELEMENT song     (#PCDATA)>");
            w.WriteLine("  <!ATTLIST song id ID #REQUIRED>");
            w.WriteLine("]>");
            w.WriteLine("<tracklist>");

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                        if (nextTextIsID)
                        {
                            nextTextIsID = false;
                            currentID = reader.Value;
                        }
                        else if (nextTextIsLoc)
                        {
                            nextTextIsLoc = false;
                            currentLoc = reader.Value;
                            writeXML(w, currentID, currentLoc);
                        }
                        else if (reader.Value == "Track ID")
                            nextTextIsID = true;
                        else if (reader.Value == "Location")
                            nextTextIsID = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void writeXML(TextWriter writer, String id, String loc)
        {
            writer.WriteLine("  <song id=\'{0:000000}\'>{1}</song>", id, loc);
        }

        /// <summary>
        /// 
        /// </summary>
        private void getPlaylists()
        {
            Boolean nextTextIsPlaylistName = false;
            String currentPlaylist = "";
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
                        if (reader.Value == "array")
                            getOnePlaylist(currentPlaylist);
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void getOnePlaylist(String playlistName)
        {
            int count = 0;
            Boolean nextElementIsID = false;
            String loc = "";
            String playlist = playlistName + ".xml";
            TextWriter writer = new StreamWriter(playlist);

            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Text:
                        if (reader.Value == "Track ID")
                            nextElementIsID = true;
                        else if (nextElementIsID) {
                            count++;
                            nextElementIsID = false;
                            loc = getLocationFromID(reader.Value);
                            writePlaylistXML(writer, count, loc);
                        }
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private String getLocationFromID(String id)
        {
            return "";
        }

        /// <summary>
        /// </summary>
        private void writePlaylistXML(TextWriter playlistFile, int id, String Loc)
        {

        }
        #endregion
    }
}
