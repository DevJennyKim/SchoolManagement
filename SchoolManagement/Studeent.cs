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
    public partial class Studeent : Form
    {
        public Studeent()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();
            
            SqlCommand cnn = new SqlCommand("insert into studentab values (@studentid,@studentname,@dob,@gender,@phone,@email",con);
            cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
            cnn.Parameters.AddWithValue("@StudentName", int.Parse(txtStudentName.Text));
            cnn.Parameters.AddWithValue("@Dob", int.Parse(txtStudentDob.Text));
            cnn.Parameters.AddWithValue("@Gender", int.Parse(txtStudentGen.Text));
            cnn.Parameters.AddWithValue("@Phone", int.Parse(txtStudentPhone.Text));
            cnn.Parameters.AddWithValue("@Email", int.Parse(txtStudentEmail.Text));
            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("update studentab set studentname=@studentname, dob=@dob,gender=@gender,phone=@phone,email=@email where studentid=@studentid", con);
            cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
            cnn.Parameters.AddWithValue("@StudentName", int.Parse(txtStudentName.Text));
            cnn.Parameters.AddWithValue("@Dob", int.Parse(txtStudentDob.Text));
            cnn.Parameters.AddWithValue("@Gender", int.Parse(txtStudentGen.Text));
            cnn.Parameters.AddWithValue("@Phone", int.Parse(txtStudentPhone.Text));
            cnn.Parameters.AddWithValue("@Email", int.Parse(txtStudentEmail.Text));
            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Update Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtStudentId.Text = "";
            txtStudentName.Text = "";
            txtStudentDob.Text = "";
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
        }
    }
}
