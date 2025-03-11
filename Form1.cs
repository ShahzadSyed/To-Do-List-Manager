using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ToDoListManager
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ERP-ASST\SQLEXPRESS;Initial Catalog=crud;Integrated Security=True;Pooling=False");
        int ID = 0;

        public Form1()
        {
            InitializeComponent();
            displaydata();
        }

        private void cleardata()
        {
            textBox1.Clear();
        }
        private void displaydata()
        {
            string query = "SELECT * FROM tbl_todo";
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string query = "insert into tbl_todo(description) values (@description)";
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@description", textBox1.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Description Added!");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close(); // Ensure the connection is closed
                    }
                    cleardata(); // Clear data after closing the connection
                    displaydata();
                }
            }

            else
            {
                MessageBox.Show("Error! Empty Description");
            }

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                string query = "update tbl_todo set description=@description where ID=@id ";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@description", textBox1.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data updated");
                con.Close();
                displaydata();
            }
            else
            {

            }

        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if(ID != 0)
            {
                string query = "delete tbl_todo where ID=@id";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Deleted");
                con.Close();
                displaydata();
            }
            else
            {
                MessageBox.Show("Please Select Id");
            }
                
        }
    }
}
