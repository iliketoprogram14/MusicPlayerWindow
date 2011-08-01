using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrKlang;
using System.Windows.Forms;

namespace MusicPlayerWindow
{
    public class MusicPlayer : MusicPlayerInterface, ISoundStopEventReceiver
    {
        private ISoundEngine engine;
        private MainWindow window;

        public MusicPlayer(MainWindow window)
        {
            engine = new ISoundEngine();
            this.window = window;
        }

        #region Interface
        public void playCurrSong(Song song)
        {
            ISound sound = engine.Play2D(song.getPath());
            sound.Volume = window.getVolume();
            sound.setSoundStopEventReceiver(this);
            song.setSound(sound);
        }
        public void pauseUnpauseSong(Song song)
        {
            song.getSound().Paused = !song.getSound().Paused;
        }
        public void stopSong(Song song)
        {
            song.getSound().Stop();
        }
        public void destroyPlayer()
        {
            engine.RemoveAllSoundSources();
            engine.Dispose();
        }
        #endregion

        #region Event Handlers
        public void OnSoundStopped(ISound sound, StopEventCause cause, object userData)
        {
            if (cause == StopEventCause.SoundFinishedPlaying)
            {
                window.playNextSong();
            }
            else if (cause == StopEventCause.SoundStoppedByUser)
            {
                return;
            }
            else return;
        }
        #endregion

    }
}
