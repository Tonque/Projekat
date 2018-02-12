using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;

namespace StudentskiDom
{
    public partial class Form1 : Form
    {
        List<Student> studentList = new List<Student>();
        string connectionString = @"Data Source = database.db";
        public static string warningStudId = "";
        public static string studentWarning;
        public Form1()
        {
            InitializeComponent();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string stm = "SELECT * FROM student";

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Student stud = new Student(rdr["Ime"].ToString(), rdr["Prezime"].ToString(),
                                rdr["Godiste"].ToString(), rdr["Pol"].ToString(), rdr["Fakultet"].ToString(),
                                rdr["GodinaStudija"].ToString());
                            stud.Id = rdr["Id"].ToString();
                            stud.Room = rdr["Soba"].ToString();
                            stud.Warning = rdr["Opomena"].ToString();
                            studentList.Add(stud);
                            searchComboBox.Items.Add(stud.Id + " " + stud.FirstName + " " +stud.LastName);

                            if (rdr["Soba"].ToString() != "")
                            {
                                Button btn = this.Controls.Find(rdr["Soba"].ToString(), true).FirstOrDefault() as Button;
                                btn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                            }
                            
                        }

                    }
                }

                con.Close();
            }          
            
            
        }
      
        private void floorDownBtn_Click(object sender, EventArgs e)
        {

        }

        private void floorUpBtn_Click(object sender, EventArgs e)
        {

        }
        
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void moveStudentBtn_MouseDown(object sender, MouseEventArgs e)
        {
            moveStudentBtn.DoDragDrop(moveStudentBtn.Text, DragDropEffects.Copy);
        }

        private void button_DragDrop(object sender, DragEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Text = (string)e.Data.GetData(DataFormats.Text);      

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + moveStudentBtn.Text.Substring(0, 1);
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@brsobe", btn.Name));

                    con.Open();

                    int i = cmd.ExecuteNonQuery();

                    if(i==1)
                    {
                        MessageBox.Show("Uspjesno prebacen student");
                    }
                    roomLabel.Text = btn.Name.Substring(btn.Name.Length - 3);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void button_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lf = new Login();
            lf.Show();
            Visible = false;
        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudent asf = new AddStudent();
            asf.Show();
            Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmployee aef = new AddEmployee();
            aef.Show();
            Visible = false;
        }

        private void searchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            moveStudentBtn.Text = searchComboBox.Text;

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string rbr = searchComboBox.Text.Substring(0, 1);
                string stm = "SELECT * FROM student WHERE student.Id =" + rbr;

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            nameLabel.Text = rdr["Ime"].ToString();
                            lastNameLabel.Text = rdr["Prezime"].ToString();
                            birthLabel.Text = rdr["Godiste"].ToString();
                            genderLabel.Text = rdr["Pol"].ToString();
                            fakultyLabel.Text = rdr["Fakultet"].ToString();
                            yearLabel.Text = rdr["GodinaStudija"].ToString();
                            IDLabel.Text = rdr["Id"].ToString();
                            if( rdr["Soba"].ToString() == "")
                                roomLabel.Text = "Nema sobu";
                            else
                                roomLabel.Text = rdr["Soba"].ToString().Substring(rdr["Soba"].ToString().Length - 3);

                        }

                    }
                }

                con.Close();
            }
        }

        private void izbaciIzSobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            var tsItem = (ToolStripMenuItem)sender;
            var cms = (ContextMenuStrip)tsItem.Owner;
            Button tbx = this.Controls.Find(cms.SourceControl.Name, true).FirstOrDefault() as Button;

            if (tbx.Text != "")
            {
                try
                {
                    if (tbx.Text == moveStudentBtn.Text)
                        roomLabel.Text = "Nema sobu";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + tbx.Text.Substring(0, 1);
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@brsobe", ""));

                        con.Open();

                        int i = cmd.ExecuteNonQuery();

                        if (i == 1)
                        {
                            MessageBox.Show("Uspjesno izbacen iz sobe student");
                        }
                        tbx.Text = "";
                        
                           
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Nema studenta u tom krevetu");

        }

        private void dodajOpomenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tsItem = (ToolStripMenuItem)sender;
            var cms = (ContextMenuStrip)tsItem.Owner;
            Button tbx = this.Controls.Find(cms.SourceControl.Name, true).FirstOrDefault() as Button;
            Warning wf = new Warning();
            warningStudId = tbx.Text.Substring(0, 1);
            if (tbx.Text == "")
            {
                MessageBox.Show("Nema studenta u tom krevetu");
            }
            else
            {

                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    string stm = "SELECT * FROM student WHERE student.Id =" + warningStudId;

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Warning.warn= rdr["Opomena"].ToString();
                            }

                        }
                    }

                    con.Close();
                }
                wf.ShowDialog();
                
            }


                 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stm = "";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                if (moveStudentBtn.Text == "")
                    MessageBox.Show("Niste izabrali studenta");
                else
                {
                    stm = "SELECT * FROM student WHERE student.Id =" + moveStudentBtn.Text.Substring(0, 1);

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (rdr["Opomena"].ToString() == "")
                                    MessageBox.Show("Student nema opomena!");
                                else
                                    MessageBox.Show(rdr["Opomena"].ToString());
                            }

                        }
                    }
                }                   
                 
                con.Close();
            }
            
        }
    }
    
}
   