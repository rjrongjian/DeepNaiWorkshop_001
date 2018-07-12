using AI.Bll;
using System;
using System.Net;

namespace 新一城娱乐系统.FeiPan
{
    public static class ServerFeiPan
    {
        #region 基础

        /// <summary>
        /// 服务Cookie
        /// </summary>
        private static CookieContainer _webCookieShunFen = new CookieContainer();

        /// <summary>
        /// 服务Cookie
        /// </summary>
        private static CookieContainer _webCookieYongLi = new CookieContainer();

        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLoginSuccess = false;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string ServerUrl = "";

        /// <summary>
        /// 服务器实际飞盘地址
        /// </summary>
        public static string FeidanUrl = "";


        /// <summary>
        /// 服务器实际飞盘地址
        /// </summary>
        public static string FeidanUrlShunFen = "";

        /// <summary>
        /// 服务器实际飞盘地址
        /// </summary>
        public static string FeidanUrlYongLi = "";


        /// <summary>
        /// 服务用户名
        /// </summary>
        public static string LoginName = "";

        /// <summary>
        /// 服务密码
        /// </summary>
        public static string LoginPass = "";

        /// <summary>
        /// 可用余额
        /// </summary>
        public static decimal KeYongYuE = 0;

        /// <summary>
        /// 服务器类型
        /// </summary>
        public static string ServerType = "";

        #endregion 基础

        #region 顺丰

        /// <summary>
        /// 服务器赔率
        /// </summary>
        private static peilv _serPeilv;

        /// <summary>
        /// 提交数据验证参数
        /// </summary>
        private static string _serValidate = "";

        #endregion 顺丰

        #region 永利

        #endregion 永利

        public static feiPanJieGuo xiaZhu_shunfen(xztj xiaZhu, string qiHao, feiPanJieGuo fpjgData)
        {
            _serPeilv = ServerShunFeng.getPeilv(FeidanUrlShunFen, _webCookieShunFen);
            decimal yuE = 0;

            if (IsLoginSuccess && _serPeilv != null)
            {
                fpjgData = ServerShunFeng.xiaZhu(FeidanUrlShunFen, _webCookieShunFen, _serPeilv, xiaZhu, _serValidate, qiHao, fpjgData);
                if (fpjgData.isSuccess == true)
                {
                    //获取余额
                    yuE = ServerShunFeng.getYuE(FeidanUrlShunFen, _webCookieShunFen);
                }
            }
            else
            {
                fpjgData.isSuccess = false;
                fpjgData.errorMessage = "链接异常";
                //提交失败
                fpjgData = ServerCommon.SetFeiPanJieGuo(fpjgData, false);
            }

            fpjgData.yuE = yuE.ToString();

            KeYongYuE = yuE;

            return fpjgData;
        }

        public static feiPanJieGuo xiaZhu_yongli(xztj xiaZhu, string qiHao, feiPanJieGuo fpjgData)
        {
            decimal yuE = 0;

            _serPeilv = ServerYongLi.getPeilv(FeidanUrlYongLi, _webCookieYongLi);

            if (IsLoginSuccess && _serPeilv != null)
            {
                fpjgData = ServerYongLi.xiaZhu(FeidanUrlYongLi, _webCookieYongLi, _serPeilv, xiaZhu, qiHao, fpjgData);
                if (fpjgData.isSuccess == true)
                {
                    //获取余额
                    yuE = ServerShunFeng.getYuE(FeidanUrlYongLi, _webCookieYongLi);
                }
            }
            else
            {
                fpjgData.isSuccess = false;
                fpjgData.errorMessage = "链接异常";
                //提交失败
                fpjgData = ServerCommon.SetFeiPanJieGuo(fpjgData, false);
            }

            fpjgData.yuE = yuE.ToString();

            KeYongYuE = yuE;

            return fpjgData;
        }

        #region

        public static bool loginAgain()
        {
            if (ServerUrl.Length == 0 || LoginName.Length == 0 || ServerType.Length == 0)
            {
                return false;
            }
            return login(ServerUrl, LoginName, LoginPass, ServerType);
        }

        /// <summary>
        /// 保持登录状态
        /// </summary>
        public static bool login(string url, string username, string userpass, string serType)
        {
            if (username.Length == 0 && userpass.Length == 0)
            {
                return false;
            }

            bool isSuccess = false;

            //登录获取Cookie

            if (serType.Equals("顺丰"))
            {
                FeidanUrlShunFen = url;
                //
                if (ServerShunFeng.checkLogin(FeidanUrlShunFen, _webCookieShunFen))
                {
                    function.log("顺丰已经登录。");
                    isSuccess = true;
                }
                else
                {
                    isSuccess = ServerShunFeng.login(FeidanUrlShunFen, username, userpass, _webCookieShunFen, 5);
                }
                //


                //
                KeYongYuE = ServerShunFeng.getYuE(FeidanUrlShunFen, _webCookieShunFen);

                //获取验证
                _serValidate = ServerShunFeng.getValidate(FeidanUrlShunFen, _webCookieShunFen);

                //
                FeidanUrl = FeidanUrlShunFen;
            }
            else if (serType.Equals("永利"))
            {
                try
                {
                    if (ServerYongLi.checkLogin(FeidanUrlYongLi, _webCookieYongLi))
                    {
                        function.log("永利已经登录。");
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = ServerYongLi.login(url, username, userpass, _webCookieYongLi, out FeidanUrlYongLi);
                    }

                }
                catch (Exception ex)
                {
                    function.log("网站链接错误" + ex.Message);
                }

                //
                KeYongYuE = ServerYongLi.getYuE(FeidanUrlYongLi, _webCookieYongLi);

                //
                FeidanUrl = FeidanUrlYongLi;
            }

            //
            function.log("当期登录服务器" + FeidanUrl);

            ServerUrl = url;
            IsLoginSuccess = isSuccess;
            ServerType = serType;
            LoginName = username;
            LoginPass = userpass;

            return isSuccess;
        }

        public static bool checkLogin()
        {

            if (ServerType.Equals("顺丰"))
            {
                return ServerShunFeng.checkLogin(FeidanUrlShunFen, _webCookieShunFen);
            }
            else if (ServerType.Equals("永利"))
            {
                return ServerYongLi.checkLogin(FeidanUrlYongLi, _webCookieYongLi);
            }
            else
            {
                return false;
            }
        }

        public static decimal getYuE()
        {

            if (ServerType.Equals("顺丰"))
            {
                decimal yue = ServerShunFeng.getYuE(FeidanUrlShunFen, _webCookieShunFen);
                if (yue > -1)
                {

                    KeYongYuE = yue;
                }
            }
            else if (ServerType.Equals("永利"))
            {
                decimal yue = ServerYongLi.getYuE(FeidanUrlYongLi, _webCookieYongLi);
                if (yue > -1)
                {
                    KeYongYuE = yue;
                }
            }
            return KeYongYuE;
        }

        #endregion
    }
}