using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 养老系统.类文件;

namespace 养老系统
{
    public partial class AA2 : Form
    {
        private string videoNumber;

        public AA2(string videoNumber)
        {
            InitializeComponent();
            this.videoNumber = videoNumber;
        }

        private void AA2_Load(object sender, EventArgs e)
        {
            DB.GetCn();

            // 使用视频编号查询数据库，找到对应的视频路径
            string sql = $"SELECT mp4path FROM mp4_Table WHERE number = '{videoNumber}'";
            DataTable dataTable = DB.GetDataSet(sql);
            if (dataTable.Rows.Count > 0)
            {
                // 假设mp4path列包含视频文件的路径
                string videoPath = dataTable.Rows[0]["mp4path"].ToString();

                // 设置视频播放器控件的URL属性
                axWindowsMediaPlayer1.URL = videoPath;
                axWindowsMediaPlayer1.Ctlcontrols.play(); // 播放视频
            }
        }

        private void AA2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
        }
    }
}
