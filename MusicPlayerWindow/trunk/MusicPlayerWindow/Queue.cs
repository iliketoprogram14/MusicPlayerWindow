using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Holds a playlist and its next and previous song queues
    /// </summary>
    public class Queue
    {
        /// <summary>the playlist for which this queue was created</summary>
        public Playlist playlist;
        /// <summary>the next song queue for the playlist</summary>
        public List<Song> nextSongQueue;
        /// <summary>the previous song queue for the playlist</summary>
        public List<Song> prevSongQueue;
        /// <summary>the number of songs the previous song queue can hold</summary>
        public int prevSongLimit;

        #region Constructors
        /// <summary>
        /// Constructor makes next song queue and previous song queue given playlist
        /// </summary>
        /// <param name="playlist">the playlist the queue is based upon</param>
        /// <param name="songLimit">the maximum number of songs the previous song queue will remember</param>
        public Queue(Playlist playlist, int songLimit)
        {
            this.playlist = playlist;
            prevSongQueue = new List<Song>();
            nextSongQueue = new List<Song>();
            prevSongLimit = songLimit;
        }

        /// <summary>
        /// Constructor for deep copying
        /// </summary>
        /// <param name="prevQ">the queue to be deep copied from</param>
        public Queue(Queue prevQ)
        {
            this.playlist = new Playlist(prevQ.playlist.getPath(), prevQ.playlist.getLen());
            Song[] prevSongs = (Song[])prevQ.prevSongQueue.ToArray();
            foreach (Song song in prevSongs) { this.prevSongQueue.Add(song); }
            Song[] nextSongs = (Song[])prevQ.nextSongQueue.ToArray();
            foreach (Song song in nextSongs) { this.nextSongQueue.Add(song); }
            this.prevSongLimit = prevQ.prevSongLimit;
        }
        #endregion

        #region Interface
        /// <summary>
        /// Adds a song to next queue
        /// </summary>
        /// <param name="newSong">the song to be added to the next queue</param>
        public void addNextSong(Song newSong)
        {
            if (nextSongQueue.Contains(newSong)) { return; } //the next song queue already holds the given song, so don't add
            nextSongQueue.Add(newSong);
        }

        /// <summary>
        /// Adds a song to the previous queue
        /// </summary>
        /// <param name="oldSong">the song to be added to the previous queue</param>
        public void addPrevSong(Song oldSong)
        {
            if (prevSongQueue.Contains(oldSong)) { return; } //don't add the song if it's already in the queue
            if (prevSongQueue.Count == prevSongLimit) { prevSongQueue.RemoveAt(0); } //remove least recent song if over the limit
            prevSongQueue.Add(oldSong);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the song at the front of the next song queue and also removes it
        /// </summary>
        /// <returns>the next song to be played</returns>
        public Song getNextSong()
        {
            Song song = nextSongQueue[0];
            nextSongQueue.RemoveAt(0);
            return song;
        }

        /// <summary>
        /// Gets the song at the front of the previous song queue (the most recently played song) and also removes it
        /// </summary>
        /// <returns></returns>
        public Song getPrevSong()
        {
            if (prevSongQueue.Count == 0)
                return null;
            Song prevSong = prevSongQueue[prevSongQueue.Count - 1];
            prevSongQueue.RemoveAt(prevSongQueue.Count - 1);
            return prevSong;
        }

        /// <summary>
        /// Gets the path of the playlist xml file
        /// </summary>
        /// <returns>the path of the playlist xml file</returns>
        public String getPlaylistPath()
        {
            return playlist.getPath();
        }

        /// <summary>
        /// Gets the number of songs in the playlist
        /// </summary>
        /// <returns>the number of songs in the playlist</returns>
        public int getPlaylistLen()
        {
            return playlist.getLen();
        }

        /// <summary>
        /// Gets the name of the playlist
        /// </summary>
        /// <returns>the name of the playlist</returns>
        public String getPlaylistName()
        {
            return playlist.getName();
        }
        #endregion
    }
}
