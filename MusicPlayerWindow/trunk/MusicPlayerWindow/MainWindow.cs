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
using System.Threading;

namespace MusicPlayerWindow
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private class LabelBool {
            private bool boolean;
            public LabelBool(bool val) { boolean = val; }
            public void setBoolean(bool val) { boolean = val; }
            public bool getBoolean() { return boolean; }
        }

        #region Static Fields
        public static String outputDir = @"Playlists";
        private static String matchPattern = @"^\s*\d+[\t ]+";
        private static String matchPattern2 = @"^\s*\d-\d+[\t ]+";
        #endregion

        private MusicPlayer player;
        protected CustomMusicLoader loader;
        private Song currentSong;
        public String libLocation;
        private int playlistBoxLastIndex;
        Thread scrollThread;
        private LabelBool labelHasChanged;

        #region Constructor and Main()
        public MainWindow()
        {
            player = new MusicPlayer(this);
            if (!System.IO.Directory.Exists(outputDir) || System.IO.Directory.GetFiles(outputDir).Length == 0)
            {
                getLibLocation();
                getiTunesSongs();
                parseiTunesSongs(outputDir);
            }
            loader = new CustomMusicLoader(outputDir);
            InitializeComponent();
            InitPlaylistBox();
            currentSong = null;
            Control.CheckForIllegalCrossThreadCalls = false;
            labelHasChanged = new LabelBool(true);
            scrollThread = new Thread(new ThreadStart(ScrollText));
        }
        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }
        #endregion

        #region Event Handlers
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
            switchImagesPlayPause();
            nextButton.Enabled = true;
            prevButton.Enabled = true;
            stopButton.Enabled = true;
            updateLabels();
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);
            resetEngine();
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            Song oldSong = currentSong;
            playNextSong();
            loader.updatePrevQueue(oldSong);
        }
        private void prevButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);
            Song oldSong = currentSong;
            currentSong = loader.getPrevSong();
            if (currentSong == null) { resetEngine(); return; } //prevQueue is empty, so stop playing
            player.playCurrSong(currentSong);
            loader.addSongToNextQueueFront(oldSong);
            updateLabels();
        }
        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            if (currentSong != null)
                currentSong.getSound().Volume = volumeBar.Value / 100.0f;
        }
        private void playlistBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex == playlistBoxLastIndex) { return; }
            playlistBoxLastIndex = playlistBox.SelectedIndex;
            String playlistToPlay = playlistBox.SelectedItem.ToString();
            loader.switchToPlaylist(playlistToPlay, currentSong);
            playNextSong();
            if (stopButton.Enabled == false)
            {
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Pause_Black;
                stopButton.Enabled = true;
                nextButton.Enabled = true;
                prevButton.Enabled = true;
            }
        }
        public void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            player.destroyPlayer();
            loader.destroyStoreAndExit();
            this.Dispose();
            Application.Exit();
        }
        #endregion

        #region Accessors
        public Song getCurrSong()
        {
            return currentSong;
        }
        public int getVolume()
        {
            return volumeBar.Value;
        }
        #endregion
        
        #region Helpers
        private void resetEngine()
        {
            currentSong = null;
            playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Play_Black;
            nextButton.Enabled = false;
            prevButton.Enabled = false;
            stopButton.Enabled = false;
        }
        private void switchImagesPlayPause()
        {
            if (currentSong.getSound().Paused == true)
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Play_Black;
            else
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Pause_Black;
        }
        private void updateLabels()
        {
            String[] fields = currentSong.getPath().Split('\\');

            String nameFiltered = fields[fields.Length - 1].Replace(".mp3", "").Replace(".aac", "").Replace(".mp4", "");
            String nameTmp2 = System.Text.RegularExpressions.Regex.Replace(nameFiltered, matchPattern, "");
            String name = System.Text.RegularExpressions.Regex.Replace(nameTmp2, matchPattern2, "");

            String album = fields[fields.Length - 2];
            String artist = fields[fields.Length - 3];

            songLabel.Text = name;
            Monitor.Enter(labelHasChanged);
            System.Threading.Monitor.Enter(artistAlbumLabel);
            artistAlbumLabel.Text = artist + " - " + album;
            artistAlbumLabel.Refresh();
            Monitor.Exit(artistAlbumLabel);
            labelHasChanged.setBoolean(true);
            Monitor.Pulse(labelHasChanged);
            Monitor.Exit(labelHasChanged);
        }
        private void ScrollText()
        {
            int textLen;
            int sizeLen = 28;
            String labelString;
            System.Text.StringBuilder sb = null;

            while (true)
            {
                Monitor.Enter(labelHasChanged);
                if (labelHasChanged.getBoolean())
                {
                    labelHasChanged.setBoolean(false);
                    Monitor.Enter(artistAlbumLabel);
                    labelString = artistAlbumLabel.Text;
                    Monitor.Exit(artistAlbumLabel);
                    textLen = labelString.Length;
                    if (sizeLen - textLen > 0)
                    {
                        Monitor.Wait(labelHasChanged);
                        Monitor.Exit(labelHasChanged);
                        continue;
                    }
                    else
                    {
                        sb = null;
                        sb = new System.Text.StringBuilder(labelString+"        ");
                    }
                }
                Monitor.Exit(labelHasChanged);
                Thread.Sleep(500);

                char ch = sb[0];
                sb.Remove(0, 1);
                sb.Insert(sb.Length - 1, ch);

                Monitor.Enter(artistAlbumLabel);
                artistAlbumLabel.Text = sb.ToString();
                artistAlbumLabel.Refresh();
                Monitor.Exit(artistAlbumLabel);
            }
        }
        #endregion

        #region Public Methods/Interface
        public void playNextSong()
        {
            if (currentSong != null) { player.stopSong(currentSong); }
            currentSong = loader.getNextSong();
            player.playCurrSong(currentSong);
            loader.updateNextQueue();
            switchImagesPlayPause();
            updateLabels();
        }
        #endregion

        #region Library-related Methods
        ///<summary>
        /// Executes a process and waits for it to end. 
        ///</summary>
        ///<param name="cmd">Full Path of process to execute.</param>
        ///<param name="cmdParams">Command Line params of process</param>
        ///<param name="workingDirectory">Process' working directory</param>
        ///<param name="timeout">Time to wait for process to end</param>
        ///<param name="stdOutput">Redirected standard output of process</param>
        ///<returns>Process exit code</returns>
        private int ExecuteProcess(string cmd, string cmdParams, string workingDirectory, int timeout)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(cmd, cmdParams);
            pInfo.WorkingDirectory = workingDirectory;
            pInfo.UseShellExecute = false;
            pInfo.RedirectStandardOutput = true;
            pInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process p = Process.Start(pInfo);
            String stdOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit(timeout);
            return p.ExitCode;
        }
        private void ExecuteThread()
        {
            int timeout = 30000;
            System.IO.Directory.CreateDirectory(outputDir);
            String cmdArgs = String.Format("-mx1024m -jar ..\\..\\itunesexport.jar -playlistType=M3U -library=\"{0}\\..\\iTunes Music Library.xml\" -outputDir=\"{1}\" -includeBuiltInPlaylists -excludePlaylist=\"iTunes DJ, Movies, iTunes U, Purchased on iPod touch, Purchased, Purchased on iPhone, Podcasts, Recently Played\"",
                libLocation, outputDir); 
            int exitcode = ExecuteProcess(@"C:\Program Files (x86)\Java\jre6\bin\java.exe",
                cmdArgs,
                System.IO.Directory.GetCurrentDirectory(),
                timeout);
        }
        private void getiTunesSongs()
        {
            Thread t = new Thread(new ThreadStart(ExecuteThread));
            t.Start();
            MessageBox.Show("The application is importing your iTunes playlists right now.  " +
                "When the black command line window disappears, the application has finished importing your songs and the shuffler will launch.  " +
                "Click OK to continue.",
                "Importing your iTunes songs right now",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            t.Join(30000);
        }
        private void parseiTunesSongs(String outputDir)
        {
            String[] playlistPaths = System.IO.Directory.GetFiles(outputDir);
            foreach (String file in playlistPaths)
            {
                System.IO.TextReader tr = new System.IO.StreamReader(file);
                String tmp = tr.ReadLine();

                tmp = tmp.Replace("#Playlist: '","").Replace("' exported by iTunesExport-Scala v2.2.2 http://www.ericdaugherty.com/dev/itunesexport/scala/", "");
                String file_to_write = tmp.Replace("/","_");
                file_to_write = outputDir + "/" + file_to_write + ".xml";

                System.IO.TextWriter w = new System.IO.StreamWriter(file_to_write);
                w.WriteLine("<?xml version='1.0' ?>");
                w.WriteLine("<!DOCTYPE playlist [");
                w.WriteLine("  <!ELEMENT playlist (song)>");
                w.WriteLine("  <!ELEMENT song     (#PCDATA)>");
                w.WriteLine("  <!ATTLIST song id ID #REQUIRED>");
                w.WriteLine("]>");
                w.WriteLine("<playlist>");
                String line;
                int count = 0;
                while ((line = tr.ReadLine()) != null)
                {
                    line = line.Replace("&", "&amp;");
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
        private void getLibLocation()
        {
            MessageBox.Show("Please specify the location of your iTunes Music folder in order to import your playlists.\n\n" +
                "For example, C:/Users/YOUR_NAME/Music/iTunes/Music would work.\n\n" +
                "The application assumes that all your iTunes music is in the same place. Click OK to continue.", "Specify iTunes Music Location",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            FolderBrowserDialog browser;
            bool shouldExit = false;
            while (true)
            {
                browser = new FolderBrowserDialog();
                browser.Description = "Please specify the location of your iTunes Music library (ie C:/....../iTunes/Music).\n";
                DialogResult result = browser.ShowDialog();
                if (result == DialogResult.OK) { break; }
                DialogResult retry = MessageBox.Show("You have cancelled specifying your iTunes library.  The application cannot complete setup without importing the iTunes playlists.\n\n" +
                    "If you do not have an iTunes Music folder, exit setup, open iTunes, "+
                    "and under Edit->Preferences->Advanced, check \"Keep iTunes Media Folder organized\" and \" Copy files to iTunes Media Folder when adding to library\".\n\n" +
                    "Click OK to try specifying the folder again.  To exit setup, please click Cancel",
                    "Specify iTunes Music Location", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (retry == DialogResult.Cancel) { shouldExit = true; break;  }
            }
            if (shouldExit) { this.Close(); System.Environment.Exit(1); return; }
            libLocation = browser.SelectedPath;
        }
        #endregion
    }
}
