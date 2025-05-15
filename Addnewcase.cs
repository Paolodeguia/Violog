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
    public partial class Addnewcase : Form
    {
        Class1 newcases = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        private string username;
        private int errorcount;
        public string StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Level { get; set; }
        public string StrandCourse { get; set; }
        public Addnewcase(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateform()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (cmbviolation.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbviolation, "Violation is empty.");
                errorcount++;
            }

        }
        private void loadViolations()
        {
            try
            {
                DataTable violationsdata = newcases.GetData("SELECT violationcode FROM tblviolation");
                cmbviolation.DisplayMember = "violationcode";
                cmbviolation.ValueMember = "violationcode";
                cmbviolation.DataSource = violationsdata;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on accounts load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Addnewcase_Load(object sender, EventArgs e)
        {
            txtstudentID.Text = StudentID;
            txtlastname.Text = LastName;
            txtfirstname.Text = FirstName;
            txtmiddlename.Text = MiddleName;
            txtyearlevel.Text = Level;
            txtcourse.Text = StrandCourse;
            loadViolations();
            cmbviolation.SelectedIndex = -1;
        }

        private void cmbviolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbviolation.SelectedValue != null)
            {
                string selectedcode = cmbviolation.SelectedValue.ToString();
                //fetch description
                DataTable violationData = newcases.GetData($"SELECT description FROM tblviolation WHERE violationcode = '{selectedcode}'");
                if (violationData.Rows.Count > 0)
                {
                    // Display description in the RichTextBox
                    txtdescription.Text = violationData.Rows[0]["description"].ToString();
                }
            }
            else
            {
                txtdescription.Clear();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateform();

            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this case?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        //unique caseID
                        string caseID = "case" + DateTime.Now.ToString("yyyyMMddHHmmss");

                        // Check previous violations
                        DataTable previousCases = newcases.GetData("SELECT COUNT(*) AS violationcount FROM tblcase WHERE studentID = '" + txtstudentID.Text + "' AND violationID = '" + cmbviolation.SelectedValue.ToString() + "'");
                        int violationCount = (int)previousCases.Rows[0]["ViolationCount"] + 1; // Increment by 1 for the new violation
                        string violationLabel;
                        switch (violationCount)
                        {
                            case 1:
                                violationLabel = "1st ";
                                break;
                            case 2:
                                violationLabel = "2nd ";
                                break;
                            case 3:
                                violationLabel = "3rd ";
                                break;
                            default:
                                violationLabel = $"{violationCount}th ";
                                break;
                        }

                        newcases.executeSQL("INSERT INTO tblcase (caseID, studentID, violationID, violationcount, status, resolution, createdby, datecreated) " +
                            "VALUES ('" + caseID + "', '" + txtstudentID.Text + "', '" + cmbviolation.SelectedValue.ToString() + "', '" + violationLabel + "', 'Ongoing', '', '" + username + "', '" + DateTime.Now.ToShortDateString() + "')");

                        if (newcases.rowAffected > 0)
                        {
                            // Log the action in tbllogs
                            newcases.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'Add', 'Cases Management', '" + caseID + "', '" + username + "')");

                            MessageBox.Show("New case added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


