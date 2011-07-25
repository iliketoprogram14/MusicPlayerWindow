using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    public class QueueStore
    {
        private HashSet<Queue> playlistQueues;
        private Queue currentQueue;
        public QueueStore()
        {
            playlistQueues = new HashSet<Queue>();
            currentQueue = null;
        }

        public void addSongToNextQueue()
        {
            //get song from playlist 
            Song newSong = null;
            currentQueue.nextSongQueue.Enqueue(newSong);
        }
        public void addSongToPrevQueue(Song song)
        {
            currentQueue.prevSongQueue.Push(song);
        }
        public void switchPlaylist(Playlist playlist_for_op)
        {
        }
        public void writeCurrentPlaylistToStore()
        {
        }
        public Song getSongFromNextQueue()
        {
            return currentQueue.nextSongQueue.Dequeue();
        }
        public Song getSongFromPrevQueue()
        {
            return currentQueue.prevSongQueue.Pop();
        }
    }
}
