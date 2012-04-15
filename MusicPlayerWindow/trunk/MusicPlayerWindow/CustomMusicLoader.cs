using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Updates the previous and next song queues in a separate thread
    /// </summary>
    public class CustomMusicLoader
    {
        #region Definitions
        /// <summary>
        /// The thread is given a queue operation (QueueOp) to do next
        /// </summary>
        private enum QueueOp { UPDATE_NEXT_Q, UPDATE_PREV_Q, NEXT_Q_FRONT, SWITCH_PLAYLIST, WRITE_PLAYLIST, DESTROY, NO_OP };

        /// <summary>
        /// Contains a QueueOp object as well as any objects that are needed in the given operation
        /// </summary>
        private class OperationObject
        {
            public QueueOp next_operation;
            public String playlist_name_for_op;
            public Song song_for_op;
            public OperationObject(QueueOp next_op, String playlistName, Song song)
            {
                next_operation = next_op;
                playlist_name_for_op = playlistName;
                song_for_op = song;
            }
        }
        #endregion

        Thread loaderThread;
        QueueStore store;
        OperationObject opObject;

        /// <summary>
        /// Constructor initializes the OperationObject and starts the loader thread
        /// </summary>
        /// <param name="outputDir">the location of the playlists, so that the loader can get songs from the playlist xml files</param>
        public CustomMusicLoader(String outputDir)
        {
            loaderThread = new Thread(new ThreadStart(this.ThreadActivity));
            opObject = new OperationObject(QueueOp.NO_OP, null, null);
            this.store = new QueueStore(outputDir, this);
            loaderThread.Start();
        }

        #region Thread Method
        /// <summary>
        /// the thread runs in this method; it can call other methods depending on the given operation
        /// </summary>
        private void ThreadActivity()
        {
            Monitor.Enter(opObject);
            bool shouldExit = false;
            Monitor.Wait(opObject);
            while (true)
            {
                //do the operation
                switch (opObject.next_operation)
                {
                    case QueueOp.DESTROY: _destroyStore(); shouldExit = true; break;
                    case QueueOp.UPDATE_NEXT_Q: _updateNextQueue(); break;
                    case QueueOp.UPDATE_PREV_Q: _updatePrevQueue(); break;
                    case QueueOp.NEXT_Q_FRONT: _addSongToNextQueueFront(); break;
                    case QueueOp.SWITCH_PLAYLIST: _switchToPlaylist(); break;
                    case QueueOp.NO_OP:
                    default: break;
                }
                if (shouldExit) { break; }
                Monitor.Pulse(opObject);
                Monitor.Wait(opObject);
                GC.Collect();
            }
            Monitor.Exit(opObject);
        }
        #endregion

        #region Interface
        /// <summary>
        /// Asks loader to add a song to the next queue <see cref="MusicPlayerWindow.CustomMusicLoader._updateNextQueue()"/>
        /// </summary>
        public void updateNextQueue()
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.UPDATE_NEXT_Q;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }

        /// <summary>
        /// Asks loader to add a song to the previous song queue <see cref="MusicPlayerWindow.CustomMusicLoader._updatePrevQueue()"/>
        /// </summary>
        /// <param name="song">the song to be added to the previous song queue</param>
        public void updatePrevQueue(Song song)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.UPDATE_PREV_Q;
            opObject.song_for_op = song;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }

        /// <summary>
        /// Asks loader to add the song to front of the next queue (the previous button was clicked) <see cref="MusicPlayerWindow.CustomMusicLoader._addSongToNextQueueFront()"/>
        /// </summary>
        /// <param name="song"></param>
        public void addSongToNextQueueFront(Song song)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.NEXT_Q_FRONT;
            opObject.song_for_op = song;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }

        /// <summary>
        /// Asks loader to switch playlists <see cref="MusicPlayerWindow.CustomMusicLoader._switchToPlaylist()"/>
        /// </summary>
        /// <param name="newPlaylistName">the playlist to be shuffled next</param>
        /// <param name="oldSong">the song to be added to the previous song queue of the old playlist</param>
        public void switchToPlaylist(String newPlaylistName, Song oldSong)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.SWITCH_PLAYLIST;
            opObject.playlist_name_for_op = newPlaylistName;
            opObject.song_for_op = oldSong;
            Monitor.Pulse(opObject);
            Monitor.Wait(opObject); //wait to change playlist...
            Monitor.Exit(opObject);
        }

        /// <summary>
        /// Asks loader to destroy the store and then exits <see cref="MusicPlayerWindow.CustomMusicLoader._destroyStore()"/>
        /// </summary>
        public void destroyStoreAndExit()
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.DESTROY;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
            loaderThread.Join(500);
        }
        #endregion

        #region Internal Methods (they actually implement the above methods in the interface)
        /// <summary>
        /// The loader adds a song to the next song queue
        /// </summary>
        private void _updateNextQueue()
        {
            Song newSong = getOneSong(store.getCurrentQueue());
            store.addSongToNextQueue(newSong);
        }

        /// <summary>
        /// The loader adds a song (given in the OperationObject) to the previous song queue
        /// </summary>
        private void _updatePrevQueue()
        {
            store.addSongToPrevQueue(opObject.song_for_op);
            opObject.song_for_op = null;
        }

        /// <summary>
        /// The loader adds a song (given in the OperationObject) to the front of the next song queue (previous button was clicked)
        /// </summary>
        private void _addSongToNextQueueFront()
        {
            store.addSongToNextQueueFront(opObject.song_for_op);
        }        

        /// <summary>
        /// The loader switches playlist (which was stored in the OperationObject)
        /// </summary>
        private void _switchToPlaylist()
        {
            store.addSongToPrevQueue(opObject.song_for_op);
            store.switchPlaylist(opObject.playlist_name_for_op);
        }

        /// <summary>
        /// The loader destroys the store
        /// </summary>
        private void _destroyStore()
        {
            store = null;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        internal Song getOneSong(Queue q)
        {
            //get song from playlist
            String path = q.getPlaylistPath();
            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(path);
            System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();
            Random random = new Random();
            int num = random.Next(0, q.getPlaylistLen());
            String id = String.Format("{0:000000}", num);
            nav.MoveToId(id);
            Song newSong = new Song(nav.Value);
            doc = null;
            
            return newSong;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Song getNextSong()
        {
            return store.getSongFromNextQueue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Song getPrevSong()
        {
            return store.getSongFromPrevQueue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<String> getPlaylistNames()
        {
            return store.getPlaylistNames();
        }
        #endregion
    }
}
