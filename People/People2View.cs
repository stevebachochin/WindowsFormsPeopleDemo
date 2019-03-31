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
        int ID = 0;
        int ROW = -1;

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addName(txtFirst.Text, txtLast.Text);
            //MessageBox.Show(txtLast.Text);
        }

        public void clearData()
        {
            txtFirst.Text = "";
            txtLast.Text = "";
        }
        /// <summary>
        /// ADD NAME
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void addName(string firstName, string lastName)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.PeopleConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("addName", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                        sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        ////getAllPeople();
                        ////clearData();
                        ID = -1;
                        MessageBox.Show("Name Added.");
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
        /// <summary>
        /// DISPLAY SELECTED RECORD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtFirst.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtLast.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }


        private void updateName(string firstName, string lastName)
        {
            //MessageBox.Show(ID.ToString()+" <<<");
            if (txtFirst.Text != "" && txtLast.Text != "" && ID != 0)

            {

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.PeopleConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("updateName", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            sqlCommand.Parameters.AddWithValue("@ID", ID);
                            sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                            sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                            getAllPeople();
                            clearData();
                            ID = 0;
                            MessageBox.Show("Name UPDATED.");
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
        }
        //Update Record  


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateName(txtFirst.Text, txtLast.Text);
        }
        //DELETE RECORD
        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteName();
        }

        private void deleteName()
        {
            if (txtFirst.Text != "" && txtLast.Text != "" && ID != 0)

            {

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.PeopleConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("deleteName", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            sqlCommand.Parameters.AddWithValue("@ID", ID);
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                            getAllPeople();
                            clearData();
                            MessageBox.Show("Name " + ID + " Deleted.");
                            ID = -1;
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
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {


            //MessageBox.Show("----> " + dataGridView1.SelectedCells[0].Value.ToString());
            //MessageBox.Show("----> " + dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //MessageBox.Show("----> " + dataGridView1.Rows[e.RowIndex].ToString() + " --- " + dataGridView1.NewRowIndex.ToString());
            //if (e.RowIndex == dataGridView1.NewRowIndex)
            if(ROW != -1)
            {
                // user is in the new row, disable controls.
                MessageBox.Show(ROW.ToString() + "----> " + dataGridView1.Rows[ROW].Cells[1].Value.ToString());
                addName(dataGridView1.Rows[ROW].Cells[1].Value.ToString(), dataGridView1.Rows[ROW].Cells[2].Value.ToString());
                ROW = -1;
            }
            
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.NewRowIndex)
            {
                //MessageBox.Show("Row Enter----> " + e.RowIndex.ToString());
                ROW = e.RowIndex;
            }
            else
            {
                ROW = -1;
            }
                
        }

        public void addNameRow(string firstName, string lastName, string idStr)
        {
            int id;
                
                int.TryParse(idStr, out id);

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.PeopleConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("addNameIdx", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //try
                   // {
                        sqlCommand.Parameters.AddWithValue("@ID", id);
                        sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                        sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        getAllPeople();
                        clearData();
                        ID = -1;
                        MessageBox.Show("Name Added iDX.");
                  //  }
                  //  catch
                  //  {
                 //       MessageBox.Show("The operation was not completed.");
                 //   }
                 //   finally
                 //   {
                        // Close the connection.
                        connection.Close();
                  //  }
                }
            }
        }
    }



}

