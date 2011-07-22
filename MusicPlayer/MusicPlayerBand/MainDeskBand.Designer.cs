namespace MusicPlayerBand
{
    partial class MainDeskBand
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            //set up
            this.playButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //b1
            this.playButton.Anchor = (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom) |
                System.Windows.Forms.AnchorStyles.Left) |
                System.Windows.Forms.AnchorStyles.Right);
            this.playButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.playButton.ForeColor = System.Drawing.SystemColors.Info;
            this.playButton.Name = "button1";
            this.playButton.Size = new System.Drawing.Size(150, 24);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            this.playButton.Click += new System.EventHandler(this.playButton_click);

            //Music Player Bar
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.playButton });
            this.MinSize = new System.Drawing.Size(150, 24);
            this.Name = "Music Player Bar";
            this.Size = new System.Drawing.Size(150, 24);
            this.Title = "";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
