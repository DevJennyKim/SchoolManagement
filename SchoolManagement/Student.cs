using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace SchoolManagement
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            txtStudentId.Text = Guid.NewGuid().ToString("N").Substring(0, 12);
            txtStudentGen.Items.Add("Male");
            txtStudentGen.Items.Add("Female");
            txtStudentGen.Items.Add("Other");

            
            txtStudentGen.SelectedIndex = 0;  
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True"))
            {
                con.Open();
                SqlCommand cnn = new SqlCommand("SELECT * FROM studentab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private bool IsValidEmail(string email)
        {
            // Regular expression pattern for email format
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtStudentDob.CustomFormat = "dd/mm/yyyy";
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Back)
            {
                txtStudentDob.CustomFormat = "";
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Please enter a student name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStudentPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            if (!IsValidEmail(txtStudentEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            
            SqlCommand checkIdCmd = new SqlCommand("SELECT COUNT(*) FROM studentab WHERE studentid = @studentid", con);
            checkIdCmd.Parameters.AddWithValue("@studentid", int.Parse(txtStudentId.Text));
            int idCount = (int)checkIdCmd.ExecuteScalar();

            if (idCount > 0)
            {
                MessageBox.Show("This Student ID already exists. Please enter a unique ID.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return; 
            }

            
            SqlCommand cnn = new SqlCommand("INSERT INTO studentab VALUES (@studentid, @studentname, @dob, @gender, @phone, @email)", con);
            cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
            cnn.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
            cnn.Parameters.AddWithValue("@Dob", txtStudentDob.Value);
            cnn.Parameters.AddWithValue("@Gender", txtStudentGen.SelectedItem.ToString());
            cnn.Parameters.AddWithValue("@Phone", txtStudentPhone.Text);
            cnn.Parameters.AddWithValue("@Email", txtStudentEmail.Text);
            cnn.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Record saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("select * from studentab", con);
            SqlDataAdapter da = new SqlDataAdapter(cnn);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("update studentab set studentname=@studentname, dob=@dob,gender=@gender,phone=@phone,email=@email where studentid=@studentid", con);
            cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
            cnn.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
            cnn.Parameters.AddWithValue("@Dob", txtStudentDob.Value);
            cnn.Parameters.AddWithValue("@Gender", txtStudentGen.SelectedItem.ToString());
            cnn.Parameters.AddWithValue("@Phone", txtStudentPhone.Text);
            cnn.Parameters.AddWithValue("@Email", txtStudentEmail.Text);
            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Update Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("delete studentab where studentid=@studentid", con);
            cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
      
            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtStudentId.Text = Guid.NewGuid().ToString("N").Substring(0, 12);
            txtStudentName.Text = "";
            txtStudentDob.Value = DateTime.Now;  
            txtStudentDob.Format = DateTimePickerFormat.Custom;  
            txtStudentDob.CustomFormat = "dd/MM/yyyy";  
            txtStudentGen.Text = "";
            txtStudentPhone.Text = "";
            txtStudentEmail.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("select * from studentab", con);
            SqlDataAdapter da = new SqlDataAdapter(cnn);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtStudentId.Text = row.Cells["studentid"].Value != DBNull.Value ? row.Cells["studentid"].Value.ToString() : "";
                txtStudentName.Text = row.Cells["studentname"].Value != DBNull.Value ? row.Cells["studentname"].Value.ToString() : "";

                if (row.Cells["dob"].Value != DBNull.Value)
                {
                    txtStudentDob.Value = Convert.ToDateTime(row.Cells["dob"].Value);
                    txtStudentDob.CustomFormat = "dd/MM/yyyy";  
                }
                else
                {
                    txtStudentDob.CustomFormat = ""; 
                }

                txtStudentGen.Text = row.Cells["gender"].Value != DBNull.Value ? row.Cells["gender"].Value.ToString() : "";
                txtStudentPhone.Text = row.Cells["phone"].Value != DBNull.Value ? row.Cells["phone"].Value.ToString() : "";
                txtStudentEmail.Text = row.Cells["email"].Value != DBNull.Value ? row.Cells["email"].Value.ToString() : "";
            }
        }
    }
}
