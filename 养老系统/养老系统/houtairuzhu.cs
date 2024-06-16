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
    public partial class houtairuzhu : Form
    {
        public houtairuzhu()
        {
            InitializeComponent();
        }

        SqlDataAdapter daorder;
        DataSet ds = new DataSet();

        void init()
        {
            DB.GetCn();
            string str = "select * from ruzu_Table";
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


        private void houtairuzhu_Load(object sender, EventArgs e)
        {
            showXz();
            showAll();
            DataView dvOrder = new DataView(ds.Tables["order_info"]);
           
            dataGridView1.DataSource = dvOrder;


            // 假设您有以下列名称，并希望设置它们在DataGridView中的宽度
            string[] columnWidths = new string[] 
            {
                "Status",
                "number",
                "name",
                "sex",
                "ruzhu",
                 "phone",
                "city",
               
                "decs",
                
            };

            // 列的宽度设置值
            int[] widths = new int[] 
            {
                60,
                30,
                60,
                30,
                80,
                100,
                200,
                250

            };

            // 确保列名称数组和宽度数组长度相同
            if (columnWidths.Length == widths.Length)
            {
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    // 通过列名称设置宽度
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Name.Equals(columnWidths[i], StringComparison.OrdinalIgnoreCase))
                        {
                            column.Width = widths[i];
                            break; // 找到匹配的列后退出循环
                        }
                    }
                }
            }
            else
            {
                // 如果数组长度不匹配，抛出异常或处理错误
                throw new InvalidOperationException("列名称数组和宽度数组长度不匹配。");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int s = dataGridView1.Rows.Count;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["acCode"].EditedFormattedValue.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["Status"].Value = "已联系";
                    UpdateDB();
                }
                else
                {
                    s = s - 1;
                }

            }
            if (s == 0)
            {
                MessageBox.Show("请选择要联系的项");
            }
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
    }
}
