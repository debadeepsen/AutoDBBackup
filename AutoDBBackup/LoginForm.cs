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
using System.Security.Cryptography;
using System.IO;
using AutoDBBackup.Utils;

namespace AutoDBBackup
{
    public partial class LoginForm : Form
    {
        public string ConnectionString { get; private set; }
        public string HostName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtDb.Focus();

            SetupFolders();
        }

        private static void SetupFolders()
        {
            // these methods skip folder creation
            // if the folder already exists
            Directory.CreateDirectory(Constants.SAVED_FOLDER);
            Directory.CreateDirectory(Constants.SETTINGS_FOLDER);
            Directory.CreateDirectory(Constants.BACKUP_FOLDER);
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
                HostName = txtDb.Text.Trim();
                Username = txtU.Text.Trim();
                Password = txtP.Text.Trim();
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var hostname = txtDb.Text.Trim();
            var username = txtU.Text.Trim();
            var password = Lib.SHA512(txtP.Text.Trim());



        }


    }
}
