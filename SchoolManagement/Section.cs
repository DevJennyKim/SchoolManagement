﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            txtSection.Items.Add("A");
            txtSection.Items.Add("B");
            txtSection.Items.Add("C");
            txtSection.Items.Add("D");
            txtSection.Items.Add("F");


            txtSection.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
