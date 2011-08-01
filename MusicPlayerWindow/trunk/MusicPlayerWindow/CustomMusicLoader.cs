using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MusicPlayerWindow
{
    public class CustomMusicLoader
    {
        #region Definitions
        private enum QueueOp { UPDATE_NEXT_Q, UPDATE_PREV_Q, NEXT_Q_FRONT, SWITCH_PLAYLIST, WRITE_PLAYLIST, DESTROY, NO_OP };
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

        public CustomMusicLoader(String outputDir)
        {
            loaderThread = new Thread(new ThreadStart(this.ThreadActivity));
            opObject = new OperationObject(QueueOp.NO_OP, null, null);
            this.store = new QueueStore(outputDir, this);
            loaderThread.Start();
        }

        #region Thread Method
        private void ThreadActivity()
        {
            Monitor.Enter(opObject);
            bool shouldExit = false;
            Monitor.Wait(opObject);
            while (true)
            {
                switch (opObject.next_operation)
                {
                    case QueueOp.DESTROY: _destroyStore(); shouldExit = true; break;
                    case QueueOp.UPDATE_NEXT_Q: _updateNextQueue(); break;
                    case QueueOp.UPDATE_PREV_Q: _updatePrevQueue(); break;
                    case QueueOp.NEXT_Q_FRONT: _addSongToNextQueueFront(); break;
                    case QueueOp.SWITCH_PLAYLIST: _switchToPlaylist(); break;
                    //case QueueOp.WRITE_PLAYLIST: _writePlaylistToStore(); break;
                    case QueueOp.NO_OP:
                    default: break;
                }
                if (shouldExit) { break; }
                Monitor.Pulse(opObject);
                Monitor.Wait(opObject);
            }
            Monitor.Exit(opObject);
        }
        #endregion

        #region Interface
        public void updateNextQueue()
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.UPDATE_NEXT_Q;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }
        public void updatePrevQueue(Song song)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.UPDATE_PREV_Q;
            opObject.song_for_op = song;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }
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
        public void addSongToNextQueueFront(Song song)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.NEXT_Q_FRONT;
            opObject.song_for_op = song;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }
        public void destroyStoreAndExit()
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.DESTROY;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
            loaderThread.Join(500);
        }
        #endregion

        #region Internal Methods (implement the interface)
        private void _updateNextQueue()
        {
            Song newSong = getOneSong(store.getCurrentQueue());
            store.addSongToNextQueue(newSong);
        }
        private void _updatePrevQueue()
        {
            store.addSongToPrevQueue(opObject.song_for_op);
            opObject.song_for_op = null;
        }
        private void _addSongToNextQueueFront()
        {
            store.addSongToNextQueueFront(opObject.song_for_op);
        }        
        private void _switchToPlaylist()
        {
            store.addSongToPrevQueue(opObject.song_for_op);
            store.switchPlaylist(opObject.playlist_name_for_op);
        }
        private void _destroyStore()
        {
            store = null;
        }
        #endregion

        #region Accessors
        internal Song getOneSong(Queue q)
        {
            //get song from playlist
            String path = q.getPlaylistPath();
            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(q.getPlaylistPath());
            System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();
            Random random = new Random();
            int num = random.Next(0, q.getPlaylistLen());
            String id = String.Format("{0:000000}", num);
            nav.MoveToId(id);
            Song newSong = new Song(nav.Value);
            return newSong;
        }
        public Song getNextSong()
        {
            return store.getSongFromNextQueue();
        }
        public Song getPrevSong()
        {
            return store.getSongFromPrevQueue();
        }
        public List<String> getPlaylistNames()
        {
            return store.getPlaylistNames();
        }
        #endregion
    }
}
