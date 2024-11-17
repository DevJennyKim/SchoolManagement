using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace SchoolManagement
{
    public partial class Subject : Form
    {
        public Subject()
        {
            InitializeComponent();
        }
        private void Subject_Load(object sender, EventArgs e)
        {
            LoadData();

        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
        }
        private void LoadData()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cnn = new SqlCommand("SELECT * FROM subtab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtSubjectName.Text))
            {
                MessageBox.Show("Please enter a Subject name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCredit.Text))
            {
                MessageBox.Show("Please enter the credit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();


                SqlCommand checkIdCmd = new SqlCommand("SELECT COUNT(*) FROM subtab WHERE subjectid = @subjectid", con);
                checkIdCmd.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
                int idCount = (int)checkIdCmd.ExecuteScalar();

                if (idCount > 0)
                {
                    MessageBox.Show("This Subject ID already exists. Please enter a unique ID.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    con.Close();
                    return;
                }


                SqlCommand cnn = new SqlCommand("INSERT INTO subtab VALUES (@subjectid, @subjectname,@description,@credits)", con);
                cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
                cnn.Parameters.AddWithValue("@subjectName", txtSubjectName.Text);
                cnn.Parameters.AddWithValue("@description", txtDesc.Text);
                cnn.Parameters.AddWithValue("@credits", int.Parse(txtCredit.Text));
                

                cnn.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("update subtab set subjectname=@subjectname where subjectid=@subjectid", con);
                cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
                cnn.Parameters.AddWithValue("@subjectName", txtSubjectName.Text);
                cnn.Parameters.AddWithValue("@description", txtSubjectName.Text);
                cnn.Parameters.AddWithValue("@credits", int.Parse(txtCredit.Text));

                cnn.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Update Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("select * from subtab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("delete subtab where subjectid=@subjectid", con);
                cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));

                cnn.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtSubjectId.Text = "";
            txtSubjectName.Text = "";
            txtDesc.Text = "";
            txtCredit.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("select * from subtab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
                LoadData();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtSubjectId.Text = row.Cells["subjectid"].Value != DBNull.Value ? row.Cells["subjectid"].Value.ToString() : "";
                txtSubjectName.Text = row.Cells["subjectName"].Value != DBNull.Value ? row.Cells["subjectName"].Value.ToString() : "";
                txtDesc.Text = row.Cells["description"].Value != DBNull.Value ? row.Cells["description"].Value.ToString() : "";
                txtCredit.Text = row.Cells["credits"].Value != DBNull.Value ? row.Cells["credits"].Value.ToString() : "";

            }
        }
    }
}
