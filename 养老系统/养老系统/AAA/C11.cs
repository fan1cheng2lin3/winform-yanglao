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
using System.Xml.Linq;
using 养老系统.类文件;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace 养老系统
{
    public partial class C11 : Form
    {
        Form1 form1;

        private string name;
        private string name2;
        public C11(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;

            // 订阅Form1的事件
            this.form1.UpdatePictureBoxEvent += Form1_UpdatePictureBoxEvent;

            name = Login.bbb;
            
        }

        // 事件处理方法
        private void Form1_UpdatePictureBoxEvent(Image image, string labelText)
        {
            // 这里使用Form1的pictureBox1控件
            form1.pictureBox1.Image = image;

            form1.label28.Text = labelText;
        }

        public static string path_source = "";
        DataSet ds = new DataSet();

        public delegate void HidePanelDelegate();
        public HidePanelDelegate HidePanelHandler; // 声明一个委托变量

        public delegate void HidePanelDelegate2();
        public HidePanelDelegate2 OnLoginSuccess; // 声明一个委托变量
       
        
        
        private void LoadDataFromDatabase()
        {
            DB.GetCn();
            flowLayoutPanel1.Controls.Clear();

            string sql = "SELECT name,chengni FROM zhuangtai_Table where jiashubianhao = '"+ Login.bbb +"'"; // 替换为您的表名和列名

            DataTable dataTable = DB.GetDataSet(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                

                Panel panel1 = new Panel();
                panel1.Size = new Size(789, 100);
                panel1.BackColor = Color.FromArgb(240, 248, 255);

                Label label1 = new Label();
                string labelText1 = row["name"].ToString().Replace("<br>", Environment.NewLine);
                label1.Text = labelText1;
                label1.AutoSize = true;
                label1.Location = new Point(250, 10);
                label1.Font = new Font(label1.Font.FontFamily, 30, FontStyle.Bold);
                label1.Enabled = true;
                label1.FlatStyle = FlatStyle.Flat;
                label1.BringToFront();
                label1.Click += new EventHandler(Control_Click);
                panel1.Click += new EventHandler(Control_Click);
                panel1.Controls.Add(label1);

                flowLayoutPanel1.Controls.Add(panel1);
            }

            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
        }

        public static string nn;

        private void Control_Click(object sender, EventArgs e)
        {
            if (sender is Control clickedControl)
            {
                //检查 clickedControl 是否是 Label 或 PictureBox
                if (clickedControl is Label || clickedControl is PictureBox)
                {
                    // 找到 Panel 控件
                    Panel panel = clickedControl.Parent as Panel;

                    // 确保 panel 是 flowLayoutPanel1 的子控件
                    if (panel != null && flowLayoutPanel1.Controls.Contains(panel))
                    {
                        panel7.Visible =true;


                        string username = ((Label)panel.Controls[0]).Text.Trim(); // 假设 label1 是面板的第一个控件
                        nn = username;


                        string query = "SELECT * FROM zhuangtai_Table WHERE name = '" + username + "'";
                        DataTable result = DB.GetDataSet(query);
                        if (result.Rows.Count > 0)
                        {
                            textBox2.Text = result.Rows[0]["chengni"].ToString();
                            textBox3.Text = result.Rows[0]["name"].ToString();
                            textBox4.Text = result.Rows[0]["phone"].ToString();
                            textBox5.Text = result.Rows[0]["city"].ToString();
                            textBox6.Text = result.Rows[0]["decs"].ToString();
                            //MessageBox.Show("mei");
                            if (result.Rows[0]["date"] != DBNull.Value)
                            {
                                dateTimePicker1.Value = Convert.ToDateTime(result.Rows[0]["date"]);
                            }
                            else
                            {
                                dateTimePicker1.Value = DateTime.Now; // 或其他默认值
                            }
                            

                        }

                        



                    }
                }
            }
        }

        public static bool UpdateZhuangtaiRecord(string name, string chengji, DateTime date, string phone, string city,  string decs,string chengjiNumber)
        {



            string updateQuery = "UPDATE zhuangtai_table SET " +
                                 "name = @name, " +
                                 "chengni = @chengni, " +
                                 "date = @date, " +
                                 "phone = @phone, " +
                                 "city = @city, " +
                                 "decs = @decs " +
                                 "WHERE name = @name2";

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // 为参数赋值
                    command.Parameters.AddWithValue("@name", name); 
                    command.Parameters.AddWithValue("@chengni", chengji);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@decs", decs);
                    command.Parameters.AddWithValue("@name2", chengjiNumber);

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

        private void button6_Click(object sender, EventArgs e)
        {




            UpdateZhuangtaiRecord(textBox3.Text, textBox2.Text, dateTimePicker1.Value, textBox4.Text, textBox5.Text, textBox6.Text,nn);
            MessageBox.Show("修改成功");

            



        }

        private void C11_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
            panel8.Visible = false;
            panel7.Visible = false;
            panel8.Dock = DockStyle.Fill;
            panel7.Dock = DockStyle.Fill;
            inittouxiang();

            
        }

        


        public void inittouxiang()
        {
            DB.GetCn();

            string query = "SELECT touxiang FROM Login_table WHERE number = '" + name + "'";

            DataTable result = DB.GetDataSet(query);
            if (result.Rows.Count > 0)
            {
                string a = result.Rows[0]["touxiang"].ToString();
                pictureBox1.Image = System.Drawing.Image.FromFile(a);

            }

            DB.cn.Close();
        }
        
        private void label2_Click(object sender, EventArgs e)
        {
            HidePanelHandler?.Invoke();
            this.Close();
        }

        public bool LoginSuccess { get; private set; }
        public static bool isSuccess = false;


        private void button1_Click_1(object sender, EventArgs e)
        {
            isSuccess = true;
            LoginSuccess = isSuccess;

            if (isSuccess)
            {
                this.DialogResult = DialogResult.OK;
                HidePanelHandler?.Invoke();
                OnLoginSuccess?.Invoke();

            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {

                //名称
                panel8.Visible = true;
                label18.Text = "名字";
                textBox1.Text = InitData("username");

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }


        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            panel8.Visible = true;
            label18.Text = "邮箱";
            textBox1.Text = InitData("email");
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            panel8.Visible = true;
            label18.Text = "现住址";
            textBox1.Text = InitData("city");
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        public void genxin()
        {
            Image image = Image.FromFile(InitData("touxiang"));
            form1.OnUpdatePictureBox(image, InitData("username"));
        }

       

        public string InitData(string columnName)
        {
            DB.GetCn();
            string query = "SELECT " + columnName + " FROM Login_table WHERE number = '" + name + "'";
            DataTable result = DB.GetDataSet(query);
            string re = "";
            if (result.Rows.Count > 0)
            {
                re = result.Rows[0][columnName].ToString().Trim();
            }
            DB.cn.Close();

            return re;
        }

      
        public void initupdata(string columnName, string columnName2)
        {

            DB.GetCn();
            string updateQuery = "UPDATE Login_table SET " + columnName + " = '" + columnName2 + "' WHERE number = '" + name + "'";
            DB.sqlEx(updateQuery);
            DB.cn.Close();
        }

        //initupdata2(textBox2.Text, nn);

        public void initupdata2(string columnName, string columnName4)
        {

            DB.GetCn();
            string updateQuery = "UPDATE older_Table SET chengni = '" + columnName+ "' WHERE chengni = '" + columnName4 + "'";
            DB.sqlEx(updateQuery);
            DB.cn.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path_source = dlg.FileName;
                pictureBox1.Image = System.Drawing.Image.FromFile(path_source);

                DB.GetCn();
                string updateQuery = "UPDATE Login_table SET touxiang = '" + path_source + "' WHERE number = '" + name + "'";
                DB.sqlEx(updateQuery);
                DB.cn.Close();

                genxin();

            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            if(label18.Text == "名字")
            {

                string query = "SELECT * FROM login_Table WHERE username = @Name";
                SqlCommand cmd2 = new SqlCommand(query, DB.GetCn());
                cmd2.Parameters.AddWithValue("@Name", textBox1.Text);
                DataTable dt = DB.GetDataSet(cmd2);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("此用户名已存在，请重新输入");
                }
                else
                {
                    initupdata("username", textBox1.Text.Trim());
                    form1.label28.Text = textBox1.Text;
                    MessageBox.Show("已更换名字");
                    name2 = name;

                }
                
            }
            else if(label18.Text == "邮箱")
            {
                initupdata("email", textBox1.Text.Trim());
                MessageBox.Show("已更换邮箱");
            }
            else if (label18.Text == "现住址")
            {
                initupdata("city", textBox1.Text.Trim());
                MessageBox.Show("已更换现住址");

            }
            else if (label18.Text == "")
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }

       
    }
}
