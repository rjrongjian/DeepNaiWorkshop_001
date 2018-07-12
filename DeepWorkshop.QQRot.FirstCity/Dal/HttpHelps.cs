using AI.Bll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace AI.Dal
{
    public class HttpHelps
    {

        #region wininet
        [DllImport("wininet.dll", EntryPoint = "InternetOpenA")]
        public static extern int InternetOpenA(string handleName, int type, string daili, string dailiyanma, int dwFlags);

        [DllImport("wininet.dll", EntryPoint = "InternetCloseHandle")]
        public static extern bool InternetCloseHandle(int handle);

        [DllImport("wininet.dll", EntryPoint = "HttpQueryInfoA")]
        public static extern bool HttpQueryInfoA(int handle, int xx, ref string rq, ref int rqlen, int lpdwIndex);

        [DllImport("wininet.dll", EntryPoint = "InternetReadFile")]
        public static extern bool InternetReadFile(int handle, byte[] sBuffer, int lNumBytesToRead, out int lNumberOfBytesRead);

        [DllImport("wininet.dll", EntryPoint = "InternetConnectA")]
        public static extern int InternetConnectA(int InternetHandle, string yuming, int dk, string user, string pass, int type, int dwFlags, int dwContext);

        [DllImport("wininet.dll", EntryPoint = "HttpOpenRequestA")]
        public static extern int HttpOpenRequestA(int InternetHandle, string fangshi, string path, string banben, string yinyong, string type, int dwFlags, int dwContext);

        [DllImport("wininet.dll", EntryPoint = "HttpSendRequestA")]
        public static extern bool HttpSendRequestA(int InternetHandle, string xieyi, int len, string date, int datelen);

        [DllImport("wininet.dll", EntryPoint = "HttpSendRequestA")]
        public static extern bool HttpSendRequestA_byte(int InternetHandle, string xieyi, int len, byte[] date, int datelen);

        /// <summary>
        /// 网页_访问   //获取视频
        /// </summary>
        public static byte[] HttpDownloadFile(string url, string cki)
        {
            bool s;
            string way = "GET";
            string User_Agent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            int InternetJB = InternetOpenA(User_Agent, 1, "", "", 0);
            string xxxxx = function.middlestring(url, "https://", "/");
            int InternetLJJB = InternetConnectA(InternetJB, xxxxx, 443, "", "", 3, 0, 0);
            int biaoji = -2138570736;
            int HTTPQQJB = HttpOpenRequestA(InternetLJJB, way, url.Replace("https://" + function.middlestring(url, "https://", "/"), ""), "HTTP/1.1", "", "", biaoji, 0);
            string xieyitou = @"Range:bytes=0-
Referer: " + url + @"
Accept: */*
Accept-Language: zh-cn
Content-Type: application/x-www-form-urlencoded
Cookie: " + cki;
            s = HttpSendRequestA(HTTPQQJB, xieyitou, xieyitou.Length, "", 0);
            int bufferSize = 1024;
            int revSize = 0;
            byte[] bytes = new byte[bufferSize];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                bool readResult = InternetReadFile(HTTPQQJB, bytes, bufferSize, out revSize);
                if (readResult && revSize > 0)
                {
                    ms.Write(bytes, 0, revSize);
                }
                else
                {
                    break;
                }
            }
            byte[] byt = new byte[5000];
            string bytestring = byt.ToString();
            //HttpQueryInfoA(HTTPQQJB, 22, bytestring, 5000, 0); //获取协议头
            InternetCloseHandle(HTTPQQJB);
            InternetCloseHandle(InternetLJJB);
            InternetCloseHandle(InternetJB);
            return ms.ToArray();
        }

        /// <summary>
        /// 网页_访问 
        /// </summary>
        public static string GetQrh(string url, string cook)
        {
            bool s;
            string way = "GET";
            string User_Agent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            int InternetJB = InternetOpenA(User_Agent, 1, "", "", 0);

            string xxxxx = function.middlestring(url, "https://", "/");
            int InternetLJJB = InternetConnectA(InternetJB, xxxxx, 443, "", "", 3, 0, 0);
            int biaoji = -2138570736;
            int HTTPQQJB = HttpOpenRequestA(InternetLJJB, way, url.Replace("https://" + function.middlestring(url, "https://", "/"), ""), "HTTP/1.1", "", "", biaoji, 0);
            string xieyitou = @"Range:bytes=0-
Referer: " + url + @"
Accept: */*
Accept-Language: zh-cn
Content-Type: application/x-www-form-urlencoded
Cookie:" + cook;
            s = HttpSendRequestA(HTTPQQJB, xieyitou, xieyitou.Length, "", 0);
            int bufferSize = 1024;
            int revSize = 0;
            byte[] bytes = new byte[bufferSize];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                bool readResult = InternetReadFile(HTTPQQJB, bytes, bufferSize, out revSize);
                if (readResult && revSize > 0)
                {
                    ms.Write(bytes, 0, revSize);
                }
                else
                {
                    break;
                }
            }
            byte[] byt = new byte[5000];
            string bytestring = byt.ToString();
            //HttpQueryInfoA(HTTPQQJB, 22, bytestring, 5000, 0); //获取协议头
            InternetCloseHandle(HTTPQQJB);
            InternetCloseHandle(InternetLJJB);
            InternetCloseHandle(InternetJB);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// 网页_访问 
        /// </summary>
        public static byte[] GetQr(string url)
        {
            bool s;
            string way = "GET";
            string User_Agent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            int InternetJB = InternetOpenA(User_Agent, 1, "", "", 0);
            string xxxxx = function.middlestring(url, "https://", "/");
            int InternetLJJB = InternetConnectA(InternetJB, xxxxx, 443, "", "", 3, 0, 0);
            int biaoji = -2139095024;
            int HTTPQQJB = HttpOpenRequestA(InternetLJJB, way, url.Replace("https://" + function.middlestring(url, "https://", "/"), ""), "HTTP/1.1", "", "", biaoji, 0);
            string xieyitou = @"Range:bytes=0-
Referer: " + url + @"
Accept: */*
Accept-Language: zh-cn
Content-Type: application/x-www-form-urlencoded
";
            s = HttpSendRequestA(HTTPQQJB, xieyitou, xieyitou.Length, "", 0);
            int bufferSize = 1024;
            int revSize = 0;
            byte[] bytes = new byte[bufferSize];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                bool readResult = InternetReadFile(HTTPQQJB, bytes, bufferSize, out revSize);
                if (readResult && revSize > 0)
                {
                    ms.Write(bytes, 0, revSize);
                }
                else
                {
                    break;
                }
            }
            //string str = new string(new char[5000]);
            //int len=0;
            // bool xs= HttpQueryInfoA(HTTPQQJB, 22,ref str, ref len, 0); //获取协议头

            InternetCloseHandle(HTTPQQJB);
            InternetCloseHandle(InternetLJJB);
            InternetCloseHandle(InternetJB);
            return ms.ToArray();
        }

        #endregion

        #region Get Post
        /// <summary>
        /// GET string
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static byte[] GetAudio(string requestUrl, string cki)
        {
            try
            {
                byte[] bytes;
                System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                Uri uri = new Uri(requestUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AllowAutoRedirect = false;
                request.Referer = requestUrl;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";
                request.Accept = "audio/webm,audio/ogg,audio/wav,audio/*;q=0.9,application/ogg;q=0.7,video/*;q=0.6,*/*;q=0.5";
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("Cookie", cki);
                request.Timeout = 50000;
                using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = responseSorce.GetResponseStream())
                    {
                        List<byte> byt = new List<byte>();
                        int temp = stream.ReadByte();
                        while (temp != -1)
                        {
                            byt.Add((byte)temp);
                            temp = stream.ReadByte();
                        }

                        bytes = byt.ToArray();

                        responseSorce.Close();
                        stream.Close();

                    }
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        /// <summary>
        /// GET string
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static string Get(Encoding encoding, string requestUrl, ref string cki)
        {
            try
            {
                string requestHtml = string.Empty;
                //System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                Uri uri = new Uri(requestUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.Referer = requestUrl;
                if (requestUrl.IndexOf(".wx.qq.com/cgi-bin/mmwebwx-bin/synccheck?r=") != -1)
                    request.KeepAlive = true;


                request.ProtocolVersion = HttpVersion.Version10;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";

                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("Cookie", cki);
                // request.Headers.Add("_X_FORWARDED_FOR", "162.150.10.16");//伪代理
                request.Timeout = 50000;

                using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = responseSorce.GetResponseStream())
                    {
                        using (StreamReader readerOfStream = new StreamReader(stream, encoding))
                        {
                            if (responseSorce.Headers.Get("Set-Cookie") != null)
                            {
                                cki = responseSorce.Headers.Get("Set-Cookie");
                            }
                            requestHtml = readerOfStream.ReadToEnd();
                            readerOfStream.Close();
                            responseSorce.Close();
                            stream.Close();
                        }
                    }
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }

                return requestHtml;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Post(string postData, string url, ref string cki, Encoding bm)
        {
            var resultString = "";
            try
            {

                // System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(postData);
                request.Method = "Post";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "zh-cn");

                request.ContentType = "application/x-www-form-urlencoded";
                if (url.IndexOf("/mms/") != -1)

                    request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.Headers.Add("Cookie", cki);
                request.KeepAlive = false;

                request.Timeout = 30000;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(responseSorce.GetResponseStream(), bm))
                        {
                            if (responseSorce.Headers.Get("Set-Cookie") != null)
                            {
                                cki = responseSorce.Headers.Get("Set-Cookie");
                            }
                            resultString = reader.ReadToEnd();

                        }
                    }
                }
                return resultString;
            }

            catch (Exception ex)
            {
                return "";
            }
        }

        public static Image GetPicture(string url, string cki)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.Accept = "image/png, image/*;q=0.8, */*;q=0.5";
            request.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3");
            request.Headers.Add("Cookie", cki);
            //获取图片数据
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream sstreamRes = response.GetResponseStream();

            return System.Drawing.Image.FromStream(sstreamRes);
        }


        /// <summary>
        /// Http上传pic doc 
        /// </summary>
        public static string HttpUploadFile(string url, string postData, byte[] img, ref string cki)
        {
            string resultString = string.Empty;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                byte[] merged = Encoding.UTF8.GetBytes(postData.Replace("\r\n", "\n"));
                byte[] ix = Encoding.UTF8.GetBytes("\r\n-----------------------------1093099678267--");

                List<byte> tmp = new List<byte>(merged.Length + img.Length + ix.Length);
                tmp.AddRange(merged);
                tmp.AddRange(img);
                tmp.AddRange(ix);
                byte[] data = tmp.ToArray();

                request.Method = "Post";
                request.ContentType = " multipart/form-data; boundary=-----------------------------1093099678267";
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.Headers.Add("Cookie", cki);

                System.Net.WebClient myWebClient = new System.Net.WebClient();

                request.Timeout = 30000;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(responseSorce.GetResponseStream(), Encoding.UTF8))
                        {

                            resultString = reader.ReadToEnd();
                        }
                    }
                }
                return resultString;
            }

            catch (Exception ex)
            {
                return "";
            }

        }


        /// <summary> 
        /// 上传图片文件 
        /// </summary> 
        /// <param name="url">提交的地址</param> 
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param> 
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param> 
        /// <param name="cookie">cookie数据</param> 
        /// <param name="refre">头部的跳转地址</param> 
        /// <returns></returns> 
        public static string HttpUploadFile1(string url, string poststr, byte[] img)
        {

            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "multipart/form-data; boundary=---------------------------153073255032032";
            webrequest.Method = "POST";

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(poststr);

            //构造尾部数据 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n-----------------------------153073255032032--\r\n");

            long length = postHeaderBytes.Length + img.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            //输入图片
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)img.Length))];

            int z = img.Length / 4096;
            int y = img.Length % 4096;
            for (int i = 0; i < z; i++)
            {
                Array.Copy(img, i * 4096, buffer, 0, 4096);
                requestStream.Write(buffer, 0, buffer.Length);
            }
            if (y != 0)
            {
                buffer = new Byte[y];
                Array.Copy(img, img.Length - y, buffer, 0, y);
                requestStream.Write(buffer, 0, buffer.Length);
            }


            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();
        }

        #endregion


        #region CookieContainer
        /// <summary>
        /// post
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Post(string postData, string url, CookieContainer cookie, Encoding bm)
        {

            var resultString = "";
            try
            {
                // System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(postData);
                request.Method = "Post";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "zh-cn");

                request.ContentType = "application/x-www-form-urlencoded";
                if (url.IndexOf("/mms/") != -1)
                    request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.Timeout = 30000;
                request.CookieContainer = cookie;


                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(responseSorce.GetResponseStream(), bm))
                        {
                            resultString = reader.ReadToEnd();
                        }
                    }
                }
                cookie = request.CookieContainer;//访问后更新cookie  
                return resultString;
            }

            catch (Exception ex)
            {
                return "";
            }
        }

        public static string PostJson(string postData, string url, CookieContainer cookie, Encoding bm)
        {
            var resultString = "";
            try
            {
                // System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(postData);
                request.Method = "Post";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "zh-cn");

                request.ContentType = "application/json; charset=utf-8";

                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.Timeout = 30000;

                request.CookieContainer = cookie;


                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(responseSorce.GetResponseStream(), bm))
                        {
                            resultString = reader.ReadToEnd();
                        }
                    }
                }
                cookie = request.CookieContainer;//访问后更新cookie  
                return resultString;
            }

            catch (Exception ex)
            {
                return "";
            }
        }


        public static string Get(string requestUrl, CookieContainer cookie, Encoding encoding)
        {
            return Get(requestUrl, cookie, encoding, 50000);
        }

        public static string Get(string requestUrl, CookieContainer cookie, Encoding encoding, int timeOut)
        {
            try
            {
                string requestHtml = string.Empty;
                //System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                Uri uri = new Uri(requestUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.Referer = requestUrl;
                request.KeepAlive = false;

                request.ProtocolVersion = HttpVersion.Version10;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";

                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");

                request.Timeout = timeOut;
                request.CookieContainer = cookie;

                using (HttpWebResponse responseSorce = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = responseSorce.GetResponseStream())
                    {
                        using (StreamReader readerOfStream = new StreamReader(stream, encoding))
                        {
                            cookie = request.CookieContainer;//访问后更新cookie  

                            requestHtml = readerOfStream.ReadToEnd();
                            readerOfStream.Close();
                            responseSorce.Close();
                            stream.Close();
                        }
                    }
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }

                return requestHtml;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public static Image GetPicture(string url, CookieContainer cookie)
        {
            if (cookie == null)
            {
                cookie = new CookieContainer();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.Accept = "image/png, image/*;q=0.8, */*;q=0.5";
            request.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3");
            request.CookieContainer = cookie;
            //获取图片数据
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;//访问后更新cookie  
            Stream sstreamRes = response.GetResponseStream();

            return System.Drawing.Image.FromStream(sstreamRes);
        }


        #endregion
    }
}
