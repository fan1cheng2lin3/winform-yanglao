using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 养老系统.AAA;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class C1 : Form
    {
       

        public C1()
        {
            InitializeComponent();
        }

        // 定义一个委托
        public delegate void HidePanelDelegate();

        public HidePanelDelegate HidePanelHandler; // 声明一个委托变量

        private void button1_Click(object sender, EventArgs e)
        {
            HidePanelHandler?.Invoke();
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void C1_Load_1(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
            panel1.Visible = false;
            //panel1.Dock = DockStyle.Fill;
        }


        public void UpdatePanels()
        {
            panel1.Visible = false;
        }


        private void LoadDataFromDatabase()
        {
            DB.GetCn();
            flowLayoutPanel1.Controls.Clear();

            string sql = "SELECT number,username,touxiang FROM Login_Table"; // 替换为您的表名和列名

            DataTable dataTable = DB.GetDataSet(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                string number = row["number"].ToString().Trim();

                if (number == Login.bbb)
                {
                    continue; 
                }

                Panel panel1 = new Panel();
                panel1.Size = new Size(789, 100);
                panel1.BackColor = Color.White;
                
                Panel imagePanel = new Panel();
                imagePanel.Size = new Size(100, 100);
                imagePanel.BackColor = Color.LightGray;
                panel1.Controls.Add(imagePanel);
                
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(100, 100);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.ImageLocation = row["touxiang"].ToString();
                pictureBox.BackColor = Color.White;
                pictureBox.Location = new Point(5, 5);
                pictureBox.Tag = row["number"].ToString();
               
                imagePanel.Controls.Add(pictureBox);

                Label label1 = new Label();
                string labelText1 = row["username"].ToString().Replace("<br>", Environment.NewLine);
                label1.Text = labelText1;
                label1.AutoSize = true;
                label1.Location = new Point(250, 10);
                label1.Font = new Font(label1.Font.FontFamily, 30, FontStyle.Bold);
                label1.Enabled = true;
                label1.FlatStyle = FlatStyle.Flat;
                label1.BringToFront();
                label1.Click += new EventHandler(Control_Click);
                pictureBox.Click += new EventHandler(Control_Click);
                panel1.Click += new EventHandler(Control_Click);
                panel1.Controls.Add(label1);

                flowLayoutPanel1.Controls.Add(panel1);
            }

            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
        }

        public static string lk;

        private async void Control_Click(object sender, EventArgs e)
        {
            if (sender is Control clickedControl)
            {
                try
                {
                    // 检查 clickedControl 是否是 Label 或 PictureBox
                    if (clickedControl is Label || clickedControl is PictureBox)
                    {
                        // 找到 Panel 控件
                        Panel panel = clickedControl.Parent as Panel;

                        // 确保 panel 是 flowLayoutPanel1 的子控件
                        if (panel != null && flowLayoutPanel1.Controls.Contains(panel))
                        {
                            // 异步执行数据库访问
                            string username = ((Label)panel.Controls[1]).Text.Trim(); // 假设 label1 是面板的第一个控件
                            string query = "SELECT number FROM Login_table WHERE username = '" + username + "'";
                            DataTable result = await Task.Run(() => DB.GetDataSet(query));

                            if (result.Rows.Count > 0)
                            {
                                string a = result.Rows[0]["number"].ToString();
                                lk = a;

                            }

                            // 显示 ceshi 窗体并设置 label1 的文本
                            xinxi form2 = new xinxi();
                            form2.TopLevel = false;
                            form2.Dock = DockStyle.Fill;
                            form2.FormBorderStyle = FormBorderStyle.None;
                            form2.Label1Text = username;
                          

                            // 委托赋值，如果 UpdatePanels 需要更新 UI，请确保使用 Invoke 或 BeginInvoke
                            form2.HidePanelHandler3 = new xinxi.HidePanelDelegate3(UpdatePanels);

                            this.Controls.Add(form2); // 将 form2 添加到当前窗体中
                            form2.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误: " + ex.Message);
                }
            }
        }


    }
}
