using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Holds every single playlist's queue object
    /// </summary>
    public class QueueStore
    {
        private Dictionary<String, Queue> playlistQueues;
        private Queue currentQueue;
        private CustomMusicLoader loader;

        #region Constructor
        ///<summary>
        ///Constructor initializes queues
        ///</summary>
        ///<param name="outputDir">the playlists are stored here</param>
        ///<param name="loader">gets songs and puts them in the queuestore</param>
        public QueueStore(String outputDir, CustomMusicLoader loader)
        {
            this.loader = loader;
            playlistQueues = new Dictionary<String, Queue>();
            currentQueue = null;
            String[] playlists = Directory.GetFiles(outputDir);
            foreach (String file in playlists)
            {
                int len = File.ReadLines(file).Count();
                Playlist p = new Playlist(file, len);
                Queue q = new Queue(p, len);
                initNextQueue(q);
                playlistQueues.Add(p.getName(), q);
            }
            currentQueue = playlistQueues["Music"];
        }

        ///<summary>
        ///Fills the given next song queue with 5 songs
        ///</summary>
        ///<param name="q">the queue to be filled with songs</param>
        private void initNextQueue(Queue q)
        {
            List<String> songsInQueue = new List<String>();
            Song tmp;
            Boolean gotNewSong = false;

            for (int i = 0; i < 5; i++) {
                gotNewSong = false;
                do {
                    tmp = loader.getOneSong(q);
                    if (!songsInQueue.Contains(tmp.getPath())) {
                        q.addNextSong(tmp);
                        songsInQueue.Add(tmp.getPath());
                        gotNewSong = true;
                    }
                } while (!gotNewSong);
            }
        }
        #endregion

        #region Interface
        ///<summary>
        ///Adds a song to the end of the next song queue
        ///</summary>
        ///<param name="newSong">the song to be added to the next song queue</param>
        public void addSongToNextQueue(Song newSong)
        {
            currentQueue.addNextSong(newSong);
        }

        ///<summary>
        ///Adds a song to the end of the previous song queue
        ///</summary>
        ///<param name="oldSong">the song to be added to the previous song queue</param>
        public void addSongToPrevQueue(Song oldSong)
        {
            currentQueue.addPrevSong(oldSong);
        }

        ///<summary>
        ///Adds a song to the front of the next song queue (when prev button clicked)
        ///</summary>
        ///<param name="song">the song to be inserted at the front of the next song queue</param>
        public void addSongToNextQueueFront(Song song)
        {
            currentQueue.nextSongQueue.Insert(0, song);
            int last_song = currentQueue.nextSongQueue.Count - 1;
            currentQueue.nextSongQueue.RemoveAt(last_song);
        }

        ///<summary>
        ///Sets the current queue to the given playlist
        ///</summary>
        ///<param name="next_playlist_name">the next playlist to  be played</param>
        public void switchPlaylist(String next_playlist_name)
        {
            currentQueue = playlistQueues[next_playlist_name];
        }
        #endregion

        #region Accessors        
        /// <summary>
        /// Gets the next song from the next song queue
        /// </summary>
        /// <returns>the song from the next song queue</returns>
        public Song getSongFromNextQueue()
        {
            return currentQueue.getNextSong();
        }

        ///<summary>
        ///Gets the next song from the previous song queue
        ///</summary>
        /// <returns>the song from the previoussong queue</returns>
        public Song getSongFromPrevQueue()
        {
            return currentQueue.getPrevSong();
        }

        ///<summary>
        ///Gets the current queue
        ///</summary>
        ///<returns>the current queue</returns>
        internal Queue getCurrentQueue()
        {
            return currentQueue;
        }

        ///<summary>
        ///Gets all the playlist names
        ///</summary>
        ///<returns>a list of all playlist names</returns>
        public List<String> getPlaylistNames()
        {
            return new List<String>(playlistQueues.Keys);
        }
        #endregion
    }
}
