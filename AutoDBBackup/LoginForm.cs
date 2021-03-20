using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDBBackup
{
    public partial class LoginForm : Form
    {
        public string ConnectionString { get; set; }
        public LoginForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtDb.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + txtDb.Text.Trim() + ";user=" + txtU.Text.Trim() + ";port=3306;password=" + txtP.Text.Trim();
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Cursor = Cursors.WaitCursor;
                btnLogin.Enabled = false;
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Cursor = Cursors.Default;
                btnLogin.Enabled = true;
                ConnectionString = connStr;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
            }
        }
    }
}
