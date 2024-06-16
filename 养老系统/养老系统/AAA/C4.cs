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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 养老系统
{
    public partial class C4 : Form
    {
        public C4()
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

        private void C4_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“yanglaoDataSet1.zhuangtai_Table”中。您可以根据需要移动或移除它。
            this.zhuangtai_TableTableAdapter.Fill(this.yanglaoDataSet1.zhuangtai_Table);

        }


        public void initzhuangtai()
        {
            string query = "SELECT * FROM zhuangtai_Table WHERE name = '" + comboBox1.Text + "'";
            DataTable result = DB.GetDataSet(query);
            if (result.Rows.Count > 0)
            {
                // 现有字段的处理
                label3.Text = result.Rows[0]["sex"].ToString();
                label11.Text = result.Rows[0]["phone"].ToString();
                label8.Text = result.Rows[0]["Oid"].ToString();

                // date 字段的处理
                string dateString = result.Rows[0]["date"].ToString(); // 获取日期字符串
                if (!string.IsNullOrEmpty(dateString))
                {
                    // 尝试将字符串转换为 DateTime 对象
                    if (DateTime.TryParse(dateString, out DateTime birthDate))
                    {
                        // 计算年龄
                        int age = CalculateAge(birthDate);
                        // 将年龄赋值给 label5
                        label5.Text = age.ToString()+"岁";
                    }
                    else
                    {
                        MessageBox.Show("日期格式不正确。");
                    }
                }
                else
                {
                    MessageBox.Show("日期字段为空。");
                }
            }
            else
            {
                // 没有找到记录时的处理
                MessageBox.Show("没有找到记录。");
            }
        }


        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB.cn.Close();
            initzhuangtai();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("已提交护理申请，我们会尽快安排，请保持电话通畅");
        }
    }
}
