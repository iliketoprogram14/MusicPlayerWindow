namespace MusicPlayerWindow
{
    partial class MainWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.playButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.playlistBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.songLabel = new System.Windows.Forms.Label();
            this.artistAlbumLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(58, 12);
            this.playButton.MinimumSize = new System.Drawing.Size(60, 60);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(60, 60);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(124, 12);
            this.stopButton.MinimumSize = new System.Drawing.Size(60, 60);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(60, 60);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Enabled = false;
            this.prevButton.Location = new System.Drawing.Point(13, 22);
            this.prevButton.MaximumSize = new System.Drawing.Size(40, 40);
            this.prevButton.MinimumSize = new System.Drawing.Size(40, 40);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(40, 40);
            this.prevButton.TabIndex = 3;
            this.prevButton.Text = "Prev";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Enabled = false;
            this.nextButton.Location = new System.Drawing.Point(190, 22);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(40, 40);
            this.nextButton.TabIndex = 4;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // volumeBar
            // 
            this.volumeBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.volumeBar.Location = new System.Drawing.Point(13, 74);
            this.volumeBar.Maximum = 100;
            this.volumeBar.MaximumSize = new System.Drawing.Size(220, 30);
            this.volumeBar.MinimumSize = new System.Drawing.Size(215, 30);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(220, 30);
            this.volumeBar.SmallChange = 5;
            this.volumeBar.TabIndex = 3;
            this.volumeBar.TickFrequency = 10;
            this.volumeBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeBar.Value = 100;
            this.volumeBar.Scroll += new System.EventHandler(this.volumeBar_Scroll);
            // 
            // playlistBox
            // 
            this.playlistBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.playlistBox.FormattingEnabled = true;
            this.playlistBox.Location = new System.Drawing.Point(0, 62);
            this.playlistBox.Name = "playlistBox";
            this.playlistBox.Size = new System.Drawing.Size(200, 21);
            this.playlistBox.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.artistAlbumLabel);
            this.panel1.Controls.Add(this.songLabel);
            this.panel1.Controls.Add(this.playlistBox);
            this.panel1.Location = new System.Drawing.Point(244, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 83);
            this.panel1.TabIndex = 6;
            // 
            // songLabel
            // 
            this.songLabel.AutoSize = true;
            this.songLabel.Location = new System.Drawing.Point(10, 10);
            this.songLabel.MinimumSize = new System.Drawing.Size(180, 0);
            this.songLabel.Name = "songLabel";
            this.songLabel.Size = new System.Drawing.Size(180, 13);
            this.songLabel.TabIndex = 6;
            this.songLabel.Text = "Song goes here";
            this.songLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // artistAlbumLabel
            // 
            this.artistAlbumLabel.AutoSize = true;
            this.artistAlbumLabel.Location = new System.Drawing.Point(10, 40);
            this.artistAlbumLabel.MinimumSize = new System.Drawing.Size(180, 10);
            this.artistAlbumLabel.Name = "artistAlbumLabel";
            this.artistAlbumLabel.Size = new System.Drawing.Size(180, 13);
            this.artistAlbumLabel.TabIndex = 7;
            this.artistAlbumLabel.Text = "Artist - Album";
            this.artistAlbumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 113);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.volumeBar);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playButton);
            this.Name = "MainWindow";
            this.Text = "Music Shuffler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.ComboBox playlistBox;

        private void InitPlaylistBox()
        {
            this.playlistBox.Items.AddRange(loader.getPlaylistNames().ToArray());
            this.playlistBox.SelectedItem = "Music";
            this.playlistBox.SelectedValueChanged += new System.EventHandler(this.playlistBox_SelectedValueChanged);
            playlistBoxLastIndex = playlistBox.SelectedIndex;
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label artistAlbumLabel;
        private System.Windows.Forms.Label songLabel;
    }
}

