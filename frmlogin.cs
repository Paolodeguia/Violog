﻿using System.Data;
using System.Windows.Forms;
using System;

namespace CS311A2024_DATABASE
{
    public partial class frmlogin : Form
    {
        public frmlogin()
        {
            InitializeComponent();
            
        }

        Class1 login = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        private int errorcount;

        private void btnlogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorProvider1.SetError(txtusername, "Input is empty");
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Input is empty");
            }
            
            errorcount = 0;
            foreach (Control c in errorProvider1.ContainerControl.Controls)
            {
                if (!(string.IsNullOrEmpty(errorProvider1.GetError(c))))
                {
                    errorcount++;
                }
            }
            
            if (errorcount == 0)
            {
                try
                {
                    DataTable dt = login.GetData("SELECT * FROM tblaccounts WHERE username = '" +
                   txtusername.Text + "'AND password = '" + txtpassword.Text + "'AND status = 'ACTIVE'");
                    if (dt.Rows.Count > 0)
                    {
                        frmMain mainform = new frmMain(txtusername.Text, dt.Rows[0].Field<string>("usertype"));
                        mainform.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect account information or account is inactive", "Message",
                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on Login", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
       
        private void cbshow_CheckedChanged(object sender, EventArgs e)
        {

            if (cbshow.Checked)
            {
                
                txtpassword.PasswordChar = '\0';
            }
            else
            {
               
                txtpassword.PasswordChar = '*';
            }

        }   
        private void btnreset_Click(object sender, EventArgs e)
        {
            txtpassword.Clear();
            txtusername.Clear();
            txtusername.Focus();
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                btnlogin_Click(sender, e); 
            }
        }

        private void frmlogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
