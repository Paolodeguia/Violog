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
    public partial class frmMain : Form
    {
        private string studentID = "",username, usertype, violationcode;

        public frmMain(string username, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            frmlogin loginform = new frmlogin();
            loginform.Show();
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts accountsform = new frmAccounts(username);
            accountsform.MdiParent = this;
            accountsform.Show();
        }

        private void caseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaseManagement caseform = new CaseManagement(username);
            caseform.MdiParent = this;
            caseform.Show();
        }

        private void violationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmstudentviolation violationForm = new frmstudentviolation(violationcode, username);
            violationForm.MdiParent = this;
            violationForm.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudent studentform = new frmStudent(studentID, username);
            studentform.MdiParent = this;
            studentform.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Username:" + username;
            toolStripStatusLabel2.Text = "Usertype:" + usertype;

            if (usertype == "ADMINISTRATOR")
            {
                accountsToolStripMenuItem.Visible = true;
                eventsToolStripMenuItem.Visible = true;
                ticketsToolStripMenuItem.Visible = true;
            }
            else if (usertype == "BRANCHADMINISTRATOR")
            {
                accountsToolStripMenuItem.Visible = false;
                eventsToolStripMenuItem.Visible = true;
                ticketsToolStripMenuItem.Visible = true;
            }
            else
            {
                accountsToolStripMenuItem.Visible = false;
                eventsToolStripMenuItem.Visible = false;
                ticketsToolStripMenuItem.Visible = true;
            }






        }
    }
}
