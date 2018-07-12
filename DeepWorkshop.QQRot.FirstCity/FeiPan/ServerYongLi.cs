using AI.Bll;
using AI.Dal;
using System;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;

namespace 新一城娱乐系统.FeiPan
{
    public class ServerYongLi
    {
        /// <summary>
        /// 提交参数
        /// </summary>
        private static feiPanCanShu fpCanShu = new feiPanCanShu()
        {
            QD = new string[5, 10] {{ "205_0", "205_1", "205_2", "205_3", "205_4", "205_5", "205_6", "205_7", "205_8", "205_9" },
                                       {  "206_0", "206_1", "206_2", "206_3", "206_4", "206_5", "206_6", "206_7", "206_8", "206_9" },
                                       {  "207_0", "207_1", "207_2", "207_3", "207_4", "207_5", "207_6", "207_7", "207_8", "207_9"  },
                                       {  "208_0", "208_1", "208_2", "208_3", "208_4", "208_5", "208_6", "208_7", "208_8", "208_9"  },
                                       {  "209_0", "209_1", "209_2", "209_3", "209_4", "209_5", "209_6", "209_7", "209_8", "209_9"  }},

            DXDS = new string[5, 4] { { "210_1", "210_2", "215_1", "215_2" }, { "211_1", "211_2", "216_1", "216_2" },
                                          { "212_1", "212_2", "217_1", "217_2" }, { "213_1", "213_2", "218_1", "218_2" },
                                          { "214_1", "214_2", "219_1", "219_2"} },

            ZHDXDS = new string[] { "346_1", "346_2", "345_1", "345_2" },

            ZHZHDXDS = new string[] { "", "", "", "" },

            LHH = new string[] { "347_1", "347_2", "347_3" }
        };

        /// <summary>
        /// 获取最新赔率
        /// </summary>
        public static peilv getPeilv(string url, CookieContainer webCookie)
        {
            //获取赔率

            string resultPeilv = "";
            peilv serPeilv = new peilv();

            try
            {
                function.log("获取赔率");

                //赔率
                resultPeilv = HttpHelps.Post("stype=getoddsbytype&gameno=6&oddsgroupnos=205;206;207;208;209;210;211;212;213;214;215;216;217;218;219;330;331;332;333;334;335;336;337;338;339;340;341;342;343;344;345;346;347;&wagerroundno=B&ts="
                    + function.MilliTime(), url + "/cp2-dfgj-mb/ashx/orderHandler.ashx", webCookie, Encoding.UTF8);

                if (resultPeilv.Length < 200)
                {
                    return null;
                }

                string xunhao = "";
                decimal val = 0;

                #region 获取下注赔率

                //{"oddsgroupno":206,"objectid":2,"odds":9.7100}

                for (int i = 0; i < 5; i++)//单球
                {
                    for (int x = 0; x < 10; x++)
                    {
                        xunhao = "{\"oddsgroupno\":" + fpCanShu.QD[i, x].Replace("_", ",\"objectid\":") + ",\"odds\":";
                        string peilvNote = function.middlestring(resultPeilv, xunhao, "}");
                        decimal.TryParse(peilvNote, out val);
                        serPeilv.QD[i, x] = val;
                    }
                }
                for (int i = 0; i < 5; i++)//大小单双
                {
                    for (int x = 0; x < 4; x++)
                    {
                        xunhao = "{\"oddsgroupno\":" + fpCanShu.DXDS[i, x].Replace("_", ",\"objectid\":") + ",\"odds\":";
                        string peilvNote = function.middlestring(resultPeilv, xunhao, "}");
                        decimal.TryParse(peilvNote, out val);
                        serPeilv.DXDS[i, x] = val;
                    }
                }
                for (int i = 0; i < 3; i++)//龙虎和
                {
                    xunhao = "{\"oddsgroupno\":" + fpCanShu.LHH[i].Replace("_", ",\"objectid\":") + ",\"odds\":";
                    string peilvNote = function.middlestring(resultPeilv, xunhao, "}");
                    decimal.TryParse(peilvNote, out val);
                    serPeilv.LHH[i] = val;
                }

                for (int i = 0; i < 4; i++)//总和
                {
                    xunhao = "{\"oddsgroupno\":" + fpCanShu.ZHDXDS[i].Replace("_", ",\"objectid\":") + ",\"odds\":";
                    string peilvNote = function.middlestring(resultPeilv, xunhao, "}");
                    decimal.TryParse(peilvNote, out val);
                    serPeilv.ZHDXDS[i] = val;
                }

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
        public static bool login(string url, string username, string userpass, CookieContainer webCookie, out string postUrl)
        {
            if (username.Length == 0 || userpass.Length == 0)
            {
                postUrl = "";
                return false;
            }


            bool loginSuccess = false;
            string formPost = "";
            postUrl = "";
            string hidden_verificationToken = "";
            string hidden_VIEWSTATE = "";
            string hidden_VIEWSTATEGENERATOR = "";
            //登录获取Cookie

            //

            for (int i = 0; i < 3; i++)
            {
                string frameUrl = "";
                for (int j = 0; j < 2; j++)
                {
                    string resultMain = HttpHelps.Get(url, webCookie, Encoding.Default, 3000);

                    //string resultMain = HttpHelps.Post("", url, webCookie, Encoding.Default);

                    frameUrl = function.middlestring(resultMain, "<iframe src=\"", "\" width=\"100%\"");
                    if (!string.IsNullOrWhiteSpace(frameUrl))
                    {
                        function.log("实际地址" + frameUrl);
                        break;
                    }

                }
                if (frameUrl.Length > 0)
                {
                    Uri frameUri = new Uri(frameUrl);
                    string resultFrame = HttpHelps.Get(frameUrl, webCookie, Encoding.UTF8);

                    formPost = function.middlestring(resultFrame, "<form name=\"form1\" method=\"post\" action=\"./?", "\" id=");
                    if (formPost.Length == 0)
                    {
                        formPost = function.middlestring(resultFrame, "<form name=\"form1\" method=\"post\" action=\"?", "\" id=");
                    }
                    hidden_verificationToken = function.middlestring(resultFrame, "__RequestVerificationToken\" type=\"hidden\" value=\"", "\" />");
                    hidden_VIEWSTATE = function.middlestring(resultFrame, "__VIEWSTATE\" value=\"", "\" />");
                    hidden_VIEWSTATEGENERATOR = function.middlestring(resultFrame, "__VIEWSTATEGENERATOR\" value=\"", "\" />");
                    postUrl = "http://" + frameUri.Host;
                    if (hidden_verificationToken.Length > 0)
                    {
                        if (hidden_VIEWSTATEGENERATOR.Length > 0)
                        {

                        }
                        break;
                    }

                    string jsCookie = HttpHelps.Post("", postUrl + "/cp2-dfgj-mb/bk.aspx/GetCookie", webCookie, Encoding.Default);
                }
                Thread.Sleep(100);
            }


            int tryCount = 0;

            while (true)
            {
                if (string.IsNullOrWhiteSpace(postUrl))
                {
                    loginSuccess = false;
                    break;
                }

                #region 获取验证码

                //图像验证码
                string validateCode = "";
                Bitmap bitmap;
                validateCode = ServerCommon.GetValidateCode(postUrl + "/cp2-dfgj-mb/checknum.aspx?ts=" + DateTime.Now.Ticks,
                                      webCookie, out bitmap);




                #endregion 获取验证码

                #region 登录

                if (validateCode.Length == 4)
                {
                    string resultCredits = HttpHelps.Post("txt_U_name=" + username + "&txt_U_Password=" + userpass + "&txt_validate=" + validateCode
                       + "&__VIEWSTATE=" + hidden_VIEWSTATE + "&__VIEWSTATEGENERATOR=" + hidden_VIEWSTATEGENERATOR + "&__RequestVerificationToken=" + hidden_verificationToken,
                        postUrl + "/cp2-dfgj-mb/" + "?" + formPost, webCookie, Encoding.UTF8);

                    string resultLeft = HttpHelps.Post("", postUrl + "/cp2-dfgj-mb/ch/left.aspx", webCookie, Encoding.UTF8);

                    if (resultLeft.Length > 2000)
                    {
                        loginSuccess = true;
                        function.log("永利登录成功");
                        break;
                    }
                    else
                    {
                        tryCount++;
                        loginSuccess = false;
                    }

                    function.log("永利登录失败，重新尝试");
                    Thread.Sleep(500);
                }

                #endregion 登录

                if (tryCount > 10)
                {
                    function.log("尝试登录次数过多");
                    break;
                }
            }

            return loginSuccess;
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
                //{"d":"[{\"Rows\":[{\"memberno\":\"ffgggg7788\",\"membername\":\"哥\",\"opena\":false,\"openb\":true,\"openc\":false,
                //\"opend\":false,\"opene\":false,\"credittype\":2,\"accounttype\":1,\"creditquota\":10.0000,\"usecreditquota\":0.00,
                //\"usecreditquota2\":0.00,\"allowcreditquota\":10.00}]},0.0000]"}
                string resultGetMembersInfo = HttpHelps.PostJson("", url + "/cp2-dfgj-mb/ch/main.aspx/GetMembersMbinfo", webCookie, Encoding.UTF8);

                string credits = function.middlestring(resultGetMembersInfo, ",\\\"creditquota\\\":", ",");
                if (credits.Length > 0)
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
                //function.log("永利获取余额失败" + ex.Message);
                keYongJinE = -1;
            }

            return keYongJinE;
        }

        /// <summary>
        /// 获取用户信息x
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <returns></returns>
        public static string getUserInfo(string url, CookieContainer webCookie)
        {
            string resultGetMembersInfo = HttpHelps.PostJson("", url + "/cp2-dfgj-mb/ch/main.aspx/GetMembersMbinfo", webCookie, Encoding.UTF8);

            return resultGetMembersInfo;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <returns></returns>
        public static bool checkLogin(string url, CookieContainer webCookie)
        {
            string userInfo = getUserInfo(url, webCookie);
            //登录 294个字符
            //未登录：{"d":null}
            if (userInfo.Length > 100)
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
        /// <param name="url"></param>
        /// <param name="webCookie"></param>
        /// <param name="transtring">"212,,1,,1.9428,10;"</param>
        /// <param name="arrstring">"212:1:10;"</param>
        /// <param name="creditquota">金额</param>
        /// <returns></returns>
        public static bool postXiaZhu(string url, CookieContainer webCookie, string transtring, string arrstring)
        {
            string pb = "B";//parent.main_mbopen
            string gameno = "6";
            string lianma_transtrin = "";//空
            string creditquota = getYuE(url, webCookie).ToString();

            //获取 xindan_roundno
            //cd-> xindan_roundno  -> parent.roundno
            //{"d":"{\"code\":1,\"message\":{\"sd\":\"20171124-018\",\"soTs\":-1,
            //\"seTs\":-1,\"swTs\":-1,\"cd\":\"20171124-019\",\"coTs\":-137,\"ceTs\":123,\"cwTs\":204,\"nd\":\"20171124-020\",\"noTs\":40,\"neTs\":260,
            //\"nwTs\":81,\"jgTs\":41,\"kjTs\":300,\"ld\":\"20171124-018\",\"lr\":[\"8\",\"9\",\"1\",\"9\",\"7\"]}}"}
            string resultLoadDrawsInfo = HttpHelps.PostJson("{gameno:" + gameno + "}", url + "/cp2-dfgj-mb/app/ws_game.asmx/LoadDrawsInfo",
                webCookie, Encoding.UTF8);
            string xindan_roundno = function.middlestring(resultLoadDrawsInfo, "\\\"cd\\\":\\\"", "\\\"");

            //
            string formToken = "";
            if (xindan_roundno.Length > 0)
            {
                //下注前验证
                //{wagerround:"B",transtring:"212,,1,,1.9428,10;",arrstring:"212:1:10;",wagetype:0,allowcreditquota:10,hasToken:true}
                //"{wagerround:\"" + pb + "\",transtring:\"" + transtring + "\",arrstring:\"" + arrstring + "\",wagetype:"
                //    + (wagetype != undefined ? wagetype : 0) + ",allowcreditquota:" + (pricecount != undefined || pricecount != "" ? pricecount : 0) + ",hasToken:true}"

                string postDataA = "{wagerround:\"" + pb + "\",transtring:\"" + transtring + "\",arrstring:\"" + arrstring
                    + "\",wagetype:0,allowcreditquota:" + creditquota + ",hasToken:true}";
                string resuleA = HttpHelps.PostJson(postDataA, url + "/cp2-dfgj-mb/ch/left.aspx/GetMemberMtran", webCookie, Encoding.UTF8);
                string[] res = resuleA.Split(new string[] { "$@" }, StringSplitOptions.None);

                if (res.Length == 7)
                {
                    formToken = res[6].Replace("\"}", "");
                }
            }

            function.log("下注参数：" + transtring + "  " + arrstring + "  " + xindan_roundno + "  " + formToken);

            bool isSuccess = false;
            if (formToken.Length > 0 && xindan_roundno.Length > 0)
            {
                //确认下注
                //"{gameno:" + parent.gameno + ",wagerroundstring:\"" + pb + "\",arrstring:\"" + arrs + "\",roundno:\"" + xindan_roundno + "\",lianma_transtrin:\"" + lianma_transtrin + "\",token:\"" + formToken + "\"}"
                string postDataB = "{gameno:\"" + gameno + "\",wagerroundstring:\"" + pb + "\",arrstring:\"" + arrstring
                    + "\",roundno:\"" + xindan_roundno + "\",lianma_transtrin:\"" + lianma_transtrin
                    + "\",token:\"" + formToken + "\"}";
                string resuleB = HttpHelps.PostJson(postDataB, url + "/cp2-dfgj-mb/ch/left.aspx/mtran_XiaDan_New", webCookie, Encoding.UTF8);

                //function.log("下注结果：" + resuleB);

                //没有该游戏操作权限！
                //超过您的每日信用额度,无法下注,请联系上级代理
                if (resuleB.IndexOf("没有该游戏操作权限") > -1 || resuleB.IndexOf("超过您的每日信用额度") > -1 || resuleB.IndexOf("索引超出了数组界限") > -1)
                {
                    isSuccess = false;
                    function.log("下注错误：" + resuleB);
                }
                else
                {
                    isSuccess = true;
                    function.log("下注成功：" + resuleB);
                }
            }
            return isSuccess;
        }

        public static feiPanJieGuo xiaZhu(string url, CookieContainer webCookie, peilv peiLv, xztj xiaZhu, string qiHao, feiPanJieGuo fpjgData)
        {
            fpjgData.isSuccess = false;
            feiPanJieGuo fpjgDataTemp = new feiPanJieGuo();
            int xiaZhuJiFen = 0;
            //获取余额
            decimal yuE = getYuE(url, webCookie);

            try
            {
                int xiaZhuJiFen_Temp = 0;
                string transtring = "";//212,,1,,1.9428,10;
                string arrstring = "";//622:1:10;

                bool tiJiaoResult = false;

                #region 大小单双

                transtring = "";
                arrstring = "";
                tiJiaoResult = false;
                xiaZhuJiFen_Temp = 0;

                for (int i = 0; i < 5; i++)//大小单双
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (xiaZhu.DXDS[i, x] > 0 && fpjgData.DXDS[i, x] == false)
                        {
                            transtring += fpCanShu.DXDS[i, x].Replace("_", ",,") + ",," + peiLv.DXDS[i, x] + "," + xiaZhu.DXDS[i, x] + ";";
                            arrstring += fpCanShu.DXDS[i, x].Replace("_", ":") + ":" + xiaZhu.DXDS[i, x] + ";";
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
                else if (arrstring.Length > 0)
                {
                    tiJiaoResult = postXiaZhu(url, webCookie, transtring, arrstring);
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
                        transtring = "";
                        arrstring = "";
                        xiaZhuJiFen_Temp = 0;

                        for (int i = 0; i < 5; i++)
                        {
                            if (xiaZhu.QD[x, i] > 0 && fpjgData.QD[x, i] == false)
                            {
                                transtring += fpCanShu.QD[x, i].Replace("_", ",,") + ",," + peiLv.QD[x, i] + "," + xiaZhu.QD[x, i] + ";";
                                arrstring += fpCanShu.QD[x, i].Replace("_", ":") + ":" + xiaZhu.QD[x, i] + ";";
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
                        else if (arrstring.Length > 0)
                        {
                            tiJiaoResult = postXiaZhu(url, webCookie, transtring, arrstring);
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
                        transtring = "";
                        arrstring = "";
                        xiaZhuJiFen_Temp = 0;

                        for (int i = 5; i < 10; i++)
                        {
                            if (xiaZhu.QD[x, i] > 0 && fpjgData.QD[x, i] == false)
                            {
                                transtring += fpCanShu.QD[x, i].Replace("_", ",,") + ",," + peiLv.QD[x, i] + "," + xiaZhu.QD[x, i] + ";";
                                arrstring += fpCanShu.QD[x, i].Replace("_", ":") + ":" + xiaZhu.QD[x, i] + ";";
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
                        else if (arrstring.Length > 0)
                        {
                            tiJiaoResult = postXiaZhu(url, webCookie, transtring, arrstring);
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
                    transtring = "";
                    arrstring = "";
                    tiJiaoResult = false;
                    xiaZhuJiFen_Temp = 0;

                    for (int i = 0; i < 3; i++)//龙虎和
                    {
                        if (xiaZhu.LHH[i] > 0 && fpjgData.LHH[i] == false)
                        {
                            transtring += fpCanShu.LHH[i].Replace("_", ",,") + ",," + peiLv.LHH[i] + "," + xiaZhu.LHH[i] + ";";
                            arrstring += fpCanShu.LHH[i].Replace("_", ":") + ":" + xiaZhu.LHH[i] + ";";
                            xiaZhuJiFen_Temp += xiaZhu.LHH[i];
                        }
                    }

                    for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                    {
                        if (xiaZhu.ZHDXDS[i] > 0 && fpjgData.ZHDXDS[i] == false)
                        {
                            transtring += fpCanShu.ZHDXDS[i].Replace("_", ",,") + ",," + peiLv.ZHDXDS[i] + "," + xiaZhu.ZHDXDS[i] + ";";
                            arrstring += fpCanShu.ZHDXDS[i].Replace("_", ":") + ":" + +xiaZhu.ZHDXDS[i] + ";";
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
                    else if (arrstring.Length > 0)
                    {
                        tiJiaoResult = postXiaZhu(url, webCookie, transtring, arrstring);
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
                                fpjgDataTemp.ZHDXDS[i] = true;
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

                //
                if (tiJiaoResult == true)
                {
                    fpjgData.isSuccess = true;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                fpjgData.isSuccess = false;
                fpjgData.errorMessage = "下注错误";
                function.log("下注错误" + qiHao + "  " + fpjgData.errorMessage);
                //提交失败
                //fpjgData = ServerCommon.SetFeiPanJieGuo(fpjgData, false);

                throw ex;
            }

            //
            fpjgData.xiaZhu = xiaZhuJiFen.ToString();

            return fpjgData;
        }
    }
}