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
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(65, 27);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(53, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(124, 27);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(39, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Location = new System.Drawing.Point(13, 27);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(46, 23);
            this.prevButton.TabIndex = 3;
            this.prevButton.Text = "Prev";
            this.prevButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(169, 27);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(45, 23);
            this.nextButton.TabIndex = 4;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playButton);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button nextButton;
    }
}

