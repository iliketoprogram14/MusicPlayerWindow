using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace MusicPlayerWindow
{
    class MusicPlayer_Bass : MusicPlayerInterface
    {
        #region Properties

        private MainWindow window;
        private SYNCPROC syncer; //used for sensing when a song ends so that the next song can be played immediately after
        
        //add plugin references here
        private String[] plugins = { "bassacc.dll", "bassalac.dll" };
        private Dictionary<String, int> plugin_dict;

        #endregion

        #region Constructor

        /// <summary>
        /// Music player that uses the Bass.Net API
        /// </summary>
        /// <param name="window">Reference to the main window</param>
        public MusicPlayer_Bass(MainWindow window)
        {
            this.window = window;

            //initialize the Bass.Net player and the syncer for sensing the end of songs
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            syncer = new SYNCPROC(SyncEnded);

            //load the Bass.Net plugins
            plugin_dict = new Dictionary<String, int>();
            foreach (String plg in plugins) {
                int plugin = Bass.BASS_PluginLoad(plg);
                plugin_dict.Add(plg, plugin);
            }
        }

        #endregion

        #region Interface

        /// <summary>
        /// Plays given song
        /// </summary>
        /// <param name="song">The song to be played</param>
        public void playCurrSong(Song song)
        {
            //Create the stream and play it using a channel
            int stream = Bass.BASS_StreamCreateFile(song.getPath(), 0, 0, BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_AUTOFREE);
            Bass.BASS_ChannelPlay(stream, false);
            
            //Set up the syncer, and update the song and the volume
            Bass.BASS_ChannelSetSync(stream, BASSSync.BASS_SYNC_END | BASSSync.BASS_SYNC_MIXTIME, 0, syncer, IntPtr.Zero);
            song.setSound(new Sound(stream));
            Bass.BASS_SetVolume(window.getVolume());
        }

        /// <summary>
        /// Pauses or unpauses a song
        /// </summary>
        /// <param name="song">The song to be paused or unpaused</param>
        public void pauseUnpauseSong(Song song)
        {
            song.getSound().pauseUnpause();
        }

        /// <summary>
        /// Stops the song that is currently playing
        /// </summary>
        /// <param name="song">The song to be stopped</param>
        public void stopSong(Song song)
        {
            song.getSound().stop();
        }

        /// <summary>
        /// Changes the volume of the song playing
        /// </summary>
        /// <param name="song">The current song playing</param>
        /// <param name="volume">The new volume</param>
        public void setVolume(Song song, float volume)
        {
            song.getSound().setVolume(volume);
        }

        /// <summary>
        /// Checks to see whether the current song is paused or not
        /// </summary>
        /// <param name="song">The current song</param>
        /// <returns>A boolean that specifies whether current song is paused or not</returns>
        public Boolean isPaused(Song song)
        {
            return song.getSound().isPaused();
        }

        /// <summary>
        /// Stops the player and removes it and its plugins from memory
        /// </summary>
        public void destroyPlayer()
        {
            foreach (int plg in plugin_dict.Values)
                Bass.BASS_PluginFree(plg);
            Bass.BASS_Free();

        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event handler that gets triggered when the current song has finished playing; plays the next song in line
        /// </summary>
        /// <param name="handle">the current song playing</param>
        /// <param name="channel">The channel in which the current song is playing</param>
        /// <param name="data">Miscellaneous data</param>
        /// <param name="user"></param>
        private void SyncEnded(int handle, int channel, int data, IntPtr user)
        {
            window.playNextSong();
        }

        #endregion
    }
}
