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
    public partial class Progress_Bar : Form
    {
        public Progress_Bar()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Sets the information label in the Progress_Bar form
        /// </summary>
        /// <param name="msg">The message to be put in the information label</param>
        public void setLabel(String msg)
        {
            label1.Text = msg;
        }

        /// <summary>
        /// Sets the value of the progress bar in the Progress_Bar form
        /// </summary>
        /// <param name="value">The new value of the progress bar</param>
        public void setValue(int value)
        {
            progressBar1.Value = value;
        }
    }
}
