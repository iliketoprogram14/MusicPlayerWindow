using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    interface MusicLoaderInterface
    {
        void updateNextQueue();
        void updatePrevQueue(Song song);
        void switchToPlaylist(Playlist newPlaylist);
        //void writePlaylistToStore(Playlist playlist);
        Song getNextSong();
        Song getPrevSong();
    }
}
