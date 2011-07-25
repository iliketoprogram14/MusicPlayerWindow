using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MusicPlayerWindow
{
    public class CustomMusicLoader
    {
        enum QueueOp { UPDATE_NEXT_Q, UPDATE_PREV_Q, SWITCH_PLAYLIST, WRITE_PLAYLIST, NO_OP};
        private class OperationObject
        {
            public QueueOp next_operation;
            public Playlist playlist_for_op;
            public Song song_for_op;
            public OperationObject(QueueOp next_op, Playlist playlist, Song song)
            {
                next_operation = next_op;
                playlist_for_op = playlist;
                song_for_op = song;
            }
        }
        Thread loaderThread;
        QueueStore store;
        OperationObject opObject;

        public CustomMusicLoader()
        {
            loaderThread = new Thread(new ThreadStart(this.ThreadActivity));
            opObject = new OperationObject(QueueOp.NO_OP, null, null);
            this.store = null;
        }

        private void ThreadActivity()
        {
            Monitor.Enter(opObject);
            Monitor.Wait(opObject);
            while (true)
            {
                switch(opObject.next_operation)
                {
                    case QueueOp.UPDATE_NEXT_Q: _updateNextQueue(); break;
                    case QueueOp.UPDATE_PREV_Q: _updatePrevQueue(); break;
                    case QueueOp.SWITCH_PLAYLIST: _switchToPlaylist(); break;
                    case QueueOp.WRITE_PLAYLIST: _writePlaylistToStore(); break;
                    case QueueOp.NO_OP:
                    default: break;
                }
                Monitor.Wait(opObject);
            }
        }

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
        public void switchToPlaylist(Playlist newPlaylist)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.SWITCH_PLAYLIST;
            opObject.playlist_for_op = newPlaylist;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }
        public void writePlaylistToStore(Playlist playlist)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.WRITE_PLAYLIST;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }

        private void _updateNextQueue()
        {
            store.addSongToNextQueue();
        }
        private void _updatePrevQueue()
        {
            store.addSongToPrevQueue(opObject.song_for_op);
            opObject.song_for_op = null;
        }
        private void _switchToPlaylist()
        {
            _writePlaylistToStore();
            store.switchPlaylist(opObject.playlist_for_op);
            opObject.playlist_for_op = null;
        }
        private void _writePlaylistToStore()
        {
            store.writeCurrentPlaylistToStore();
        }

        public Song getNextSong()
        {
            return store.getSongFromNextQueue();
        }
        public Song getPrevSong()
        {
            return store.getSongFromPrevQueue();
        }
    }
}
