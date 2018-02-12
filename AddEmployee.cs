using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentskiDom
{
    public partial class AddEmployee : Form
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            //ima dosta posla
            MessageBox.Show("Dodali ste zaposlenog");

            if (passTextBox.Text != confPassTextBox.Text)
            {
                wrongPassBtn.Visible = true;
            } 
            else
            {
                wrongPassBtn.Visible = false;
            }
        }

        private void AddEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }
    }
}
