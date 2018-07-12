using AI.Bll;
using AI.Dal;
using System;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;

namespace 新一城娱乐系统.FeiPan
{
    public class ServerShunFeng
    {
        /// <summary>
        /// 提交参数
        /// </summary>
        private static feiPanCanShu fpCanShu = new feiPanCanShu()
        {
            QD = new string[5, 10] {{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" },
                                       { "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" },
                                       { "29", "30", "31", "32", "33", "34", "35", "36", "37", "38" },
                                       { "43", "44", "45", "46", "47", "48", "49", "50", "51", "52" },
                                       { "57", "58", "59", "60", "61", "62", "63", "64", "65", "66" }},

            DXDS = new string[5, 4] { { "11", "12", "13", "14" }, { "25", "26", "17", "18" },
                                          { "39", "40", "41", "42" }, { "53", "54", "55", "56" }, { "67", "68", "69", "70" } },

            ZHDXDS = new string[] { "71", "72", "73", "74" },

            ZHZHDXDS = new string[] { "", "", "", "" },

            LHH = new string[] { "75", "76", "77" }
        };

        /// <summary>
        /// 获取最新赔率
        /// </summary>
        public static peilv getPeilv(string url, CookieContainer webCookie)
        {
            string resultPeilv = "";
            peilv serPeilv = new peilv();

            try
            {
                function.log("获取赔率");

                //赔率
                //http://mem1.paeghe214.dqbpkj.com:88/user/xml_cqsc/Read_Multiple.aspx?LT=1&T=6&GT=2,3,5,6,8,9,11,12,14,15,16,17,18&_=1511706182975
                //http://mem1.paeghe214.dqbpkj.com:88/user/xml_cqsc/Read_Multiple.aspx?LT=1&T=24&GT=1,4,7,10,13&_=1511706183857
                resultPeilv = HttpHelps.Post("?LT=1&T=6&GT=2,3,5,6,8,9,11,12,14,15,16,17,18&_=" + function.MilliTime(),
                   url + "/user/xml_cqsc/Read_Multiple.aspx", webCookie, Encoding.Default);
                string resultPeilv2 = HttpHelps.Post("?LT=1&T=24&GT=1,4,7,10,13&_=" + function.MilliTime(),
               url + "/user/xml_cqsc/Read_Multiple.aspx", webCookie, Encoding.Default);
                // resultPeilv = "<?xml version=\"1.0\" encoding=\"gb2312\"?><update><k_qs>20171115109</k_qs><k_stat>y</k_stat><k_open_date>23:05:00</k_open_date><Stoptime>00:00:57</Stoptime><k_id>265126</k_id><Multiple_Info><m_2_11>1.943</m_2_11><m_2_12>1.943</m_2_12><m_3_13>1.943</m_3_13><m_3_14>1.943</m_3_14><m_5_25>1.943</m_5_25><m_5_26>1.943</m_5_26><m_6_27>1.943</m_6_27><m_6_28>1.943</m_6_28><m_8_39>1.943</m_8_39><m_8_40>1.943</m_8_40><m_9_41>1.943</m_9_41><m_9_42>1.943</m_9_42><m_11_53>1.943</m_11_53><m_11_54>1.943</m_11_54><m_12_55>1.943</m_12_55><m_12_56>1.943</m_12_56><m_14_67>1.943</m_14_67><m_14_68>1.943</m_14_68><m_15_69>1.943</m_15_69><m_15_70>1.943</m_15_70><m_16_71>1.943</m_16_71><m_16_72>1.943</m_16_72><m_17_73>1.943</m_17_73><m_17_74>1.943</m_17_74><m_18_75>1.943</m_18_75><m_18_76>1.943</m_18_76><m_18_77>9.26</m_18_77></Multiple_Info><Limitation_Info><i_2>第一球大小,5,50000</i_2><i_3>第一球單雙,5,50000</i_3><i_5>第二球大小,5,50000</i_5><i_6>第二球單雙,5,50000</i_6><i_8>第三球大小,5,50000</i_8><i_9>第三球單雙,5,50000</i_9><i_11>第四球大小,5,50000</i_11><i_12>第四球單雙,5,50000</i_12><i_14>第五球大小,5,50000</i_14><i_15>第五球單雙,5,50000</i_15><i_16>總和大小,5,50000</i_16><i_17>總和單雙,5,50000</i_17><i_18>龍虎,5,50000</i_18></Limitation_Info><Money_Credits>10</Money_Credits><Money_KY>10</Money_KY></update>";
                resultPeilv += resultPeilv2;

                if (resultPeilv.Length < 300)
                {
                    return null;
                }

                string xunhao = "";

                #region 获取下注赔率

                for (int i = 0; i < 77; i++)
                {
                    //2_11 2_12 3_13 3_14
                    //5_25 5_26 6_27 6_28
                    //...
                    int qiudao = i / 14 + 1;
                    int leix = i % 14 + 1;//1-10 球 11 12 大小 13 14 单双
                    int hao = i + 1;
                    decimal val = 0;
                    if (qiudao < 6)
                    {
                        if (leix >= 1 && leix <= 10)
                        {
                            //球
                            xunhao = ((qiudao - 1) * 3 + 1).ToString() + "_" + hao.ToString();
                            string peilvNote = function.middlestring(resultPeilv, "<m_" + xunhao + ">", "</m_" + xunhao + ">");
                            decimal.TryParse(peilvNote, out val);
                            serPeilv.QD[qiudao - 1, leix - 1] = val;
                        }
                        else if (leix >= 11 && leix <= 12)
                        {
                            //大小
                            xunhao = ((qiudao - 1) * 3 + 2).ToString() + "_" + hao.ToString();
                            //
                            string peilvNote = function.middlestring(resultPeilv, "<m_" + xunhao + ">", "</m_" + xunhao + ">");
                            decimal.TryParse(peilvNote, out val);
                            if (leix == 11)
                            {
                                serPeilv.DXDS[qiudao - 1, 0] = val;
                            }
                            else
                            {
                                serPeilv.DXDS[qiudao - 1, 1] = val;
                            }
                        }
                        else if (leix >= 13 && leix <= 14)
                        {
                            //单双
                            xunhao = ((qiudao - 1) * 3 + 3).ToString() + "_" + hao.ToString();
                            string peilvNote = function.middlestring(resultPeilv, "<m_" + xunhao + ">", "</m_" + xunhao + ">");
                            decimal.TryParse(peilvNote, out val);
                            if (leix == 13)
                            {
                                serPeilv.DXDS[qiudao - 1, 2] = val;
                            }
                            else
                            {
                                serPeilv.DXDS[qiudao - 1, 3] = val;
                            }
                        }
                        else if (leix >= 13 && leix <= 14)
                        {

                        }
                    }

                }
                decimal valA = 0;
                string peilvNoteA = function.middlestring(resultPeilv, "<m_16_71>", "</m_16_71>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.ZHDXDS[0] = valA;


                peilvNoteA = function.middlestring(resultPeilv, "<m_16_72>", "</m_16_72>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.ZHDXDS[1] = valA;

                peilvNoteA = function.middlestring(resultPeilv, "<m_17_73>", "</m_17_73>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.ZHDXDS[2] = valA;

                peilvNoteA = function.middlestring(resultPeilv, "<m_17_74>", "</m_17_74>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.ZHDXDS[3] = valA;

                peilvNoteA = function.middlestring(resultPeilv, "<m_18_75>", "</m_18_75>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.LHH[0] = valA;

                peilvNoteA = function.middlestring(resultPeilv, "<m_18_76>", "</m_18_76>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.LHH[1] = valA;

                peilvNoteA = function.middlestring(resultPeilv, "<m_18_77>", "</m_18_77>");
                decimal.TryParse(peilvNoteA, out valA);
                serPeilv.LHH[2] = valA;
                #endregion 获取下注赔率

                function.log("获取赔率成功");
            }
            catch (Exception ex)
            {
                function.log("获取赔率失败" + ex.Message + "\n" + resultPeilv);
                return null;
            }

            return serPeilv;
        }

        /// <summary>
        /// 登录状态
        /// </summary>
        public static bool login(string url, string username, string userpass, CookieContainer webCookie, int tryMax)
        {
            bool isSuccess = false;
            int tryCount = 0;

            try
            {
                while (true)
                {
                    #region 获取验证码

                    //图像验证码
                    string validateCode = "";
                    string urlVali = url + "/user/ValidateImage.aspx?time=0." + DateTime.Now.Ticks;
                    Bitmap bitmap;

                    validateCode = ServerCommon.GetValidateCode(urlVali, webCookie, out bitmap);

                    #endregion 获取验证码

                    #region 登录

                    if (validateCode.Length == 4)
                    {
                        //登录获取Cookie
                        string resultLogin = HttpHelps.Post("loginName=" + username + "&loginPwd=" + userpass + "&ValidateCode=" + validateCode,
                        url + "/user/login_validate.aspx", webCookie, Encoding.Default);

                        if (resultLogin.IndexOf("系統已經自動把你列入黑名單") > -1)
                        {
                            //密碼輸入多次不正確，系統已經自動把你列入黑名單！！！
                            isSuccess = false;
                            break;
                        }
                        else if (resultLogin.IndexOf("驗證碼輸入不正確") == -1 && resultLogin.IndexOf("帳號或密碼不正確") == -1)
                        {
                            //
                            if (checkLogin(url, webCookie))
                            {
                                //登录成功
                                isSuccess = true;
                                function.log("顺丰登录成功");
                                break;
                            }
                        }
                        else
                        {
                            tryCount++;
                        }

                        function.log("顺丰登录失败，重新尝试");
                        Thread.Sleep(500);
                    }

                    #endregion 登录


                    if (tryCount > tryMax)
                    {
                        function.log("尝试登录次数过多");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                function.log("尝试登录错误" + ex.Message);
            }

            return isSuccess;
        }

        /// <summary>
        /// 获取余额
        /// </summary>
        /// <returns></returns>
        public static decimal getYuE(string url, CookieContainer webCookie)
        {
            decimal keYongJinE = 0;

            try
            {
                string resultCredits = HttpHelps.Post("LT=1", url + "/user/Refresh_Credits.aspx?_=" + DateTime.Now.Ticks, webCookie,
                    Encoding.Default);
                string credits = function.middlestring(resultCredits, "Current_Credits_kc\":", "}");
                if (credits.Length < 16)
                {
                    keYongJinE = decimal.Parse(credits);
                }
                else
                {
                    keYongJinE = -1;
                }
            }
            catch (Exception ex)
            {
                function.log("顺丰获取余额失败" + ex.Message);
                keYongJinE = -1;
            }

            return keYongJinE;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <returns></returns>
        public static string getUserInfo(string url, CookieContainer webCookie)
        {
            string resultUserInfo = HttpHelps.Post("LT=1", url + "/user/L_UserInfo.aspx", webCookie, Encoding.Default);
            string jeuv = function.middlestring(resultUserInfo, "parent.mainFrame.Update_JV(\"", "\");");
            if (jeuv.Length <= 20)
            {
                return jeuv;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <returns></returns>
        public static string getValidate(string url, CookieContainer webCookie)
        {
            string resultUserInfo = HttpHelps.Post("LT=1", url + "/user/L_UserInfo.aspx", webCookie, Encoding.Default);
            string jeuv = function.middlestring(resultUserInfo, "parent.mainFrame.Update_JV(\"", "\");");
            if (jeuv.Length <= 16 && jeuv.Length > 0)
            {
                return jeuv;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <returns></returns>
        public static bool checkLogin(string url, CookieContainer webCookie)
        {
            string userInfo = getUserInfo(url, webCookie);
            if (userInfo.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试下注
        /// </summary>
        /// <param name="s_uPI_ID"></param>
        /// <param name="s_uPI_ID"></param>
        /// <param name="s_uPI_P"></param>
        /// <param name="s_uPI_M"></param>
        /// <returns></returns>
        private static bool postXiaZhu(string url, CookieContainer webCookie, string validate, string uPI_ID, string uPI_P, string uPI_M)
        {

            //获取验证
            validate = ServerShunFeng.getValidate(url, webCookie);

            string postData = "JeuValidate=" + validate + "&uPI_ID=" + uPI_ID.Substring(1)
                + "&uPI_P=" + uPI_P.Substring(1) + "&uPI_M=" + uPI_M.Substring(1);
            string result = "";

            //JeuValidate=11271100276929&uPI_ID=73&uPI_P=1.943&uPI_M=5
            function.log("下注：" + postData);

            result = HttpHelps.Post(postData, url + "/user/L_Confirm_Jeu_cqsc.aspx", webCookie, Encoding.Default);

            //下註金額有誤！
            //下註规则有误,请重新下註,谢谢合作！


            if (result.IndexOf("下註金額有誤") > -1 || result.IndexOf("下註规则有误") > -1 || result.IndexOf("已經截止下註") > -1)
            {
                function.log("下注错误：" + result);
                return false;
            }
            else
            {
                function.log("下注成功：" + result);
                return true;
            }
        }

        public static feiPanJieGuo xiaZhu(string url, CookieContainer webCookie, peilv peiLv, xztj xiaZhu,
            string validate, string qiHao, feiPanJieGuo fpjgData)
        {

       
            fpjgData.isSuccess = false;
            feiPanJieGuo fpjgDataTemp = new feiPanJieGuo();
            int xiaZhuJiFen = 0;

            //获取余额
            decimal yuE = ServerShunFeng.getYuE(url, webCookie);

            try
            {
                int xiaZhuJiFen_Temp = 0;
                string s_uPI_ID = "";
                string s_uPI_P = "";
                string s_uPI_M = "";
               
                bool tiJiaoResult = false;

                #region 大小单双

                s_uPI_ID = "";
                s_uPI_P = "";
                s_uPI_M = "";
                xiaZhuJiFen_Temp = 0;

                for (int i = 0; i < 5; i++)//大小单双
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (xiaZhu.DXDS[i, x] > 0 && fpjgData.DXDS[i, x] == false)
                        {
                            s_uPI_ID += "," + fpCanShu.DXDS[i, x];
                            s_uPI_P += "," + peiLv.DXDS[i, x];
                            s_uPI_M += "," + xiaZhu.DXDS[i, x];
                            xiaZhuJiFen_Temp += xiaZhu.DXDS[i, x];
                        }
                    }
                }
                //提交数据
                if (xiaZhuJiFen_Temp > yuE)
                {
                    tiJiaoResult = false;
                    fpjgData.isSuccess = false;
                    fpjgData.errorMessage = "金额不足";
                }
                else if (s_uPI_ID.Length > 0)
                {
                    function.log("下注大小单双");
                    tiJiaoResult = postXiaZhu(url, webCookie, validate, s_uPI_ID, s_uPI_P, s_uPI_M);
                    if (tiJiaoResult == true)
                    { 
                        //===================2018-02===================
                        //提交成功
                         fpjgDataTemp = new feiPanJieGuo();
                        for (int i = 0; i < 5; i++)//大小单双
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                fpjgData.DXDS[i, x] = true;
                                fpjgDataTemp.DXDS[i, x] = true;
                            }
                        }
                        xiaZhuJiFen += xiaZhuJiFen_Temp;
                        yuE -= xiaZhuJiFen_Temp;
                       
                        function.FpLog(qiHao, xiaZhu, fpjgDataTemp);
                        //===================2018-02===================
                    }
                    else
                    {
                        fpjgData.isSuccess = false;
                        fpjgData.errorMessage = "下注失败";
                    }
                }
                else
                {
                    tiJiaoResult = true;//x
                }

                #endregion 大小单双

                #region 单球

                for (int x = 0; x < 5; x++)
                {
                    if (tiJiaoResult == true)
                    {
                        //提交成功继续
                        s_uPI_ID = "";
                        s_uPI_P = "";
                        s_uPI_M = "";
                        xiaZhuJiFen_Temp = 0;

                        for (int i = 0; i < 5; i++)
                        {
                            if (xiaZhu.QD[x, i] > 0 && fpjgData.QD[x, i] == false)
                            {
                                s_uPI_ID += "," + fpCanShu.QD[x, i];
                                s_uPI_P += "," + peiLv.QD[x, i];
                                s_uPI_M += "," + xiaZhu.QD[x, i];
                                xiaZhuJiFen_Temp += xiaZhu.QD[x, i];
                            }
                        }


                        //提交数据
                        if (xiaZhuJiFen_Temp > yuE)
                        {
                            tiJiaoResult = false;
                            fpjgData.isSuccess = false;
                            fpjgData.errorMessage = "金额不足";
                        }
                        else if (s_uPI_ID.Length > 0)
                        {
                            tiJiaoResult = postXiaZhu(url, webCookie, validate, s_uPI_ID, s_uPI_P, s_uPI_M);
                            if (tiJiaoResult == true)
                            {
                                //===================2018-02===================
                                //提交成功
                                fpjgDataTemp = new feiPanJieGuo();
                                for (int i = 0; i < 5; i++)
                                {
                                    fpjgData.QD[x, i] = true;
                                    fpjgDataTemp.QD[x, i] = true;
                                }
                                xiaZhuJiFen += xiaZhuJiFen_Temp;
                                yuE -= xiaZhuJiFen_Temp;

                                function.FpLog(qiHao, xiaZhu, fpjgDataTemp);
                                //===================2018-02===================
                            }
                            else
                            {
                                fpjgData.isSuccess = false;
                                fpjgData.errorMessage = "下注失败";
                            }
                        }
                        else
                        {
                            tiJiaoResult = true;//x
                        }
                    }

                    if (tiJiaoResult == true)
                    {
                        //提交成功继续
                        s_uPI_ID = "";
                        s_uPI_P = "";
                        s_uPI_M = "";
                        xiaZhuJiFen_Temp = 0;

                        for (int i = 5; i < 10; i++)
                        {
                            if (xiaZhu.QD[x, i] > 0 && fpjgData.QD[x, i] == false)
                            {
                                s_uPI_ID += "," + fpCanShu.QD[x, i];
                                s_uPI_P += "," + peiLv.QD[x, i];
                                s_uPI_M += "," + xiaZhu.QD[x, i];
                                xiaZhuJiFen_Temp += xiaZhu.QD[x, i];
                            }
                        }


                        //提交数据
                        if (xiaZhuJiFen_Temp > yuE)
                        {
                            tiJiaoResult = false;
                            fpjgData.isSuccess = false;
                            fpjgData.errorMessage = "金额不足";
                        }
                        else if (s_uPI_ID.Length > 0)
                        {
                            tiJiaoResult = postXiaZhu(url, webCookie, validate, s_uPI_ID, s_uPI_P, s_uPI_M);
                            if (tiJiaoResult == true)
                            {
                                //===================2018-02===================
                                //提交成功
                                fpjgDataTemp = new feiPanJieGuo();
                                for (int i = 5; i < 10; i++)
                                {
                                    fpjgData.QD[x, i] = true;
                                    fpjgDataTemp.QD[x, i] = true;
                                }
                                xiaZhuJiFen += xiaZhuJiFen_Temp;
                                yuE -= xiaZhuJiFen_Temp;
                                function.FpLog(qiHao, xiaZhu, fpjgDataTemp);
                                //===================2018-02===================
                            }
                            else
                            {
                                fpjgData.isSuccess = false;
                                fpjgData.errorMessage = "下注失败";
                            }
                        }
                        else
                        {
                            tiJiaoResult = true;//x
                        }
                    }
                }

                #endregion 单球

                #region 龙虎和 总和

                if (tiJiaoResult == true)
                {
                    //提交成功继续
                    s_uPI_ID = "";
                    s_uPI_P = "";
                    s_uPI_M = "";
                    xiaZhuJiFen_Temp = 0;

                    for (int i = 0; i < 3; i++)//龙虎和
                    {
                        if (xiaZhu.LHH[i] > 0 && fpjgData.LHH[i] == false)
                        {
                            s_uPI_ID += "," + fpCanShu.LHH[i];
                            s_uPI_P += "," + peiLv.LHH[i];
                            s_uPI_M += "," + xiaZhu.LHH[i];
                            xiaZhuJiFen_Temp += xiaZhu.LHH[i];
                        }
                    }

                    for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                    {
                        if (xiaZhu.ZHDXDS[i] > 0 && fpjgData.ZHDXDS[i] == false)
                        {
                            s_uPI_ID += "," + fpCanShu.ZHDXDS[i];
                            s_uPI_P += "," + peiLv.ZHDXDS[i];
                            s_uPI_M += "," + xiaZhu.ZHDXDS[i];
                            xiaZhuJiFen_Temp += xiaZhu.ZHDXDS[i];
                        }
                    }

                    //提交数据
                    if (xiaZhuJiFen_Temp > yuE)
                    {
                        tiJiaoResult = false;
                        fpjgData.isSuccess = false;
                        fpjgData.errorMessage = "金额不足";
                    }
                    else if (s_uPI_ID.Length > 0)
                    {
                        tiJiaoResult = postXiaZhu(url, webCookie, validate, s_uPI_ID, s_uPI_P, s_uPI_M);
                        if (tiJiaoResult == true)
                        {
                            //===================2018-02===================
                            //提交成功
                            fpjgDataTemp = new feiPanJieGuo();
                            for (int i = 0; i < 3; i++)//龙虎和
                            {
                                fpjgData.LHH[i] = true;
                                fpjgDataTemp.LHH[i] = true;
                            }
                            for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                            {
                                fpjgData.ZHDXDS[i] = true;
                                fpjgDataTemp.LHH[i] = true;
                            }
                            xiaZhuJiFen += xiaZhuJiFen_Temp;
                            yuE -= xiaZhuJiFen_Temp;

                            function.FpLog(qiHao, xiaZhu, fpjgDataTemp);
                            //===================2018-02===================
                        }
                        else
                        {
                            fpjgData.isSuccess = false;
                            fpjgData.errorMessage = "下注失败";
                        }
                    }
                    else
                    {
                        tiJiaoResult = true;//x
                    }
                }

                #endregion 龙虎和 总和

                for (int i = 0; i < 4; i++)//总和组合
                {
                    fpjgData.ZHZHDXDS[i] = true;
                }



                if (tiJiaoResult == true)
                {
                    fpjgData.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                fpjgData.isSuccess = false;
                fpjgData.errorMessage = "下注错误";
                function.log("下注错误" + qiHao + "  " + fpjgData.errorMessage);
                //提交失败
                //fpjgData = ServerCommon.SetJieGuoFaile(fpjgData);

                throw ex;
            }

            //
            fpjgData.xiaZhu = xiaZhuJiFen.ToString();

            return fpjgData;
        }
    }
}