using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUpdateAccount : Form
    {
        private string username, editusername, editpassword, edittype, editstatus;
        private int errorcount;
        Class1 updateaccounts = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
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
            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbtype, "Select status");
                errorcount++;
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if(errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateaccounts.executeSQL("UPDATE tblaccounts SET password = '" + txtpassword.Text + "', usertype = '" + cmbtype.Text.ToUpper() + "', status = '" + cmbstatus.Text.ToUpper() + 
                            "' WHERE username = '" + txtusername.Text + "'");
                        if (updateaccounts.rowAffected > 0)
                        {
                            updateaccounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                            + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                            + "', 'Update', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");
                            MessageBox.Show("Account Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnclear_Click(object sender, EventArgs e)
        {          
            txtusername.Clear();
        }

        private void cbshow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbshow.Checked == true)
            {

                txtpassword.PasswordChar = '\0';
            }
            else
            {

                txtpassword.PasswordChar = '*';
            }
        }

        public frmUpdateAccount(string editusername, string editpassword, string edittype, string editstatus, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editusername = editusername;
            this.editpassword = editpassword;
            this.edittype = edittype;
            this.editstatus = editstatus;
        }

        private void frmUpdateAccount_Load(object sender, EventArgs e)
        {
            txtusername.Text = editusername;
            txtpassword.Text = editpassword;
            if(edittype == "ADMINISTRATOR")
            {
                cmbtype.SelectedIndex = 0;
            }
            else if (edittype == "BRANCH ADMINISTRATOR")
            {
                cmbtype.SelectedIndex = 1;
            }
            else
            { 
                cmbtype.SelectedIndex = 2;
            }

            if (editstatus == "ACTIVE")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
            }
        }
    }
}
