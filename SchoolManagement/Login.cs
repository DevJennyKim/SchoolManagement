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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JBJ28O4;Initial Catalog=schooldb;Integrated Security=True;Trust Server Certificate=True");
            con.Open();
            string username = texUsername.Text;
            string password = texPassword.Text;
            SqlCommand cnn = new SqlCommand("select Username,Password from logintab where Username='" + texUsername.Text + "'and password='" + texPassword.Text + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0) {
                Main mn = new Main();
                mn.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
            con. Close();
        }
    }
}
