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
        private int len;
        public Playlist(String path, int len)
        {
            this.path = path;
            this.len = System.IO.File.ReadLines(path).Count();
            String[] tmp = path.Split('\\');
            this.name = (tmp[tmp.Length - 1].Split('.'))[0];
        }
        public String getName()
        {
            return name;
        }
        public String getPath()
        {
            return path;
        }
        public int getLen()
        {
            return len;
        }
    }
}
