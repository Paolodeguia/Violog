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
    public partial class frmStudent : Form
    {
        private string studentID, username;
        private int row;
        public frmStudent(string studentID, string username)
        {
            InitializeComponent();
            this.studentID = studentID;
            this.username = username;   
        }
        Class1 students = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                row = dataGridView1.SelectedRows[0].Index;
            }
        }
        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = students.GetData("SELECT studentID, studentLN, studentFN, studentMN, yearLevel, studentCourse, dateCreated, createdBy " +
                       "FROM tblstudent WHERE studentID <> '" + studentID + "' ORDER BY studentID");
                dataGridView1.DataSource = dt;

                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on students load", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }   
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmStudent_Load(sender, e);
            txtsearch.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtsearch.Clear();
            txtsearch.Focus();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this Student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    students.executeSQL("DELETE FROM tblstudent WHERE studentID = '" + selectedUser + "'");
                    if (students.rowAffected > 0)
                    {
                        students.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                                + "', 'Delete', 'Student Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Student Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }

            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editstudentID = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editlastname = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string editfirstname = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editmiddlename = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string edityearlevel = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string editcourse = dataGridView1.Rows[row].Cells[5].Value.ToString();
            frmStudentUpdate updateAccountfrm = new frmStudentUpdate (username, editstudentID, editfirstname, editlastname, editmiddlename, edityearlevel, editcourse);
            updateAccountfrm.Show();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = students.GetData("SELECT studentID, studentLN, studentFN, studentMN, yearLevel, studentCourse, datecreated, createdby " +
                       "FROM tblstudent WHERE studentID <> '" + studentID + "' AND (studentID LIKE '%" + txtsearch.Text +
                       "%' OR studentLN LIKE '%" + txtsearch.Text + "%' OR yearLevel LIKE '%" + txtsearch.Text + "%') ORDER BY studentID");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddStudent newstudentform = new frmAddStudent(username);
            frmAddStudent.StudentAdded += (s,args) = =>; frmStudent_Load(sender, e);
            newstudentform.Show();
        }
    }
}
