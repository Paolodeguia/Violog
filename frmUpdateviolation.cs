using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUpdateviolation : Form
    {
        private string editviolationcode, editdescription, edittype, editstatus, username;
        private int errorcount;

        Class1 updateviolation = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
       
        private void validateform()
        {
            errorProvider1.Clear();
            errorcount = 0;
           
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is Empty");
                errorcount++;
            }

            if (cmbtype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbtype, "Type is Empty");
                errorcount++;
            }

            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbstatus, "Status is Empty");
                errorcount++;
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateform();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this Violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateviolation.executeSQL("UPDATE tblviolation SET description = '" + txtdescription.Text + "', type = '" + cmbtype.Text +
                            "', status = '" + cmbstatus.Text +
                            "' WHERE violationcode = '" + txtviolation.Text + "'");
                        if (updateviolation.rowAffected > 0)
                        {
                            updateviolation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                            + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                            + "', 'Update', 'Violation Management', '" + txtviolation.Text + "', '" + username + "')");
                            MessageBox.Show("Violation Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            errorProvider1.Clear();
            cmbtype.SelectedIndex = -1;
            cmbstatus.SelectedIndex = -1;
            txtdescription.Clear();
        }
        public frmUpdateviolation(string editviolationcode, string editdescription, string edittype, string editstatus, string username)
        {
            InitializeComponent();
            txtdescription.Multiline = true;
            this.editviolationcode = editviolationcode;
            this.editdescription = editdescription;
            this.edittype = edittype;
            this.editstatus = editstatus;
            this.username = username;

        }
        private void frmUpdateviolation_Load(object sender, EventArgs e)
        {
            txtviolation.Text = editviolationcode;
            txtdescription.Text = editdescription;

            
            if (edittype.Equals("MAJOR OFFENSE", StringComparison.OrdinalIgnoreCase))
            {
                cmbtype.SelectedIndex = 0;
            }
            else if (edittype.Equals("MINOR OFFENSE", StringComparison.OrdinalIgnoreCase))
            {
                cmbtype.SelectedIndex = 1;
            }

            
            if (editstatus.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                cmbstatus.SelectedIndex = 0;
            }
            else if (editstatus.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                cmbstatus.SelectedIndex = 1;
            }
        }
    }
}
        

