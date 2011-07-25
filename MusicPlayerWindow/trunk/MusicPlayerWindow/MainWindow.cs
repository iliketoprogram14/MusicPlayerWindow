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
        private Song currentSong;
        public bool stopPressed;

        public MainWindow()
        {
            InitializeComponent();
            player = new MusicPlayer(this);
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

            
        }
        
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopPressed = true;
            player.stopSong(currentSong);
            currentSong = null;
            playButton.Text = "Play";
        }

        public Song getCurrSong()
        {
            return currentSong;
        }

        public void playNextSong()
        {
            stopPressed = false;
            player.stopSong(currentSong);
            player.getNextSong(currentSong);
            player.playCurrSong(currentSong);
            //update next queue
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            playNextSong();
        }
    }
}
