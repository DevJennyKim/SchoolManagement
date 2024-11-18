using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Windows.Forms;
using System.Xml;
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
            XmlDocument doc = new XmlDocument();
            doc.Load("./Database.config");

            string connectionString = doc.SelectSingleNode("//connectionStrings/add[@name='SchoolDbConnection']").Attributes["connectionString"].Value;
            return new SqlConnection(connectionString);
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
            if (string.IsNullOrWhiteSpace(txtSubjectId.Text))
            {
                MessageBox.Show("Please enter a subject Id.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
            if (string.IsNullOrWhiteSpace(txtSubjectId.Text))
            {
                MessageBox.Show("Please enter a subject Id.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand checkSubjectExistsCmd = new SqlCommand("SELECT COUNT(*) FROM subtab WHERE subjectid = @subjectid", con);
                checkSubjectExistsCmd.Parameters.AddWithValue("@subjectid", int.Parse(txtSubjectId.Text));
                int subjectExists = (int)checkSubjectExistsCmd.ExecuteScalar();

                if (subjectExists == 0)
                {
                    MessageBox.Show("Subject ID not found. Please enter a valid subject ID or use the Save button to add a new subject.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubjectId.Text))
            {
                MessageBox.Show("Please enter a subject Id.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int subjectId = int.Parse(txtSubjectId.Text);
            
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand checkExistCmd = new SqlCommand("SELECT COUNT(*) FROM subtab WHERE subjectid = @subjectid", con);
                checkExistCmd.Parameters.AddWithValue("@subjectid", subjectId);

                int count1 = (int)checkExistCmd.ExecuteScalar();

                
                if (count1 == 0)
                {
                    MessageBox.Show("No data found to delete.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand checkUsageCmd = new SqlCommand("SELECT COUNT(*) FROM teachertab WHERE subjectid = @subjectid", con);
                checkUsageCmd.Parameters.AddWithValue("@subjectid", subjectId);

                int count2 = (int)checkUsageCmd.ExecuteScalar();

                if (count2 > 0)
                {
                    
                    MessageBox.Show("This subject is currently in use and cannot be deleted.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
               

                SqlCommand deleteCmd = new SqlCommand("DELETE FROM subtab WHERE subjectid = @subjectid", con);
                deleteCmd.Parameters.AddWithValue("@subjectid", subjectId);

                deleteCmd.ExecuteNonQuery();
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
