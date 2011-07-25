using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayerWindow
{
    interface MusicPlayerInterface
    {
        void playCurrSong(Song song);
        void pauseUnpauseSong(Song song);
        void stopSong(Song song);
        Song getNextSong(Song currentSong);
        Song getPrevSong(Song currentSong);
    }
}
