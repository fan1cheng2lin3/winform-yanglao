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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static 养老系统.类文件.DB;
using System.Xml.Linq;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class Login : Form
    {
        Form1 form1;
        public Login(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;

            // 订阅Form1的事件
            this.form1.UpdatePictureBoxEvent += Form1_UpdatePictureBoxEvent;
            this.form1.zhuangtai += FF;
        }




        // 事件处理方法
        private void Form1_UpdatePictureBoxEvent(Image image, string labelText)
        {
            // 这里使用Form1的pictureBox1控件
            form1.pictureBox1.Image = image;

            form1.label28.Text = labelText;
        }

        private void FF(string jiashuid)
        {
            form1.kkk = jiashuid;


        }


        public static string a;
        public static string bbb;


        

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.Gray;
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
        }

        private void Login_Load(object sender, EventArgs e)
        {
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool LoginSuccess { get; private set; }

        public void HandleButtonClick()
        {
            // 在此处编写按钮按下事件的处理逻辑
            MessageBox.Show("Button clicked in C2");
        }

        private string GetNumberFromLoginTable()
        {
            DB.GetCn();
            string query = "SELECT number FROM Login_table WHERE username = '" + txtName.Text + "'";
            DataTable result = DB.GetDataSet(query);
            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["number"].ToString();
            }
            DB.cn.Close();
            return string.Empty;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPwd.Text == "")
            {
                MessageBox.Show("请输入账号密码");
            }
            else
            {
                // 执行查询并验证用户信息是否匹配
                DB.GetCn();


                string query = "SELECT * FROM login_Table WHERE username = @Name AND password = @Password";
                SqlCommand cmd = new SqlCommand(query, DB.GetCn());
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Password", txtPwd.Text);
                DataTable dt = DB.GetDataSet(cmd);



                if (dt.Rows.Count > 0)
                {
                    bool isSuccess = true; 

                    LoginSuccess = isSuccess; 

                    if (isSuccess)
                    {
                        this.DialogResult = DialogResult.OK;
                        a = txtName.Text;
                        bbb = GetNumberFromLoginTable().Trim();
                        genxin();
                        form1.zhunagtaiidhuqv(bbb);
                        this.Close();

                    }
                }
                else
                {
                    MessageBox.Show("用户名或者密码错误");
                }
                DB.cn.Close();


            }
        }

        
        

        public void genxin()
        {
            Image image = Image.FromFile(InitData("touxiang"));
            form1.OnUpdatePictureBox(image, InitData("username"));
        }

        public string InitData(string columnName)
        {
            DB.GetCn();
            string query = "SELECT " + columnName + " FROM Login_table WHERE username = '" + a + "'";
            DataTable result = DB.GetDataSet(query);
            string re = "";
            if (result.Rows.Count > 0)
            {
                re = result.Rows[0][columnName].ToString().Trim();
            }
            DB.cn.Close();

            return re;
        }


        private void label5_Click(object sender, EventArgs e)
        {
            Signup t1 = new Signup();
            t1.ShowDialog();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
