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
    public partial class Updatecase : Form
    {
        Class1 updatecase = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        private string editStatus, editResolution, caseID, violationID, description, username;
        private int errorcount;
        public string StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string Level { get; set; }

        public string StrandCourse { get; set; }
        public Updatecase(string editStatus, string editResolution, string caseID, string violationID, string description, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editStatus = editStatus;
            this.editResolution = editResolution;
            this.caseID = caseID;
            this.violationID = violationID;
            this.description = description;
        }

        private void cmbstatus_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbstatus.SelectedItem != null)
            {
                string selectedValue = cmbstatus.SelectedItem.ToString();

                if (selectedValue == "Result")
                {
                    txtreso.Enabled = true;
                }
                else
                {
                    txtreso.Clear();
                    txtreso.Enabled = false;
                }
            }
        }

        private void validateform()
        {
            errorProvider1.Clear();
            errorcount = 0;

            if (cmbstatus.SelectedIndex == 1)
            {
                if (string.IsNullOrEmpty(txtreso.Text))
                {
                    errorProvider1.SetError(txtreso, "Resolution is empty");
                    errorcount++;
                }
            }
        }

        private void Updatecase_Load(object sender, EventArgs e)
        {
            txtstudentID.Text = StudentID;
            txtlastname.Text = LastName;
            txtfirstname.Text = FirstName;
            txtmiddlename.Text = MiddleName;
            txtyearlevel.Text = Level;
            txtcourse.Text = StrandCourse;
            txtreso.Text = editResolution;
            txtcode.Text = violationID;
            txtdescription.Text = description;
            if (editStatus == "Ongoing")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
            }


        }
        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtreso.Clear();
            cmbstatus.SelectedIndex = -1;
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateform();

            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this case?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        // Insert the update student record
                        updatecase.executeSQL("UPDATE tblcase SET status = '" + cmbstatus.Text + "', resolution = '" + txtreso.Text +
                            "' WHERE caseID = '" + caseID + "'");

                        if (updatecase.rowAffected > 0)
                        {
                            // Log the action
                            updatecase.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'Update', 'Cases Management', '" + caseID + "', '" + username + "')");

                            MessageBox.Show("Case data updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

