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
        public List<Song> prevSongQueue;
        public int prevSongLimit;
        public List<Song> nextSongQueue;
        public Queue(Playlist playlist, int songLimit)
        {
            this.playlist = playlist;
            prevSongQueue = new List<Song>();
            nextSongQueue = new List<Song>();
            prevSongLimit = songLimit;
        }
	public Queue(Queue prevQ)
	{
	    this.playlist = new Playlist(prevQ.playlist.getPath(), prevQ.playlist.getLen());
        Song [] prevSongs = (Song[]) prevQ.prevSongQueue.ToArray();
        foreach (Song song in prevSongs) { this.prevSongQueue.Add(song); }
        Song[] nextSongs = (Song[]) prevQ.nextSongQueue.ToArray();
        foreach (Song song in nextSongs) { this.nextSongQueue.Add(song); }
	    this.prevSongLimit = prevQ.prevSongLimit;
	}
	public void addNextSong(Song newSong)
	{
	    nextSongQueue.Add(newSong);
	}
	public Song getNextSong()
	{
        Song song = nextSongQueue[0];
        nextSongQueue.RemoveAt(0);
        return song;
	}
	public void addPrevSong(Song oldSong)
	{
	    if (prevSongQueue.Count == prevSongLimit)
	    {
		    prevSongQueue.RemoveAt(0); //remove least recent song
	    }
	    prevSongQueue.Add(oldSong);
	}
	public Song getPrevSong()
	{
        if (prevSongQueue.Count == 0)
            return null;
        Song prevSong = prevSongQueue[prevSongQueue.Count - 1];
        prevSongQueue.RemoveAt(prevSongQueue.Count - 1);
	    return prevSong;
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
