using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    /// <summary>
    /// Internal song representation; includes the path to the song and an irrKlang sound
    /// </summary>
    public class Song
    {
        private String path;
        private Sound sound;

        #region Constructors
        ///<summary>
        ///Normal constructor; song is null until set
        ///</summary>
        ///<param name="path">the path to the song for playback</param>
        public Song(String path)
        {
            this.path = path;
        }

        ///<summary>
        ///For deep copying
        ///</summary>
        ///<param name="prevSong">the song that is deep copied</param>
        public Song(Song prevSong)
        {
            this.path = prevSong.getPath();
            this.sound = prevSong.getSound();
        }
        #endregion

        #region Interface
        ///<summary>
        ///Changes path of song
        ///</summary>
        ///<param name="newPath">the path to the song; overwrites old path</param>
        public void setPath(String newPath)
        {
            this.path = null;
            this.path = newPath;
        }

        ///<summary>
        ///Changes the irrKlang sound of the song
        ///</summary>
        ///<param name="sound">the irrKlang sound played that is associated with the song/path</param>
        public void setSound(Sound sound)
        {
            this.sound = null;
            this.sound = sound;
        }
        #endregion

        #region Accessors
        ///<summary>
        ///Gets path of song
        ///</summary>
        public String getPath()
        {
            return this.path;
        }

        ///<summary>
        ///Gets irrKlang sound of song
        ///</summary>
        public Sound getSound()
        {
            return this.sound;
        }
        #endregion
    }
}
