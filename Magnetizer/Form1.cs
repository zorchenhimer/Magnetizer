using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Magnetizer {
    public partial class Form1 : Form {
        private Regex hashcheck = new Regex("^[A-F0-9]+$", RegexOptions.IgnoreCase);
        private Regex trackercheck = new Regex("^(http|https|udp)://(.+)announce(.*)$",RegexOptions.IgnoreCase);
        
        public Form1() {
            InitializeComponent();

            // Prep magnet link text box
            this.txtMagnet.ReadOnly = true;
            this.txtMagnet.Click += delegate(object sender, EventArgs e) { this.txtMagnet.SelectAll(); };

            // Set default values
            this.txtHash.Text = "B05930182B4FC941E73A4278FC612D154D5A822B";

            // Overwrite default trackers with ones in this file, if present
            if(File.Exists("trackers.txt")) {
                StreamReader reader = new StreamReader("trackers.txt");
                string[] trackers = reader.ReadToEnd().Replace(System.Environment.NewLine, "\n").Split('\n');
                reader.Close();

                // Make sure input is valid
                foreach(string tr in trackers) {
                    if(trackercheck.IsMatch(tr))
                        this.txtTrackers.Text += tr + System.Environment.NewLine;
                }
            } else {
                this.txtTrackers.Text = "udp://tracker.openbittorrent.com:80/announce"
                    + System.Environment.NewLine + "udp://open.demonii.com:1337"
                    + System.Environment.NewLine + "udp://tracker.coppersurfer.tk:6969"
                    + System.Environment.NewLine + "udp://tracker.leechers-paradise.org:6969";
            }

            // Update magnet link text box
            Input_TextChanged();

            this.txtHash.TextChanged += Input_TextChanged;
            this.txtTrackers.TextChanged += Input_TextChanged;
        }

        private void Input_TextChanged(object sender, EventArgs e) {
            Input_TextChanged();
        }

        private void Input_TextChanged() {
            if(this.txtHash.Text.Length != 40) {
                // Invalid length
                this.txtMagnet.Text = "Invalid hash length.";
            } else {
                if(hashcheck.IsMatch(this.txtHash.Text)) {
                    string trackerstring = "";

                    // Build the tracker string
                    if( this.txtTrackers.Text.Length != 0 ) {
                        string[] trackers = this.txtTrackers.Text.Replace(System.Environment.NewLine, "\n").Split('\n');
                        foreach(string tr in trackers) {
                            if(trackercheck.IsMatch(tr))
                                trackerstring += "&tr=" + tr;
                        }
                    }

                    // Build the full magnet string
                    this.txtMagnet.Text = "magnet:?xt=urn:btih:" + this.txtHash.Text.ToUpper() + trackerstring;
                } else {
                    this.txtMagnet.Text = "Invalid characters in Info Hash.";
                }
            }
        }
    }
}
