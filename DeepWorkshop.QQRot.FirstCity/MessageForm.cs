using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepWorkshop.QQRot.FirstCity
{
    public partial class MessageForm : Form
    {
        private Object obc = new Object();
        public Timer timer;
        public MessageForm()
        {
            InitializeComponent();
            timer = this.timer1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                this.timer.Stop();
            }catch(Exception ex)
            {

            }
            
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                this.timer.Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void MessageForm_Shown(object sender, EventArgs e)
        {
            this.timer.Start();
        }
    }
}
