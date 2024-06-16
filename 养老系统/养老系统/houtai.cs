﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using 养老系统.类文件;
using static System.Net.Mime.MediaTypeNames;

namespace 养老系统
{
    public partial class houtai : Form
    {
        public houtai()
        {
            InitializeComponent();
        }


        public static string path_source = "";
        public static string mp4path_source = "";

        SqlDataAdapter adapter; // 合并为一个 SqlDataAdapter
        DataSet ds = new DataSet();


        void init()
        {
            DB.GetCn();
            string str = "select * from mp4_Table"; // 假设 mp4_Table 包含 imagepath 和 text 字段
            adapter = new SqlDataAdapter(str, DB.cn);
            adapter.Fill(ds, "media_info"); // 将数据集表名统一为 media_info

        }

        private void houtai_Load(object sender, EventArgs e)
        {

            panel2.Dock = DockStyle.Fill;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            
            axWindowsMediaPlayer1.uiMode = "none";
            init();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path_source = dlg.FileName;
                pictureBox1.Image = System.Drawing.Image.FromFile(path_source);
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        public string SelectedVideoPath { get; private set; }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog videoFile = new OpenFileDialog();
            videoFile.Title = "选择视频文件";
            videoFile.Filter = "视频文件|*.mp4;*.avi;*.wmv;*.mkv;*.flv"; // 根据需要设置支持的视频文件类型

            // 显示 OpenFileDialog 并检查用户是否选择了文件
            if (videoFile.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的视频文件的路径
                string videoPath = videoFile.FileName;

                SelectedVideoPath = videoPath;
                // 设置 AxWindowsMediaPlayer 控件的 URL 属性为视频文件路径
                axWindowsMediaPlayer1.URL = videoPath;
                axWindowsMediaPlayer1.uiMode = "full";
                // 播放视频
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("标题不能为空");
            }
            else if (string.IsNullOrEmpty(SelectedVideoPath)) // 确保视频路径不为空
            {
                MessageBox.Show("请先选择视频文件");
            }
            else
            {

                string filename;
                string fileFolder;
                string mp4FileNameWithTimestamp;
                string mp4fileFolder;
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss"); // 格式化时间戳

                // 为图片和视频文件生成带时间戳的文件名
                filename = Path.GetFileNameWithoutExtension(path_source) + "_" + dateTime;
                mp4FileNameWithTimestamp = filename + Path.GetExtension(SelectedVideoPath); // 视频文件新名称，包含时间戳

                string baseFolder = Directory.GetCurrentDirectory();
                fileFolder = Path.Combine(baseFolder, "image", filename + Path.GetExtension(path_source));
                mp4fileFolder = Path.Combine(baseFolder, "MP4", mp4FileNameWithTimestamp); // 视频文件新路径

                // 确保文件夹存在
                Directory.CreateDirectory(Path.GetDirectoryName(fileFolder));
                Directory.CreateDirectory(Path.GetDirectoryName(mp4fileFolder));

                // 复制并重命名图片文件
                File.Copy(path_source, fileFolder, true);

                // 复制并重命名视频文件
                File.Copy(SelectedVideoPath, mp4fileFolder, true); // 复制视频文件


                // 更新数据库
                DataRow dr = ds.Tables["media_info"].NewRow(); // 使用统一的表名

                dr["text"] = textBox1.Text;
                dr["imagepath"] = Path.Combine("image", filename + Path.GetExtension(path_source));
                dr["mp4path"] = Path.Combine("MP4", mp4FileNameWithTimestamp); // 数据库中的视频路径与新文件名一致
                dr["date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }


       

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

   

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            houtairuzhu form2 = new houtairuzhu();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel1.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体


            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            ceshi2 form2 = new ceshi2();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel1.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体


            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            zhuangtaifrom form2 = new zhuangtaifrom();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel1.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体


            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            juanzhengguanli form2 = new juanzhengguanli();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel1.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体


            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel2.AutoScrollPosition = new Point(0, 0);
            panel3.BringToFront();
            panel3.Visible = true;

            ruyuanguanli form2 = new ruyuanguanli();
            form2.TopLevel = false; // 设置子窗体为非顶层窗体
            form2.Dock = DockStyle.Fill; // 让子窗体填充整个面板
            form2.FormBorderStyle = FormBorderStyle.None; // 可选：去除子窗体的边框
            flowLayoutPanel1.Controls.Add(form2); // 将子窗体添加到面板中

            form2.Show(); // 显示子窗体


            // 关闭panel3中的其它子窗体
            foreach (Control control in panel3.Controls)
            {
                if (control is Form form && control != panel3.Controls[panel3.Controls.Count - 1])
                {
                    form.Close();
                }
            }
        }

        private void houtai_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
        }
    }
    
}
