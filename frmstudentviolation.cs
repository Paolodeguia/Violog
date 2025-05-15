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
    public partial class frmstudentviolation : Form
    {
        private string violationcode, username;
        private int row;
        public frmstudentviolation(string violationcode, string username)
        {
            InitializeComponent();
            this.violationcode = violationcode;
            this.username = username;
        }
        Class1 violation = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");

        private void frmstudentviolation_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = violation.GetData("SELECT violationcode, description, type, status, createdby, datecreated " +
                       "FROM tblviolation WHERE violationcode <> '" + violationcode + "' ORDER BY violationcode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on accounts load", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = violation.GetData("SELECT violationcode, description, type, status, createdby, datecreated " +
                       "FROM tblviolation WHERE violationcode <> '" + violationcode + "' " +
                       "AND (violationcode LIKE '%" + txtsearch.Text + "%' " +
                       "OR description LIKE '%" + txtsearch.Text + "%' " +
                       "OR type LIKE '%" + txtsearch.Text + "%') " +
                       "ORDER BY violationcode");

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmstudentviolation_Load(sender, e);
            txtsearch.Clear();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmnewviolation newviolationform = new frmnewviolation(username);
            newviolationform.Show();
        }

        private void btnrefresh_Click_1(object sender, EventArgs e)
        {

            frmstudentviolation_Load(sender, e);
            txtsearch.Clear();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editviolationcode = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editdescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string edittype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateviolation updateviolationfrm = new frmUpdateviolation(editviolationcode, editdescription, edittype, editstatus, username);
            updateviolationfrm.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error on datagrid cellclick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  
            }
        }

        private void btndelete_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this Violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    violation.executeSQL("DELETE FROM tblviolation WHERE violationcode = '" + selectedUser + "'");
                    if (violation.rowAffected > 0)
                    {
                       violation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                       + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                       + "', 'Delete', 'Violation Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Violation Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }

            }
        }
        }
    }

