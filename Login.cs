using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SQLite;
using System.IO;

namespace StudentskiDom
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Database baza = new Database();
        }
        

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void LoginBtn_Click(object sender, MouseEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passwordTextBox.UseSystemPasswordChar = false;
            }
            else
                passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
