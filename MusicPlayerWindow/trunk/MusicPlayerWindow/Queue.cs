using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    
    public class Queue
    {
        public Playlist playlist;
        public Stack<Song> prevSongQueue;
        public int prevSongLimit;
        public Queue<Song> nextSongQueue;
        public Queue(Playlist playlist, int songLimit)
        {
            this.playlist = playlist;
            prevSongQueue = new Stack<Song>();
            nextSongQueue = new Queue<Song>();
            loadNextSongQueue();
            prevSongLimit = songLimit;
        }
        private void loadNextSongQueue()
        {

        }
    }
}
