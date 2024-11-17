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
    public partial class Teacher : Form
    {
        public Teacher()
        {
            InitializeComponent();
        }

        private void Teacher_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadSubjects();


            txtTeacherGen.Items.Add("M");
            txtTeacherGen.Items.Add("F");
            txtTeacherGen.Items.Add("They");
            txtTeacherGen.SelectedIndex = 0;


            txtHireDate.Format = DateTimePickerFormat.Custom;
            txtHireDate.CustomFormat = "dd/MM/yyyy";

            
        }

        private void LoadSubjects()
        {
            try
            {
                using (SqlConnection con = GetSqlConnection())
                {
                    con.Open();


                    SqlCommand cmd = new SqlCommand("SELECT subjectName FROM subtab", con);
                    SqlDataReader reader = cmd.ExecuteReader();


                    txtSubject.Items.Clear();
                    while (reader.Read())
                    {
                        txtSubject.Items.Add(reader["subjectName"].ToString());
                    }

                    reader.Close();
                }


                if (txtSubject.Items.Count > 0)
                {
                    txtSubject.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
        }
        private bool IsValidEmail(string email)
        {

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private void txtHireDate_ValueChanged(object sender, EventArgs e)
        {
            txtHireDate.CustomFormat = "dd/MM/yyyy";
        }

        private void txtHireDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                txtHireDate.CustomFormat = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherName.Text))
            {
                MessageBox.Show("Please enter a student name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTeacherPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidEmail(txtTeacherEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Please enter a Subject.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int subjectId;
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();


                SqlCommand getsubjectIdCmd = new SqlCommand("SELECT subjectId FROM subtab WHERE subjectname = @subjectname", con);
                getsubjectIdCmd.Parameters.AddWithValue("@subjectname", txtSubject.SelectedItem.ToString());
                object subjectIdObj = getsubjectIdCmd.ExecuteScalar();

                if (subjectIdObj == null)
                {
                    MessageBox.Show("subject name not found. Please enter a valid subject name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                subjectId = Convert.ToInt32(subjectIdObj);

                SqlCommand checkIdCmd = new SqlCommand("SELECT COUNT(*) FROM teachertab WHERE teacherId = @teacherId", con);
                checkIdCmd.Parameters.AddWithValue("@teacherId", int.Parse(txtTeacherId.Text));
                int idCount = (int)checkIdCmd.ExecuteScalar();

                if (idCount > 0)
                {
                    MessageBox.Show("This Subject ID already exists. Please enter a unique ID.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                SqlCommand cnn = new SqlCommand(@"
            INSERT INTO teachertab 
            (teacherid, teachername, gender, phone, email, hiredate, subjectid) 
            VALUES 
            (@teacherid, @teachername, @gender, @phone, @email, @hiredate, @subjectid)", con);

                cnn.Parameters.AddWithValue("@teacherid", int.Parse(txtTeacherId.Text));
                cnn.Parameters.AddWithValue("@teachername", txtTeacherName.Text);
                cnn.Parameters.AddWithValue("@Gender", txtTeacherGen.SelectedItem.ToString());
                cnn.Parameters.AddWithValue("@Phone", txtTeacherPhone.Text);
                cnn.Parameters.AddWithValue("@Email", txtTeacherEmail.Text);
                cnn.Parameters.AddWithValue("@hiredate", txtHireDate.Value.Date);
                cnn.Parameters.AddWithValue("@subjectid", subjectId);
                cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e);
            LoadData();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

             
                SqlCommand getSubjectIdCmd = new SqlCommand("SELECT subjectId FROM subtab WHERE subjectname = @subjectname", con);
                getSubjectIdCmd.Parameters.AddWithValue("@subjectname", txtSubject.SelectedItem.ToString());
                object subjectIdObj = getSubjectIdCmd.ExecuteScalar();

                if (subjectIdObj == null)
                {
                    MessageBox.Show("Subject name not found. Please enter a valid subject name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int subjectId = Convert.ToInt32(subjectIdObj);

               
                SqlCommand cnn = new SqlCommand(@"
            UPDATE teachertab 
            SET 
                teachername = @teachername,
                gender = @gender,
                phone = @phone,
                email = @email,
                hiredate = @hiredate,
                subjectid = @subjectid 
            WHERE 
                teacherid = @teacherid", con);

                cnn.Parameters.AddWithValue("@teacherid", int.Parse(txtTeacherId.Text));
                cnn.Parameters.AddWithValue("@teachername", txtTeacherName.Text);
                cnn.Parameters.AddWithValue("@Gender", txtTeacherGen.SelectedItem.ToString());
                cnn.Parameters.AddWithValue("@Phone", txtTeacherPhone.Text);
                cnn.Parameters.AddWithValue("@Email", txtTeacherEmail.Text);
                cnn.Parameters.AddWithValue("@hiredate", txtHireDate.Value.Date);
                cnn.Parameters.AddWithValue("@subjectid", subjectId);

                cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e);
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cnn = new SqlCommand(@"
                SELECT 
                    t.teacherid,
                    t.teachername,
                    t.gender,
                    t.phone,
                    t.email,
                    t.hireDate,
                    s.subjectName AS subjectName 
                FROM
                    teachertab t
                LEFT JOIN
                    subtab s
                ON
                    t.subjectId = s.subjectId", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;

              
                dataGridView1.Columns["hireDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("DELETE FROM teachertab WHERE teacherid=@teacherid", con);
                cnn.Parameters.AddWithValue("@teacherid", int.Parse(txtTeacherId.Text));
                cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e);
            LoadData();
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("select * from teachertab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
                LoadData();
            }
        }
        private void reset(object sender, EventArgs e)
        {
            txtTeacherId.Text = "";
            txtTeacherName.Text = "";
            txtHireDate.Value = DateTime.Now;
            txtHireDate.CustomFormat = "dd/MM/yyyy";
            txtTeacherGen.SelectedIndex = 0;
            txtTeacherPhone.Text = "";
            txtTeacherEmail.Text = "";
            txtSubject.Text = "";
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtTeacherId.Text = row.Cells["teacherid"].Value?.ToString() ?? "";
                txtTeacherName.Text = row.Cells["teachername"].Value?.ToString() ?? "";
                txtTeacherGen.Text = row.Cells["gender"].Value?.ToString() ?? "";
                txtTeacherPhone.Text = row.Cells["phone"].Value?.ToString() ?? "";
                txtTeacherEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
                txtHireDate.Value = row.Cells["hireDate"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["hiredate"].Value) : DateTime.Now;
                txtSubject.SelectedItem = row.Cells["SubjectName"].Value?.ToString() ?? "";
            }
        }
    }
}
