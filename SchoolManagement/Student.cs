using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
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
            LoadTeacher();


            txtStudentGen.Items.Add("M");
            txtStudentGen.Items.Add("F");
            txtStudentGen.Items.Add("They");
            txtStudentGen.SelectedIndex = 0;

            txtStudentDob.Format = DateTimePickerFormat.Custom;
            txtStudentDob.CustomFormat = "dd/MM/yyyy";

            txtEnrollmentDate.Format = DateTimePickerFormat.Custom;
            txtEnrollmentDate.CustomFormat = "dd/MM/yyyy";

        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
        }

        private void LoadTeacher()
        {
            try
            {
                using (SqlConnection con = GetSqlConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select teacherName From teachertab", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    txtTeacher.Items.Clear();
                    while (reader.Read())
                    {
                        txtTeacher.Items.Add(reader["teacherName"].ToString());
                    }
                    reader.Close();
                }
                if (txtTeacher.Items.Count > 0)
                {
                    txtTeacher.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading teacher: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void LoadData()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cnn = new SqlCommand(@"
                SELECT 
                    s.studentid,
                    s.studentname,
                    s.dob,
                    s.gender,
                    s.phone,
                    s.email,
                    s.address,
                    s.enrollmentDate,
                    t.teachername AS TeacherName 
                FROM
                    studentab s
                LEFT JOIN
                    teachertab t
                ON
                    s.teacherid = t.teacherid", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;

                dataGridView1.Columns["dob"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dataGridView1.Columns["enrollmentDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private bool IsValidEmail(string email)
        {
            
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtStudentDob.CustomFormat = "dd/MM/yyyy";
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

            if (string.IsNullOrWhiteSpace(txtTeacher.Text))
            {
                MessageBox.Show("Please enter a teacher name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int teacherId;
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand getTeacherIdCmd = new SqlCommand("SELECT teacherid FROM teachertab WHERE teachername = @teachername", con);
                getTeacherIdCmd.Parameters.AddWithValue("@teachername", txtTeacher.Text);
                object teacherIdObj = getTeacherIdCmd.ExecuteScalar();

                if (teacherIdObj == null)
                {
                    MessageBox.Show("Teacher name not found. Please enter a valid teacher name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                teacherId = Convert.ToInt32(teacherIdObj);

                SqlCommand checkIdCmd = new SqlCommand("SELECT COUNT(*) FROM studentab WHERE studentid = @studentid", con);
                checkIdCmd.Parameters.AddWithValue("@studentid", int.Parse(txtStudentId.Text));
                int idCount = (int)checkIdCmd.ExecuteScalar();

                if (idCount > 0)
                {
                    MessageBox.Show("This Student ID already exists. Please enter a unique ID.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

           
                SqlCommand cnn = new SqlCommand(@"
            INSERT INTO studentab 
            (studentid, studentname, dob, gender, phone, email, address, enrollmentDate, teacherid) 
            VALUES 
            (@studentid, @studentname, @dob, @gender, @phone, @address, @email, @enrollmentDate, @teacherid)", con);

                cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
                cnn.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
                cnn.Parameters.AddWithValue("@Dob", txtStudentDob.Value.Date);
                cnn.Parameters.AddWithValue("@Gender", txtStudentGen.SelectedItem.ToString());
                cnn.Parameters.AddWithValue("@Phone", txtStudentPhone.Text);
                cnn.Parameters.AddWithValue("@Email", txtStudentEmail.Text);
                cnn.Parameters.AddWithValue("@Address", txtStudentAdd.Text);
                cnn.Parameters.AddWithValue("@EnrollmentDate", txtEnrollmentDate.Value.Date);
                cnn.Parameters.AddWithValue("@TeacherId", teacherId);

                cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e);
            LoadData();
        }





        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            if (!IsValidEmail(txtStudentEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                    SqlCommand checkStudentExistsCmd = new SqlCommand("SELECT COUNT(*) FROM studentab WHERE studentid = @studentid", con);
                    checkStudentExistsCmd.Parameters.AddWithValue("@studentid", int.Parse(txtStudentId.Text));
                    int studentExists = (int)checkStudentExistsCmd.ExecuteScalar();

                    if (studentExists == 0)
                    {
                        MessageBox.Show("Student ID not found. Please enter a valid Student ID or use the Save button to add a new student.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int teacherId;
                using (SqlCommand getTeacherIdCmd = new SqlCommand("SELECT teacherid FROM teachertab WHERE teachername = @teachername", con))
                {
                    getTeacherIdCmd.Parameters.AddWithValue("@teachername", txtTeacher.Text);
                    object teacherIdObj = getTeacherIdCmd.ExecuteScalar();
                    if (teacherIdObj == null)
                    {
                        MessageBox.Show("Teacher name not found. Please enter a valid teacher name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    teacherId = Convert.ToInt32(teacherIdObj);
                }

                SqlCommand cnn = new SqlCommand(@"
                    UPDATE studentab 
                    SET 
                        studentname = @studentname, 
                        dob = @dob, 
                        gender = @gender, 
                        phone = @phone, 
                        email = @email, 
                        address = @address, 
                        enrollmentDate = @enrollmentDate, 
                        teacherid = @teacherid
                    WHERE 
                        studentid = @studentid", con);
                        cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
                        cnn.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
                        cnn.Parameters.AddWithValue("@Dob", txtStudentDob.Value.Date);
                        cnn.Parameters.AddWithValue("@Gender", txtStudentGen.SelectedItem.ToString());
                        cnn.Parameters.AddWithValue("@Phone", txtStudentPhone.Text);
                        cnn.Parameters.AddWithValue("@Email", txtStudentEmail.Text);
                        cnn.Parameters.AddWithValue("@Address", txtStudentAdd.Text);
                        cnn.Parameters.AddWithValue("@EnrollmentDate", txtEnrollmentDate.Value.Date);
                        cnn.Parameters.AddWithValue("@TeacherId", teacherId);
                        cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e); 
            LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentId.Text))
            {
                MessageBox.Show("Please enter a student ID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int studentId = int.Parse(txtStudentId.Text);

            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand checkUsageCmd = new SqlCommand("SELECT COUNT(*) FROM studentSectiontab WHERE studentid = @studentid", con);
                checkUsageCmd.Parameters.AddWithValue("@studentid", studentId);

                int usageCount = (int)checkUsageCmd.ExecuteScalar();

                if (usageCount > 0)
                {
                    MessageBox.Show("This student is currently enrolled in a section and cannot be deleted.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SqlCommand checkExistCmd = new SqlCommand("SELECT COUNT(*) FROM studentab WHERE studentid = @studentid", con);
                checkExistCmd.Parameters.AddWithValue("@studentid", studentId);

                int existCount = (int)checkExistCmd.ExecuteScalar();

                if (existCount == 0)
                {
                    MessageBox.Show("No data found to delete.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SqlCommand cnn = new SqlCommand("DELETE FROM studentab WHERE studentid=@studentid", con);
                cnn.Parameters.AddWithValue("@StudentId", int.Parse(txtStudentId.Text));
                cnn.ExecuteNonQuery();
            }

            MessageBox.Show("Record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset(sender, e); 
            LoadData();
        }

        private void reset(object sender, EventArgs e)
        {
            txtStudentId.Text = "";
            txtStudentName.Text = "";
            txtStudentDob.Value = DateTime.Now;  
            txtStudentDob.CustomFormat = "dd/MM/yyyy";
            txtStudentGen.SelectedIndex = 0;
            txtStudentPhone.Text = "";
            txtStudentEmail.Text = "";
            txtStudentAdd.Text = "";
            txtEnrollmentDate.Value = DateTime.Now;
            txtEnrollmentDate.CustomFormat = "dd/MM/yyyy";
            txtTeacher.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("select * from studentab", con);
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

                txtStudentId.Text = row.Cells["studentid"].Value?.ToString() ?? "";
                txtStudentName.Text = row.Cells["studentname"].Value?.ToString() ?? "";
                txtStudentDob.Value = row.Cells["dob"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["dob"].Value) : DateTime.Now;
                txtStudentGen.Text = row.Cells["gender"].Value?.ToString() ?? "";
                txtStudentPhone.Text = row.Cells["phone"].Value?.ToString() ?? "";
                txtStudentEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
                txtStudentAdd.Text = row.Cells["address"].Value?.ToString() ?? "";
                txtEnrollmentDate.Value = row.Cells["enrollmentDate"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["enrollmentDate"].Value) : DateTime.Now;
                txtTeacher.Text = row.Cells["TeacherName"].Value?.ToString() ?? "";
            }
        }
    }
}
