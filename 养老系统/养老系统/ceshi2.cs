using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Messaging;
using System.Threading.Tasks;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class ceshi2 : Form
    {
        private const string CurrentUser = "3";
        private const string ToUser = "2";

        private string msgPath = ".\\Private$\\MyMsg";
        SqlDataAdapter savetext;
        DataSet ds = new DataSet();

        public static bool shouldReceiveMsg = true;


       


        public ceshi2()
        {
            InitializeComponent();
            ReceiveMsg(msgPath);
        }

        void init()
        {
            DB.GetCn();
            string str = "select * from info_Table2";
            savetext = new SqlDataAdapter(str, DB.cn);
            savetext.Fill(ds, "info_info");
        }

        private void DisplayMessagesFromDatabase()
        {
            if (DB.cn.State == ConnectionState.Closed)
            {
                DB.cn.Open();
            }

            try
            {
                panel1.Controls.Clear();

                string query = "SELECT text, customer, tocustomer, date FROM info_Table2 WHERE (customer = @currentUser AND tocustomer = @toUser) OR (customer = @toUser AND tocustomer = @currentUser) ORDER BY date ASC";
                using (SqlCommand cmd = new SqlCommand(query, DB.cn))
                {
                    cmd.Parameters.AddWithValue("@currentUser", CurrentUser);
                    cmd.Parameters.AddWithValue("@toUser", ToUser);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string text = reader["text"].ToString();
                            string customer = reader["customer"].ToString();
                            string tocustomer = reader["tocustomer"].ToString();
                            DateTime date = reader["date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["date"]);

                            if ((customer == CurrentUser && tocustomer == ToUser) || (customer == ToUser && tocustomer == CurrentUser))
                            {
                                TextBox newTextBox = new TextBox();
                                newTextBox.Text = text;
                                newTextBox.ReadOnly = true;
                                newTextBox.Font = new Font(newTextBox.Font.FontFamily, 30, FontStyle.Bold);
                                newTextBox.TextAlign = HorizontalAlignment.Right;

                                using (Graphics graphics = panel1.CreateGraphics())
                                {
                                    SizeF textSize = graphics.MeasureString(text, newTextBox.Font);
                                    newTextBox.Width = (int)(textSize.Width + 10);
                                }

                                int currentHeight = 0;
                                foreach (Control c in panel1.Controls)
                                {
                                    if (c is TextBox)
                                    {
                                        currentHeight = Math.Max(currentHeight, c.Bottom);
                                    }
                                }

                                int xPosition = (customer == CurrentUser && tocustomer == ToUser) ? (panel1.ClientSize.Width - newTextBox.Width - 200) : 100;
                                newTextBox.Location = new Point(xPosition, currentHeight + 20);

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

        private void ReceiveMsg(string msgPath)
        {
            if (MessageQueue.Exists(msgPath))
            {
                try
                {
                    using (MessageQueue mq = new MessageQueue(msgPath))
                    {
                        mq.ReceiveCompleted += new ReceiveCompletedEventHandler(ReceiveCompletedCallback);
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

                    if (mqMessage.Formatter == null)
                    {
                        mqMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    }

                    string messageText = mqMessage.Body.ToString();

                    TextBox newTextBox = new TextBox();
                    newTextBox.Text = messageText;
                    newTextBox.ReadOnly = true;
                    newTextBox.Font = new Font(newTextBox.Font.FontFamily, 30, FontStyle.Bold);
                    newTextBox.TextAlign = HorizontalAlignment.Right;

                    using (Graphics graphics = panel1.CreateGraphics())
                    {
                        SizeF textSize = graphics.MeasureString(messageText, newTextBox.Font);
                        newTextBox.Width = (int)(textSize.Width + 10);
                    }

                    int currentHeight = 0;
                    foreach (Control c in panel1.Controls)
                    {
                        if (c is TextBox)
                        {
                            currentHeight = Math.Max(currentHeight, c.Bottom);
                        }
                    }

                    newTextBox.Location = new Point(100, currentHeight + 20);

                    panel1.Controls.Add(newTextBox);
                    panel1.ScrollControlIntoView(newTextBox);
                    panel1.PerformLayout();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("消息队列接收错误: " + ex.Message);
            }
            finally
            {
                try
                {
                    ((MessageQueue)sender).BeginReceive();
                }
                catch (MessageQueueException ex)
                {
                    MessageBox.Show("消息队列接收错误: " + ex.Message);
                }
            }
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
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void ceshi2_Load(object sender, EventArgs e)
        {
            init();
            DisplayMessagesFromDatabase();
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            shouldReceiveMsg = false;
            textbox();
            DisplayMessagesFromDatabase();
            await Task.Delay(1000);
            shouldReceiveMsg = true;
        }

        public void savedatabase(string text)
        {
            DataRow dr = ds.Tables["info_info"].NewRow();
            dr["text"] = text;
            dr["date"] = DateTime.Now;
            dr["tocustomer"] = ToUser;
            dr["customer"] = CurrentUser;
            ds.Tables["info_info"].Rows.Add(dr);

            try
            {
                SqlCommandBuilder db = new SqlCommandBuilder(savetext);
                savetext.Update(ds, "info_info");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}
