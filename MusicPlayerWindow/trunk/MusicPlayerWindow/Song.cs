using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    public class Song
    {
        private String path;
        private IrrKlang.ISound sound;

        #region Constructors
        public Song(String path)
        {
            this.path = path;
            this.sound = null;
        }
        public Song(Song prevSong)
        {
            this.path = prevSong.getPath();
            this.sound = prevSong.getSound();
        }
        #endregion

        #region Interface
        public void setPath(String newPath)
        {
            this.path = newPath;
        }
        public void setSound(IrrKlang.ISound sound)
        {
            this.sound = sound;
        }
        #endregion

        #region Accessors
        public String getPath()
        {
            return this.path;
        }
        public IrrKlang.ISound getSound()
        {
            return sound;
        }
        #endregion
    }
}
