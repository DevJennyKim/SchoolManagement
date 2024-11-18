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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            DisplayStudentCount();
            DisplayTeacherCount();
            DisplaySubjectStudentCounts();
            DisplaySubjectTeacnerCounts();
        }

        private void DisplaySubjectStudentCounts()
        {

            DisplayStudentCountForSubject(1, txtSMath);      
            DisplayStudentCountForSubject(2, txtSEng);      
            DisplayStudentCountForSubject(3, txtSSc);        
            DisplayStudentCountForSubject(4, txtSHis);      
            DisplayStudentCountForSubject(5, txtSPhysical);  
            DisplayStudentCountForSubject(6, txtSChemi);     
            DisplayStudentCountForSubject(7, txtSPhysics);  
            DisplayStudentCountForSubject(8, txtSArt);      
            DisplayStudentCountForSubject(9, txtSComSc);    
        }

        private void DisplaySubjectTeacnerCounts()
        {

            DisplayTeacherCountForSubject(1, txtTMath);
            DisplayTeacherCountForSubject(2, txtTEng);
            DisplayTeacherCountForSubject(3, txtTSc);
            DisplayTeacherCountForSubject(4, txtTHis);
            DisplayTeacherCountForSubject(5, txtTPhysical);
            DisplayTeacherCountForSubject(6, txtTChemi);
            DisplayTeacherCountForSubject(7, txtTPhysics);
            DisplayTeacherCountForSubject(8, txtTArt);
            DisplayTeacherCountForSubject(9, txtTComSc);
        }
        private void DisplayTeacherCountForSubject(int subjectId, TextBox targetTextBox)
        {
            string query = @"
        SELECT COUNT(DISTINCT TeacherId) 
        FROM teachertab
        WHERE SubjectId = @SubjectId";

            using (SqlConnection conn = GetSqlConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);  
                    int teacherCount = (int)cmd.ExecuteScalar();
                    targetTextBox.Text = teacherCount.ToString(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void DisplayStudentCountForSubject(int subjectId, TextBox targetTextBox)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM studentSectiontab ss
                JOIN sectiontab s ON ss.SectionId = s.SectionId
                WHERE s.SubjectId = @SubjectId";

            using (SqlConnection conn = GetSqlConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);  
                    int studentCount = (int)cmd.ExecuteScalar(); 
                    targetTextBox.Text = studentCount.ToString(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        
        private void DisplayStudentCount()
        {
            string query = "SELECT COUNT(*) FROM studentab";  
            using (SqlConnection conn = GetSqlConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int studentCount = (int)cmd.ExecuteScalar();
                    txtStudentNum.Text = studentCount.ToString(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DisplayTeacherCount()
        {
            string query = "SELECT COUNT(*) FROM teachertab";
            using (SqlConnection conn = GetSqlConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int studentCount = (int)cmd.ExecuteScalar();
                    txtTeacherNum.Text = studentCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            m.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Student st = new Student();
            st.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Subject sb = new Subject();
            sb.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Teacher tc = new Teacher();
            tc.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Section st = new Section();
            st.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StudentSection ss = new StudentSection();
            ss.Show();

        }
    }
}
