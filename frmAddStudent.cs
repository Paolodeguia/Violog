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

namespace CS311A2024_DATABASE
{
    public partial class frmAddStudent : Form
    {
        public delegate void StudentAddedEventHandler(object sender, EventArgs e);
        public event StudentAddedEventHandler StudentAdded;
        private string username, studentID;
        private int errorcount;
        Class1 newstudent = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        public frmAddStudent(string username)
        {
            InitializeComponent();
            this.username = username;
            this.studentID = studentID;
        }
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
                errorProvider1.SetError(cmbyearlevel, "Select usertype");
                errorcount++;
            }
            if (cmbcourse.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbcourse, "Select usertype");
                errorcount++;
            }
            try
            {
                DataTable dt = newstudent.GetData("SELECT * FROM tblstudent WHERE studentID = '" + txtStudentID.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtStudentID, "Student ID already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing account", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void cmbyearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            cmbcourse.Items.Clear();

            
            if (cmbyearlevel.SelectedIndex == 0 )
            {
                cmbcourse.Enabled = true;
                cmbcourse.Items.AddRange(new string[]
                {
                    "Grade 1",
                    "Grade 2",
                    "Grade 3",
                    "Grade 4",
                    "Grade 5",
                    "Grade 6"
                });
            }
            else if (cmbyearlevel.SelectedIndex == 1)
            {
                cmbcourse.Enabled = true;
                cmbcourse.Items.AddRange(new string[]
                {
                    "Grade 7",
                    "Grade 8",
                    "Grade 9",
                    "Grade 10"                   
                });
            }
            else
            {
                cmbcourse.Enabled = true;

                if (cmbyearlevel.SelectedIndex == 2)
                {

                    cmbcourse.Items.AddRange(new string[]
                     {
                        "HUMSS",
                        "STEM",
                        "GAS",
                        "ABM"
                     });
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
                "Bachelor of Science in Computer Science (BSCS)"
                    });
                }
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtStudentID.Clear();
            txtlastname.Clear();   
            txtfirstname.Clear();
            txtmiddlename.Clear();
            cmbyearlevel.SelectedIndex = -1;
            cmbcourse.SelectedIndex = -1;
        }

        private void frmAddStudent_Load(object sender, EventArgs e)
        {

        }

        private void cmbcourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this Student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newstudent.executeSQL("INSERT INTO tblstudent(studentID , studentLN, studentFN, studentMN, yearLevel, studentCourse, dateCreated, createdBy) VALUES('" + txtStudentID.Text + "', '" + txtlastname.Text + "' ,'" + txtfirstname.Text + "','" + txtmiddlename.Text + "' , '" +
                            cmbyearlevel.Text.ToUpper() + "', '" + cmbcourse.Text.ToUpper() + "', '" + DateTime.Now.ToShortDateString() + "', '" + username + "')");
                        if (newstudent.rowAffected > 0)
                        {
                            newstudent.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                            + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                            + "', 'Add', 'Students Management', '" + txtStudentID.Text + "', '" + username + "')");
                            MessageBox.Show("New Student added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            StudentAdded?.Invoke(this, EventArgs.Empty);
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

        
    }
}
