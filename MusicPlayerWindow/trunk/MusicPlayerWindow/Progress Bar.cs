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
    /// <summary>
    /// A form that consists of a progress bar and an information label
    /// </summary>
    public partial class Progress_Bar : Form
    {
        /// <summary>
        /// Constructor for the Progress_Bar form; initializes the progress bar and the information label
        /// </summary>
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
            infoLabel.Text = msg;
        }

        /// <summary>
        /// Sets the value of the progress bar in the Progress_Bar form
        /// </summary>
        /// <param name="value">The new value of the progress bar</param>
        public void setValue(int value)
        {
            progressBar.Value = value;
        }
    }
}
