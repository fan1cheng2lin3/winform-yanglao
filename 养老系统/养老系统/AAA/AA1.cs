using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 养老系统.类文件;

namespace 养老系统.AAA
{
    public partial class AA1 : Form
    {
        public AA1()
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

        public string AutoWrapText(string text, int maxCharsPerLine)
        {
            StringBuilder wrappedText = new StringBuilder();
            int currentLength = 0;
            string breakTag = "<br>";

            for (int i = 0; i < text.Length; i++)
            {
                
                if (currentLength >= maxCharsPerLine || text.Substring(i).StartsWith(breakTag))
                {
                    if (text.Substring(i).StartsWith(breakTag) && (i + breakTag.Length < text.Length) && char.IsPunctuation(text[i + breakTag.Length]))
                    {
                        // 添加 "<br>" 和紧接着的标点符号
                        wrappedText.Append(text.Substring(i, breakTag.Length + 1));
                        i += breakTag.Length; // 跳过 "<br>" 和标点符号
                    }
                    else
                    {
                        // 达到最大字符数，添加换行符
                        wrappedText.Append(Environment.NewLine);
                    }
                    currentLength = 0; // 重置计数器
                }
                else
                {
                    wrappedText.Append(text[i]);
                    currentLength++;
                }
            }

            return wrappedText.ToString();
        }

        private void AA1_Load(object sender, EventArgs e)
        {
            DB.GetCn();
            // 使用DB类中的GetDataSet方法来获取数据
            string sql = "SELECT * FROM new_Table WHERE new = '" + Form1.StaticNewIdentifier + "'";
            DataTable dt = DB.GetDataSet(sql);

            // 检查DataTable是否有数据
            if (dt.Rows.Count > 0)
            {
                // 假设每个new标识符只有一条记录，获取第一行数据
                DataRow row = dt.Rows[0];

                // 更新控件文本，并处理<br>标签

                

                label2.Text = AutoWrapText(row["label1"].ToString(),13);


                label3.Text = AutoWrapText(row["label2"].ToString(), 13);
                label4.Text = AutoWrapText(row["label3"].ToString(), 13);
                string a = row["im1"].ToString();
                pictureBox1.Image = System.Drawing.Image.FromFile(a);

                string b = row["im2"].ToString();
                pictureBox2.Image = System.Drawing.Image.FromFile(b);

                string c = row["im3"].ToString();
                pictureBox3.Image = System.Drawing.Image.FromFile(c);
            }
            else
            {
                MessageBox.Show("未找到数据。");
            }


        }

    }
}
