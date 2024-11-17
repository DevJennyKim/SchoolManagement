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

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True"))
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



            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
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


            SqlCommand cnn = new SqlCommand("INSERT INTO subtab VALUES (@subjectid, @subjectname)", con);
            cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
            cnn.Parameters.AddWithValue("@subjectName", txtSubjectName.Text);

            cnn.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Record saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            LoadData();
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("update subtab set subjectname=@subjectname where subjectid=@subjectid", con);
            cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
            cnn.Parameters.AddWithValue("@subjectName", txtSubjectName.Text);

            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Update Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("select * from subtab", con);
            SqlDataAdapter da = new SqlDataAdapter(cnn);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("delete subtab where subjectid=@subjectid", con);
            cnn.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));

            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtSubjectId.Text = "";
            txtSubjectName.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();

            SqlCommand cnn = new SqlCommand("select * from subtab", con);
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

                txtSubjectId.Text = row.Cells["subjectid"].Value != DBNull.Value ? row.Cells["subjectid"].Value.ToString() : "";
                txtSubjectName.Text = row.Cells["subjectName"].Value != DBNull.Value ? row.Cells["subjectName"].Value.ToString() : "";

            }
        }
    }
}
