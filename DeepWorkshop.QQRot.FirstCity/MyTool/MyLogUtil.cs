using DeepWorkshop.QQRot.FirstCity.MyTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class MyLogUtil
    {
        public static void WriteQQDialogueLog(long qq, string str)
        {

            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetQQDialogueDir() + qq + ".txt", true);
            streamWriter.WriteLine(qq+" "+DateTime.Now.ToString("HH:mm:ss") + "=>" + str);
            //streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }
        /// <summary>
        /// 我给与的回复
        /// </summary>
        /// <param name="str"></param>
        public static void WriteQQDialogueLogOfMe(long qq, string str)
        {

            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetQQDialogueDir() + qq + ".txt", true);
            streamWriter.WriteLine("我 "+DateTime.Now.ToString("HH:mm:ss") + "=>" + str);
            //streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }

        public static void WriteZhuanZhangLog(long qq, string str)
        {
            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetDllRoot() + "zhuanzhang.txt", true);
            streamWriter.WriteLine(qq+"在"+DateTime.Now.ToString() + "发起转账：" + str);
            //streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }

        public static void ErrToLog(long qq,string str)
        {
            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetDllRoot() + "errLog.txt", true);
            streamWriter.WriteLine(DateTime.Now.ToString() + "=>" + str);
            streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }

        public static void ToLog(string str)
        {
            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetDllRoot() + "log.txt", true);
            streamWriter.WriteLine(DateTime.Now.ToString() + "=>" + str);
            streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }

        public static void ErrToLog(string str)
        {
            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetDllRoot() + "errLog.txt", true);
            streamWriter.WriteLine(DateTime.Now.ToString() + "=>" + str);
            streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }

        public static void ToLogFotTest(string str)
        {
            Console.WriteLine(str);
            StreamWriter streamWriter = new StreamWriter(MySystemUtil.GetDllRoot() + "testLog.txt", true);
            streamWriter.WriteLine(DateTime.Now.ToString() + "=>" + str);
            streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();
        }
    }
}
