using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;
using Un4seen.Bass;

namespace MusicPlayerWindow
{
    public class Sound
    {
        private int stream;            //for Bass
        private IrrKlang.ISound sound; //for IrrKlang
        private enum Engines { Bass, IrrKlang };
        private Engines engine;

        #region Constructors
        public Sound(int stream)
        {
            this.stream = stream;
            engine = Engines.Bass;
        }

        public Sound(IrrKlang.ISound sound)
        {
            this.sound = sound;
            engine = Engines.IrrKlang;
        }
        #endregion

        #region Interface
        public void stop()
        {
            switch (engine)
            {
                case Engines.Bass:
                    Bass.BASS_ChannelStop(stream);
                    break;
                case Engines.IrrKlang:
                    sound.Stop();
                    break;
            }
        }

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
        public float getVolume()
        {
            float volume = 100;
            switch (engine)
            {
                case Engines.Bass:
                    volume = Bass.BASS_GetVolume();
                    break;
                case Engines.IrrKlang:
                    volume = sound.Volume;
                    break;
            }
            return volume;
        }

        public Boolean isPaused()
        {
            Boolean isPaused = false;
            switch (engine)
            {
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
