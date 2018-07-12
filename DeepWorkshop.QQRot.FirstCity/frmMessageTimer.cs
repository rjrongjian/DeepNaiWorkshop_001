using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 新一城娱乐系统
{
    public partial class frmMessageTimer : Form
    {
        public frmMessageTimer()
        {
            InitializeComponent();
        }

        public frmMessageTimer(string value)
        {
            InitializeComponent();

            label1.Text = value;
            timer1.Interval =5000;
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
