using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string query = "SELECT * FROM login_Table WHERE username = @Name";
            SqlCommand cmd2 = new SqlCommand(query, DB.GetCn());
            cmd2.Parameters.AddWithValue("@Name", txtName.Text);
            DataTable dt = DB.GetDataSet(cmd2);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("此用户名已存在，请重新输入");
            }
            
            else
            {
                if (txtName.Text == "" || txtPwd.Text == "" || txtEmail.Text == "")
                {
                    MessageBox.Show("请输入账号密码和邮箱");

                }
                else
                {

                    string hashedPassword = DB.PasswordHasher.HashPassword(txtPwd.Text);
                    string sdr = "SET IDENTITY_INSERT login_Table ON; " +
                                 "insert into login_Table(number, username, email, password,touxiang) values " +
                                "(IDENT_CURRENT('login_Table') + 1, @username, @email, @password,@touxiang);" +
                                "SET IDENTITY_INSERT login_Table OFF;";

                    SqlCommand cmd = new SqlCommand(sdr, DB.GetCn());
                    cmd.Parameters.AddWithValue("@username", txtName.Text);
                    cmd.Parameters.AddWithValue("@password", txtPwd.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@touxiang", "C:\\Users\\Administrator\\Pictures\\头像.jpg");

                    DB.GetDataSet(cmd);
                    MessageBox.Show("注册成功");
                    this.Close();

                }
            }
        }

        private void Signup_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
