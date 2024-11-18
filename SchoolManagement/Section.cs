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
    public partial class Section : Form
    {
        public Section()
        {
            InitializeComponent();
        }


        private void Section_Load(object sender, EventArgs e)
        {
            LoadTeacher();
            LoadSubjects();
            LoadData();

            txtDayofWeek.Items.Add("Monday");
            txtDayofWeek.Items.Add("Tuesday");
            txtDayofWeek.Items.Add("Wednesday");
            txtDayofWeek.Items.Add("Thursday");
            txtDayofWeek.Items.Add("Friday");
            txtDayofWeek.SelectedIndex = 0;

            txtBlock.Items.Add("Block 1");
            txtBlock.Items.Add("Block 2");
            txtBlock.Items.Add("Block 3");
            txtBlock.Items.Add("Block 4");
            txtBlock.SelectedIndex = 0;

            txtBlock.SelectedIndexChanged += TxtBlock_SelectedIndexChanged;

            SetBlockTimes(txtBlock.SelectedIndex);

            txtClassRoom.Items.Add("Room 101");
            txtClassRoom.Items.Add("Room 102");
            txtClassRoom.Items.Add("Room 103");
            txtClassRoom.Items.Add("Room 104");
            txtClassRoom.Items.Add("Room 105");
            txtClassRoom.Items.Add("Room 106");
            txtClassRoom.Items.Add("Room 107");
            txtClassRoom.Items.Add("Room 108");

            txtClassRoom.Items.Add("Room 201");
            txtClassRoom.Items.Add("Room 202");
            txtClassRoom.Items.Add("Room 203");
            txtClassRoom.SelectedIndex = 0;



        }

        private void TxtBlock_SelectedIndexChanged(object sender, EventArgs e)
        {            
            SetBlockTimes(txtBlock.SelectedIndex);
        }

        private void SetBlockTimes(int blockIndex)
        {            
            switch (blockIndex)
            {
                case 0: 
                    txtStartTime.Text = "09:00";
                    txtEndTime.Text = "10:30";
                    break;
                case 1: 
                    txtStartTime.Text = "10:45";
                    txtEndTime.Text = "12:15";
                    break;
                case 2: 
                    txtStartTime.Text = "13:00";
                    txtEndTime.Text = "14:30";
                    break;
                case 3: 
                    txtStartTime.Text = "14:45";
                    txtEndTime.Text = "16:15";
                    break;
                default:
                    txtStartTime.Text = "";
                    txtEndTime.Text = "";
                    break;
            }
        }
        
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
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
                    s.sectionId,
                    t.teachername AS teachername,
                    sb.subjectname AS subjectname,
                    s.block,
                    s.dayofweek,
                    s.startTime,
                    s.endTime,
                    s.classroom
                FROM
                    sectiontab s
                LEFT JOIN
                    teachertab t ON s.teacherid = t.teacherid
                LEFT JOIN
                    subtab sb ON s.subjectid = sb.subjectid
                ORDER BY 
                    CASE s.dayofweek
                        WHEN 'Monday' THEN 1
                        WHEN 'Tuesday' THEN 2
                        WHEN 'Wednesday' THEN 3
                        WHEN 'Thursday' THEN 4
                        WHEN 'Friday' THEN 5
                        ELSE 6
                END", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    if (row["startTime"] is TimeSpan startTime)
                        row["startTime"] = startTime.ToString(@"hh\:mm");

                    if (row["endTime"] is TimeSpan endTime)
                        row["endTime"] = endTime.ToString(@"hh\:mm");
                }
                dataGridView1.DataSource = table;
            }
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtSectionId.Text = row.Cells["sectionId"].Value?.ToString() ?? "";
                txtTeacher.Text = row.Cells["teacherName"].Value?.ToString() ?? "";
                txtSubject.Text = row.Cells["subjectName"].Value?.ToString() ?? "";
                txtDayofWeek.Text = row.Cells["dayofweek"].Value?.ToString() ?? "";
                txtBlock.Text = row.Cells["block"].Value?.ToString() ?? "";
                if (row.Cells["starttime"].Value != null && TimeSpan.TryParse(row.Cells["starttime"].Value.ToString(), out TimeSpan startTime))
                {
                    txtStartTime.Text = startTime.ToString(@"hh\:mm");
                }
                else
                {
                    txtStartTime.Text = "";
                }
                if (row.Cells["endtime"].Value != null && TimeSpan.TryParse(row.Cells["endtime"].Value.ToString(), out TimeSpan endTime))
                {
                    txtEndTime.Text = endTime.ToString(@"hh\:mm");
                }
                else
                {
                    txtEndTime.Text = "";
                }
                txtClassRoom.Text = row.Cells["classroom"].Value?.ToString() ?? "";

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSectionId.Text) || string.IsNullOrEmpty(txtTeacher.Text) ||
                string.IsNullOrEmpty(txtSubject.Text) || string.IsNullOrEmpty(txtDayofWeek.Text) ||
                string.IsNullOrEmpty(txtBlock.Text) || string.IsNullOrEmpty(txtStartTime.Text) ||
                string.IsNullOrEmpty(txtEndTime.Text) || string.IsNullOrEmpty(txtClassRoom.Text))
            {
                MessageBox.Show("All fields must be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsClassroomInUse(txtClassRoom.Text, txtBlock.Text, txtDayofWeek.Text))
            {
                MessageBox.Show("This classroom is already in use during the selected time slot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsTeacherScheduledAtSameTime())
            {
                MessageBox.Show("A class already exists at the same time. Please choose a different time slot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidTeacherForSubject())
            {
                MessageBox.Show("The selected teacher cannot teach the chosen subject.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection con = GetSqlConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                INSERT INTO sectiontab (teacherid, subjectid, block, dayofweek, startTime, endTime, classroom)
                VALUES (@teacherid, @subjectid, @block, @dayofweek, @startTime, @endTime, @classroom)", con);

                    cmd.Parameters.AddWithValue("@teacherid", GetTeacherIdByName(txtTeacher.Text));
                    cmd.Parameters.AddWithValue("@subjectid", GetSubjectIdByName(txtSubject.Text));
                    cmd.Parameters.AddWithValue("@block", txtBlock.Text);
                    cmd.Parameters.AddWithValue("@dayofweek", txtDayofWeek.Text);
                    cmd.Parameters.AddWithValue("@startTime", txtStartTime.Text);
                    cmd.Parameters.AddWithValue("@endTime", txtEndTime.Text);
                    cmd.Parameters.AddWithValue("@classroom", txtClassRoom.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsValidTeacherForSubject()
        {
            int teacherId = GetTeacherIdByName(txtTeacher.Text);
            int subjectId = GetSubjectIdByName(txtSubject.Text);

            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM teachertab
            WHERE teacherid = @teacherid AND subjectid = @subjectid", con);

                cmd.Parameters.AddWithValue("@teacherid", teacherId);
                cmd.Parameters.AddWithValue("@subjectid", subjectId);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private int GetTeacherIdByName(string teacherName)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT teacherid FROM teachertab WHERE teacherName = @teacherName", con);
                cmd.Parameters.AddWithValue("@teacherName", teacherName);
                return (int)cmd.ExecuteScalar();
            }
        }
        private int GetSubjectIdByName(string subjectName)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT subjectid FROM subtab WHERE subjectName = @subjectName", con);
                cmd.Parameters.AddWithValue("@subjectName", subjectName);
                return (int)cmd.ExecuteScalar();
            }
        }

        private bool IsTeacherScheduledAtSameTime()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM sectiontab
            WHERE teacherid = @teacherid AND block = @block AND dayofweek = @dayofweek
            AND startTime = @startTime AND endTime = @endTime", con);

                cmd.Parameters.AddWithValue("@teacherid", GetTeacherIdByName(txtTeacher.Text));
                cmd.Parameters.AddWithValue("@block", txtBlock.Text);
                cmd.Parameters.AddWithValue("@dayofweek", txtDayofWeek.Text);
                cmd.Parameters.AddWithValue("@startTime", txtStartTime.Text);
                cmd.Parameters.AddWithValue("@endTime", txtEndTime.Text);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool IsClassroomInUse(string classroom, string block, string dayOfWeek)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM sectiontab
            WHERE classroom = @classroom AND block = @block AND dayofweek = @dayOfWeek", con);

                cmd.Parameters.AddWithValue("@classroom", classroom);
                cmd.Parameters.AddWithValue("@block", block);
                cmd.Parameters.AddWithValue("@dayOfWeek", dayOfWeek);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSectionId.Text) || string.IsNullOrEmpty(txtTeacher.Text) ||
        string.IsNullOrEmpty(txtSubject.Text) || string.IsNullOrEmpty(txtDayofWeek.Text) ||
        string.IsNullOrEmpty(txtBlock.Text) || string.IsNullOrEmpty(txtStartTime.Text) ||
        string.IsNullOrEmpty(txtEndTime.Text) || string.IsNullOrEmpty(txtClassRoom.Text))
            {
                MessageBox.Show("All fields must be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsTeacherScheduledAtSameTimeForUpdate())
            {
                MessageBox.Show("A class already exists at the same time. Please choose a different time slot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidTeacherForSubject())
            {
                MessageBox.Show("The selected teacher cannot teach the chosen subject.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsClassroomInUse(txtClassRoom.Text, txtBlock.Text, txtDayofWeek.Text))
            {
                MessageBox.Show("This classroom is already in use during the selected time slot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (SqlConnection con = GetSqlConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE sectiontab
                SET teacherid = @teacherid,
                    subjectid = @subjectid,
                    block = @block,
                    dayofweek = @dayofweek,
                    startTime = @startTime,
                    endTime = @endTime,
                    classroom = @classroom
                WHERE sectionId = @sectionId", con);

                    cmd.Parameters.AddWithValue("@sectionId", txtSectionId.Text);
                    cmd.Parameters.AddWithValue("@teacherid", GetTeacherIdByName(txtTeacher.Text));
                    cmd.Parameters.AddWithValue("@subjectid", GetSubjectIdByName(txtSubject.Text));
                    cmd.Parameters.AddWithValue("@block", txtBlock.Text);
                    cmd.Parameters.AddWithValue("@dayofweek", txtDayofWeek.Text);
                    cmd.Parameters.AddWithValue("@startTime", txtStartTime.Text);
                    cmd.Parameters.AddWithValue("@endTime", txtEndTime.Text);
                    cmd.Parameters.AddWithValue("@classroom", txtClassRoom.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Class has been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsTeacherScheduledAtSameTimeForUpdate()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM sectiontab
            WHERE teacherid = @teacherid AND block = @block AND dayofweek = @dayofweek
            AND startTime = @startTime AND endTime = @endTime
            AND sectionId != @sectionId", con);

                cmd.Parameters.AddWithValue("@teacherid", GetTeacherIdByName(txtTeacher.Text));
                cmd.Parameters.AddWithValue("@block", txtBlock.Text);
                cmd.Parameters.AddWithValue("@dayofweek", txtDayofWeek.Text);
                cmd.Parameters.AddWithValue("@startTime", txtStartTime.Text);
                cmd.Parameters.AddWithValue("@endTime", txtEndTime.Text);
                cmd.Parameters.AddWithValue("@sectionId", txtSectionId.Text);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                SqlCommand cnn = new SqlCommand("DELETE FROM sectiontab WHERE sectionid=@sectionid", con);
                cnn.Parameters.AddWithValue("@sectionid", int.Parse(txtSectionId.Text));
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

                SqlCommand cnn = new SqlCommand("select * from sectiontab", con);
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
                LoadData();
            }
        }
        

        private void reset(object sender, EventArgs e)
        {
            txtSectionId.Text = "";
            txtTeacher.SelectedIndex = 0;
            txtSubject.SelectedIndex = 0;
            txtBlock.SelectedIndex = 0;
            txtDayofWeek.SelectedIndex = 0;
            txtStartTime.Value = DateTime.Today;
            txtStartTime.CustomFormat = "HH:mm";
            txtEndTime.Value = DateTime.Today;
            txtEndTime.CustomFormat = "HH:mm";
            txtClassRoom.SelectedIndex = 0;
        }

        private void TxtTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTeacher = txtTeacher.SelectedItem.ToString();

            int teacherId = GetTeacherIdByName(selectedTeacher);

            LoadSubjectsForTeacher(teacherId);
        }
        private void LoadSubjectsForTeacher(int teacherId)
        {
            try
            {
                using (SqlConnection con = GetSqlConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT sb.subjectName 
                FROM subtab sb
                INNER JOIN teachertab tt ON sb.subjectid = tt.subjectid
                WHERE tt.teacherid = @teacherId", con);

                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
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
                MessageBox.Show($"Error loading subjects for teacher: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
