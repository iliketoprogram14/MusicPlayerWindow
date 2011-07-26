﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MusicPlayerWindow
{
    public class CustomMusicLoader
    {
        enum QueueOp { UPDATE_NEXT_Q, UPDATE_PREV_Q, NEXT_Q_FRONT, SWITCH_PLAYLIST, WRITE_PLAYLIST, NO_OP };
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

        public CustomMusicLoader(String outputDir)
        {
            loaderThread = new Thread(new ThreadStart(this.ThreadActivity));
            opObject = new OperationObject(QueueOp.NO_OP, null, null);
            this.store = new QueueStore(outputDir, this);
            loaderThread.Start();
        }

        private void ThreadActivity()
        {
            Monitor.Enter(opObject);
            Monitor.Wait(opObject);
            while (true)
            {
                switch (opObject.next_operation)
                {
                    case QueueOp.UPDATE_NEXT_Q: _updateNextQueue(); break;
                    case QueueOp.UPDATE_PREV_Q: _updatePrevQueue(); break;
                    case QueueOp.NEXT_Q_FRONT: _addSongToNextQueueFront(); break;
                    case QueueOp.SWITCH_PLAYLIST: _switchToPlaylist(); break;
                    //case QueueOp.WRITE_PLAYLIST: _writePlaylistToStore(); break;
                    case QueueOp.NO_OP:
                    default: break;
                }
                Monitor.Wait(opObject);
            }
        }

        //INTERFACE
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
        public void addSongToNextQueueFront(Song song)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.NEXT_Q_FRONT;
            opObject.song_for_op = song;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }
        /*public void writePlaylistToStore(Playlist playlist)
        {
            Monitor.Enter(opObject);
            opObject.next_operation = QueueOp.WRITE_PLAYLIST;
            Monitor.Pulse(opObject);
            Monitor.Exit(opObject);
        }*/
        public Song getNextSong()
        {
            return store.getSongFromNextQueue();
        }
        public Song getPrevSong()
        {
            return store.getSongFromPrevQueue();
        }

        //INTERNAL AND PRIVATE METHODS/HELPERS
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
            //_writePlaylistToStore();
            store.switchPlaylist(opObject.playlist_for_op);
            opObject.playlist_for_op = null;
        }
        /*private void _writePlaylistToStore()
        {
            store.writeCurrentPlaylistToStore();
        }*/

    }
}
