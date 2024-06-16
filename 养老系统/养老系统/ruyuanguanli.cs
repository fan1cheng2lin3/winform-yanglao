using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using 养老系统.类文件;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 养老系统
{
    public partial class ruyuanguanli : Form
    {
        public ruyuanguanli()
        {
            InitializeComponent();
        }

        SqlDataAdapter adapter; // 合并为一个 SqlDataAdapter
        DataSet ds = new DataSet();


        void init()
        {
            DB.GetCn();
            string str = "select * from zhuangtai_Table"; // 假设 mp4_Table 包含 imagepath 和 text 字段
            adapter = new SqlDataAdapter(str, DB.cn);
            adapter.Fill(ds, "media_info"); // 将数据集表名统一为 media_info

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void ruyuanguanli_Load(object sender, EventArgs e)
        {



            // TODO: 这行代码将数据加载到表“yanglaoDataSet3.Login_Table”中。您可以根据需要移动或移除它。
            this.login_TableTableAdapter.Fill(this.yanglaoDataSet3.Login_Table);
            init();
        }



        public string InitData(string columnName)
        {
            DB.GetCn();
            string query = "SELECT " + columnName + " FROM Login_table WHERE username = '" + comboBox2.Text + "'";
            DataTable result = DB.GetDataSet(query);
            string re = "";
            if (result.Rows.Count > 0)
            {
                re = result.Rows[0][columnName].ToString().Trim();
            }
            DB.cn.Close();

            return re;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)||string.IsNullOrEmpty(comboBox2.Text))
            {
                MessageBox.Show("姓名和家属不能为空");
            }
            else
            {

                // 更新数据库
                DataRow dr = ds.Tables["media_info"].NewRow(); // 使用统一的表名

                dr["name"] = textBox1.Text;
                dr["sex"] = comboBox1.Text;
                dr["phone"] = textBox6.Text;
                dr["city"] = textBox7.Text;
                dr["decs"] = textBox8.Text;
                dr["Oid"] = textBox5.Text;
                dr["shuifei"] = "0";
                dr["dianfei"] = "0";
                dr["shenghuofei"] = "0";


                dr["jiashubianhao"] = InitData("number");
                ds.Tables["media_info"].Rows.Add(dr);

                try
                {
                    SqlCommandBuilder dbBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds, "media_info"); // 使用统一的表名
                    MessageBox.Show("增加成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                DB.cn.Close();
            }
        }
    }
}
