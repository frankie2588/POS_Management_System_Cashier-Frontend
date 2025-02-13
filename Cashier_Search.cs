using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; 
 using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public partial class Cashier_Search : Form
    {

        public Cashier_Search()
        {
            InitializeComponent();
        }

        static string connstring = ConfigurationManager.ConnectionStrings["POS_MANAGEMENT_SYSTEM___Cashier__.Properties.Settings.Setting"].ConnectionString;

        SqlConnection conn = new SqlConnection(connstring);

        
        public void SearchData(string valueToFind)
        {
            string query = "SELECT Item_Code, Short_Name, Unit, Item_Cost_Price, item_Selling_Price FROM Item_Details_Tb " +
                           "WHERE CONCAT(Item_code, Short_Name, Barcode_One, Barcode_two, Barcode_Three) LIKE '%" + valueToFind + "%'";

            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            Searchgridview.DataSource = dataTable;
        }

        public void PopulateGrid2()
        {
            conn.Open();
            string query = "SELECT Item_Code, Short_Name, Unit, Item_Cost_Price, item_Selling_Price FROM Item_Details_Tb";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            Searchgridview.DataSource = dataSet.Tables[0];
            conn.Close();
        }

        public void TextBoxFilterGrid()
        {
            conn.Open();
            string query = "SELECT Item_Code, Short_Name, Unit, Item_Cost_Price, item_Selling_Price FROM Item_Details_Tb " +
                           "WHERE Short_Name = '" + searchtxtbx.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            Searchgridview.DataSource = dataSet.Tables[0];
            conn.Close();
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            TextBoxFilterGrid();
        }

        private void Cashier_Search_Load(object sender, EventArgs e)
        {
            PopulateGrid2();
            SearchData("");
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Searchtxtbx_TextChanged(object sender, EventArgs e)
        {
            SearchData(searchtxtbx.Text);
        }

        private void Searchgridview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Searchgridview_SelectionChanged(object sender, EventArgs e)
        {
            

        }

        private void Searchgridview_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
            
        }

        // Method to populate the gridview in Cashier_Search form
        public void PopulateGrid()
        {
            // Your existing code...
            conn.Close();
        }

        private void Searchgridview_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // it enables the item wen clicked it reflects on the other gridview

            // Check if a valid cell is clicked

            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = Searchgridview.Rows[e.RowIndex];

                // Retrieve item details
                string itemCode = row.Cells["Item_Code"].Value.ToString();
                string shortName = row.Cells["Short_Name"].Value.ToString();
                string unit = row.Cells["Unit"].Value.ToString();
                decimal itemPrice = Convert.ToDecimal(row.Cells["item_Selling_Price"].Value);

                // Pass the item details back to Main_Form
                Main_Form mainForm = Application.OpenForms.OfType<Main_Form>().FirstOrDefault();
                if (mainForm != null)
                {
                    // Call a method in Main_Form to add the item to the gridview
                    mainForm.AddItemToGrid(itemCode, shortName, unit, itemPrice);
                }
            }
        }
    }
    
}
