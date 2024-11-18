using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace SchoolManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private SqlConnection GetSqlConnection()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./Database.config");

            string connectionString = doc.SelectSingleNode("//connectionStrings/add[@name='SchoolDbConnection']").Attributes["connectionString"].Value;
            return new SqlConnection(connectionString);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(texUsername.Text))
            {
                MessageBox.Show("Please enter your username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(texPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (SqlConnection con = GetSqlConnection())
            {
                con.Open();

                string username = texUsername.Text;
                string password = texPassword.Text;


                SqlCommand cnn = new SqlCommand("select Username,Password from logintab where Username=@username and password=@password", con);
                cnn.Parameters.AddWithValue("@username", username);
                cnn.Parameters.AddWithValue("@password", password);

                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    Main mn = new Main();
                    mn.Show();

                    this.Hide();

                }
                else
                {

                    MessageBox.Show("Invalid username or password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                con.Close();
            }
        }
    }
}