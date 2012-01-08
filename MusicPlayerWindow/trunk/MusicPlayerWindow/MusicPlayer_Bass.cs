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
        private MainWindow window;
        private SYNCPROC syncer;
        private String[] plugins = { "bassacc.dll", "bassalac.dll" };
        private Dictionary<String, int> plugin_dict;

        public MusicPlayer_Bass(MainWindow window)
        {
            this.window = window;
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            syncer = new SYNCPROC(SyncEnded);
            plugin_dict = new Dictionary<String,int>();

            foreach (String plg in plugins) {
                int plugin = Bass.BASS_PluginLoad(plg);
                plugin_dict.Add(plg, plugin);
            }
        }

        #region Interface

        public void playCurrSong(Song song)
        {
            //int stream = Bass.BASS_StreamCreateFile(@"D:/Music/iTunes/Music/Yes/Fragile/01 Roundabout.mp3", 0, 0, BASSFlag.BASS_DEFAULT);
            int stream = Bass.BASS_StreamCreateFile(song.getPath(), 0, 0, BASSFlag.BASS_DEFAULT);
            Bass.BASS_ChannelPlay(stream, false);
            Bass.BASS_ChannelSetSync(stream, BASSSync.BASS_SYNC_END | BASSSync.BASS_SYNC_MIXTIME, 0, syncer, IntPtr.Zero);
            song.setSound(new Sound(stream));
            Bass.BASS_SetVolume(window.getVolume());
        }

        public void pauseUnpauseSong(Song song)
        {
            song.getSound().pauseUnpause();
        }

        public void stopSong(Song song)
        {
            song.getSound().stop();
        }

        public void setVolume(Song song, float volume)
        {
            song.getSound().setVolume(volume);
        }

        public Boolean isPaused(Song song)
        {
            return song.getSound().isPaused();
        }

        public void destroyPlayer()
        {
            foreach (int plg in plugin_dict.Values)
                Bass.BASS_PluginFree(plg);
            Bass.BASS_Free();

        }
        #endregion

        #region Event Handlers
        private void SyncEnded(int handle, int channel, int data, IntPtr user)
        {
            Console.WriteLine("in the handler");
            window.playNextSong();
        }
        #endregion
    }
}
