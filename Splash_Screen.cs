using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public partial class Splash_Screen : Form
    {
        public Splash_Screen()
        {
            InitializeComponent();

            //CheckForUpdates();
        }
        private void CheckForUpdates()
        {
            WebClient webClient = new WebClient();

            if (!webClient.DownloadString("https://www.dropbox.com/scl/fi/h6f7cz48sik5ulovugu2t/Pos_Updater.txt?rlkey=c3cg8nx5os9j4r4p0rjt3gk2e&st=631wyt5z&dl=1").Contains("1.0.0.0"))
            {
                DialogResult result = MessageBox.Show("New Updates Available..! Would you like to Install It..", "OFO FINANCING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    InstallUpdates();
                }
            }
        }

        private void InstallUpdates()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string zipPath = @".\MyAppSetup.zip";
                    string extraPath = @".\";

                    if (File.Exists(zipPath))
                        File.Delete(zipPath);

                    client.DownloadFile("https://www.dropbox.com/scl/fi/yjc270sx98r68hydbvlmb/MyAppSetup.zip?rlkey=svi4nwtbve5jz2ckk6r1f8aza&st=frwt9ngr&dl=1", zipPath);
                    ZipFile.ExtractToDirectory(zipPath, extraPath);

                    Process.Start("msiexec", "/C MyAppSetup.msi");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred during update installation: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      //  private string updateServerUrl = "https://your-update-server.com/version.txt"; // Replace with your update server URL

      /*  private void CheckForUpdates2()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string updateData = webClient.DownloadString(updateServerUrl);
                    Version latestVersion = Version.Parse(updateData.Split(':')[0]); // Parse version from server response

                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                    if (latestVersion > currentVersion)
                    {
                        DialogResult result = MessageBox.Show(
                          $"New update available! (v{latestVersion.ToString()}). Would you like to install it?",
                          "OFO FINANCING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            InstallUpdates(latestVersion);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are already using the latest version.", "OFO FINANCING", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while checking for updates: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InstallUpdates(Version latestVersion)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string updateUrl = $"https://your-update-server.com/updates/MyAppSetup_{latestVersion}.zip"; // Build update download URL
                    string zipPath = Path.Combine(Path.GetTempPath(), $"MyAppSetup_{latestVersion}.zip"); // Download to temporary location

                    if (File.Exists(zipPath))
                        File.Delete(zipPath);

                    client.DownloadFile(updateUrl, zipPath);

                    // Implement hash verification using a secure hashing algorithm (e.g., SHA256)

                    ZipFile.ExtractToDirectory(zipPath, Path.GetDirectoryName(Application.ExecutablePath)); // Extract to application directory

                    Process.Start("msiexec", "/C MyAppSetup.msi");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred during update installation: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/



        private void Timer1_Tick(object sender, EventArgs e)
        {
          
            ExpandProgressPanel();
        }
        private void ExpandProgressPanel()
        {
            const int expansionRate = 3;
            const int targetWidth = 300;

            // Increase the width of the progress panel
            progresspanel.Width += expansionRate;

            // Check if the target width is reached or exceeded
            if (progresspanel.Width >= targetWidth)
            {
                // Stop the timer
                timer1.Stop();

                // Show the login form
                Login loginForm = new Login();
                loginForm.Show();

                // Hide the current form
                this.Hide();
            }
        }


        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Splash_Screen_Load(object sender, EventArgs e)
        {

        }

        private void PrintPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
