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
using 养老系统.类文件;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 养老系统
{
    public partial class C5 : Form
    {
        public C5()
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


        public void chacun()
        {
            string sql = "SELECT jiashubianhao, name FROM dbo.zhuangtai_Table WHERE jiashubianhao = @jiashubianhao";

            // 获取数据库连接
            SqlConnection connection = DB.GetCn();

            // 确保连接不为空
            if (connection != null)
            {
                
                    // 创建SqlCommand对象并添加参数
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@jiashubianhao", Login.bbb);

                    

                    // 执行查询并获取结果
                    SqlDataReader reader = command.ExecuteReader();

                    // 清空comboBox1的现有项
                    comboBox1.Items.Clear();

                    // 遍历查询结果，并将每行添加到comboBox1的项中
                    while (reader.Read())
                    {
                        // 添加项到comboBox1
                        comboBox1.Items.Add(reader["name"].ToString());
                    }

                    // 关闭SqlDataReader
                    reader.Close();
                

                    // 如果您确定不再需要使用连接，可以在这里关闭它
                    // 如果其他地方的代码还需要使用这个连接，请注释掉下面这行
                    // connection.Close();
                
            }
            else
            {
                MessageBox.Show("未能获取数据库连接。");
            }
        }



        private void C5_Load(object sender, EventArgs e)
        {

             

            chacun();
            LoadDataFromDatabase();
            button8.PerformClick();

            

        }


        string shuifei, dianfei, shenghuofei;

       

        private void LoadDataFromDatabase()
        {
            DB.GetCn();
            flowLayoutPanel1.Controls.Clear();

            string sql = "SELECT * FROM zhuangtai_Table where jiashubianhao = '" + Login.bbb + "'"; // 替换为您的表名和列名

            DataTable dataTable = DB.GetDataSet(sql);

            foreach (DataRow row in dataTable.Rows)
            {


                Panel panel1 = new Panel();
                panel1.Size = new Size(220, 220);
                panel1.BackColor = Color.FromArgb(112, 128, 144);

                Label label1 = new Label();
                string labelText1 = row["name"].ToString().Replace("<br>", Environment.NewLine);
                label1.Text = labelText1;
                label1.AutoSize = true;
                label1.Location = new Point(65, 8);
                label1.Font = new Font(label1.Font.FontFamily, 20, FontStyle.Bold);
                label1.Enabled = true;
                label1.FlatStyle = FlatStyle.Flat;
                label1.BringToFront();
                panel1.Controls.Add(label1);


                Label label2 = new Label();
                label2.Text = "余额";
                label2.AutoSize = true;
                label2.Location = new Point(20, 50);
                label2.Font = new Font(label2.Font.FontFamily, 10, FontStyle.Bold);
                label2.Enabled = true;
                label2.FlatStyle = FlatStyle.Flat;
                label2.BringToFront();
                panel1.Controls.Add(label2);

                Label label3 = new Label();
                string labelText2 = row["shuifei"].ToString().Replace("<br>", Environment.NewLine);
                label3.Text = "￥" + labelText2 + "元";
                label3.AutoSize = true;
                label3.Location = new Point(90, 90);
                label3.Font = new Font(label3.Font.FontFamily, 10, FontStyle.Bold);
                label3.Enabled = true;
                label3.FlatStyle = FlatStyle.Flat;
                label3.BringToFront();
                panel1.Controls.Add(label3);

                Label label4 = new Label();
                label4.Text = "水费：";
                label4.AutoSize = true;
                label4.Location = new Point(30, 90);
                label4.Font = new Font(label4.Font.FontFamily, 10, FontStyle.Bold);
                label4.Enabled = true;
                label4.FlatStyle = FlatStyle.Flat;
                label4.BringToFront();
                panel1.Controls.Add(label4);

                Label label5 = new Label();
                string labelText3 = row["dianfei"].ToString().Replace("<br>", Environment.NewLine);
                label5.Text = "￥" + labelText3 + "元";
                label5.AutoSize = true;
                label5.Location = new Point(90, 110);
                label5.Font = new Font(label5.Font.FontFamily, 10, FontStyle.Bold);
                label5.Enabled = true;
                label5.FlatStyle = FlatStyle.Flat;
                label5.BringToFront();
                panel1.Controls.Add(label5);

                Label label6 = new Label();
                label6.Text = "电费：";
                label6.AutoSize = true;
                label6.Location = new Point(30, 110);
                label6.Font = new Font(label6.Font.FontFamily, 10, FontStyle.Bold);
                label6.Enabled = true;
                label6.FlatStyle = FlatStyle.Flat;
                label6.BringToFront();
                panel1.Controls.Add(label6);

                Label label7 = new Label();
                string labelText4 = row["shenghuofei"].ToString().Replace("<br>", Environment.NewLine);
                label7.Text = "￥"+labelText4+"元";
                label7.AutoSize = true;
                label7.Location = new Point(90, 130);
                label7.Font = new Font(label7.Font.FontFamily, 10, FontStyle.Bold);
                label7.Enabled = true;
                label7.FlatStyle = FlatStyle.Flat;
                label7.BringToFront();
                panel1.Controls.Add(label7);

                Label label8 = new Label();
                label8.Text = "生活费：";
                label8.AutoSize = true;
                label8.Location = new Point(15, 130);
                label8.Font = new Font(label8.Font.FontFamily, 10, FontStyle.Bold);
                label8.Enabled = true;
                label8.FlatStyle = FlatStyle.Flat;
                label8.BringToFront();
                panel1.Controls.Add(label8);

                flowLayoutPanel1.Controls.Add(panel1);
            }

            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
        }

        public string jine()
        {
            if (shuifei == "水费")
            {
                return "水费";
            }
            else if (dianfei == "电费")
            {
                return "电费";
            }
            else if (shenghuofei == "生活费")
            {
                return "生活费";
            }

            return "";
        }

        public static string bbb, ccc, aaa,qqq;

        private void button12_Click(object sender, EventArgs e)
        {
            qqq = comboBox1.Text;
            ccc = jine();
            bbb = label40.Text;
            aaa = GetCurrentRecordValue(ccc, Login.bbb);
            if (label40.Text == "")
            {
                MessageBox.Show("请输入充值金额");
            }
            else
            {
                UpdateZhuangtaiRecord(ccc);
            }
           
            //MessageBox.Show(aaa);
            label40.Text = "";
            panel1.Visible = false;



        }

        public static bool UpdateZhuangtaiRecord(string type)
        {
            string valueToInsert = ""; // 默认值为空
            SqlParameter paramValue = null; // 用于添加参数的变量
            string cleanBbb = Regex.Replace(bbb.Trim(), @"[^\d]", "");
            // 根据类型设置要插入的值和更新的列
            switch (type)
            {

                case "水费":
                    valueToInsert = (Convert.ToInt32(aaa) + Convert.ToInt32(cleanBbb)).ToString();
                    paramValue = new SqlParameter("@value", valueToInsert);
                    break;
                case "电费":
                    valueToInsert = (Convert.ToInt32(aaa) + Convert.ToInt32(cleanBbb)).ToString();
                    paramValue = new SqlParameter("@value", valueToInsert);
                    break;
                case "生活费":
                    valueToInsert = (Convert.ToInt32(aaa) + Convert.ToInt32(cleanBbb)).ToString();
                    paramValue = new SqlParameter("@value", valueToInsert);
                    break;
                default:
                    MessageBox.Show("费用类型未指定或无效。");
                    return false;
            }

            // 构建更新查询，注意 SET 后面不应该有逗号
            string updateQuery = "UPDATE zhuangtai_table SET ";
            updateQuery += type == "水费" ? "shuifei" : type == "电费" ? "dianfei" : "shenghuofei";
            updateQuery += " = @value WHERE jiashubianhao = @jiashubianhao and name = '"+ qqq + "'";

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // 添加参数
                    command.Parameters.Add(paramValue);
                    command.Parameters.AddWithValue("@jiashubianhao", Login.bbb);

                    try
                    {
                        // 执行命令
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("充值成功，请等待及时刷新");
                            return true; // 更新成功
                        }
                        else
                        {
                            MessageBox.Show("请选择需要充值的姓名。");
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


        private static string GetCurrentRecordValue(string type, string jiashubianhao)
        {
            string columnName = "";
            string currentValue = "0"; // 默认返回值，如果未找到记录或查询结果为null

            switch (type)
            {
                case "水费":
                    columnName = "shuifei";
                    break;
                case "电费":
                    columnName = "dianfei";
                    break;
                case "生活费":
                    columnName = "shenghuofei";
                    break;
                default:
                    MessageBox.Show("费用类型未指定或无效。");
                    throw new ArgumentException("费用类型未指定或无效。");
            }

            // 使用参数化查询来避免SQL注入
            string sql = "SELECT " + columnName + " FROM zhuangtai_Table WHERE jiashubianhao = @jiashubianhao and name = '"+ qqq + "'";
            using (SqlConnection connection = DB.GetCn())
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@jiashubianhao", jiashubianhao);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentValue = reader[0].ToString(); // 读取第一列的值
                        }
                    }
                }
            }

            return currentValue;
        }

       


        private void label39_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label40.Text = "￥50元";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label40.Text = "￥100元";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label40.Text = "￥300元";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label40.Text = "￥600元";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label40.Text = "￥1000元";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            label40.Text = "￥"+textBox1.Text+ "元";
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.zhuangtai_TableTableAdapter.FillBy(this.yanglaoDataSet2.zhuangtai_Table);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            button8.BackColor = Color.White;
            button10.BackColor = Color.Black;
            button9.BackColor = Color.White;
            dianfei = "电费";
            shuifei = "";
            shenghuofei = "";
        }


        private void button9_Click(object sender, EventArgs e)
        {
            button8.BackColor = Color.White;
            button10.BackColor = Color.White;
            button9.BackColor = Color.Black;
            shenghuofei = "生活费";
            dianfei = "";
            shuifei = "";

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.BackColor = Color.Black;
            button10.BackColor = Color.White;
            button9.BackColor = Color.White;
            shuifei = "水费";
            dianfei = "";
            shenghuofei = "";
        }
        

       
    }
}
