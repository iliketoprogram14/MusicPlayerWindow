using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MusicPlayerWindow
{
    
    public class Queue
    {
        public Playlist playlist;
        public ArrayList prevSongQueue;
        public int prevSongLimit;
        public Queue<Song> nextSongQueue;
        public Queue(Playlist playlist, int songLimit)
        {
            this.playlist = playlist;
            prevSongQueue = new ArrayList();
            nextSongQueue = new Queue<Song>();
            prevSongLimit = songLimit;
        }
	public Queue(Queue prevQ)
	{
	    this.playlist = new Playlist(prevQ.playlist.getPath(), prevQ.playlist.getLen());
        Song [] prevSongs = (Song[]) prevQ.prevSongQueue.ToArray();
        foreach (Song song in prevSongs) { this.prevSongQueue.Add(song); }
        Song[] nextSongs = (Song[]) prevQ.nextSongQueue.ToArray();
        foreach (Song song in nextSongs) { this.nextSongQueue.Enqueue(song); }
	    this.prevSongLimit = prevQ.prevSongLimit;
	}
	public void addNextSong(Song newSong)
	{
	    nextSongQueue.Enqueue(newSong);
	}
	public Song getNextSong()
	{
	    return nextSongQueue.Dequeue();
	}
	public void addPrevSong(Song oldSong)
	{
	    if (prevSongQueue.Count == prevSongLimit)
	    {
		    prevSongQueue.Remove(0); //remove least recent song
	    }
	    prevSongQueue.Add(oldSong);
	}
	public Song getPrevSong()
	{
	    return (Song)prevSongQueue[prevSongQueue.Count-1];
	}
	public String getPlaylistPath()
	{
	    return playlist.getPath();
	}
	public int getPlaylistLen()
	{
	    return playlist.getLen();
	}
    }
}
