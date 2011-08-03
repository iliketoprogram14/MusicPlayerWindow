using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.playlistBox = new System.Windows.Forms.ComboBox();
            this.labelPanel = new System.Windows.Forms.Panel();
            this.artistAlbumLabel = new System.Windows.Forms.Label();
            this.songLabel = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.labelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // volumeBar
            // 
            this.volumeBar.AutoSize = false;
            this.volumeBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.volumeBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.volumeBar.Location = new System.Drawing.Point(10, 70);
            this.volumeBar.Maximum = 100;
            this.volumeBar.MaximumSize = new System.Drawing.Size(180, 20);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(180, 20);
            this.volumeBar.SmallChange = 5;
            this.volumeBar.TabIndex = 3;
            this.volumeBar.TickFrequency = 10;
            this.volumeBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeBar.Value = 100;
            this.volumeBar.Scroll += new System.EventHandler(this.volumeBar_Scroll);
            // 
            // playlistBox
            // 
            this.playlistBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.playlistBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.playlistBox.ForeColor = System.Drawing.Color.White;
            this.playlistBox.FormattingEnabled = true;
            this.playlistBox.Location = new System.Drawing.Point(200, 70);
            this.playlistBox.Name = "playlistBox";
            this.playlistBox.Size = new System.Drawing.Size(200, 21);
            this.playlistBox.TabIndex = 5;
            // 
            // labelPanel
            // 
            this.labelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPanel.Controls.Add(this.artistAlbumLabel);
            this.labelPanel.Controls.Add(this.songLabel);
            this.labelPanel.Location = new System.Drawing.Point(200, 10);
            this.labelPanel.Name = "labelPanel";
            this.labelPanel.Size = new System.Drawing.Size(200, 60);
            this.labelPanel.TabIndex = 6;
            // 
            // artistAlbumLabel
            // 
            this.artistAlbumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistAlbumLabel.ForeColor = System.Drawing.Color.White;
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
            this.songLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songLabel.ForeColor = System.Drawing.Color.White;
            this.songLabel.Location = new System.Drawing.Point(5, 10);
            this.songLabel.MinimumSize = new System.Drawing.Size(190, 0);
            this.songLabel.Name = "songLabel";
            this.songLabel.Size = new System.Drawing.Size(190, 16);
            this.songLabel.TabIndex = 6;
            this.songLabel.Text = "Song";
            this.songLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.Enabled = false;
            this.nextButton.Image = ((System.Drawing.Image)(resources.GetObject("nextButton.Image")));
            this.nextButton.Location = new System.Drawing.Point(150, 20);
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
            this.prevButton.Image = ((System.Drawing.Image)(resources.GetObject("prevButton.Image")));
            this.prevButton.Location = new System.Drawing.Point(10, 20);
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
            this.stopButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.stopButton.Image = global::MusicPlayerWindow.Properties.Resources.Small_Glass_Stop_Black40;
            this.stopButton.Location = new System.Drawing.Point(110, 20);
            this.stopButton.MinimumSize = new System.Drawing.Size(40, 40);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(40, 40);
            this.stopButton.TabIndex = 2;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            this.stopButton.Paint += new System.Windows.Forms.PaintEventHandler(this.stopButton_Paint);
            // 
            // playButton
            // 
            this.playButton.BackColor = System.Drawing.Color.Transparent;
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.Location = new System.Drawing.Point(50, 10);
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
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(414, 102);
            this.Controls.Add(this.labelPanel);
            this.Controls.Add(this.volumeBar);
            this.Controls.Add(this.playlistBox);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Playlist Shuffler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWindow_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            this.labelPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.ComboBox playlistBox;
        private ThumbnailToolBarButton thumbButtonPrev;
        private ThumbnailToolBarButton thumbButtonNext;
        private ThumbnailToolBarButton thumbButtonStop;
        private ThumbnailToolBarButton thumbButtonPlay;

        private void InitThumbnailToolbar()
        {
            // Create our Thumbnail toolbar buttons for the Browser doc
            thumbButtonPrev = new ThumbnailToolBarButton(Properties.Resources.PrevIconWhite, "Previous Song");
            thumbButtonPrev.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(thumbButtonPrev_Click);
            thumbButtonNext = new ThumbnailToolBarButton(Properties.Resources.NextIconWhite, "Next Song");
            thumbButtonNext.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(thumbButtonNext_Click);
            thumbButtonStop = new ThumbnailToolBarButton(Properties.Resources.StopIconWhite, "Stop");
            thumbButtonStop.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(thumbButtonStop_Click);
            thumbButtonPlay = new ThumbnailToolBarButton(Properties.Resources.PlayIconWhite, "Play");
            thumbButtonPlay.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(thumbButtonPlay_Click);

            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(this.Handle, thumbButtonPrev, thumbButtonPlay, thumbButtonStop, thumbButtonNext);
            toggleButtons(prevButton.Enabled, playButton.Enabled, stopButton.Enabled, nextButton.Enabled);
        }

        void thumbButtonPrev_Click(object sender, EventArgs e)
        {
            prevButton_Click(sender, e);
        }
        void thumbButtonNext_Click(object sender, EventArgs e)
        {
            nextButton_Click(sender, e);
        }
        void thumbButtonPlay_Click(object sender, EventArgs e)
        {
            playButton_Click(sender, e);
        }
        void thumbButtonStop_Click(object sender, EventArgs e)
        {
            stopButton_Click(sender, e);
        }

        private void InitPlaylistBox()
        {
            this.playlistBox.Items.AddRange(loader.getPlaylistNames().ToArray());
            this.playlistBox.SelectedItem = "Music";
            this.playlistBox.SelectedValueChanged += new System.EventHandler(this.playlistBox_SelectedValueChanged);
            playlistBoxLastIndex = playlistBox.SelectedIndex;
        }

        #region Paint Methods
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
        
        private void MainWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Black, Color.FromArgb(40, 40, 40), 270F))
            { e.Graphics.FillRectangle(brush, rect); }
        }
        #endregion

        private System.Windows.Forms.Panel labelPanel;
        private System.Windows.Forms.Label artistAlbumLabel;
        private System.Windows.Forms.Label songLabel;
        private System.Windows.Forms.Timer timer1;
    }
}

