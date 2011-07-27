using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace MusicPlayerWindow
{
    public class QueueStore
    {
        private Dictionary<String,Queue> playlistQueues;
        private Queue currentQueue;
        private Queue lastQueue;
        private CustomMusicLoader loader;
        public QueueStore(String outputDir, CustomMusicLoader loader)
        {
            this.loader = loader;
            playlistQueues = new Dictionary<String,Queue>();
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
        private void initNextQueue(Queue q)
	    {
            for (int i = 0; i < 5; i++)
            {
                q.addNextSong(loader.getOneSong(q));
            }
	    }
        public void addSongToNextQueue(Song newSong)
        {
            currentQueue.addNextSong(newSong);
        }

        public void addSongToPrevQueue(Song oldSong)
        {
            currentQueue.addPrevSong(oldSong);
        }

        public void addSongToNextQueueFront(Song song)
        {
            currentQueue.nextSongQueue.Insert(0, song);
        }

        public Song getSongFromNextQueue()
        {
            return currentQueue.getNextSong();
        }
        public Song getSongFromPrevQueue()
        {
            return currentQueue.getPrevSong();
        }
        public void switchPlaylist(String next_playlist_name)
        {
            currentQueue = playlistQueues[next_playlist_name];
        }
        /*public void writeCurrentPlaylistToStore()
        {
	    playlistQueues[currentQueue.playlist] = new Queue(currentQueue);
        }*/
        internal Queue getCurrentQueue()
        {
            return currentQueue;
        }
        public List<String> getPlaylistNames()
        {
            return new List<String>(playlistQueues.Keys);
        }
    }
}
