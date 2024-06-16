using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using 养老系统.AAA;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class Form1 : Form
    {
        private Timer timer;
        int scrollSpeed = 3; // 滚动速度
        int startX = 0; // 开始滚动的 X 坐标
        int endX = 1800; // 结束滚动的 X 坐标
        public static string StaticNewIdentifier = "";
        private int clickCount = 0;



        // 定义一个委托类型
        public delegate void UpdatePictureBoxEventHandler(Image image,string labelText);
        // 定义一个事件，使用上面的委托类型
        public event UpdatePictureBoxEventHandler UpdatePictureBoxEvent;



        // 定义一个委托类型
        public delegate void zhuangtaiid(string jiashuid);
        // 定义一个事件，使用上面的委托类型
        public event zhuangtaiid zhuangtai;


        public string kkk;

       

        // 一个方法来引发事件
        public void OnUpdatePictureBox(Image image, string labelText)
        {
            if (pictureBox1.InvokeRequired)
            {
                // 如果是在非UI线程调用，确保在UI线程上更新pictureBox1
                pictureBox1.Invoke(new Action(() => UpdatePictureBoxEvent?.Invoke(image,labelText)));
            }
            else
            {
                UpdatePictureBoxEvent?.Invoke(image, labelText);
            }
        }

        public void zhunagtaiidhuqv(string jiashuid)
        {
            
            zhuangtai?.Invoke(jiashuid);
            
        }



        public Form1()
        {
            InitializeComponent();
            

            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            //Init();

            // 初始化flowLayoutPanel
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;


           



        }

       

        private void Init()
        {
            // 初始化 Timer
            timer = new Timer();
            timer.Interval = 5; // 设置滚动速度，单位为毫秒
            timer.Tick += timer1_Tick;

            // 初始化 Label
            label1.Text = "Your scrolling text goes here..bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
            label1.AutoSize = true;
            // 启动 Timer
            timer.Start();
        }

        private void HidePanel10()
        {
            panel10.Hide(); // 或者 panel10.Visible = false;
        }
        public void HidePanel6()
        {
            panel6.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadDataFromDatabase();
            
            label2.Click += label2_Click;
            panel10.Dock = DockStyle.Fill;
            panel8.Dock = DockStyle.Fill;
            panel1.Dock = DockStyle.Fill;
            flowLayoutPanel3.Dock = DockStyle.Fill;
            panel6.Dock = DockStyle.Fill;
            panel11.Dock = DockStyle.Fill;

            

        }


        private void zhuangtaiLoadDataFromDatabase()
        {
            DB.GetCn();
            // 清空现有的控件
            flowLayoutPanel2.Controls.Clear();

            // 查询数据库
            string sql = "SELECT * FROM zhuangtai_Table where jiashubianhao ='"+ kkk + "'"; // 替换为您的表名和列名
            DataTable dataTable = DB.GetDataSet(sql);

            // 为每条记录创建Panel
            foreach (DataRow row in dataTable.Rows)
            {
                Panel panel1 = new Panel();
                panel1.Size = new Size(165, 300);
                panel1.BackColor = Color.FromArgb(240, 248, 255);
                // 创建一个新的Panel来代替Image控件
                //Panel imagePanel = new Panel();
                //imagePanel.Size = new Size(300, 400);
                //imagePanel.BackColor = Color.LightGray;
                //panel1.Controls.Add(imagePanel);

                //PictureBox pictureBox = new PictureBox();
                //pictureBox.Size = new Size(290, 390);
                //pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //pictureBox.ImageLocation = row["image"].ToString();
                //pictureBox.BackColor = Color.White;
                //pictureBox.Location = new Point(5, 5);
                //// 为PictureBox添加Click事件
                //// 将new标识符存储在Tag属性中
                //pictureBox.Tag = row["new"].ToString();
                //pictureBox.Click += new EventHandler(pictureBox_Click);

                //imagePanel.Controls.Add(pictureBox);

                // 创建Label控件并处理<br>标签以实现换行
                Label label1 = new Label();
                string labelText1 = row["chengni"].ToString().Replace("<br>", Environment.NewLine);
                label1.Text = labelText1;
                label1.AutoSize = true;
                label1.Location = new Point(60, 10);
                label1.Font = new Font(label1.Font.FontFamily, 16, FontStyle.Bold); // 设置字体大小和加粗

                panel1.Controls.Add(label1);

                // 对其他标签重复相同的处理
                Label label2 = new Label();
                string labelText2 = row["qingxv"].ToString().Replace("<br>", Environment.NewLine);
                label2.Text = labelText2;
                label2.AutoSize = true;
                label2.Location = new Point(120, 50);
                label2.Font = new Font(label2.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label2);


                Label label5 = new Label();
                label5.Text = "情绪：";
                label5.AutoSize = true;
                label5.Location = new Point(50, 50);
                label5.Font = new Font(label5.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label5);

                Label label3 = new Label();
                string labelText3 = row["Oxygen_Saturation"].ToString().Replace("<br>", Environment.NewLine);
                label3.Text = labelText3;
                label3.AutoSize = true;
                label3.Location = new Point(120, 80);
                label3.Font = new Font(label3.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label3);


                Label label6 = new Label();
                
                label6.Text = "血氧饱和度:";
                label6.AutoSize = true;
                label6.Location = new Point(5, 80);
                label6.Font = new Font(label6.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label6);

                Label label4 = new Label();
                string labelText4 = row["xinlv"].ToString().Replace("<br>", Environment.NewLine);
                label4.Text = labelText4;
                label4.AutoSize = true;
                label4.Location = new Point(120, 110);
                label4.Font = new Font(label4.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label4);


                Label label7 = new Label();
                label7.Text = "心率:";
                label7.AutoSize = true;
                label7.Location = new Point(50, 110);
                label7.Font = new Font(label7.Font.FontFamily, 10, FontStyle.Bold); // 设置字体大小和加粗
                panel1.Controls.Add(label7);


                // 第5对标签：血压（假设数据列名为 "Blood_Pressure"）
                Label label8 = new Label();
                string labelBloodPressure = row["xueya"].ToString().Replace("<br>", Environment.NewLine);
                label8.Text = labelBloodPressure;
                label8.AutoSize = true;
                label8.Location = new Point(120, 140);
                label8.Font = new Font(label8.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label8);

                Label label9 = new Label();
                label9.Text = "血压:";
                label9.AutoSize = true;
                label9.Location = new Point(50, 140);
                label9.Font = new Font(label9.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label9);

                // 第6对标签：体温（假设数据列名为 "Temperature"）
                Label label10 = new Label();
                string labelTemperature = row["Body_Temperature"].ToString().Replace("<br>", Environment.NewLine);
                label10.Text = labelTemperature;
                label10.AutoSize = true;
                label10.Location = new Point(120, 170);
                label10.Font = new Font(label10.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label10);

                Label label11 = new Label();
                label11.Text = "体温:";
                label11.AutoSize = true;
                label11.Location = new Point(50, 170);
                label11.Font = new Font(label11.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label11);

                // 第7对标签：呼吸率（假设数据列名为 "Respiratory_Rate"）
                Label label12 = new Label();
                string labelRespiratoryRate = row["xuetang"].ToString().Replace("<br>", Environment.NewLine);
                label12.Text = labelRespiratoryRate;
                label12.AutoSize = true;
                label12.Location = new Point(120, 200);
                label12.Font = new Font(label12.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label12);

                Label label13 = new Label();
                label13.Text = "血糖:";
                label13.AutoSize = true;
                label13.Location = new Point(50, 200);
                label13.Font = new Font(label13.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label13);

                // 第8对标签：胆固醇（假设数据列名为 "Cholesterol"）
                Label label14 = new Label();
                string labelCholesterol = row["Cholesterol"].ToString().Replace("<br>", Environment.NewLine);
                label14.Text = labelCholesterol;
                label14.AutoSize = true;
                label14.Location = new Point(120, 230);
                label14.Font = new Font(label14.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label14);

                Label label15 = new Label();
                label15.Text = "胆固醇:";
                label15.AutoSize = true;
                label15.Location = new Point(35, 230);
                label15.Font = new Font(label15.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label15);

                // 第9对标签：血糖（假设数据列名为 "Blood_Sugar"）
                Label label16 = new Label();
                string labelBloodSugar = row["Weight"].ToString().Replace("<br>", Environment.NewLine);
                label16.Text = labelBloodSugar;
                label16.AutoSize = true;
                label16.Location = new Point(120, 260);
                label16.Font = new Font(label16.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label16);

                Label label17 = new Label();
                label17.Text = "体重:";
                label17.AutoSize = true;
                label17.Location = new Point(50, 260);
                label17.Font = new Font(label17.Font.FontFamily, 10, FontStyle.Bold);
                panel1.Controls.Add(label17);


                Label label199 = new Label();
                string update = row["updatezhuangtai"].ToString().Replace("<br>", Environment.NewLine); 


                DateTime dateTime;
                bool success = DateTime.TryParse(update, out dateTime);

                if (success)
                {
                    // 使用自定义的格式化字符串
                    string formattedDateTime = dateTime.ToString("MM dd h:mm tt");
                    label199.Text = formattedDateTime;
                }
                else
                {
                    // 转换失败时的处理
                    label199.Text = "Invalid date format";
                }
                label199.AutoSize = true;
                label199.Location = new Point(60, 285);
                label199.Font = new Font(label199.Font.FontFamily, 8, FontStyle.Bold);
                panel1.Controls.Add(label199);

                Label label198 = new Label();
                label198.Text = "时间:";
                label198.AutoSize = true;
                label198.Location = new Point(30, 285);
                label198.Font = new Font(label198.Font.FontFamily, 8, FontStyle.Bold);
                panel1.Controls.Add(label198);




                flowLayoutPanel2.Controls.Add(panel1);
            }

            // 设置FlowLayoutPanel的属性
            //flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            //flowLayoutPanel1.WrapContents = true;


        }



        private void LoadDataFromDatabase()
        {
            DB.GetCn();
            // 清空现有的控件
            //flowLayoutPanel1.Controls.Clear();

            // 查询数据库
            string sql = "SELECT new, image, label1, label2, label3 FROM new_Table"; // 替换为您的表名和列名
            DataTable dataTable = DB.GetDataSet(sql);

            // 为每条记录创建Panel
            foreach (DataRow row in dataTable.Rows)
            {
                Panel panel1 = new Panel();
                panel1.Size = new Size(789, 450);
                panel1.BackColor = Color.Transparent;

                // 创建一个新的Panel来代替Image控件
                Panel imagePanel = new Panel();
                imagePanel.Size = new Size(789, 450);
                imagePanel.BackColor = Color.Transparent;
                panel1.Controls.Add(imagePanel);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(789, 400);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.ImageLocation = row["image"].ToString();
                pictureBox.BackColor = Color.White;
                // 为PictureBox添加Click事件
                // 将new标识符存储在Tag属性中
                pictureBox.Tag = row["new"].ToString();
                pictureBox.Click += new EventHandler(pictureBox_Click);

                imagePanel.Controls.Add(pictureBox);

                //// 创建Label控件并处理<br>标签以实现换行
                //Label label1 = new Label();
                //string labelText1 = row["label1"].ToString().Replace("<br>", Environment.NewLine);
                //label1.Text = labelText1;
                //label1.AutoSize = true;
                //label1.Location = new Point(180, 10);
                //panel1.Controls.Add(label1);

                //// 对其他标签重复相同的处理
                //Label label2 = new Label();
                //string labelText2 = row["label2"].ToString().Replace("<br>", Environment.NewLine);
                //label2.Text = labelText2;
                //label2.AutoSize = true;
                //label2.Location = new Point(180, 80);
                //panel1.Controls.Add(label2);

                //Label label3 = new Label();
                //string labelText3 = row["label3"].ToString().Replace("<br>", Environment.NewLine);
                //label3.Text = labelText3;
                //label3.AutoSize = true;
                //label3.Location = new Point(180, 140);
                //panel1.Controls.Add(label3);

                // 添加到flowLayoutPanel
                flowLayoutPanel1.Controls.Add(panel1);
            }

            // 设置FlowLayoutPanel的属性
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                
                Form1.StaticNewIdentifier = pictureBox.Tag.ToString();

                
                    // 显示panel10并滚动到顶部
                    panel2.AutoScrollPosition = new Point(0, 0);
                    panel10.BringToFront();
                    panel10.Visible = true;

                    // 移除panel10中除了当前要显示的子窗体外的所有子窗体
                    foreach (Control control in panel10.Controls.Cast<Control>().Reverse()) // 使用LINQ反转集合以便安全移除
                    {
                        if (control is AA1)
                        {
                            control.Dispose();
                        }
                    }

                    // 创建AA1窗体的实例
                    AA1 aa1 = new AA1(); // 传递newIdentifier给AA1的构造函数
                    aa1.TopLevel = false; // 设置子窗体为非顶层窗体
                    aa1.FormBorderStyle = FormBorderStyle.None; // 去除子窗体的边框
                    panel10.Controls.Add(aa1); // 将子窗体添加到panel10中
                    aa1.Dock = DockStyle.Fill; // 让子窗体填充整个panel10
                    aa1.Show(); // 显示子窗体

                    // 绑定关闭panel10的委托
                    aa1.HidePanelHandler = new AA1.HidePanelDelegate(HidePanel10);
               
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(kkk);
            // 关闭panel10中的其它子窗体
           
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            C1 form2 = new C1();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C1.HidePanelDelegate(HidePanel10);

            
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }

           
        }


        private void button9_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
            panel2.AutoScrollPosition = new Point(0, 0);

            panel10.BringToFront();
            panel10.Visible = true;
            C3 form2 = new C3();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框

            form2.WindowState = FormWindowState.Maximized;
            flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C3.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                    form.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);

            panel10.BringToFront();

            panel10.Visible = true;
            C4 form2 = new C4();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框

            form2.WindowState = FormWindowState.Maximized;
            flowLayoutPanel3.Controls.Add(form2);// 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C4.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                    form.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);

            panel10.BringToFront();

            panel10.Visible = true;
            C5 form2 = new C5();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框

            flowLayoutPanel3.Controls.Add(form2);// 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C5.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);

            panel10.BringToFront();

            panel10.Visible = true;
            C6 form2 = new C6();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框

             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体


            form2.HidePanelHandler = new C6.HidePanelDelegate(HidePanel10);

            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void UpdatePanels()
        {
            panel6.Visible = true;
            panel11.Visible = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            

        }


       
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 向左滚动
            label1.Left -= scrollSpeed;

            // 检查是否超出左边界
            if (label1.Right < startX)
            {
                // 将 Label 重新放置到结束位置
                label1.Left = endX;
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);

            panel10.BringToFront();
            panel10.Visible = true;
            C7 form2 = new C7();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C7.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            C8 form2 = new C8();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C8.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            C9 form2 = new C9();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C9.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            C10 form2 = new C10();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C10.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

       

        private void panel20_MouseDown(object sender, MouseEventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            AA1 form2 = new AA1();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new AA1.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label26_MouseDown(object sender, MouseEventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            AA1 form2 = new AA1();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体

            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
             flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new AA1.HidePanelDelegate(HidePanel10);
            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }


        private void button15_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            using (Login loginForm = new Login(this))
            {
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    panel6.Visible = false;
                    panel11.Visible = false;
                }
            }
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Signup signup = new Signup();
            signup.ShowDialog();
        }


        private void label2_Click(object sender, EventArgs e)
        {

            clickCount++; // 每次点击时，点击次数加1
          
            if (clickCount == 3) 
            {
                Loginlogcs newForm = new Loginlogcs();
                newForm.Show();
                
                clickCount = 0;
                
            }

            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel26_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 关闭panel10中的其它子窗体

            panel2.AutoScrollPosition = new Point(0, 0);
            panel10.BringToFront();
            panel10.Visible = true;

            C11 form2 = new C11(this);
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel3.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体

            form2.HidePanelHandler = new C11.HidePanelDelegate(HidePanel10);
            form2.OnLoginSuccess = new C11.HidePanelDelegate2(UpdatePanels);


            // 关闭panel10中的其它子窗体
            foreach (Control control in panel10.Controls)
            {
                if (control is Form form && control != panel10.Controls[panel10.Controls.Count - 1])
                {
                    form.Close();
                }
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            
             zhuangtaiLoadDataFromDatabase();
               
            
        }
    }
}
