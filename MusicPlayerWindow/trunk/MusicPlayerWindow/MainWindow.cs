using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IrrKlang;
using Un4seen.Bass;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MusicPlayerWindow
{

    /// <summary>
    /// Main class.  Holds the GUI and is responsible for delegating functionality to different objects like the music player and the music database.
    /// </summary>
    public partial class MainWindow : System.Windows.Forms.Form
    {
        #region Definitions
        /// <summary>the directory where the playlist xml files are stored</summary>
        public static String outputDir = @"Playlists";
        private static String matchPattern = @"^\s*\d+[\t ]+";
        private static String matchPattern2 = @"^\s*\d-\d+[\t ]+";

        /// <summary>
        /// For some reason, .NET doesn't think Boolean are thread-safe (which they define as thread-safe), so this is a bool wrapper that's thread-safe
        /// </summary>
        private class BoolObject {
            private bool boolean;
            public BoolObject(bool val) { boolean = val; }
            public void setBoolean(bool val) { boolean = val; }
            public bool getBoolean() { return boolean; }
        }
        #endregion

        /// <summary>the object that is responsible for updating the queues</summary>
        protected CustomMusicLoader loader;
        private MusicPlayer_Bass player;
        private Song currentSong;
        private String libLocation;        
        private int playlistBoxLastIndex;
        
        private Thread scrollThread;
        private BoolObject labelHasChanged;
        private BoolObject scrollThreadShouldExit;

        [DllImport("user32.dll", SetLastError = true)]

        /// <summary>
        /// Modifies the User Interface Privilege Isolation (UIPI) message filter for a specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose UIPI message filter is to be modified</param>
        /// <param name="message">The message that the message filter allows through or blocks</param>
        /// <param name="action">The action to be performed (allow, disallow, etc)</param>
        /// <param name="pChangeFilterStruct">Optional pointer to a CHANGEFILTERSTRUCT structure</param>
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, 
            uint action, [Optional] IntPtr str);

        #region Constructor and Main()
        /// <summary>
        /// Initializes music player, music loader thread, the GUI, and the artist/album scroller thread
        /// If the iTunes playlists haven't been imported, it calls the appropriate methods to do so
        /// </summary>
        public MainWindow()
        {            
            //import the iTunes playlists if they don't already exist
            if (!System.IO.Directory.Exists(outputDir) || System.IO.Directory.GetFiles(outputDir).Length == 0)
            {
                getLibLocation();
                //getiTunesSongs();
                parseiTunesSongs(outputDir);
            }

            //init the player and music loader thread
            player = new MusicPlayer_Bass(this);
            loader = new CustomMusicLoader(outputDir);

            //create hole in admin priveleges for messages to the taskbar
            //params: handle, WM_COMMAND, allow, null
            ChangeWindowMessageFilterEx(this.Handle, 0x0111, 1, IntPtr.Zero);

            //init the GUI
            InitializeComponent();
            InitPlaylistBox();

            currentSong = null;
            Control.CheckForIllegalCrossThreadCalls = false;
            
            //init the artist/album scroller thread and its related objects
            labelHasChanged = new BoolObject(true);
            scrollThreadShouldExit = new BoolObject(false);
            scrollThread = new Thread(new ThreadStart(ScrollText));
            scrollThread.Start();
        }

        /// <summary>
        /// Runs the application by calling the MainWindow constructor
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// The Play/Pause button has been clicked
        /// Pauses and unpauses songs (if a song is already playing) and updates the GUI
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void playButton_Click(object sender, EventArgs e)
        {
            //no song is playing, so play a new song
            if (currentSong == null) {
                currentSong = loader.getNextSong();
                player.playCurrSong(currentSong);
            }
            //a song is playing, so pause/unpause it
            else {
                player.pauseUnpauseSong(currentSong);
            }

            //update the GUI
            switchImagesPlayPause();
            toggleButtons(true, playButton.Enabled, true, true);
            updateLabels();
        }

        /// <summary>
        /// Stops the currently playing song, and resets the GUI
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void stopButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);
            resetEngine();
        }

        /// <summary>
        /// Plays the next song and adds the last song to its previous queue
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void nextButton_Click(object sender, EventArgs e)
        {
            Song oldSong = currentSong;
            playNextSong();
            loader.updatePrevQueue(oldSong);
        }

        /// <summary>
        /// Plays the last song played, or stops everything if the previous queue is empty, and then updates the GUI
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void prevButton_Click(object sender, EventArgs e)
        {
            player.stopSong(currentSong);

            Song oldSong = currentSong;
            currentSong = loader.getPrevSong();
            
            if (currentSong == null) { resetEngine(); return; } //prevQueue is empty, so stop everything

            //play the previous song and update the queues and the GUI
            player.playCurrSong(currentSong);
            loader.addSongToNextQueueFront(oldSong);
            updateLabels();
        }

        /// <summary>
        /// Adjusts the volume of the player
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            if (currentSong != null)
                currentSong.getSound().setVolume(volumeBar.Value / 100.0f);
        }

        /// <summary>
        /// The playlist has been changed, so begin shuffling songs of that playlist
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        private void playlistBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex == playlistBoxLastIndex) { return; } //same playlist has been selected

            //save the last playlist to compare against later (see previous line)
            playlistBoxLastIndex = playlistBox.SelectedIndex;

            //switch playlists and shuffle songs
            String playlistToPlay = playlistBox.SelectedItem.ToString();
            loader.switchToPlaylist(playlistToPlay, currentSong);
            playNextSong();

            //update the GUI
            if (stopButton.Enabled == false)
            {
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Pause_Black;
                toggleButtons(true, playButton.Enabled, true, true);
            }
        }

        /// <summary>
        /// Destroy the player, the music loader thread, and the artist/album scroller thread, and then the application
        /// </summary>
        /// <param name="sender">the object sending the event</param>
        /// <param name="e">the event itself</param>
        public void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            player.destroyPlayer();
            loader.destroyStoreAndExit();
            Monitor.Enter(labelHasChanged);
            scrollThreadShouldExit.setBoolean(true);
            Monitor.Pulse(labelHasChanged);
            Monitor.Exit(labelHasChanged);
            this.Dispose();
            Application.Exit();
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the currently playing song (paused or playing, but not stopped)
        /// </summary>
        /// <returns></returns>
        public Song getCurrSong()
        {
            return currentSong;
        }

        /// <summary>
        /// Gets the current volume of the trackbar
        /// </summary>
        /// <returns>the volume</returns>
        public int getVolume()
        {
            return volumeBar.Value;
        }
        #endregion
        
        #region Helpers
        /// <summary>
        /// Stops all music and resets the GUI
        /// </summary>
        private void resetEngine()
        {
            //reset the buttons
            currentSong = null;
            playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Play_Black;
            thumbButtonPlay.Icon = MusicPlayerWindow.Properties.Resources.PlayIconWhite;
            thumbButtonPlay.Tooltip = "Play";
            toggleButtons(false, playButton.Enabled, false, false);

            //reset the info pane
            songLabel.Text = "Song";
            Monitor.Enter(labelHasChanged);
            Monitor.Enter(artistAlbumLabel);
            artistAlbumLabel.Text = "Artist - Album";
            artistAlbumLabel.Refresh();
            Monitor.Exit(artistAlbumLabel);
            labelHasChanged.setBoolean(true);
            Monitor.Pulse(labelHasChanged);
            Monitor.Exit(labelHasChanged);
        }

        /// <summary>
        /// Switches the play and the pause icons on the play button
        /// </summary>
        private void switchImagesPlayPause()
        {
            if (currentSong.getSound().isPaused())
            {
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Play_Black;
                thumbButtonPlay.Icon = MusicPlayerWindow.Properties.Resources.PlayIconWhite;
                thumbButtonPlay.Tooltip = "Play";
            }
            else
            {
                playButton.Image = MusicPlayerWindow.Properties.Resources.Small_Glass_Pause_Black;
                thumbButtonPlay.Icon = MusicPlayerWindow.Properties.Resources.PauseIconWhite;
                thumbButtonPlay.Tooltip = "Pause";
            }
        }

        /// <summary>
        /// Updates the song label and the artist/album label
        /// Specifically, it parses the current song's path to get this information, and then calls the scroller thread to scroll the label
        /// </summary>
        private void updateLabels()
        {
            //parse path to get name, album, and artist
            String[] fields = currentSong.getPath().Split('/');

            String nameFiltered = fields[fields.Length - 1].Replace(".mp3", "").Replace(".aac", "").Replace(".mp4", "").Replace(".m4a", "");
            String nameTmp2 = System.Text.RegularExpressions.Regex.Replace(nameFiltered, matchPattern, "");
            String name = System.Text.RegularExpressions.Regex.Replace(nameTmp2, matchPattern2, "");

            String album = fields[fields.Length - 2];
            String artist = fields[fields.Length - 3];

            //update labels and notify scroller thread
            songLabel.Text = name;
            Monitor.Enter(labelHasChanged);
            Monitor.Enter(artistAlbumLabel);
            artistAlbumLabel.Text = artist + " - " + album;
            artistAlbumLabel.Refresh();
            Monitor.Exit(artistAlbumLabel);
            labelHasChanged.setBoolean(true);
            Monitor.Pulse(labelHasChanged);
            Monitor.Exit(labelHasChanged);
        }

        /// <summary>
        /// The artist/album scrolling happens here, executed by a dedicated thread
        /// Basically, the thread checks if the artist/album label is too long and shifts the string over and over again
        /// </summary>
        private void ScrollText()
        {
            int textLen;        //number of characters in the label
            int sizeLen = 24;   //approximate label component size in characters
            String labelString; //the text of the artist/album label
            System.Text.StringBuilder sb = null; //used to shift the characters

            while (true)
            {
                Monitor.Enter(labelHasChanged);
                if (scrollThreadShouldExit.getBoolean()) { Monitor.Exit(labelHasChanged); break; } //kill this thread

                //label has changed, so update labelString and textLen
                //if the length of the text exceeds the label size, then update the string builder and scroll the text
                //otherwise, sleep until notified that the label has been changed again
                if (labelHasChanged.getBoolean())
                {
                    labelHasChanged.setBoolean(false);
                    Monitor.Enter(artistAlbumLabel);
                    labelString = artistAlbumLabel.Text;
                    Monitor.Exit(artistAlbumLabel);
                    textLen = labelString.Length;

                    //the label's text length is smaller than the label itself, so sleep until notified that the label has changed again
                    if (sizeLen - textLen > 0)
                    {
                        Monitor.Wait(labelHasChanged);
                        Monitor.Exit(labelHasChanged);
                        continue;
                    }
                    //otherwise, update the string builder
                    else
                    {
                        sb = null;
                        sb = new System.Text.StringBuilder(labelString+"        ");
                    }
                }
                Monitor.Exit(labelHasChanged);
                Thread.Sleep(500); //this gives the scrolling effect

                //move the first character to the end of the string
                char ch = sb[0];
                sb.Remove(0, 1);
                sb.Insert(sb.Length - 1, ch);

                //udpate the label with the new string
                Monitor.Enter(labelHasChanged);
                if (labelHasChanged.getBoolean()) { Monitor.Exit(labelHasChanged); continue; }
                Monitor.Enter(artistAlbumLabel);
                artistAlbumLabel.Text = sb.ToString();
                artistAlbumLabel.Refresh();
                Monitor.Exit(artistAlbumLabel);
                Monitor.Exit(labelHasChanged);
            }
        }

        /// <summary>
        /// Toggles both the form's buttons and the thumbnail's buttons, keeping them in sync
        /// </summary>
        /// <param name="prevEnabled">the previous button's enabled status</param>
        /// <param name="playEnabled">the play button's enabled status</param>
        /// <param name="stopEnabled">the stop button's enabled status</param>
        /// <param name="nextEnabled">the next button's enabled status</param>
        private void toggleButtons(bool prevEnabled, bool playEnabled, bool stopEnabled, bool nextEnabled)
        {
            prevButton.Enabled = prevEnabled; 
            playButton.Enabled = playEnabled; 
            stopButton.Enabled = stopEnabled; 
            nextButton.Enabled = nextEnabled;

            if (TaskbarManager.IsPlatformSupported)
            {
                thumbButtonPrev.Enabled = prevEnabled;
                thumbButtonPrev.Icon = !prevEnabled ? MusicPlayerWindow.Properties.Resources.PrevIconDisabled : MusicPlayerWindow.Properties.Resources.PrevIconWhite;

                thumbButtonPlay.Enabled = playEnabled;
                if (currentSong == null || currentSong.getSound().isPaused())
                    thumbButtonPlay.Icon = !playEnabled ? MusicPlayerWindow.Properties.Resources.PlayIconDisabled : MusicPlayerWindow.Properties.Resources.PlayIconWhite;
                else
                    thumbButtonPlay.Icon = !playEnabled ? MusicPlayerWindow.Properties.Resources.PauseIconDisabled : MusicPlayerWindow.Properties.Resources.PauseIconWhite;
                
                thumbButtonStop.Enabled = stopEnabled;
                thumbButtonStop.Icon = !stopEnabled ? MusicPlayerWindow.Properties.Resources.StopIconDisabled : MusicPlayerWindow.Properties.Resources.StopIconWhite;
                
                thumbButtonNext.Enabled = nextEnabled;
                thumbButtonNext.Icon = !nextEnabled ? MusicPlayerWindow.Properties.Resources.NextIconDisabled : MusicPlayerWindow.Properties.Resources.NextIconWhite;
            }
        }
        #endregion

        #region Public Methods/Interface
        /// <summary>
        /// Plays the next song in the playlist
        /// </summary>
        public void playNextSong()
        {
            if (currentSong != null) { player.stopSong(currentSong); } //stop current song to play the next one

            //grab the next song, play it, ask the loader to update the next song queue, and update the GUI
            currentSong = loader.getNextSong();
            player.playCurrSong(currentSong);
            loader.updateNextQueue();
            switchImagesPlayPause();
            updateLabels();
        }
        #endregion

        #region Library-related Methods
        /*///<summary>
        /// Executes a process and waits for it to end. 
        ///</summary>
        ///<param name="cmd">Full Path of process to execute.</param>
        ///<param name="cmdParams">Command Line params of process</param>
        ///<param name="workingDirectory">Process' working directory</param>
        ///<param name="timeout">Time to wait for process to end</param>
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
        }*/

        /*/// <summary>
        /// Called in a separate thread
        /// It executes a java process to import the iTunes playlists using Eric Daugherty's iTunes Exporter <see href="http://www.ericdaugherty.com/dev/itunesexport/"/>
        /// </summary>
        private void ExecuteThread()
        {
            int timeout = 30000;
            System.IO.Directory.CreateDirectory(outputDir);
            String cmdArgs = String.Format("-mx1024m -jar itunesexport.jar -playlistType=M3U -library=\"{0}\" -outputDir=\"{1}\" -includeBuiltInPlaylists -excludePlaylist=\"iTunes DJ, Movies, iTunes U, Purchased on iPod touch, Purchased, Purchased on iPhone, Podcasts, Recently Played\"",
                libLocation, outputDir);
            String javaLoc = (System.IO.Directory.Exists(@"C:\Program Files (x86)")) ? @"C:\Program Files (x86)\Java\jre6\bin\java.exe" : @"C:\Program Files\Java\jre6\bin\java.exe";
            int exitcode = ExecuteProcess(javaLoc,
                cmdArgs,
                System.IO.Directory.GetCurrentDirectory(),
                timeout);
        }*/

        /*/// <summary>
        /// Inits the thread to import the playlists and advises the user what's going on before waiting for the importing to finish
        /// </summary>
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
        }*/

        /// <summary>
        /// Converts the playlist files from .m3u format to xml, adding an id to each song within the playlist
        /// 
        /// </summary>
        /// <param name="outputDir"></param>
        private void parseiTunesSongs(String outputDir)
        {
            PlaylistParser parser = new PlaylistParser(libLocation, outputDir);
            parser.createPlaylists();
            /*String[] playlistPaths = System.IO.Directory.GetFiles(outputDir);
            foreach (String file in playlistPaths)
            {
                System.IO.TextReader tr = new System.IO.StreamReader(file);
                String tmp = tr.ReadLine();

                //remove extra text
                tmp = tmp.Replace("#Playlist: '","").Replace("' exported by iTunesExport-Scala v2.2.2 http://www.ericdaugherty.com/dev/itunesexport/scala/", "");
                String file_to_write = tmp.Replace("/","_");
                file_to_write = outputDir + "/" + file_to_write + ".xml";

                //write the xml header
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
                //write each song to the playlist xml file
                while ((line = tr.ReadLine()) != null)
                {
                    line = line.Replace("&", "&amp;");
                    String line_to_write = String.Format("    <song id=\'{0:000000}\'>{1}</song>", count, line);
                    w.WriteLine(line_to_write);
                    count++;
                }

                //wrap up the playlist files
                w.WriteLine("</playlist>");
                tr.Close();
                w.Close();
                System.IO.File.Delete(file); //deletes old playlist
            }*/
        }

        /// <summary>
        /// Asks the user for the location of their iTunes/Music directory so that the application can find the iTunes Music Library.xml file to import the iTunes playlists
        /// </summary>
        private void getLibLocation()
        {
            //Show dialog to ask for the xml library.
            TaskDialog.Show("For example, \"C:/Users/YOUR_NAME/Music/iTunes/iTunes Music Library.xml\" would work.\n\n" +
                "The application assumes that all your iTunes music is in the same place. Click OK to continue.",
                "Please specify the location of your iTunes Library\n\n",
                "Specify iTunes Music Location");
            
            OpenFileDialog browser;
            Boolean shouldExit = false;
            //open a file browser, and display a retry dialog if the user exits out of the file browser
            do {
                //show file browser
                browser = new OpenFileDialog();
                browser.Title = "Please specify the location of your iTunes Music library (ie C:/....../iTunes/iTunes Music Library.xml).\n";
                DialogResult result = browser.ShowDialog();
                if (result == DialogResult.OK)
                    break;

                //display a retry dialog
                TaskDialog tdRetry = new TaskDialog();
                tdRetry.Caption = "Specify your iTunes Music Library XML file Location";
                tdRetry.InstructionText = "Are you sure you want to exit setup?";
                tdRetry.Text = "The application cannot complete setup without importing the iTunes playlists.\n\n" +
                    "If you do not have an iTunes Music folder, exit setup, open iTunes, " +
                    "and under Edit->Preferences->Advanced, check \"Keep iTunes Media Folder organized\" and \" Copy files to iTunes Media Folder when adding to library\".";
                tdRetry.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                tdRetry.Icon = TaskDialogStandardIcon.Warning;
                TaskDialogResult retry = tdRetry.Show();
                if (retry == TaskDialogResult.Yes)
                    shouldExit = true;
            } while (!shouldExit);
            
            if (shouldExit) { this.Close(); System.Environment.Exit(1); return; }
            libLocation = browser.FileName;
        }
        #endregion
    }
}
