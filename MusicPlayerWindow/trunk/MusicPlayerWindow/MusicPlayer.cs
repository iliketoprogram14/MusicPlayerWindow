using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrKlang;
using System.Windows.Forms;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Plays the music
    /// </summary>
    public class MusicPlayer : MusicPlayerInterface, ISoundStopEventReceiver
    {
        private ISoundEngine engine;
        private MainWindow window;

        /// <summary>
        /// Constructor that initializes irrKlang 2D music player
        /// </summary>
        /// <param name="window">reference to the main window</param>
        public MusicPlayer(MainWindow window)
        {
            engine = new ISoundEngine();
            this.window = window;
        }

        #region Interface
        /// <summary>
        /// Plays given song
        /// </summary>
        /// <param name="song">the song to be played</param>
        public void playCurrSong(Song song)
        {
            ISound sound = engine.Play2D(song.getPath());
            sound.Volume = window.getVolume();
            sound.setSoundStopEventReceiver(this); //set stop handler
            song.setSound(sound);
        }

        /// <summary>
        /// Pauses or unpauses a song
        /// </summary>
        /// <param name="song">the song to be paused or unpaused</param>
        public void pauseUnpauseSong(Song song)
        {
            song.getSound().Paused = !song.getSound().Paused;
        }

        /// <summary>
        /// Stops the playing song
        /// </summary>
        /// <param name="song">the song to be stopped</param>
        public void stopSong(Song song)
        {
            song.getSound().Stop();
        }

        /// <summary>
        /// Stops the player and removes it from memory
        /// </summary>
        public void destroyPlayer()
        {
            engine.RemoveAllSoundSources();
            engine.Dispose();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// The event handler for when the music engine stops playing a sound
        /// </summary>
        /// <param name="sound">the sound that was stopped</param>
        /// <param name="cause">the reason why the sound was stopped</param>
        /// <param name="userData">extra data (unused)</param>
        public void OnSoundStopped(ISound sound, StopEventCause cause, object userData)
        {
            //play the next song if song finished naturally
            if (cause == StopEventCause.SoundFinishedPlaying)
                window.playNextSong();
            //the stop button has pressed, so stop playing permanently
            else if (cause == StopEventCause.SoundStoppedByUser)
                return;
            else return;
        }
        #endregion

    }
}
