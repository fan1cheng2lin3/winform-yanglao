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
    public partial class C10 : Form
    {
        public C10()
        {
            InitializeComponent();
        }
        public delegate void HidePanelDelegate();

        public HidePanelDelegate HidePanelHandler; // 声明一个委托变量

        private void button1_Click(object sender, EventArgs e)
        {
            HidePanelHandler?.Invoke();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == ""|| textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("请完善你的信息并送上祝福吧");
            }
            else
            {
                if (!UpdateZhuangtaiRecord(textBox1.Text, textBox2.Text, textBox3.Text,"未审核"))
                {
                    return;
                }

                // 更新成功后，显示带有“是”和“否”按钮的消息框
                DialogResult result = MessageBox.Show("您确定要进行二维码付款吗？", "付款确认", MessageBoxButtons.YesNo);

                // 检查用户是否点击了“是”按钮
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("您的慈心善举，为老人院的岁月添上了一抹温馨的色彩，我们深表感激。");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";

                    // 这里可以添加二维码显示或付款处理的代码
                }

            }
        }

        private void C10_Load(object sender, EventArgs e)
        {

        }


        public static bool UpdateZhuangtaiRecord(string name, string cost, string decs,string Status)
        {
            decimal parsedCost; // 用于存储解析后的decimal值

            // 尝试将成本字符串转换为decimal
            if (!decimal.TryParse(cost, out parsedCost))
            {
                MessageBox.Show("值无效，请输入一个有效的金额。");
                return false;
            }

            string insertQuery = "INSERT INTO juanzheng_Table (name, cost, decs,Status) " +
                                  "VALUES (@name, @cost, @decs,@Status)";

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // 为参数赋值，并指定SqlDbType为Decimal，确保精度
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.Add(new SqlParameter("@cost", SqlDbType.Decimal)
                    {
                        Value = parsedCost
                    });
                    command.Parameters.AddWithValue("@decs", decs);

                    try
                    {
                        // 执行命令
                        int result = command.ExecuteNonQuery();
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
    }
}
