﻿using DeepWorkshop.QQRot.FirstCity.MyTool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace 新一城娱乐系统.Bll
{
    class UnCode : UnCodebase
    {
        //字符表 顺序为0..9,A..Z,a..z
        string[] CodeArray = new string[] {
"0000000111000000000000001111111100000000001111111111000000001111100111110000001111100001111100000111100000001111000011110000000111100001111000000111110000111100000111111000011110001111111110001111001111111111000111111111100111100001111111100011110000111111000001111100011111000010111100001111110000011110011111110000001111110001111110001111100000011111111111100000001111111111110000000001111111100000000000000010000000",
"00000000000011100000000000000111110000000000001111111100000000001111111110000000000011101111000000000001100111110000000000000011110000000000000000111000000000000000011100000110000000001111001110000000000111100111100000000011110000000000000011111000000000000001111111000000000000001111000000000000000111100000000000111111110000000000011111111100000000001111111110000000000000001111",
"00000111111110000000000111111111111000000111111111111000000001100000111100000000000000011110000000000000001111000000000000000111100000000000000111110000000000000011110000000000000011110000000000000011110000000000000011110000000000000111110000000000000111110000000000000111110000000000001111100000000001111111111111111111001111111111111110000111111111111111000000000111110111000000",
"11101111111110000000000111111111110000000011111111111000000001000000111111000000000000001111011000000000000111100000000000000011100000000000000011110000000000111111110000000000011111111000001111111111111111000000000011111111110000000000000001111110000000000000111110000000001110011110011100000000001111000000100000011111000000011111111111100000001111111111000000000011111111110000",
"00000000011110000000000000111111000000000000011111100000000000001111111000000000001111111100000000000111011110000000000111101111000000000011110111100011000001110011111100000001111000111100000000111111011110000000111110001111000000011110000111111101011111001111111110001111111111111111100111111111111111000011111111111111000001111110000111100000010000000011110010000000000001111100000000111000110000000000000000000001010001111110000000000",
"000001111100000000000001111111111000000001111111111110000011111111111111000011111111111100111111110000000000111111111000000000000111111110000000000111111100000000000111111111100000000000111111110000000000000111110000000000000011111000000000000001111000000000000001111000000000000001111000010000000011111000011111111111111100011111111111110000001111111111000000000000111100000000",
"000000001111111110010000011111111111001000001111111111100000001111100000000001000111100000000000000111100000000000000011110000000000000001111111111000000000111111111111000000011111111111110000001111000001111000000111100000011100000011110000001111000001111000001111100000011110000011110100001111000001110000000011111111111000000001111111111100100000001111111100000000000001111000000000000000000111111",
"00000000000001001110000011111111111111000001111111111111100000111111111111110000001111100001111000010000000000111100000000000000011110000000000000001111000000000000001111111000000000000111110001111111111111110000000000000001111000000000000000111100000000000000111100001100000000011110011111111000001111000000000010001111100000000000010111110000000000000011110000000000000001111000000000000000111100000000000000000000100000000000000000000110000000000000000000000011000000000000000000000000011000",
"0000000111100000000000011111111100000000011111111111000000011111000111100000001110000001111000001111000000111000000111100000011100000011110000011110000000111110011110000000011111111110000000000111111111100000000001111111111000000001111111111110000001111100111111111000111111000111110000111100000011110000011110000001111000001111000000111100010111110001111110001111111111111111110100111111111100000000000111111000000000",
"00000001111111100000000001111111111000000001111111111111000011111100001111000000111100000111110000011110000001111010011110000000111100001111111110111110000111110000111111100001111111111111101111111111111111100100001111111111110011000000000001111111100000000001111000110000000000111100011000000001111100001100111111111110000100011111111110000000011111111100000000000111100000000000",

"001100010010100001100001100001111111100001100001100001100001",
"111110100001100001100001111110100001100001100001100001111110",
"011110100001100000100000100000100000100000100000100001011110",
"111100100010100001100001100001100001100001100001100010111100",
"111111100000100000100000111110100000100000100000100000111111",
"111111100000100000100000111110100000100000100000100000100000",
"011110100001100000100000100000100111100001100001100011011101",
"100001100001100001100001111111100001100001100001100001100001",
"11111001000010000100001000010000100001000010011111",
"000111000010000010000010000010000010000010100010100010011100",
"100001100010100100101000110000110000101000100100100010100001",
"100000100000100000100000100000100000100000100000100000111111",
"1000001110001111000111010101101010110010011001001100000110000011000001",
"100001110001110001101001101001100101100101100011100011100001",
"011110100001100001100001100001100001100001100001100001011110",
"111110100001100001100001111110100000100000100000100000100000",
"01111001000010100001010000101000010100001010000101011010110011001111000000011",
"111110100001100001100001111110100100100010100010100001100001",
"011110100001100001100000011000000110000001100001100001011110",
"1111111000100000010000001000000100000010000001000000100000010000001000",
"100001100001100001100001100001100001100001100001100001011110",
"1000001100000110000010100010010001001000100010100001010000010000001000",
"1000001100000110000011001001100100110010011001001101010110101010100010",
"100001100001010010010010001100001100010010010010100001100001",
"1000001100000101000100100010001010000010000001000000100000010000001000",
"111111000001000001000010000100001000010000100000100000111111",
"011110100001000111011001100001100011011101",
"100000100000100000101110110001100001100001100001110001101110",
"011110100001100000100000100000100001011110",
"000001000001000001011101100011100001100001100001100011011101",
"011110100001100001111111100000100000011110",
"001110010001010000010000111110010000010000010000010000010000",
"000001011101100010100010100010011100010000011110100001100001011110",
"100000100000100000101110110001100001100001100001100001100001",
"00100001000000001100001000010000100001000010011111",
"00001000010000000011000010000100001000010000100001000011001001100",
"100000100000100000100010100100101000111000100100100010100001",
"01100001000010000100001000010000100001000010011111",
"1110110100100110010011001001100100110010011001001",
"101110110001100001100001100001100001100001",
"011110100001100001100001100001100001011110",
"101110110001100001100001100001110001101110100000100000100000",
"011101100011100001100001100001100011011101000001000001000001",
"101110110001100000100000100000100000100000",
"011110100001100000011110000001100001011110",
"001000001000111110001000001000001000001000001000001001000110",
"100001100001100001100001100001100011011101",
"100001100001100001010010010010001100001100",
"1000001100100110010011001001100100110101010100010",
"100001100001010010001100010010100001100001",
"100001100001100001100001100001010011001101000001000010011100",
"111111000010000100001000010000100000111111"
        };





        public UnCode(Bitmap pic)
            : base(pic)
        {
        }

        public string getPicnum()
        {
            GrayByPixels(); //灰度处理
            GetPicValidByValue(128, 4); //得到有效空间
            Bitmap[] pics = GetSplitPics(4, 1);     //分割

            if (pics.Length != 4)
            {
                return ""; //分割错误
            }
            else  // 重新调整大小
            {
                pics[0] = GetPicValidByValue(pics[0], 128);
                pics[1] = GetPicValidByValue(pics[1], 128);
                pics[2] = GetPicValidByValue(pics[2], 128);
                pics[3] = GetPicValidByValue(pics[3], 128);
            }

            //      if (!textBoxInput.Text.Equals(""))
            string result = "";
            char singleChar = ' ';
            {
                for (int i = 0; i < 4; i++)
                {
                    string code = GetSingleBmpCode(pics[i], 128);   //得到代码串
                    log(code + "\r\n");

                    float maxrate = 0;
                    int selIndex = 0;
                    for (int arrayIndex = 0; arrayIndex < CodeArray.Length; arrayIndex++)
                    {
                        //StringCompute stringcompute1 = new StringCompute();
                        //stringcompute1.SpeedyCompute(CodeArray[arrayIndex], code);    // 计算相似度， 不记录比较时间
                        float rate = levenshtein(CodeArray[arrayIndex], code);       // 相似度百分之几，完全匹配相似度为1

                        if (rate > maxrate)  //相等
                        {
                            maxrate = rate;
                            selIndex = arrayIndex;
                        }
                    }
                    if (selIndex < 10)   // 0..9
                        singleChar = (char)(48 + selIndex);
                    else if (selIndex < 36) //A..Z
                        singleChar = (char)(65 + selIndex - 10);
                    else
                        singleChar = (char)(97 + selIndex - 36);
                    result = result + singleChar;

                }
            }
            return result;
        }


        private void log(string str)
        {
            FileStream fs = new FileStream(MySystemUtil.GetDllRoot()+DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("HHmmss") + ":" + str);
            sw.Close();
            fs.Close();
        }

        public  float levenshtein(string str1, string str2)
        {
            //计算两个字符串的长度。
            int len1 = str1.Length;
            int len2 = str2.Length;
            //建立上面说的数组，比字符长度大一个空间
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的
                    dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
                }
            }
         
            //计算相似度
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
        
            return similarity;
        }
    }
}
