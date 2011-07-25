using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    public class Playlist
    {
        private String name;
        private String path;
        public Playlist(String name, String path)
        {
            this.name = name;
            this.path = path;
        }
        public String getName()
        {
            return name;
        }
        public String getPath()
        {
            return path;
        }
    }
}
