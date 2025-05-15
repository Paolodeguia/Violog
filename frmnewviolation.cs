using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmnewviolation : Form
    {

        private string username;
        private int errorcount;
        Class1 newviolation = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        public frmnewviolation(string username)
        {
            InitializeComponent();
            txtdescription.Multiline = true;
            this.username = username;
        }

        private void valiadateform()
        {
            errorProvider1.Clear();
            errorcount = 0;

            if (string.IsNullOrEmpty(txtviolation.Text))
            {
                errorProvider1.SetError(txtviolation, "Violation code is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is empty");
                errorcount++;
            }
            if (cmbtype.SelectedIndex < 0)
            { 
                errorProvider1.SetError(cmbtype, "Type is empty");
                errorcount++;
            }
            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbstatus, "Status is empty");
                errorcount++;
            }
            try
            {
                DataTable dt = newviolation.GetData("SELECT * FROM tblviolation WHERE violationcode = '" + txtviolation.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtviolation, "Violation Code already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing violation", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            valiadateform();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newviolation.executeSQL("INSERT INTO tblviolation(violationcode, description, type, status, createdby, datecreated) VALUES('" + txtviolation.Text + "', '" + txtdescription.Text + "' , '" +
                            cmbtype.Text + "', '" + cmbstatus.Text + "', '" + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newviolation.rowAffected > 0)
                        {
                            newviolation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                            + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                            + "', 'Add', 'Violation Management', '" + txtviolation.Text + "', '" + username + "')");
                            MessageBox.Show("New Violation added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            cmbstatus.SelectedIndex = -1;
            cmbtype.SelectedIndex = -1;
            txtviolation.Clear();
            txtdescription.Clear();
        }

        private void frmnewviolation_Load(object sender, EventArgs e)
        {

        }
    }
}
