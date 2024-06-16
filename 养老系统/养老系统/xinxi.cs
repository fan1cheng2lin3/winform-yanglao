﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Messaging;
using System.Threading.Tasks;
using 养老系统.类文件;
using AxWMPLib;
using WMPLib;

namespace 养老系统
{
    public partial class xinxi : Form
    {
        public string Label1Text
        {
            get { return label1.Text; }
            set
            {
                label1.Text = value;
            }
        }


        public string currentUser;
        private static string b;
        public delegate void HidePanelDelegate3();
        public HidePanelDelegate3 HidePanelHandler3; // 声明一个委托变量
        private string msgPath = ".\\Private$\\MyMsg";  // 消息队列路径

        public string ccc;


        public string toUser;

        public static bool shouldReceiveMsg = true;

        SqlDataAdapter savetext; // 合并为一个 SqlDataAdapter
        DataSet ds = new DataSet();


        public string user;

        void init()
        {
            DB.GetCn();
            string str = "select * from info_Table2";
            savetext = new SqlDataAdapter(str, DB.cn);
            savetext.Fill(ds, "info_info");
        }

        private string GetNumberFromLoginTable()
        {
            DB.GetCn();
            string query = "SELECT username FROM Login_table WHERE number = '" + Login.bbb + "'";
            DataTable result = DB.GetDataSet(query);
            DB.cn.Close();
            return result.Rows[0]["username"].ToString().Trim();
        }

        private string GetNumberFromLoginTable2()
        {
            DB.GetCn();
            string query = "SELECT number FROM Login_table WHERE username = '" + GetNumberFromLoginTable() + "'";
            DataTable result = DB.GetDataSet(query);
            DB.cn.Close();
            return result.Rows[0]["number"].ToString().Trim();
        }



        public xinxi()
        {
            InitializeComponent();
            InitializeMediaPlayer();
            SetCurrentUser(C1.lk);
            ReceiveMsg(msgPath);
            user = GetNumberFromLoginTable();
            toUser = GetNumberFromLoginTable2();
        }

        public void SetCurrentUser(string username)
        {
            currentUser = username;
        }

        private void xinxi_Load(object sender, EventArgs e)
        {
            init();
            DisplayMessagesFromDatabase();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1 != null)
            {
                // 禁用消息接收
                shouldReceiveMsg = false;
                //await Task.Delay(500);
                textbox();
                DisplayMessagesFromDatabase();

                // 延时3秒
                await Task.Delay(3000);
                // 重新启用消息接收
                shouldReceiveMsg = true;
            }
        }

        public void savedatabase(string text)
        {
            DataRow dr = ds.Tables["info_info"].NewRow();
            dr["text"] = text;
            dr["date"] = DateTime.Now;
            dr["customer"] = toUser;
            dr["tocustomer"] = currentUser;
            ds.Tables["info_info"].Rows.Add(dr);

            try
            {
                SqlCommandBuilder db = new SqlCommandBuilder(savetext);
                savetext.Update(ds, "info_info");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存到数据库失败: " + ex.Message);
            }

            DB.cn.Close();
        }

        public void textbox()
        {
            string studentName = textBox1.Text;
            SendMsg(msgPath, studentName);
            SendMsg(msgPath, studentName);
            savedatabase(studentName);
            textBox1.Text = "";
        }

        private void SendMsg(string mQPath, string studentName)
        {
            try
            {
                MessageQueue mq = MessageQueue.Exists(mQPath) ? new MessageQueue(mQPath) : MessageQueue.Create(mQPath);
                mq.Send(studentName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息到消息队列错误: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            HidePanelHandler3?.Invoke();
            this.Close();
        }


        private void ReceiveMsg(string msgPath)
        {

            if (MessageQueue.Exists(msgPath))
            {
                try
                {
                    using (MessageQueue mq = new MessageQueue(msgPath))
                    {
                        // 设置消息接收完成事件的处理方法
                        mq.ReceiveCompleted += new ReceiveCompletedEventHandler(ReceiveCompletedCallback);
                        // 开始异步接收消息
                        mq.BeginReceive();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("消息队列不存在。");
            }
        }

        private void ReceiveCompletedCallback(object sender, ReceiveCompletedEventArgs e)
        {
            // 检查窗体是否已经被关闭
            if (this.IsDisposed || this.Disposing || this.panel1.IsDisposed || this.panel1.Disposing)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new ReceiveCompletedEventHandler(ReceiveCompletedCallback), sender, e);
                return;
            }

            try
            {
                if (!shouldReceiveMsg) return;
                if (sender is MessageQueue mq)
                {
                    System.Messaging.Message mqMessage = mq.EndReceive(e.AsyncResult);

                    // 确保消息队列的消息格式与Formatter匹配
                    // 如果发送消息时使用的是XmlMessageFormatter，则需要在这里设置
                    if (mqMessage.Formatter == null)
                    {
                        mqMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    }

                    string messageText = mqMessage.Body.ToString();
                    string studentName = messageText;

                    // 创建一个新的文本框并设置其文本
                    TextBox newTextBox = new TextBox();
                    newTextBox.Text = studentName;
                    newTextBox.ReadOnly = true;  // 设置为只读
                    newTextBox.Font = new Font(newTextBox.Font.FontFamily, 18, FontStyle.Bold); // 设置字体大小和加粗
                    newTextBox.TextAlign = HorizontalAlignment.Right;  // 文本右对齐






                    // 测量文本的宽度并设置文本框的宽度
                    using (Graphics graphics = panel1.CreateGraphics())
                    {
                        SizeF textSize = graphics.MeasureString(studentName, newTextBox.Font);
                        newTextBox.Width = (int)(textSize.Width + 10); // 10是额外的边距
                    }

                    // 确定文本框的位置
                    int currentHeight = 0;
                    foreach (Control c in panel1.Controls)
                    {
                        if (c is TextBox)
                        {
                            currentHeight = Math.Max(currentHeight, c.Bottom);
                        }
                    }
                    // 将新文本框放置在panel1的右边，留出一定的边距
                    newTextBox.Location = new Point(100, currentHeight + 20); // 5是边距


                    Label userLabel = new Label();
                    userLabel.Font = new Font(userLabel.Font.FontFamily, 16, FontStyle.Bold);
                    userLabel.AutoSize = true; // 根据文本内容自动调整大小
                    userLabel.Text = Label1Text;
                    // 确定Label的位置
                    userLabel.Location = new Point(30, newTextBox.Top + (newTextBox.Height - userLabel.Height) / 2);

                    // 将文本框和Label添加到panel1
                    panel1.Controls.Add(userLabel);
                    panel1.Controls.Add(newTextBox);
                    panel1.ScrollControlIntoView(newTextBox);
                    // 更新panel1的布局
                    panel1.PerformLayout();

                    // 设置自动完成模式和源
                    newTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    newTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;



                }
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    MessageBox.Show("消息队列接收超时，队列中没有消息。");
                }
                else
                {
                    //MessageBox.Show("消息队列错误: " + ex.Message);
                }

            }
            finally
            {
                try
                {
                    // 继续接收下一条消息，但不再在这里重置 shouldReceiveMsg
                    ((MessageQueue)sender).BeginReceive();
                }
                catch (MessageQueueException ex)
                {
                    // 处理异常
                    MessageBox.Show("消息队列接收错误: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
        }

        public string SelectedVideoPath2 { get; private set; }


        private AxWindowsMediaPlayer axWindowsMediaPlayer;

        public void DisplayMessagesFromDatabase()
        {


            if (DB.cn.State == ConnectionState.Closed)
            {
                DB.cn.Open();
            }

            try
            {
                panel1.Controls.Clear();

                // 修改SQL查询语句以按日期排序
                string query = "SELECT text, customer, tocustomer, date FROM info_Table2 WHERE (customer = @currentUser AND tocustomer = @toUser) OR (customer = @toUser AND tocustomer = @currentUser) ORDER BY date ASC";
                using (SqlCommand cmd = new SqlCommand(query, DB.cn))
                {
                    // 添加参数并设置它们的值
                    cmd.Parameters.AddWithValue("@currentUser", currentUser);
                    cmd.Parameters.AddWithValue("@toUser", toUser);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string text = reader["text"].ToString();
                            string customer = reader["customer"].ToString();
                            string tocustomer = reader["tocustomer"].ToString();
                            DateTime date = reader["date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["date"]);

                            // 仅当满足特定条件时才创建文本框
                            if ((customer == toUser && tocustomer == currentUser) || (customer == currentUser && tocustomer == toUser))
                            {
                                // 创建一个新的文本框并设置其文本
                                TextBox newTextBox = new TextBox();
                                newTextBox.Text = text;
                                newTextBox.ReadOnly = true;
                                newTextBox.Font = new Font(newTextBox.Font.FontFamily, 18, FontStyle.Bold);
                                newTextBox.TextAlign = HorizontalAlignment.Right;

                                // 测量文本的宽度并设置文本框的宽度
                                using (Graphics graphics = panel1.CreateGraphics())
                                {
                                    SizeF textSize = graphics.MeasureString(text, newTextBox.Font);
                                    newTextBox.Width = (int)(textSize.Width + 10);
                                }

                                // 确定文本框的位置
                                int currentHeight = 0;
                                foreach (Control c in panel1.Controls)
                                {
                                    if (c is TextBox)
                                    {
                                        currentHeight = Math.Max(currentHeight, c.Bottom);
                                    }
                                }

                                // 根据customer和tocustomer的值决定文本框位置
                                int xPosition = (customer == toUser && tocustomer == currentUser) ? (panel1.ClientSize.Width - newTextBox.Width - 200) : 5;
                                newTextBox.Location = new Point(xPosition + 100, currentHeight + 20);


                                // 创建Label控件来显示用户名
                                Label userLabel = new Label();
                                userLabel.Font = new Font(userLabel.Font.FontFamily, 16, FontStyle.Bold);
                                userLabel.AutoSize = true; // 根据文本内容自动调整大小

                                // 根据customer和tocustomer的值设置Label的文本
                                if (customer == toUser && tocustomer == currentUser)
                                {
                                    userLabel.Text = user; // 左侧显示当前界面上的用户名
                                                           // 确定Label的位置


                                    userLabel.Location = new Point(600, newTextBox.Top + (newTextBox.Height - userLabel.Height) / 2);
                                }
                                else if (customer == currentUser && tocustomer == toUser)
                                {
                                    userLabel.Text = Label1Text;
                                    // 确定Label的位置
                                    userLabel.Location = new Point(30, newTextBox.Top + (newTextBox.Height - userLabel.Height) / 2);
                                }


                                // 创建AxWindowsMediaPlayer的实例
                                //axWindowsMediaPlayer = new AxWindowsMediaPlayer();

                                //// 设置AxWindowsMediaPlayer控件的属性
                                //axWindowsMediaPlayer.Dock = DockStyle.Fill; // 填充父容器
                                //axWindowsMediaPlayer.uiMode = "none"; // 隐藏默认的WMP控件界面

                                //// 将AxWindowsMediaPlayer控件添加到窗体中
                                //this.Controls.Add(axWindowsMediaPlayer);

                                //// 播放媒体文件
                                //axWindowsMediaPlayer.URL = SelectedVideoPath2; // 设置要播放的媒体文件路径
                                //axWindowsMediaPlayer.Ctlcontrols.play(); // 开始播放

                                //axWindowsMediaPlayer.Location =

                                //panel1.Controls.Add(axWindowsMediaPlayer);
                                panel1.Controls.Add(userLabel);
                                panel1.Controls.Add(newTextBox);
                                panel1.ScrollControlIntoView(newTextBox);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("显示消息失败: " + ex.Message);
            }
            finally
            {
                if (DB.cn.State == ConnectionState.Open)
                {
                    DB.cn.Close();
                }
            }
        }

        private void InitializeMediaPlayer()
        {
            // 创建并配置AxWindowsMediaPlayer控件
            axWindowsMediaPlayer = new AxWindowsMediaPlayer();
            //axWindowsMediaPlayer.Dock = DockStyle.Fill; // 根据需要设置Dock属性
            //axWindowsMediaPlayer.uiMode = "none"; // 隐藏默认的WMP控件界面
            axWindowsMediaPlayer.Visible = false; // 初始时不显示
            this.Controls.Add(axWindowsMediaPlayer); // 添加到窗体
        }

        private void ShowMediaPlayerAndPlayVideo(string videoPath)
        {
            // 显示媒体播放器控件
            axWindowsMediaPlayer.Visible = true;

            // 设置媒体文件路径并播放
            axWindowsMediaPlayer.URL = videoPath;
            axWindowsMediaPlayer.Ctlcontrols.play();
        }

        // 自定义方法，用于获取视频截图路径
        private string[] GetVideoThumbnailPaths()
        {
            // 这里应该是您的逻辑来获取视频截图的路径
            // 例如从数据库查询或从文件系统中读取
            // 返回视频截图的路径数组
            return new string[] { "path_to_thumbnail_1.jpg", "path_to_thumbnail_2.jpg" }; // 示例路径
        }

        private void button5_Click(object sender, EventArgs e)
        {
            shouldReceiveMsg = true;
        }

        private void xinxi_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 禁用消息接收
            shouldReceiveMsg = false;

            // 如果您的消息队列接收是在一个单独的线程或者异步操作中进行的，
            // 这里可以添加逻辑来等待那个操作完成，或者执行其他清理任务。

            // 如果您使用了 MessageQueue 并且需要关闭它，请确保在这里关闭连接
            if (MessageQueue.Exists(msgPath))
            {
                try
                {
                    using (MessageQueue mq = new MessageQueue(msgPath))
                    {
                        // 如果之前注册了 ReceiveCompleted 事件，在这里注销
                        mq.ReceiveCompleted -= ReceiveCompletedCallback;
                        // 如果需要停止接收，可以调用下面的方法
                        // mq.CancelReceive(); // 取消接收操作
                    }
                }
                catch (MessageQueueException ex)
                {
                    MessageBox.Show("清理消息队列时出错: " + ex.Message);
                }
            }

            // 执行其他必要的清理工作
            // ...

            // 如果您使用了数据库连接，请确保在这里关闭连接
            if (DB.cn.State == ConnectionState.Open)
            {
                DB.cn.Close();
            }
        }
    }
    
}
