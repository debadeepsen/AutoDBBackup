using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDBBackup
{
    public partial class MainForm : Form
    {
        private string connectionString = "";
        private string hostName = "";
        private string username = "";
        private string password = "";
        private string dbSelected = "";
        public MainForm()
        {
            InitializeComponent();
            ShowLoginForm();
        }

        private void ShowLoginForm()
        {
            Hide();

            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    connectionString = loginForm.ConnectionString;
                    hostName = loginForm.HostName;
                    username = loginForm.Username;
                    password = loginForm.Password;
                    Show();
                }
                else if (loginForm.DialogResult == DialogResult.Cancel)
                {
                    Close();
                }
            }
        }

        private void btnManualBackup_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            //process.StartInfo = startInfo;
            //process.Start();

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("cmd.exe");
            process.StartInfo.WorkingDirectory = "C:\\wamp64\\bin\\mysql\\mysql8.0.18\\bin";
            process.StartInfo.Arguments = "/C .\\mysqldump --username=root --password= employees > c:\\users\\103731\\desktop\\sql\\___backup1.sql";
            process.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "SHOW DATABASES";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        TreeNode dbNode = new TreeNode
                        {
                            ImageKey = "database",
                            Text = rdr[0].ToString()
                        };

                        treeView1.Nodes.Add(dbNode);
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (TreeNode node in treeView1.Nodes)
                {
                    GetTables(node);
                }

                //treeView1.ExpandAll();
            }
        }

        private void GetTables(TreeNode dbNode)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Getting tables...");
                    conn.Open();

                    string sql = "use " + dbNode.Text + "; SHOW TABLES";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        TreeNode tableNode = new TreeNode
                        {
                            ImageKey = "table",
                            SelectedImageKey = "table",
                            Text = rdr[0].ToString()
                        };

                        dbNode.Nodes.Add(tableNode);
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand || e.Action == TreeViewAction.Collapse)
                return;

            if (e.Node.Parent == null)
                return;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Getting data...");
                    conn.Open();

                    string sql = "use " + e.Node.Parent.Text + "; SELECT * FROM " + e.Node.Text;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);

                    dataGridView1.DataSource = temp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Console.WriteLine()

            if (e.Node.Parent != null)
                return;
            dbSelected = e.Node.Text;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void backupNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var y = DateTime.Now.Year;
            var m = DateTime.Now.Month;
            var d = DateTime.Now.Day;
            var hh = DateTime.Now.Hour;
            var mm = DateTime.Now.Minute;
            var ss = DateTime.Now.Second;

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("cmd.exe");
            process.StartInfo.WorkingDirectory = "C:\\wamp64\\bin\\mysql\\mysql8.0.18\\bin";
            process.StartInfo.Arguments = "/C .\\mysqldump --column-statistics=0 --host=" + hostName + " --user=" + username + " --password=" + password + " " + dbSelected + " > c:\\users\\103731\\desktop\\sql\\__backup__" + dbSelected + "__"
                + y + "-"
                + m + "-"
                + d + "-"
                + "-"
                + hh + "-"
                + mm + "-"
                + ss
                + ".sql";
            process.Start();
        }
    }
}
