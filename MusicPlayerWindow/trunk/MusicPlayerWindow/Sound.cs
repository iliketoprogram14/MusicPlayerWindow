using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;
using Un4seen.Bass;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Encapsulates the underlying representation of a song;
    /// Currently encapsulates IrrKlang ISounds and Bass.Net streams
    /// </summary>
    public class Sound
    {
        #region Properties

        private int stream;            //for Bass
        private IrrKlang.ISound sound; //for IrrKlang
        private enum Engines { Bass, IrrKlang };
        private Engines engine;

        #endregion

        #region Constructors

        /// <summary>
        /// Sound constructor for a Bass.Net stream
        /// </summary>
        /// <param name="stream"></param>
        public Sound(int stream)
        {
            this.stream = stream;
            engine = Engines.Bass;
        }

        /// <summary>
        /// Sound constructor for a IrrKlang ISound
        /// </summary>
        /// <param name="sound"></param>
        public Sound(IrrKlang.ISound sound)
        {
            this.sound = sound;
            engine = Engines.IrrKlang;
        }

        #endregion

        #region Interface

        /// <summary>
        /// Stops the sound permanently
        /// </summary>
        public void stop()
        {
            switch (engine) {
                case Engines.Bass:
                    Bass.BASS_ChannelStop(stream);
                    break;
                case Engines.IrrKlang:
                    sound.Stop();
                    break;
            }
        }

        /// <summary>
        /// Pause or unpause the sound
        /// </summary>
        public void pauseUnpause()
        {
            switch (engine)
            {
                case Engines.Bass:
                    Un4seen.Bass.BASSActive isPlaying = Bass.BASS_ChannelIsActive(stream);
                    if (isPlaying == Un4seen.Bass.BASSActive.BASS_ACTIVE_PLAYING)
                        Bass.BASS_ChannelPause(stream);
                    else
                        Bass.BASS_ChannelPlay(stream, false);
                    break;
                case Engines.IrrKlang:
                    sound.Paused = !sound.Paused;
                    break;
            }
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Accessor for the volume of the current sound
        /// </summary>
        /// <returns>Volume of the current sound</returns>
        public float getVolume()
        {
            float volume = 100;
            switch (engine) {
                case Engines.Bass:
                    volume = Bass.BASS_GetVolume();
                    break;
                case Engines.IrrKlang:
                    volume = sound.Volume;
                    break;
            }
            return volume;
        }

        /// <summary>
        /// Accessor for whether the sound is paused or not
        /// </summary>
        /// <returns>A boolean that's true when the sound is paused and false otherwise</returns>
        public Boolean isPaused()
        {
            Boolean isPaused = false;
            switch (engine) {
                case Engines.Bass:
                    isPaused = (Bass.BASS_ChannelIsActive(stream) == Un4seen.Bass.BASSActive.BASS_ACTIVE_PAUSED);
                    break;
                case Engines.IrrKlang:
                    isPaused = sound.Paused;
                    break;
            }
            return isPaused;
        }
        #endregion

        #region Setters

        /// <summary>
        /// Setter method for changing the volume of the sound
        /// </summary>
        /// <param name="vol">The new volume</param>
        public void setVolume(float vol)
        {
            switch (engine)
            {
                case Engines.Bass:
                    Bass.BASS_SetVolume(vol);
                    break;
                case Engines.IrrKlang:
                    sound.Volume = vol;
                    break;
            }
        }

        #endregion
    }
}
