using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmAdd : Form
    {
        private string username;
        private int errorcount;
        Class1 newaccounts = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        public frmAdd(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateForm() 
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtusername.Text)) 
            {
                errorProvider1.SetError(txtusername, "Username is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Password is empty");
                errorcount++;
            }
            if (txtpassword.TextLength < 6)
            {
                errorProvider1.SetError(txtusername, "Password must be atleast 6 characters");
                errorcount++;
            }
            if (cmbtype.SelectedIndex < 0) 
            {
                errorProvider1.SetError(cmbtype, "Select usertype");
                errorcount++;
            }
            try
            {
                DataTable dt = newaccounts.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "'");
                if (dt.Rows.Count > 0) 
                {
                    errorProvider1.SetError(txtusername, "Username already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing account", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0) 
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) 
                {
                    try
                    {
                        newaccounts.executeSQL("INSERT INTO tblaccounts(username, password, usertype, status, createdby, datecreated) VALUES('" + txtusername.Text + "', '" + txtpassword.Text + "' , '" +
                            cmbtype.Text.ToUpper() + "', 'ACTIVE', '" + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newaccounts.rowAffected > 0)
                        {
                            newaccounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                            + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                            + "', 'Add', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");
                            MessageBox.Show("New Account added!", "Message", MessageBoxButtons.OK,MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
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

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            cmbtype.SelectedIndex = -1;
        }

        private void frmAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
