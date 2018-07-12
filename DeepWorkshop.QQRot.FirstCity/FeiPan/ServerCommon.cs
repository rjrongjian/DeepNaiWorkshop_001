using AI.Bll;
using AI.Dal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using 新一城娱乐系统.Bll;

namespace 新一城娱乐系统.FeiPan
{
    public class ServerCommon
    {
        public static bool isDebug = false;
        public static bool isSendMessage = true;
        public static DateTime serverExpire = DateTime.Now;

        public static bool isLogWechat = true;

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetValidateCode(string url, CookieContainer webCookie, out Bitmap bmpImage)
        {
            try
            {
                //获取验证码
                Image HeadImage = HttpHelps.GetPicture(url, webCookie);

                //处理图片
                Bitmap bitmap = new Bitmap(HeadImage);

                UnCodebase ud = new UnCodebase(bitmap);
                ud.GrayByPixels();
                ud.ClearNoise(128, 2);

                //识别验证码
                tessnet2.Tesseract ocr = new tessnet2.Tesseract();//声明一个OCR类
                ocr.SetVariable("tessedit_char_whitelist", "0123456789"); //设置识别变量，当前只能识别数字。
                ocr.Init(Application.StartupPath + @"\\tmpe", "eng", true); //应用当前语言包。
                List<tessnet2.Word> result = ocr.DoOCR(ud.bmpobj, Rectangle.Empty);//执行识别操作
                string validateCode = result[0].Text;

                //pictureBox1.Image = ud.bmpobj;
                bmpImage = ud.bmpobj;
                return validateCode;
            }
            catch (Exception ex)
            {
                function.log("验证码获取错误" + ex.Message);
                bmpImage = null;
                return "";
            }
        }

        /// <summary>
        /// 下注失败
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static feiPanJieGuo SetFeiPanJieGuo(feiPanJieGuo data, bool success)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    data.QD[x, i] = success;
                }
            }
            for (int i = 0; i < 5; i++)//大小单双
            {
                for (int x = 0; x < 4; x++)
                {
                    data.DXDS[i, x] = success;
                }
            }
            for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
            {
                data.ZHDXDS[i] = success;
            }
            for (int i = 0; i < 3; i++)//龙虎和
            {
                data.LHH[i] = success;
            }
            return data;
        }
    }
}