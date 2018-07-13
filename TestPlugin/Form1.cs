using AI.Bll;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using DeepWorkshop.QQRot.FirstCity.MyTool;
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
    }
}
