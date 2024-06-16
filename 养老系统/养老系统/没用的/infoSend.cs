using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 养老系统
{
    public partial class infoSend : Form
    {
        public infoSend()
        {
            InitializeComponent();
            this.OnMsg = this.ReceiveMsg;
        }

        private void ceshi_Load(object sender, EventArgs e)
        {
            
        }


        #region 发送给另外一个进程的方法
        private IntPtr SendPtr;
        private Process process;


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(this.txtProcessName.Text.Trim().Length > 0)
            {
                process = Process.GetProcesses().Where(s => s.ProcessName.Equals(this.txtProcessName.Text)).LastOrDefault();
                SendPtr = process.MainWindowHandle;
            }
        }//连接进程


        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtSend.Text.Trim().Length > 0)
            {
                SendMsg(SendPtr, this.txtSend.Text);
            }
        }//发送消息


        // 第一步调用系统方法
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public const int WM_COPYDATA = 0x004A;

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }


        private void SendMsg(IntPtr hwnd, string str)
        {
            // 如果没有句柄直接return
            if (hwnd == IntPtr.Zero)
            {
                return;
            }

            // 将字符串转换为字节数组
            byte[] arr = Encoding.Default.GetBytes(str);

            COPYDATASTRUCT cdata;

            cdata.dwData = (IntPtr)100; // 通常这里传递一些自定义数据
            cdata.cbData = arr.Length + 1;       // 设置数据长度
            cdata.lpData = str;              // 设置数据字符串

            // 发送WM_COPYDATA消息
            SendMessage(hwnd, WM_COPYDATA, this.Handle, ref cdata);
        }

        #endregion




        #region 接受另外一个进程的方法


        public delegate void EventMsg(object sender, IntPtr wnd, string str);
        public event EventMsg OnMsg;
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_COPYDATA:
                    COPYDATASTRUCT cdata = new COPYDATASTRUCT();
                    Type mytype = cdata.GetType();
                    cdata = (COPYDATASTRUCT)m.GetLParam(mytype);
                    OnMsg(this, m.WParam, cdata.lpData);
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }


        private void ReceiveMsg(object sender, IntPtr wnd, string str)
        {
            SendPtr = wnd;
            this.txtReceive.Text = str.ToString();
        }




        #endregion

        private void txtReceive_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSend_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProcessName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
