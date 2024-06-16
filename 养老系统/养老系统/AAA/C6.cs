using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 养老系统
{
    public partial class C6 : Form
    {
        public C6()
        {
            InitializeComponent();
        }


        public delegate void HidePanelDelegate();

        public HidePanelDelegate HidePanelHandler; // 声明一个委托变量


        private void button1_Click(object sender, EventArgs e)
        {
            HidePanelHandler?.Invoke();
            this.Close();
        }

        private void C6_Load(object sender, EventArgs e)
        {

        }
    }
}
