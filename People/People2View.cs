using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace People
{
    public partial class People2View : Form
    {
        public People2View()
        {
            InitializeComponent();
            getAllPeople();
        }

        private void getAllPeople()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.PeopleConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("getAllNames", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();



                        // Execute the stored procedure.
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(dataReader);

                            // Display the data from the data table in the data grid view.
                            this.dataGridView1.DataSource = dataTable;

                            // Close the SqlDataReader.
                            dataReader.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("The operation was not completed.");
                    }
                    finally
                    {
                        // Close the connection.
                        connection.Close();
                    }
                }
            }
        }

        public void submitData()
        {

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }
    }
}
