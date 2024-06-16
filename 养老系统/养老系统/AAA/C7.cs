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
using 养老系统.类文件;

namespace 养老系统
{
    public partial class C7 : Form
    {
        public C7()
        {
            InitializeComponent();
        }

        public string nan, nv, ruyuan, jiashu, zhiyuanzhe;

        public delegate void HidePanelDelegate();

        public HidePanelDelegate HidePanelHandler; // 声明一个委托变量

        private void button1_Click(object sender, EventArgs e)
        {
            HidePanelHandler?.Invoke();
            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.White;
            button3.BackColor = Color.Black;
            nv = "女";
            nan = "";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.Black;
            button3.BackColor = Color.White;
            nan = "男";
            nv = "";
            
        }

        public string sex()
        {
            if (nan == "男")
            {
                return "男";
            }
            else if (nv == "女")
            {
                return "女";
            }
            return "";
        }


        private void button5_Click(object sender, EventArgs e)
        {
            button5.BackColor = Color.Black;
            button6.BackColor = Color.White;
            button7.BackColor = Color.White;
            jiashu = "家属暂住";
            zhiyuanzhe = "";
            ruyuan = "";
        }


        private void button6_Click(object sender, EventArgs e)
        {
            button6.BackColor = Color.Black;
            button5.BackColor = Color.White;
            button7.BackColor = Color.White;
            ruyuan = "申请入院";
            jiashu = "";
            zhiyuanzhe = "";
        }

        private void C7_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.BackColor = Color.Black;
            button5.BackColor = Color.White;
            button6.BackColor = Color.White;
            zhiyuanzhe = "志愿者暂住";
            ruyuan = "";
            jiashu = "";
        }

     
        public string ruzhu()
        {
            if (jiashu == "家属暂住")
            {
                return "家属暂住";
            }
            else if (ruyuan == "申请入院")
            {
                return "申请入院";
            }
            else if (zhiyuanzhe == "志愿者暂住")
            {
                return "志愿者暂住";
            }
            return "";
        }


        public static bool UpdateZhuangtaiRecord(string name, string sex, string ruzhu, DateTime date, string phone, string city, string decs,string Status)
        {



            string insertQuery = "INSERT INTO ruzu_Table (name, sex, ruzhu, date, phone, city, decs,Status) " +
                      "VALUES (@name, @sex, @ruzhu, @date, @phone, @city, @decs,@Status)";

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // 为参数赋值
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@sex", sex);
                    command.Parameters.AddWithValue("@ruzhu", ruzhu);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@decs", decs);
                    command.Parameters.AddWithValue("@Status", Status);
                    try
                    {
                        // 执行命令
                        int result = command.ExecuteNonQuery();
                        // 可以在这里处理结果，例如输出更新的行数
                        if (result > 0)
                        {

                            return true; // 更新成功
                        }
                        else
                        {
                            MessageBox.Show("没有行被更新。");
                            return false; // 没有行被更新
                        }
                    }
                    catch (Exception ex)
                    {
                        // 处理异常
                        MessageBox.Show("更新失败：" + ex.Message);
                        return false;
                    }
                }

            }
        }


       


        private void button4_Click(object sender, EventArgs e)
        {


            if(textBox1.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("姓名和手机号是必填的");
            }
            else
            {
                UpdateZhuangtaiRecord(textBox1.Text, sex(), ruzhu(), dateTimePicker1.Value, textBox3.Text, textBox2.Text, richTextBox1.Text, "未联系");
                MessageBox.Show("已申请入住情况，确保手机号码无误，请您耐心等待通知，我们会以电话的形式与你沟通，请保持电话畅通");
            }
            
        }
    }
}
