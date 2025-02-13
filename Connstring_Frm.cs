using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public partial class Connstring_Frm : Form
    {
        public Connstring_Frm()
        {
            InitializeComponent();

            CboServer.DropDownStyle = ComboBoxStyle.DropDown;
        }
        static string connstring = ConfigurationManager.ConnectionStrings["POS_MANAGEMENT_SYSTEM___Cashier__.Properties.Settings.Setting"].ConnectionString;

        SqlConnection conn = new SqlConnection(connstring);

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Connstring_Frm_Load(object sender, EventArgs e)
        {
            CboServer.Items.Add(".");
            CboServer.Items.Add("(Local)");
            CboServer.Items.Add(@".\SQLSERVEREXPRESS");
            CboServer.Items.Add(string.Format(@"[0]\SQLEXPRESS", Environment.MachineName));
            CboServer.SelectedIndex = 3;

        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(CboServer.Text))
            {
                MessageBox.Show("Server name or IP address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtdatabase.Text))
            {
                MessageBox.Show("Database name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Txtpassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public void connect()
        {
            if (!ValidateInputs())
                return;

            string conn = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};",
                CboServer.Text, txtdatabase.Text, txtUsername.Text, Txtpassword.Text);

            try
            {
                SqlHelper helper = new SqlHelper(conn);
                if (helper.isConnection)
                {
                    MessageBox.Show("Test Connection Succeeded", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Test Connection Failed. Please check your connection details.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ConnectSave()
        {
            if (!ValidateInputs())
                return;

            string conn = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};",
                CboServer.Text, txtdatabase.Text, txtUsername.Text, Txtpassword.Text);

            try
            {
                SqlHelper helper = new SqlHelper(conn);
                if (helper.isConnection)
                {
                    AppSetting setting = new AppSetting();

                    setting.SaveConnectionstring("POS_MANAGEMENT_SYSTEM___Cashier__.Properties.Settings.Setting", conn);

                    MessageBox.Show("Your Connection String Has Been Successfully Saved.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save connection string. Please check your connection details.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            connect();// THIS BUTTON IS FOR TESTING CONNECTIVITY 
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            ConnectSave();
        }
    }
}
