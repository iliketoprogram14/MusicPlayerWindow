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

        public Song(String path) {
            this.path = path;
            this.sound = null;
        }

        public String getPath() {
            return this.path;
        }

        public void setPath(String newPath) {
            this.path = newPath;
        }

        public IrrKlang.ISound getSound() {
            return sound;
        }

        public void setSound(IrrKlang.ISound sound) {
            this.sound = sound;
        }
    }
}
