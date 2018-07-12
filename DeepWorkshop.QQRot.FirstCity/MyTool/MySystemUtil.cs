using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    class MySystemUtil
    {
        /// <summary>
        /// 获取exe所在的根目录
        /// </summary>
        /// <returns></returns>
        public static string GetExeRootPath()
        {
            return Application.StartupPath.ToString()+ Path.DirectorySeparatorChar;
        }

        /// <summary>  
        /// 获取当前执行的dll的目录 例如： D:\dir\dir\
        /// </summary>  
        /// <returns></returns>  
        public static string GetPath()
        {
            string str = Assembly.GetExecutingAssembly().CodeBase;
            int start = 8;// 去除file:///  
            int end = str.LastIndexOf('/');// 去除文件名xxx.dll及文件名前的/  
            str = str.Substring(start, end - start);
            str = str + "/";
            str = Path.GetDirectoryName(str) + "\\";
            return str;
           
        }
        /// <summary>
        /// 获取插件dll根目录，用于存放此dll应用信息
        /// </summary>
        /// <returns></returns>
        public static string GetDllRoot()
        {
            string dirPath = GetPath()+@"local\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
        /// <summary>
        /// 获取本地qq对话存储路径,每天一个
        /// </summary>
        /// <returns></returns>
        public static string GetQQDialogueDir()
        {
            string dirPath = GetDllRoot()+@"QQDialogueLog\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            dirPath = dirPath + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;

        }
    }
}
