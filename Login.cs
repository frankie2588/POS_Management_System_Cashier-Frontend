using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            //CheckForUpdates();
        }
        public static string username;
        public static string Receive
        {
            get { return username; }
            set { username = value; }

        }
        static string connstring = ConfigurationManager.ConnectionStrings["POS_MANAGEMENT_SYSTEM___Cashier__.Properties.Settings.Setting"].ConnectionString;

        SqlConnection conn = new SqlConnection(connstring);

        //SqlConnection con = new SqlConnection("Data Source=FRANKLIN-IT-STO;Initial Catalog=POS_MANAGMENT_SYSTEM;Persist Security Info=True;User ID=sa;Password=123456");
        

        public void Loginconn()
        {
            // Get the username and password from the text boxes
            string username = Usertxtbox.Text;
            string password = Passwordtxtbx.Text;

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    connection.Open(); // Open the connection

                    // Prepare the SQL query with parameters to prevent SQL injection
                    string query = "SELECT * FROM Login_Tb WHERE Username = @username AND Password = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username); // Add username parameter
                    command.Parameters.AddWithValue("@password", password); // Add password parameter

                    // Execute the query and retrieve the results
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Check if any rows were returned
                    if (dataTable.Rows.Count > 0)
                    {
                        // Load the main form
                        Main_Form mainForm = new Main_Form();
                        mainForm.Show();
                        this.Hide(); // Hide the current login form

                        // If login is successful, show a welcome message
                        string welcomeMessage = $"Welcome back, {username}!";
                        MessageBox.Show(welcomeMessage, "Login Successful", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // If login failed, display an error message
                        MessageBox.Show("Invalid login credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Usertxtbox.Clear(); // Clear the username textbox
                        Passwordtxtbx.Clear(); // Clear the password textbox
                        Usertxtbox.Focus(); // Set focus back to the username textbox
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the login process
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void Guna2Button1_Click(object sender, EventArgs e)
        {
             Loginconn();
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
