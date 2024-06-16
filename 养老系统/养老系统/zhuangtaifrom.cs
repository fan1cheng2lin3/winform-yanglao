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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 养老系统
{
    public partial class zhuangtaifrom : Form
    {
        public zhuangtaifrom()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void zhuangtaifrom_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“yanglaoDataSet.zhuangtai_Table”中。您可以根据需要移动或移除它。
            this.zhuangtai_TableTableAdapter.Fill(this.yanglaoDataSet.zhuangtai_Table);
            //UpdateZhuangtaiRecord();

            
        }



        public static bool UpdateZhuangtaiRecord(string sex, DateTime date, string phone, string city, string decs, string qingxv,
    string oxygenSaturation, string xinlv, string xueya, string bodyTemperature, string xuetang, string cholesterol, string weight, DateTime updateDate, string name, string Oid)
        {
            string updateQuery = "UPDATE zhuangtai_table SET " +
                                 "sex = @sex, " +
                                 "date = @date, " +
                                 "phone = @phone, " +
                                 "city = @city, " +
                                 "decs = @decs, " +
                                 "qingxv = @qingxv, " +
                                 "Oxygen_Saturation = @oxygenSaturation, " +
                                 "xinlv = @xinlv, " +
                                 "xueya = @xueya, " +
                                 "Body_Temperature = @bodyTemperature, " +
                                 "xuetang = @xuetang, " +
                                 "Cholesterol = @cholesterol, " +
                                 "Weight = @weight, " +
                                 "updatezhuangtai = @updatezhuangtai, " +
                                 "Oid = @Oid " +
                                 "WHERE name = @name";

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // 为参数赋值
                    command.Parameters.AddWithValue("@sex", sex);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@decs", decs);
                    command.Parameters.AddWithValue("@qingxv", qingxv);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@oxygenSaturation", oxygenSaturation);
                    command.Parameters.AddWithValue("@xinlv", xinlv);
                    command.Parameters.AddWithValue("@xueya", xueya);
                    command.Parameters.AddWithValue("@bodyTemperature", bodyTemperature);
                    command.Parameters.AddWithValue("@xuetang", xuetang);
                    command.Parameters.AddWithValue("@cholesterol", cholesterol);
                    command.Parameters.AddWithValue("@weight", weight);
                    command.Parameters.AddWithValue("@updatezhuangtai", updateDate);
                    command.Parameters.AddWithValue("@Oid", Oid);

                    try
                    {
                        // 执行命令
                        int result = command.ExecuteNonQuery();
                        MessageBox.Show("修改成功");
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


        public void initzhuangtai()
        {
            string query = "SELECT * FROM zhuangtai_Table WHERE name = '" + comboBox1.Text + "'";
            DataTable result = DB.GetDataSet(query);
            if (result.Rows.Count > 0)
            {
                // 现有字段的处理
                textBox2.Text = result.Rows[0]["sex"].ToString();
                textBox3.Text = result.Rows[0]["phone"].ToString();
                textBox4.Text = result.Rows[0]["city"].ToString();
                textBox5.Text = result.Rows[0]["decs"].ToString();
                textBox1.Text = result.Rows[0]["Oid"].ToString();

                // date 字段的处理
                if (result.Rows[0]["date"] != DBNull.Value)
                {
                    dateTimePicker2.Value = Convert.ToDateTime(result.Rows[0]["date"]);
                }
                else
                {
                    dateTimePicker2.Value = DateTime.Now; // 或其他默认值
                }

                // 新字段的处理，假设您已经为这些字段创建了对应的控件
                textBox13.Text = result.Rows[0]["Oxygen_Saturation"].ToString();
                textBox9.Text = result.Rows[0]["xinlv"].ToString();
                textBox8.Text = result.Rows[0]["xueya"].ToString();
                textBox7.Text = result.Rows[0]["Body_Temperature"].ToString();
                textBox11.Text = result.Rows[0]["xuetang"].ToString();
                textBox14.Text = result.Rows[0]["Cholesterol"].ToString();
                textBox13.Text = result.Rows[0]["Weight"].ToString();
                textBox11.Text = result.Rows[0]["Oid"].ToString();

                
            }
            else
            {
                // 没有找到记录时的处理
                MessageBox.Show("没有找到记录。");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            initzhuangtai();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateZhuangtaiRecord(textBox2.Text, dateTimePicker2.Value, textBox3.Text, textBox4.Text,textBox5.Text, textBox10.Text,
                textBox13.Text, textBox9.Text,textBox8.Text, textBox7.Text, textBox11.Text,textBox14.Text, textBox12.Text, DateTime.Now, comboBox1.Text, textBox1.Text);

            DB.cn.Close();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
