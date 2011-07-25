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

        public MainWindow()
        {
            InitializeComponent();
            player = new MusicPlayer();
            currentSong = null;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (currentSong == null) {
                currentSong = new Song("D:/Music/iTunes/Kyuss/Wretch/11 Stage III.mp3");
                player.playCurrSong(currentSong);
            }
            else {
                currentSong.getSound().Paused = !currentSong.getSound().Paused;
            }
            if (currentSong.getSound().Paused == true)
                playButton.Text = "Play";
            else
                playButton.Text = "Pause";

            
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            player.stopSong();
            currentSong = null;
            playButton.Text = "Play";
        }
    }
}
