using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class MyFileUtil
    {
        private List<string> fileNames = new List<string>();
        public static bool IsExistInRootDir(string memberFile)
        {
            string filePath = Application.StartupPath+"\\"+memberFile;
            return File.Exists(filePath);
        }
        /// <summary>
        /// 递归获取文件夹下的所有文件名（包含了后缀名），包含子目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public List<String> GetAllFiles(DirectoryInfo dir)
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                fileNames.Add(fi.Name);
            }
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                GetAllFiles(d);
            }
            return fileNames;
        }
        /// <summary>
        /// 指定目录不存在就创建
        /// </summary>
        /// <param name="fileDir"></param>
        public static void CreateDir(string fileDir)
        {
            // 判断目录是否存在
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);//创建新路径
            }
        }
        /// <summary>
        /// 按行读取文件，每行一条
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> readFile(string filePath)
        {
            
            List<string> list = new List<string>();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    //使用StreamReader类来读取文件
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    // 从数据流中读取每一行，直到文件的最后一行，并在richTextBox1中显示出内容
                    string strLine = sr.ReadLine();
                    while (strLine != null)
                    {
                        list.Add(strLine);
                        strLine = sr.ReadLine();
                    }
                    //关闭此StreamReader对象
                    sr.Close();
                }
            }
            return list;
        }

        public static void writeToFile(string filePath,string content)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8))
            {
                sw.Write(content);
                sw.Flush();
            }
        }

        public static string readFileAll(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                String content = sr.ReadToEnd();

                return content;
            }
        }
    }
}
