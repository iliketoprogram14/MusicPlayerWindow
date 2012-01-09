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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void setLabel(String msg)
        {
            label1.Text = msg;
        }

        public void setValue(int value)
        {
            progressBar1.Value = value;
        }
    }
}
