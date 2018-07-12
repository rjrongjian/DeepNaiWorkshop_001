using AI.Bll;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
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
                String suffixOfImg = "." + function.getImageType(bit).ToLower();


                String path = MySystemUtil.GetExeRootPath()+"data" + Path.DirectorySeparatorChar + "image" + Path.DirectorySeparatorChar;
                String cqPath = MyDateUtil.GetCurentDate() + Path.DirectorySeparatorChar + MyDateUtil.GetTimeStamp(DateTime.Now) + suffixOfImg;
                //判断目录是否存在
                image.Save(path+cqPath);
                return cqPath;
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("保存图片失败，原因：" + ex);
                return "";
            }
            
            
        }
    }
}
