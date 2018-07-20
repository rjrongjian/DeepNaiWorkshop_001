using System;
using System.Text;

namespace WinFrom_WebApi_Demo
{

    public static class RsaHelper
    {
        /// <summary>
        ///     字符串解密
        /// </summary>
        /// <param name="encryptString">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="modulus">公共模数</param>
        /// <returns>遇到解密失败将会返回原字符串</returns>
        public static string DecryptString(string encryptString, string key, string modulus)
        {
            var result = new StringBuilder();
            try
            {
                var biE = new BigInteger(key, 10);
                var biN = new BigInteger(modulus, 10);
                var strarr1 = encryptString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var block in strarr1)
                {
                    var temp = DecryptString(block, biE, biN);
                    result.Append(temp);
                }
            }
            catch
            {
                return encryptString;
            }
            return result.ToString();
        }

        /// <summary>
        ///  用指定的密匙解密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="e">可以是RSACryptoServiceProvider生成的Exponent</param>
        /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>
        /// <returns>返回明文</returns>
        private static string DecryptString(string encryptString, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            string[] strarr1 = encryptString.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strarr1.Length; i++)
            {
                string block = strarr1[i];
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                string temp = Encoding.UTF8.GetString(biEnText.getBytes());
                result.Append(temp);
            }
            return result.ToString();
        }

    }
}