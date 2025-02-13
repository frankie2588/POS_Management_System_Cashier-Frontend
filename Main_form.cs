using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;


namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public partial class Main_Form : Form
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["POS_MANAGEMENT_SYSTEM___Cashier__.Properties.Settings.Setting"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection();

        // Declare a PrintDocument object for printing
        ///private PrintDocument printDocument1 = new PrintDocument();
        
       // private PrintDocument printDocument1;
        public Main_Form()
        {
            InitializeComponent();
            conn.ConnectionString = connString;

            printDocument1 = new PrintDocument();

           // CheckForUpdates();
            LoadReceiptNo();
            Usernametxtbox.Text = Login.Receive;
            UpdateItemCount();

            printDocument1.PrintPage += new PrintPageEventHandler(PrintDocument1_PrintPage);

            

        }
        

        private void LoadReceiptNo()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(Receipt_no), 0) FROM Receipt_Tb2", conn);
                int maxReceiptNo = (int)cmd.ExecuteScalar();
                Receiptnotxtbx.Text = (maxReceiptNo + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading receipt number: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void UpdateItemDetails()
        {
            try
            {
                int itemCode = Convert.ToInt32(Item_gridview.CurrentRow.Cells["Item_code"].Value);

                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Short_Name, Unit, Item_Selling_Price FROM Item_Details_Tb WHERE Item_Code = @item_code", conn);
                cmd.Parameters.AddWithValue("@item_code", itemCode);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Item_gridview.CurrentRow.Cells["Item_name"].Value = reader["Short_Name"].ToString();
                    Item_gridview.CurrentRow.Cells["Unit"].Value = reader["Unit"].ToString();
                    Item_gridview.CurrentRow.Cells["Unitprice"].Value = reader["Item_Selling_Price"].ToString();

                    decimal price = Convert.ToDecimal(Item_gridview.CurrentRow.Cells["Unitprice"].Value);
                    int qty = Convert.ToInt32(Item_gridview.CurrentRow.Cells["Qty"].Value);
                    Item_gridview.CurrentRow.Cells["Item_Amount"].Value = price * qty;
                }
                else
                {
                    MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   // ClearItemRow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating item details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // **********CALCULATION OF TOTAL AMOUNT ON THE GRID VIEW***********************
        private void CalculateTotalAmount()
        {
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in Item_gridview.Rows)
            {
                if (!row.IsNewRow && row.Cells["Item_Amount"].Value != null)
                {
                    totalAmount += Convert.ToDecimal(row.Cells["Item_Amount"].Value);
                }
            }

            Receipttotaltxtbx.Text = totalAmount.ToString();
        }
        private void CalculateChangeAmount()
        {
            if (decimal.TryParse(Receipttotaltxtbx.Text, out decimal receiptTotal) &&
        decimal.TryParse(Cashtxtbx.Text, out decimal cashAmount))
            {
                decimal changeAmount = receiptTotal - cashAmount;
                Changetxtbx.Text = changeAmount.ToString();
            }
            else
            {
                Changetxtbx.Text = "Invalid input";
                // Optionally display a message to the user or log the error
            }
        }
        private void CalculateDiscount()
        {
            //Totaldisctxtbx.Text=Convert.ToInt32(Receipttotaltxtbx.Text)- 
            if (decimal.TryParse(Receipttotaltxtbx.Text, out decimal receiptTotal) &&
        int.TryParse(Totaldisctxtbx.Text, out int discount))
            {
                decimal discountAmount = receiptTotal * discount / 100; // Calculate discount amount
                decimal discountedTotal = receiptTotal - discountAmount; // Calculate total amount after discount
                Totaldisctxtbx.Text = discountedTotal.ToString(); // Display discounted total
            }
            else
            {
                Totaldisctxtbx.Text = "Invalid input";
                // Optionally display a message to the user or log the error
            }
        }

        //************* THIS COUNT NUMBER OF ITEM ADDED IN THE GRIDVIEW****************
        private void UpdateItemCount()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Item_Details_Tb", conn);
                int itemCount = (int)cmd.ExecuteScalar();
                Itemcounttxtbx.Text = itemCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating item count: " + ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        //**********************THIS ARE VALIDATIONS OR SHORTCUT KEYS*****************
        private void Main_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                ShowCashierSearch();
            else if (e.KeyCode == Keys.F10)
                ShowCashForm();
            else if (e.KeyCode == Keys.F2)
                ShowCreditCustomerForm();
            else if (e.KeyCode == Keys.F7)
                ShowChequeDetailsForm();
            else if(e.KeyCode==Keys.F5)
            {
                ShowConnstring_frm();
            }

            if (e.KeyCode == Keys.Enter)
            {
                // Calling the ReceiptNumber method to generate and print the receipt
                ReceiptNumber();

               // PrintReceipt();
            }
        }

        //***********************THIS METHOD OPENS UP CASHIER SEARCH FORM**************
        private void ShowCashierSearch()
        {
            Cashier_Search cashierSearch = new Cashier_Search();
            cashierSearch.Show();
        }

        private void ShowConnstring_frm()
        {
            Connstring_Frm _Frm = new Connstring_Frm();
            _Frm.Show();
        }


        //***********************THIS METHOD OPENS UP CASHFORM FORM**************
        private void ShowCashForm()
        {
            Cash_form cashForm = new Cash_form();
            cashForm.Show();
        }


        //***********************THIS METHOD OPENS UP CREDITCUSTOMER FORM**************
        private void ShowCreditCustomerForm()
        {
            Frmcredit_customer creditCustomerForm = new Frmcredit_customer();
            creditCustomerForm.ShowDialog();
        }


        //***********************THIS METHOD OPENS UP CHEQUE DETAILS FORM**************
        private void ShowChequeDetailsForm()
        {
            Cheque_Details chequeDetailsForm = new Cheque_Details();
            chequeDetailsForm.ShowDialog();
        }

        // *************THIS HELP IN ERASING ITEMS FROM THE GRIDVIEW*******************

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Item_gridview.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int rowIndex = Item_gridview.CurrentCell.RowIndex;
                    Item_gridview.Rows.RemoveAt(rowIndex);
                    CalculateTotalAmount();
                }
            }
        }

        // *************THIS CALLING OF AMETHOD *****************
        private void Item_gridview_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateItemDetails();
            CalculateTotalAmount();
        }

        //***********THIS CALLING OF A METHOD OF SHOWLOGIN FORM THAT CLOSES THE MAINFORM AND DISPLAY THE LOGIN FORM ******************
        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
           // ShowLoginForm();
        }


        //***********************THIS METHOD OPENS UP LOGIN FORM**************
        private void ShowLoginForm()
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void Receipttotaltxtbx_TextChanged(object sender, EventArgs e)
        {
            // DiscCalc();
        }

        private void Totaldisctxtbx_Leave(object sender, EventArgs e)
        {
            // DiscCalc();
        }
        private void Cashtxtbx_TextChanged(object sender, EventArgs e)
        {
            CalculateChangeAmount();
            // Your event handler logic goes here
        }
        private void Panel8_Paint(object sender, PaintEventArgs e)
        {
          
        }
        private void Main_Form_Load(object sender, EventArgs e)
        {
            // Your event handler logic goes here
        }
        private void Main_Form_KeyUp(object sender, KeyEventArgs e)
        {
            // Your event handler logic goes here
        }

        // ***********DISPLAYS ON THE GRIDVIEW***************
        public void AddItemToGrid(string itemCode, string shortName, string unit, decimal itemPrice)
        {
            // Add the item to the gridview
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(Item_gridview);
            newRow.Cells[0].Value = itemCode;
            newRow.Cells[1].Value = shortName;
            newRow.Cells[2].Value = unit;
            newRow.Cells[3].Value = 1; // Default quantity
            newRow.Cells[4].Value = itemPrice;
            newRow.Cells[5].Value = itemPrice; // Initial item amount
            Item_gridview.Rows.Add(newRow);

            // Recalculate total amount
            CalculateTotalAmount();
            CalculateChangeAmount();
        }
        private void Item_gridview_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Handle the data error, e.g., display a custom message box
            MessageBox.Show("An error occurred in the DataGridView: " + e.Exception.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ***********RECEIPT PRINTING STARTS HERE **************************
        public void ReceiptNumber()
        {
            StringBuilder receiptContent = new StringBuilder();

            receiptContent.AppendLine("************ RECEIPT ************");
            receiptContent.AppendLine($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            receiptContent.AppendLine($"Cashier: {Usernametxtbox.Text}");
            receiptContent.AppendLine("----------------------------------");

            decimal grandTotal = 0;

            // Loop through each row in the DataGridView to get item details
            for (int x = 0; x < Item_gridview.Rows.Count; x++)
            {
                int itemId = Convert.ToInt32(Item_gridview.Rows[x].Cells["Item_code"].Value);
                string itemName = Convert.ToString(Item_gridview.Rows[x].Cells["Item_name"].Value);
                int quantity = Convert.ToInt32(Item_gridview.Rows[x].Cells["qty"].Value);
                decimal unitPrice = Convert.ToDecimal(Item_gridview.Rows[x].Cells["Unitprice"].Value);
                decimal totalAmount = Convert.ToDecimal(Item_gridview.Rows[x].Cells["Item_Amount"].Value);

                receiptContent.AppendLine($"{itemName} (Item Code: {itemId}) x {quantity} @ ${unitPrice} each = ${totalAmount}");

                grandTotal += totalAmount;
            }

            receiptContent.AppendLine("----------------------------------");
            receiptContent.AppendLine($"TOTAL: ${grandTotal}");

            // Save the receipt data to the database
            SaveReceiptToDatabase(receiptContent.ToString(), grandTotal);
        }
        //*************THIS PART SAVES THE DETAILS OF THE RECEIPT IN THE DATABASE ON TABLE RECEIPT_Tb*********************
        private void SaveReceiptToDatabase(string receiptContent, decimal grandTotal)
        {
            try
            {
                // Open database connection
                conn.Open();

                // SQL query to insert receipt data into Receipt_Tb table
                string sql = "INSERT INTO Receipt_Tb (ReceiptContent, GrandTotal, Cashier, ReceiptDate, Cashgiven,Itemcount) " +
                             "VALUES (@ReceiptContent, @GrandTotal, @Cashier, @ReceiptDate, @Cashgiven,@Itemcount)";

                // Create SqlCommand object with the SQL query and connection
                SqlCommand sqlCommand = new SqlCommand(sql, conn);

                // Add parameters to the SqlCommand
                sqlCommand.Parameters.AddWithValue("@ReceiptContent", receiptContent);
                sqlCommand.Parameters.AddWithValue("@GrandTotal", grandTotal);
                sqlCommand.Parameters.AddWithValue("@Cashier", Usernametxtbox.Text);
                sqlCommand.Parameters.AddWithValue("@ReceiptDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@Cashgiven", int.Parse(Cashtxtbx.Text));
                sqlCommand.Parameters.AddWithValue("@Itemcount", int.Parse(Itemcounttxtbx.Text));

                // Execute the SQL command
                sqlCommand.ExecuteNonQuery();

                // Close database connection
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving receipt to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();

            }
        }
        private void PrintReceipt(string receiptContent)
        {
            // Implement printing logic here, such as sending the content to a printer
            // Example:
            // Printer.Print(receiptContent);
            // Alternatively, you can display the receipt in a MessageBox or another UI element.

            try
            {
                // Set the printer name if needed (optional)
                // printDocument1.PrinterSettings.PrinterName = "YourPrinterName";

                // Print the document
                printDocument1.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while printing: " + ex.Message);
            }
        }

        // Method to handle printing when Enter key is pressed
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Create a font and brush for printing
            Font printFont = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);

            // Define the text to be printed (example)
            string text = "Receipt Information: \n"
                        + "Item 1: $10.00\n"
                        + "Item 2: $20.00\n"
                        + "Total: $30.00";

            // Determine the position where the text will be printed
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            // Draw the text on the page
            e.Graphics.DrawString(text, printFont, brush, x, y);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ShowCashierSearch();
        }

        private void Changetxtbx_TextChanged(object sender, EventArgs e)
        {
            CalculateChangeAmount();
        }

        private void Receiptnotxtbx_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

