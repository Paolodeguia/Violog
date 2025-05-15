using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmStudentUpdate : Form
    {
        private string username, editstudentID, editfirstname, editlastname, editmiddlename, edityearlevel, editcourse;


        private int errorcount;
        Class1 updateaccounts = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");

        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtStudentID.Text))
            {
                errorProvider1.SetError(txtStudentID, "Student ID is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtlastname.Text))
            {
                errorProvider1.SetError(txtlastname, "Last Name is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                errorProvider1.SetError(txtfirstname, "First Name is empty");
                errorcount++;
            }
            if (cmbyearlevel.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbyearlevel, "Select yearlevel");
                errorcount++;
            }
            if (cmbcourse.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbcourse, "Select Course");
                errorcount++;
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this Student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateaccounts.executeSQL("UPDATE tblstudent SET studentID = '" + txtStudentID.Text + "', studentLN = '" + txtlastname.Text + "', studentFN = '" + txtfirstname.Text + "', studentMN = '" + txtmiddlename.Text + "', yearLevel = '" + cmbyearlevel.Text.ToUpper() + "', studentCourse = '" + cmbcourse.Text.ToUpper() +
                            "' WHERE studentID = '" + editstudentID + "'");

                        if (updateaccounts.rowAffected > 0)
                        {
                            updateaccounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                                + "', 'Update', 'Student Management', '" + txtStudentID.Text + "', '" + username + "')");

                            MessageBox.Show("Student Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public frmStudentUpdate(string username, string editstudentID, string editfirstname, string editlastname, string editmiddlename, string edityearlevel, string editcourse)
        {
            InitializeComponent();
            this.username = username;
            this.editstudentID = editstudentID;
            this.editlastname = editlastname;
            this.editfirstname = editfirstname;
            this.editmiddlename = editmiddlename;
            this.edityearlevel = edityearlevel;
            this.editcourse = editcourse;

        }
        private void frmStudentUpdate_Load(object sender, EventArgs e)
        {
            txtStudentID.Text = editstudentID;
            txtlastname.Text = editlastname;
            txtfirstname.Text = editfirstname;
            txtmiddlename.Text = editmiddlename;
            if (edityearlevel == "Elementary")
            {
                cmbyearlevel.SelectedIndex = 0;
            }
            if (edityearlevel == "Junior High School")
            {
                cmbyearlevel.SelectedIndex = 1;
            }
            if (edityearlevel == "Senior High School")
            {
                cmbyearlevel.SelectedIndex = 2;
            }
            if (edityearlevel == "College")
            {
                cmbyearlevel.SelectedIndex = 3;
            }


        }
        private void cmbyearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbcourse.Items.Clear();
            if (cmbyearlevel.SelectedIndex == 0)
            {
                cmbcourse.Items.AddRange(new string[] { "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6" });
            }
            else if (cmbyearlevel.SelectedIndex == 1)
            {
                cmbcourse.Items.AddRange(new string[] { "Grade 7", "Grade 8", "Grade 9", "Grade 10" });
            }
            else if (cmbyearlevel.SelectedIndex == 2)
            {
                cmbcourse.Items.AddRange(new string[] { "HUMSS", "STEM", "GAS", "ABM" });
            }
            else if (cmbyearlevel.SelectedIndex == 3)
            {
                cmbcourse.Items.AddRange(new string[]
                {
                        "Bachelor of Science in Hospitality Management (BSHM)",
                        "Bachelor of Science in Tourism Management (BSTM)",
                        "Bachelor of Science in Business Administration",
                        "Bachelor of Arts in English Language, Psychology, Political Science",
                        "Bachelor of Science in Criminology",
                        "Bachelor of Secondary Education",
                        "Bachelor of Elementary Education (General Education)",
                        "Bachelor of Physical Education",
                        "Other"
                });
                for (int i = 0; i < cmbcourse.Items.Count; i++)
                {
                    if (cmbcourse.Items[i].ToString().Equals(editcourse, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbcourse.SelectedIndex = i;
                        break;
                    }
                }
            }

        }
    }
}


        
    
    

