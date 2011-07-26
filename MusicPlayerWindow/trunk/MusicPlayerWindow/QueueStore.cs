﻿using System;
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
        public Song getSongFromNextQueue()
        {
            return currentQueue.getNextSong();
        }
        public Song getSongFromPrevQueue()
        {
            return currentQueue.getPrevSong();
        }
        public void switchPlaylist(Playlist next_playlist)
        {
            currentQueue = playlistQueues[next_playlist.getName()];
        }
        /*public void writeCurrentPlaylistToStore()
        {
	    playlistQueues[currentQueue.playlist] = new Queue(currentQueue);
        }*/
        internal Queue getCurrentQueue()
        {
            return currentQueue;
        }
    }
}
