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
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.playlistBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.artistAlbumLabel = new System.Windows.Forms.Label();
            this.songLabel = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // volumeBar
            // 
            this.volumeBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.volumeBar.Location = new System.Drawing.Point(13, 74);
            this.volumeBar.Maximum = 100;
            this.volumeBar.MaximumSize = new System.Drawing.Size(220, 30);
            this.volumeBar.MinimumSize = new System.Drawing.Size(215, 30);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(220, 45);
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
            this.playlistBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistBox.ForeColor = System.Drawing.Color.DarkBlue;
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
            // artistAlbumLabel
            // 
            this.artistAlbumLabel.AutoSize = true;
            this.artistAlbumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistAlbumLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.artistAlbumLabel.Location = new System.Drawing.Point(10, 35);
            this.artistAlbumLabel.MinimumSize = new System.Drawing.Size(180, 10);
            this.artistAlbumLabel.Name = "artistAlbumLabel";
            this.artistAlbumLabel.Size = new System.Drawing.Size(180, 16);
            this.artistAlbumLabel.TabIndex = 7;
            this.artistAlbumLabel.Text = "Artist - Album";
            this.artistAlbumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // songLabel
            // 
            this.songLabel.AutoSize = true;
            this.songLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.songLabel.Location = new System.Drawing.Point(10, 10);
            this.songLabel.MinimumSize = new System.Drawing.Size(180, 0);
            this.songLabel.Name = "songLabel";
            this.songLabel.Size = new System.Drawing.Size(180, 16);
            this.songLabel.TabIndex = 6;
            this.songLabel.Text = "Song goes here";
            this.songLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.Enabled = false;
            this.nextButton.Image = global::MusicPlayerWindow.Properties.Resources.Small_Glass_Forward;
            this.nextButton.Location = new System.Drawing.Point(190, 22);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(40, 40);
            this.nextButton.TabIndex = 4;
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            this.nextButton.Paint += new System.Windows.Forms.PaintEventHandler(this.nextButton_Paint);
            // 
            // prevButton
            // 
            this.prevButton.BackColor = System.Drawing.Color.Transparent;
            this.prevButton.Enabled = false;
            this.prevButton.Image = global::MusicPlayerWindow.Properties.Resources.Small_Glass_Previous;
            this.prevButton.Location = new System.Drawing.Point(13, 22);
            this.prevButton.MaximumSize = new System.Drawing.Size(40, 40);
            this.prevButton.MinimumSize = new System.Drawing.Size(40, 40);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(40, 40);
            this.prevButton.TabIndex = 3;
            this.prevButton.UseVisualStyleBackColor = false;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            this.prevButton.Paint += new System.Windows.Forms.PaintEventHandler(this.prevButton_Paint);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Image = global::MusicPlayerWindow.Properties.Resources.Small_Glass_Stop;
            this.stopButton.Location = new System.Drawing.Point(124, 12);
            this.stopButton.MinimumSize = new System.Drawing.Size(60, 60);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(60, 60);
            this.stopButton.TabIndex = 2;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            this.stopButton.Paint += new System.Windows.Forms.PaintEventHandler(this.stopButton_Paint);
            // 
            // playButton
            // 
            this.playButton.BackColor = System.Drawing.Color.Transparent;
            this.playButton.Image = global::MusicPlayerWindow.Properties.Resources.Small_Glass_Play;
            this.playButton.Location = new System.Drawing.Point(58, 12);
            this.playButton.MinimumSize = new System.Drawing.Size(60, 60);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(60, 60);
            this.playButton.TabIndex = 1;
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            this.playButton.Paint += new System.Windows.Forms.PaintEventHandler(this.playButton_Paint);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
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

        private void playButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            button_Paint(playButton, e);
        }

        private void stopButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            button_Paint(stopButton, e);
        }

        private void prevButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            button_Paint(prevButton, e);
        }

        private void nextButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            button_Paint(nextButton, e);
        }

        private void button_Paint(System.Windows.Forms.Button button, System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();

            // Set a new rectangle to the same size as the button's 
            // ClientRectangle property.
            System.Drawing.Rectangle newRectangle = button.ClientRectangle;

            // Decrease the size of the rectangle.
            newRectangle.Inflate(-4, -4);

            // Draw the button's border.
            e.Graphics.DrawEllipse(System.Drawing.Pens.Transparent, newRectangle);

            // Increase the size of the rectangle to include the border.
            newRectangle.Inflate(1, 1);

            // Create a circle within the new rectangle.
            buttonPath.AddEllipse(newRectangle);

            // Set the button's Region property to the newly created 
            // circle region.
            button.Region = new System.Drawing.Region(buttonPath);
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label artistAlbumLabel;
        private System.Windows.Forms.Label songLabel;
    }
}

