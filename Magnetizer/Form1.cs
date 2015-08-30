using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
            this.txtTrackers.Text = "udp://tracker.openbittorrent.com:80/announce" + System.Environment.NewLine + "udp://tracker.publicbt.com:80/announce";
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
