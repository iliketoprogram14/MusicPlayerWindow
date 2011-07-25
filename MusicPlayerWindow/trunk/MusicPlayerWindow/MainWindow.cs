using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IrrKlang;

namespace MusicPlayerWindow
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private MusicPlayer player;
        private CustomMusicLoader loader;
        private Song currentSong;
        public bool stopPressed;

        public MainWindow()
        {
            InitializeComponent();
            player = new MusicPlayer(this);
            loader = new CustomMusicLoader();
            currentSong = null;
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }

        //plays first song, and then pauses/unpauses song
        private void playButton_Click(object sender, EventArgs e)
        {
            if (currentSong == null) {
                currentSong = new Song("D:/Music/iTunes/Kyuss/Wretch/11 Stage III.mp3");
                player.playCurrSong(currentSong);
            }
            else {
                player.pauseUnpauseSong(currentSong);
            }
            if (currentSong.getSound().Paused == true)
                playButton.Text = "Play";
            else
                playButton.Text = "Pause";
            nextButton.Enabled = true;
            prevButton.Enabled = true;
            stopButton.Enabled = true;
        }
        
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopPressed = true;
            player.stopSong(currentSong);
            currentSong = null;
            playButton.Text = "Play";
            nextButton.Enabled = false;
            prevButton.Enabled = false;
            stopButton.Enabled = false;
        }

        public Song getCurrSong()
        {
            return currentSong;
        }

        public void playNextSong()
        {
            stopPressed = false;
            player.stopSong(currentSong);
            currentSong = loader.getNextSong();
            player.playCurrSong(currentSong);
            loader.updateNextQueue();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            playNextSong();
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            if (currentSong != null)
            {
                currentSong.getSound().Volume = volumeBar.Value / 100.0f;
            }
        }

        public int getVolume()
        {
            return volumeBar.Value;
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            stopPressed = false;
            player.stopSong(currentSong);
            Song oldSong = currentSong;
            currentSong = loader.getPrevSong();
            player.playCurrSong(currentSong);
            loader.updatePrevQueue(oldSong);
        }
    }
}
