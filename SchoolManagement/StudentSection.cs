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
    public partial class StudentSection : Form
    {
        public StudentSection()
        {
            InitializeComponent();
        }
        private void StudentSection_Load(object sender, EventArgs e)
        {
            LoadStudentNames(); 
            LoadAllStudentSectionInfo();
        }
        private void LoadAllStudentSectionInfo()
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                string query = @"
                    SELECT s.StudentName, sec.block, sub.SubjectName, sec.DayOfWeek, sec.StartTime, sec.EndTime, sec.Classroom, t.TeacherName
                    FROM studentSectiontab ss
                    JOIN Sectiontab sec ON ss.SectionId = sec.SectionId
                    JOIN Subtab sub ON sec.SubjectId = sub.SubjectId
                    JOIN Teachertab t ON sec.TeacherId = t.TeacherId
                    JOIN Studentab s ON ss.StudentId = s.StudentId";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
        }
        private void LoadStudentNames()
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                string query = "SELECT StudentId, StudentName FROM studentab"; 
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                txtStudent.DisplayMember = "StudentName";
                txtStudent.ValueMember = "StudentId";
                txtStudent.DataSource = dt;
            }
        }
        private void LoadStudentSectionInfo(int studentId)
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                string query = @"
                    SELECT s.StudentName, sec.block, sub.SubjectName, sec.DayOfWeek, sec.StartTime, sec.EndTime, sec.Classroom, t.TeacherName
                    FROM studentSectiontab ss
                    JOIN sectiontab sec ON ss.SectionId = sec.SectionId
                    JOIN Subtab sub ON sec.SubjectId = sub.SubjectId
                    JOIN Teachertab t ON sec.TeacherId = t.TeacherId
                    JOIN Studentab s ON ss.StudentId = s.StudentId
                    WHERE ss.StudentId = @StudentId";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@StudentId", studentId);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void txtStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtStudent.SelectedValue != null)
            {
                int studentId = (int)txtStudent.SelectedValue;
                LoadStudentSectionInfo(studentId);
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            LoadAllStudentSectionInfo();
        }
    }
}
