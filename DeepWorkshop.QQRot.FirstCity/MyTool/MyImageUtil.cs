using AI.Bll;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class MyImageUtil
    {
        /// <summary>
        /// 将图片保存到酷q中
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <returns>返回在酷q中的位置 例如：data\image\</returns>
        public static string Save(Image image)
        {
            try
            {
                
                //判断图片格式
                Bitmap bit = new Bitmap(image);
                String type = function.getImageType(bit).ToLower();
                type = string.IsNullOrWhiteSpace(type) ? "jpg" : type;
                String suffixOfImg = "." + type;
                String path = MySystemUtil.GetExeRootPath()+ "data" + Path.DirectorySeparatorChar + "image" + Path.DirectorySeparatorChar;
                //String cqPath = "data" + Path.DirectorySeparatorChar + "image" + Path.DirectorySeparatorChar+MyDateUtil.GetCurentDate() + Path.DirectorySeparatorChar ;
                String cqPath =  MyDateUtil.GetCurentDate() + Path.DirectorySeparatorChar;
                //创建目录
                MyFileUtil.CreateDir(path + cqPath);
                cqPath += MyDateUtil.GetTimeStamp(DateTime.Now) + suffixOfImg;

                //判断目录是否存在
                //MyLogUtil.ToLogFotTest("看看生成图片的路径："+ path + cqPath);
                image.Save(path + cqPath);
                Console.WriteLine("查看lujing:"+ cqPath);
                return cqPath;
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("保存图片失败，原因：" + ex);
                MessageBox.Show("保存图片失败，请查看错误日志");
                return "";
            }
            
            
        }
    }
}
