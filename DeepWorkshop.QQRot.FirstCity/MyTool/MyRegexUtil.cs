using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class MyRegexUtil
    {
        /// <summary>
        /// 剔除所有字符，只保留数字字母汉字下划线
        /// </summary>
        /// <param name="oriStr"></param>
        /// <returns></returns>
        public static String RemoveSpecialCharacters(String oriStr)
        {
            string s = Regex.Replace(oriStr, @"[^a-zA-Z0-9_\u4e00-\u9fa5]", "*");

            return s;
        }
    }
}
