using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicPlayerWindow
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        protected IrrKlang.ISoundEngine irrKlangEngine;
        protected IrrKlang.ISound sound;

        public Form1()
        {
            InitializeComponent();
            irrKlangEngine = new IrrKlang.ISoundEngine();
            label1.Text = "label not clicked";
            sound = irrKlangEngine.Play2D("D:/Music/iTunes/Kyuss/Wretch/11 Stage III.mp3");
            Console.Out.WriteLine(sound);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "label clicked";
            //IrrKlang.ISound sound = engine.Play2D("D:/Music/iTunes/Kyuss/Wretch/11 Stage III.mp3", true);
            sound.Paused = !sound.Paused;

        }

        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }
}
