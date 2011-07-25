using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    
    public class Queue
    {
        Playlist playlist;
        List<Song> prevSongQueue;
        int prevSongLimit;
        List<Song> nextSongQueue;
        public Queue(Playlist playlist, int songLimit)
        {
            this.playlist = playlist;
            loadNextSongQueue();
            prevSongLimit = songLimit;
        }
        private void loadNextSongQueue()
        {
        }
    }
}
