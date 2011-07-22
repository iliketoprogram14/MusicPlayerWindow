using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BandObjectLib;
using System.Runtime.InteropServices;

namespace MusicPlayerBand
{
    [Guid("AE07101B-46D4-4a98-AF68-0333EA26E113")]//or generate my own guid using guidgen.exe...
    [BandObject("Music Player Bar", BandObjectStyle.TaskbarToolBar | BandObjectStyle.ExplorerToolbar | BandObjectStyle.Horizontal, HelpText = "Controls to music.")]
    public partial class MainDeskBand : BandObject
    {
        //fields
        private System.Windows.Forms.Button playButton;

        //constructor
        public MainDeskBand()
        {
            InitializeComponent();
        }

        private void playButton_click(object sender, System.EventArgs e)
        {
            MessageBox.Show("ZZZZZ Hello, World!");
        }
    }
}
