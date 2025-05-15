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
    public partial class CaseManagement : Form
    {
        private string username;
        Class1 cases = new Class1("PaoloDeGuia", "cs311a2024", "paolo", "123456");
        public CaseManagement(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CaseManagement_Load(object sender, EventArgs e)
        {

        }

        private void txtlevel_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtstudentID_TextChanged(object sender, EventArgs e)
        {
            string studentID = txtstudentID.Text;
            dataGridView1.DataSource = null;
            if (!string.IsNullOrEmpty(studentID))
            {
                DataTable studentData = cases.GetData($"SELECT studentLN, studentFN, studentMN, yearLevel, studentCourse FROM tblstudent WHERE studentID = '{studentID}'");

                if (studentData.Rows.Count > 0)
                {
                    DataRow row = studentData.Rows[0];
                    txtlastname.Text = row["studentLN"].ToString();
                    txtfirstname.Text = row["studentFN"].ToString();
                    txtmiddlename.Text = row["studentMN"].ToString();
                    txtlevel.Text = row["yearLevel"].ToString();
                    txtcourse.Text = row["studentCourse"].ToString();


                    // Retrieve case and violation data for the student
                    string queryCase = $@"
                        SELECT c.caseID, c.violationID, v.description AS 'description', c.violationcount, 
                               c.status, c.resolution, c.createdby, c.datecreated
                        FROM tblcase c, tblviolation v
                        WHERE c.studentID = '{studentID}' AND c.violationID = v.violationcode ORDER BY c.caseID";
                    DataTable caseData = cases.GetData(queryCase);
                    foreach (DataRow caseRow in caseData.Rows)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow newRow = dataGridView1.Rows[rowIndex];
                        newRow.Cells["caseID"].Value = caseRow["caseID"];
                        newRow.Cells["violationID"].Value = caseRow["violationID"];
                        newRow.Cells["description"].Value = caseRow["description"];
                        newRow.Cells["violationcount"].Value = caseRow["violationcount"];
                        newRow.Cells["status"].Value = caseRow["status"];
                        newRow.Cells["resolution"].Value = caseRow["resolution"];
                        newRow.Cells["createdby"].Value = caseRow["createdby"];
                        newRow.Cells["datecreated"].Value = caseRow["datecreated"];
                    }
                    dataGridView1.Columns["description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dataGridView1.Columns["resolution"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                }
                else
                {
                    txtlastname.Clear();
                    txtfirstname.Clear();
                    txtmiddlename.Clear();
                    txtlevel.Clear();
                    txtcourse.Clear();
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
            else
            {
                txtlastname.Clear();
                txtfirstname.Clear();
                txtmiddlename.Clear();
                txtlevel.Clear();
                txtcourse.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            Addnewcase newCaseForm = new Addnewcase (username)
            {
                StudentID = txtstudentID.Text,
                LastName = txtlastname.Text,
                FirstName = txtfirstname.Text,
                MiddleName = txtmiddlename.Text,
                Level = txtlevel.Text,
                StrandCourse = txtcourse.Text,
            };
            newCaseForm.Show();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editStatus = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string editResolution = dataGridView1.Rows[row].Cells[5].Value.ToString();
            string caseID = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string violationID = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string description = dataGridView1.Rows[row].Cells[2].Value.ToString();
            Updatecase newCaseForm = new Updatecase(editStatus, editResolution, caseID, violationID, description, username)
            {
                StudentID = txtstudentID.Text,
                LastName = txtlastname.Text,
                FirstName = txtfirstname.Text,
                MiddleName = txtmiddlename.Text,
                Level = txtlevel.Text,
                StrandCourse = txtcourse.Text,
            };
            newCaseForm.Show();
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
                MessageBox.Show(ex.Message, "Error on datagrid cellclick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    }
    

