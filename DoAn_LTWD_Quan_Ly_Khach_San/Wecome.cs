﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;

namespace DoAn_LTWD_Quan_Ly_Khach_San
{
    public partial class Wecome : Form
    {
        public Wecome()
        {
            InitializeComponent();
            loadDataUser();
        }

        private void Wecome_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (this.Opacity < 1) this.Opacity += 0.05;
            {
                progressBar1.Value += 3;              
            }
            if (progressBar1.Value == 102)
            {
                timer1.Stop();
                TrangChu a = new TrangChu();
                a.Hide();
                a.Show();
                this.Close();
            }
        }
        public void loadDataUser()
        {
            lbl_Lname.Text = UserCache.LastName +" , " + UserCache.FirstName;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
