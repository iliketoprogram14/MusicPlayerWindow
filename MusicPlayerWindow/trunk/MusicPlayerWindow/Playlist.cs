using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Reference the playlist xml file that contains paths to the playlist's songs
    /// </summary>
    public class Playlist
    {
        private String name;
        private String path;
        private int len;

        /// <summary>
        /// Constructor stores path to playlist xml file and the number of songs the playlist contains
        /// </summary>
        /// <param name="path">the path to the playlist xml file</param>
        /// <param name="len">the number of songs in the playlist</param>
        public Playlist(String path, int len)
        {
            this.path = path;
            this.len = System.IO.File.ReadLines(path).Count();
            String[] tmp = path.Split('\\');
            this.name = (tmp[tmp.Length - 1].Split('.'))[0];
        }

        #region Accessors
        /// <summary>
        /// Gets the name of the playlist, derived from the path
        /// </summary>
        /// <returns>the name of the playlist</returns>
        public String getName()
        {
            return name;
        }

        /// <summary>
        /// Gets the path of the playlist xml file
        /// </summary>
        /// <returns>the path to the playlist xml file</returns>
        public String getPath()
        {
            return path;
        }

        /// <summary>
        /// Gets the number of songs in the playlist
        /// </summary>
        /// <returns>the number of songs in the playlist</returns>
        public int getLen()
        {
            return len;
        }
        #endregion
    }
}
