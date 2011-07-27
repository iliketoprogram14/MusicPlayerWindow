using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IrrKlang;
using System.Diagnostics;

namespace MusicPlayerWindow
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private MusicPlayer player;
        private CustomMusicLoader loader;
        private Song currentSong;
	    public static String outputDir = @"..\..\Playlists";
        public String libLocation;
        private int playlistBoxLastIndex;

        public MainWindow()
        {
            libLocation = @"D:\Music\iTunes";
            player = new MusicPlayer(this);
            String[] wtf = System.IO.Directory.GetFiles(outputDir);
            if (wtf.Length == 0)
            {
                getiTunesSongs(libLocation);
                parseiTunesSongs(libLocation, outputDir);
            }
            loader = new CustomMusicLoader(outputDir); //UNCOMMENT WHEN HAVE M3U FILES
            InitializeComponent();
            currentSong = null;
            playlistBoxLastIndex = playlistBox.SelectedIndex;
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }

        //plays first song, and then pauses/unpauses song
        private void playButton_Click(object sender, EventArgs e)
        {
            if (currentSong == null) {
                currentSong = loader.getNextSong();
                player.playCurrSong(currentSong);
            }
            else {
                player.pauseUnpauseSong(currentSong);
            }
            if (currentSong.getSound().Paused == true)
                playButton.Text = "Play";
            else
                playButton.Text = "Pause";
            nextButton.Enabled = true;
            prevButton.Enabled = true;
            stopButton.Enabled = true;
        }
        
        private void stopButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);
            resetEngine();
        }

        private void resetEngine()
        {
            currentSong = null;
            playButton.Text = "Play";
            nextButton.Enabled = false;
            prevButton.Enabled = false;
            stopButton.Enabled = false;
        }

        public Song getCurrSong()
        {
            return currentSong;
        }

        public void playNextSong()
        {
            if (currentSong != null) { player.stopSong(currentSong); }
            currentSong = loader.getNextSong();
            player.playCurrSong(currentSong);
            loader.updateNextQueue();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            playNextSong();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);
            Song oldSong = currentSong;
            currentSong = loader.getPrevSong();
            if (currentSong == null) { resetEngine(); return; }
            if (oldSong.Equals(currentSong)) { stopButton_Click(this, null); return; }
            player.playCurrSong(currentSong);
            loader.addSongToNextQueueFront(oldSong);
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            if (currentSong != null)
            {
                currentSong.getSound().Volume = volumeBar.Value / 100.0f;
            }
        }

        public int getVolume()
        {
            return volumeBar.Value;
        }
        
        ///<summary>
        /// Executes a process and waits for it to end. 
        ///</summary>
        ///<param name="cmd">Full Path of process to execute.</param>
        ///<param name="cmdParams">Command Line params of process</param>
        ///<param name="workingDirectory">Process' working directory</param>
        ///<param name="timeout">Time to wait for process to end</param>
        ///<param name="stdOutput">Redirected standard output of process</param>
        ///<returns>Process exit code</returns>
        private int ExecuteProcess(string cmd, string cmdParams, string workingDirectory, int timeout, out string stdOutput)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(cmd, cmdParams);
            pInfo.WorkingDirectory = workingDirectory;
            pInfo.UseShellExecute = false;
            pInfo.RedirectStandardOutput = true;
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(pInfo);
            stdOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit(timeout);
            return p.ExitCode;
        }

        private int getiTunesSongs(String libLoc)
        {
            String outputString;
            int timeout = 10000;
            System.IO.Directory.CreateDirectory(outputDir);
            String cmdArgs = String.Format("-jar ..\\..\\itunesexport.jar -playlistType=M3U -library=\"{0}\\iTunes Music Library.xml\" -outputDir=\"{1}\" -includeBuiltInPlaylists",
                libLoc, outputDir);
            int exitCode = ExecuteProcess(@"C:\Program Files (x86)\Java\jre6\bin\java.exe",
                cmdArgs,
                System.IO.Directory.GetCurrentDirectory(),
                timeout,
                out outputString);
            return exitCode;
        }

        private void parseiTunesSongs(String libLoc, String outputDir)
        {
            String[] playlistPaths = System.IO.Directory.GetFiles(outputDir);
            foreach (String file in playlistPaths)
            {
                System.IO.TextReader tr = new System.IO.StreamReader(file);
                String file_to_write = String.Format("{0}xml", file.TrimEnd('m', '3', 'u'));
                System.IO.TextWriter w = new System.IO.StreamWriter(file_to_write);
                int count = 0;
                String line;
                while ((line = tr.ReadLine()) != null)
                {
                    if (count == 0)
                    {
                        w.WriteLine("<?xml version='1.0' ?>");
                        w.WriteLine("<!DOCTYPE playlist [");
                        w.WriteLine("  <!ELEMENT playlist (song)>");
                        w.WriteLine("  <!ELEMENT song     (#PCDATA)>");
                        w.WriteLine("  <!ATTLIST song id ID #REQUIRED>");
                        w.WriteLine("]>");
                        w.WriteLine("<playlist>");
                        count += 7;
                        continue;
                    }
                    //if extinfo, skip line
                    line = line.Replace("&", "&amp;");
                    line = line.Replace(@"C:\Users\Randy", "D:");
                    line = line.Replace(@"iTunes Music\", "");
                    String line_to_write = String.Format("    <song id=\'{0:000000}\'>{1}</song>", count, line);
                    w.WriteLine(line_to_write);
                    count++;
                }
                w.WriteLine("</playlist>");
                tr.Close();
                w.Close();
                System.IO.File.Delete(file);
            }

        }

        private void playlistBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex == playlistBoxLastIndex) { return; }
            playlistBoxLastIndex = playlistBox.SelectedIndex;
            String playlistToPlay = playlistBox.SelectedItem.ToString();
            loader.switchToPlaylist(playlistToPlay, currentSong);
            playNextSong();
        }
        
        public void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            player.destroyPlayer();
            loader.destroyStoreAndExit();
            this.Dispose();
            Application.Exit();
        }

    }
}
