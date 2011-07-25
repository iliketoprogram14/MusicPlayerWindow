using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrKlang;

namespace MusicPlayerWindow
{
    public class MusicPlayer
    {
        private ISoundEngine engine;
        public MusicPlayer()
        {
            engine = new ISoundEngine();
        }

        public void playCurrSong(Song song)
        {
            ISound sound = engine.Play2D(song.getPath());
            song.setSound(sound);
        }

        public void pauseSong()
        {
            engine.SetAllSoundsPaused(true);
        }

        public void stopSong()
        {
            engine.StopAllSounds();
        }

        public void getNextSong()
        {

        }

        public void getPrevSong()
        {

        }
    }
}
