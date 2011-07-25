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
        }

        public void addSongToNextQueue()
        {
        }
        public void addSongToPrevQueue(Song song)
        {
        }
        public void switchPlaylist(Playlist playlist_for_op)
        {
        }
        public void writeCurrentPlaylistToStore()
        {
        }
    }
}
