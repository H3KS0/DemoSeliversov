using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ДЕМОЭКЗАМЕН3
{
    public partial class Form1 : Form
    {
        static string connString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Demo_Seliverstov;Data Source=DESKTOP-ALHNBP8\SQLEXPRESS";

       
        public int current_id;
        public int current_role;
        public Form1()
        {   
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GetSettings();
        }
        public void GetSettings()
        {
            textBox1.Text = Properties.Settings.Default.login;
            textBox2.Text = Properties.Settings.Default.password;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.login = textBox1.Text;
            Properties.Settings.Default.password = textBox2.Text;

            Properties.Settings.Default.Save();
        }
        //Авторизация
        private void btIn_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection();
            connect.ConnectionString = connString;
            
            try
            {
                connect.Open();
                SqlCommand enter = new SqlCommand();
                enter.CommandType = CommandType.StoredProcedure;
                enter.CommandText = "checkpassword";
                enter.Parameters.AddWithValue("@Login", textBox1.Text);
                enter.Parameters.AddWithValue("@Password", textBox2.Text);
                enter.Connection = connect;
                SqlDataReader sqlReader = enter.ExecuteReader();
                sqlReader.Read();

                
                  if (sqlReader.HasRows)
                 {
                    current_id = sqlReader.GetInt32(0);
                    current_role = sqlReader.GetInt32(3);
                    //Проверка роли
                    if (current_role == 2)
                    {
                        this.Hide();
                        AdminForm form1 = new AdminForm();
                        form1.Show();
                    }
                    else
                    {
                        this.Hide();
                        Menu form = new Menu(current_role, current_id);
                        form.Show();
                    }
                    

                  }

                   else
                   {
                    MessageBox.Show("Неправильный логин или пароль");
                  }
                
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error ({ex.Message})");
            }

        }

        private void btGuest_Click(object sender, EventArgs e)
        {
            current_id = 0;
            current_role = 0;
            this.Hide();
            Menu form = new Menu(current_role, current_id);
            form.Show();
        }
    }
}
