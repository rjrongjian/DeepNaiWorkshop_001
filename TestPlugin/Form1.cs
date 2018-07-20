using AI.Bll;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using DeepWorkshop.QQRot.FirstCity.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image image = function.TextToBitmap("开始！", Color.Black, Color.White);
            Console.WriteLine("看看文件格式："+image.RawFormat.Guid);
            Console.WriteLine("gif:"+ System.Drawing.Imaging.ImageFormat.Gif.Guid);
            Console.WriteLine("jpeg:" + System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
            Console.WriteLine("png:" + System.Drawing.Imaging.ImageFormat.Png.Guid);
            
            MyImageUtil.Save(image);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageInfo message = MyMessageUtil.ConvertMessage(textBox1.Text);
            Console.WriteLine("哈哈哈哈："+message.MessageType);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String conter = "13/13579/5";
            //测试下注
            conter = conter.Replace("/ ", "/")
                            .Replace("/  ", "/")
                            .Replace("/  ", "/")
                            .Replace("/   ", "/")
                            .Replace(" /", "/")
                            .Replace("  /", "/")
                            .Replace("  /", "/")
                            .Replace("  /", "/");
            Console.WriteLine("替换后的数据："+conter);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyValidate.T();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!MyDotNetFrameworkUtil.IsSupportedRuntimeVersion())
            {
                MessageBox.Show("当前电脑运行时版本低于4.5.2");


            }
            else
            {
                MessageBox.Show("当前电脑运行时版本高于4.5.2");
            }
        }
    }
}
