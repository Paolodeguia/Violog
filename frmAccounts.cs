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
    public partial class frmAccounts : Form
    {
        private string username;
        public frmAccounts(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 accounts = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try 
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated " +
                       "FROM tblaccounts WHERE username <> '" + username + "' ORDER BY username");
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
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated " +
                       "FROM tblaccounts WHERE username <> '" + username + "' AND (username LIKE '%" + txtsearch.Text +
                       "%' OR usertype LIKE '%" + txtsearch.Text + "%') ORDER BY username");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
            txtsearch.Clear();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAdd newaccountform = new frmAdd(username);
            newaccountform.Show();
        }
        private int row;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try 
            {
               row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cellclick", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    accounts.executeSQL("DELETE FROM tblaccounts WHERE username = '" + selectedUser + "'");
                    if (accounts.rowAffected > 0)
                    {
                        accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString()
                                + "', 'Delete', 'Accounts Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Account Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string editusername = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editpassword = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string edittype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateAccount updateAccountfrm = new frmUpdateAccount(editusername, editpassword, edittype, editstatus, username);
            updateAccountfrm.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtsearch.Clear();
            txtsearch.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
