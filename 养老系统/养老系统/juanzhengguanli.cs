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

namespace 养老系统
{
    public partial class juanzhengguanli : Form
    {
        public juanzhengguanli()
        {
            InitializeComponent();
        }

        SqlDataAdapter daorder;
        DataSet ds = new DataSet();

        void init()
        {
            DB.GetCn();
            string str = "select * from juanzheng_Table";
            daorder = new SqlDataAdapter(str, DB.cn);
            daorder.Fill(ds, "order_info");
        }



        void showAll()
        {
            init();
            

        }

        void showXz()
        {

            DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
            acCode.Name = "acCode";
            acCode.HeaderText = "选择";
            acCode.Width = 30;
            dataGridView1.Columns.Add(acCode);
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            
            int s = dataGridView1.Rows.Count;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["acCode"].EditedFormattedValue.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["Status"].Value = "已审核";
                    UpdateDB();
                }
                else
                {
                    s = s - 1;
                }

            }
            if (s == 0)
            {
                MessageBox.Show("请选择要审核的项");
            }

            DB.cn.Close();
            label2.Text = GetAuditedCostTotal().ToString() + "元";


        }


        public decimal GetAuditedCostTotal()
        {
            string selectQuery = "SELECT SUM(cost) AS TotalCost FROM juanzheng_Table WHERE Status = '已审核'";
            decimal totalCost = 0;

            // 使用GetCn方法获取数据库连接
            using (SqlConnection connection = DB.GetCn())
            {
                // 确保数据库连接是打开的
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    try
                    {
                        // 执行命令
                        object result = command.ExecuteScalar();

                        // 检查结果是否为null（如果没有行匹配或结果为DBNull）
                        if (result != null && result != DBNull.Value)
                        {
                            // 将结果转换为decimal类型
                            totalCost = Convert.ToDecimal(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 处理异常
                        MessageBox.Show("查询失败：" + ex.Message);
                    }
                }
            }

            // 返回计算的总和
            return totalCost;
        }




        void UpdateDB()
        {
            try
            {
                SqlCommandBuilder dbOrder = new SqlCommandBuilder(daorder);
                daorder.Update(ds, "Order_info");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dataGridView1.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
                    if (i != e.RowIndex)
                    {
                        ck.Value = false;
                    }
                    else
                    {
                        ck.Value = true;
                    }
                }

            }
        }

        private void juanzhengguanli_Load_1(object sender, EventArgs e)
        {
            label2.Text = GetAuditedCostTotal().ToString() + "元";
            showXz();
            showAll();
            DataView dvOrder = new DataView(ds.Tables["order_info"]);
            dataGridView1.DataSource = dvOrder;
        }
    }
}
