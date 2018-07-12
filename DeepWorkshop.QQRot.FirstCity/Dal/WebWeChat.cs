using AI.Dal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using 新一城娱乐系统.FeiPan;

namespace AI.Bll
{
    /// <summary>
    /// 微信群和群成员，前一位烂尾写的代码
    /// </summary>
    public class GROUP
    {
        public int id = 0;
        public string UserName = "";
        public string NickName = "";
        public string HeadImgUrl = "";
        public string RemarkName = "";
        public string seq = "";
        public string URLlist = "";
        public List<string> DATAlist = new List<string>();

        public List<GROUP> MemberList;

        public bool sfrj = false;//是否入局


        public string NickNameShort = "";
        //时时彩积分
        public xztj xiazhutongji = new xztj();

        //
        public xztj shangQiXiaZhu = new xztj();

        public int lsxzjf = 0;//临时下注积分
        public int lszjf = 0;//临时现有积分

        public string conter = "";//本期下注指令
        public int benqiyingkui = 0;//本期盈亏
        public int benqixiazhu = 0;//本期下注积分

        public int zongjifen = 0;//现有积分
        public int zongyingkui = 0;//总盈亏
        public int zongxiazhu = 0;//总下注积分

        public string bendibeizhu = "";//本地备注


    }

    public class WebWeChat
    {
        public string uuid = "";
        public string QrCode = "";
        public string cookie = "";
        public string domain = "";
        public string heartbeat = "";
        public string skey = "";
        public string wxsid = "";
        public string wxuin = "";
        public string pass_ticket = "";
        public string SkeyGet = "";
        public string SkeyPost = "";
        public string UserName = "";
        public string HeadImgUrl = "";
        public string NickName = "";
        public string Uin = "";
        public string HeadImageBase64;
        public bool Open = false;
        private JObject Friends;
        private JObject Offline;
        public JToken session;
        public Image HeadImage;
        private string webwx_data_ticket = "";
        private long tim = long.Parse(function.MilliTime());
        private string Err;

        /// <summary>
        /// 获取二维码
        /// </summary>
        public WebWeChat()
        {
            cookie = "";
            string s = Encoding.Default.GetString(HttpHelps.GetQr("https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb&redirect_uri="));
            uuid = function.middlestring(s, "window.QRLogin.uuid = \"", "\";");
            QrCode = "https://login.weixin.qq.com/qrcode/" + uuid;
        }

        /// <summary>
        /// 委托   二维码界面
        /// </summary>
        /// <param name="HImage"></param>
        public delegate void myHandler(Image HImage);

        public event myHandler SetImage;

        private string s;

        /// <summary>
        /// 扫二维码
        /// </summary>
        public void Scanning()
        {
            while (true)
            {
                /// MessageBox.Show("https://login.wx.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&uuid=" + uuid + "&tip=0&r=-755502095&_=1491109138225");
                s = HttpHelps.Get(Encoding.Default, "http://login.wx.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&uuid=" + uuid + "&tip=0&r=-755502095&_=1491109138225", ref cookie);

                if (function.middlestring(s, "code=", ";") == "201")
                {
                    HeadImageBase64 = function.middlestring(s, "base64,", "'");
                    if (HeadImageBase64 != "")
                        HeadImage = function.Base64ToImage(HeadImageBase64);
                    if (SetImage != null)
                        SetImage(HeadImage);
                    Waitinglogin(s);
                    return;
                }
                if (function.middlestring(s, "code=", ";") == "200")
                {
                    Waitinglogin(s);
                    return;
                }
            }
        }

        private string uri;
        private string file;

        /// <summary>
        /// 委托   成功登陆 关闭二维码界面事件
        /// </summary>
        public delegate void clos();

        public event clos cl;

        /// <summary>
        /// 允许登录
        /// </summary>
        public void Waitinglogin(string s)
        {
            for (int i = 0; i < 1; )
            {
                s = HttpHelps.Get(Encoding.UTF8, "https://wx.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&uuid=" + uuid + "&tip=0&r=-757158328&_=1491110788887", ref cookie);
                uri = function.middlestring(s.ToString(), "redirect_uri=\"", "\"");
                if (uri.IndexOf("qq.com") != -1)
                {
                    domain = "http://wx2.qq.com";
                    heartbeat = "https://webpush.wx2.";
                    file = "https://file.wx2.qq.com";
                    Err = "https://wx2.qq.com";
                    if (uri.IndexOf("https://wx.qq.com") != -1)
                    {
                        domain = "http://wx.qq.com";
                        heartbeat = "https://webpush.wx.";
                        file = "https://file.wx.qq.com";
                        Err = "https://wx.qq.com";
                    }
                    i = i + 1;
                }
                Thread.Sleep(500);
            }
            //开始获取登陆秘钥

            Thread th = new Thread(new ThreadStart(delegate { GetKey(uri); }));
            th.IsBackground = true;
            th.Start();
            //手机确认登陆事件
            if (cl != null)
                cl();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public void GetKey(string ur)
        {
            string s = HttpHelps.Get(Encoding.UTF8, ur.Replace("https", "http"), ref cookie);
            if (s.IndexOf("<error><ret>1203</ret>") != -1)
            {
                MessageBox.Show(function.middlestring(s, "<message>", "</message>"));
                return;
            }
            skey = function.middlestring(s.ToString(), "<skey>", "</skey>");
            wxsid = function.middlestring(s.ToString(), "<wxsid>", "</wxsid>");
            wxuin = function.middlestring(s.ToString(), "<wxuin>", "</wxuin>");
            pass_ticket = function.middlestring(s.ToString(), "<pass_ticket>", "</pass_ticket>");
            OfflineMessage();
        }

        /// <summary>
        /// 获取群列表
        /// </summary>
        public void GETgroup()
        {

            List<GROUP> _grox = new List<GROUP>();

            int i = 0;
            while (true)
            {
                string dtG = "";
                int x = 0;
                for (; i < Group.Count; i++)
                {
                    dtG += "{\"UserName\":\"" + Group[i] + "\",\"EncryChatRoomId\":\"\"},";
                    x++;
                    if (x == 50)
                        break;
                }
                i++;
                if (x == 0)
                    break;

                dtG = "\"Count\":" + x.ToString() + ",\"List\":[" + dtG;

                dtG = dtG.Substring(0, dtG.Length - 1);
                dtG += "]";
                string uriq = domain + "/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&lang=zh_CN&pass_ticket=" + pass_ticket + "&r=" + function.MilliTime();
                string Date = "{\"BaseRequest\":{\"Uin\":\"" + wxuin + "\",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," + dtG + "}";
                string s = HttpHelps.Post(Date, uriq, ref cookie, Encoding.UTF8);

                foreach (JObject jp in JObject.Parse(s)["ContactList"])
                {
                    GROUP gro = new GROUP();
                    gro.UserName = jp["UserName"].ToString();
                    gro.NickName = jp["NickName"].ToString();
                    gro.HeadImgUrl = jp["HeadImgUrl"].ToString();
                    gro.seq = jp["PYInitial"].ToString().Replace("?", "");
                    //gro.seq = function.middlestring(jp["HeadImgUrl"].ToString(), "seq=", "&");
                    //seq = 662208698 &
                    gro.MemberList = new List<GROUP>();
                    int a = 0;
                    while (true)
                    {
                        string dtGs = "";
                        int o = 0;
                        for (; a < jp["MemberList"].Count(); a++)
                        {
                            dtGs += "{\"UserName\":\"" + jp["MemberList"][a]["UserName"].ToString() + "\",\"EncryChatRoomId\":\"" + gro.UserName + "\"},";
                            o++;
                            if (o == 50)
                                break;
                        }
                        a++;
                        if (o == 0)
                            break;
                        dtGs = dtGs.Substring(0, dtGs.Length - 1);
                        dtGs += "]";

                        dtGs = "\"Count\":" + o.ToString() + ",\"List\":[" + dtGs;

                        Date = "{\"BaseRequest\":{\"Uin\":\"" + wxuin + "\",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," + dtGs + "}";

                        gro.DATAlist.Add(Date);
                        string URR = domain + "/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&lang=zh_CN&pass_ticket=" + pass_ticket + "&r=" + function.MilliTime();

                        string xs = HttpHelps.Post(Date, URR, ref cookie, Encoding.UTF8);
                        while (true)
                        {
                            string ls = function.middlestring(xs, "<span", "</span>");
                            if (ls != "")
                                xs = xs.Replace("<span" + ls + "</span>", "");
                            else
                                break;
                        }
                        foreach (JObject xxs in JObject.Parse(xs)["ContactList"])
                        {
                            ///保存群成员信息
                            GROUP g = new GROUP();
                            g.UserName = xxs["UserName"].ToString();
                            g.NickName = xxs["NickName"].ToString();
                            //
                            //===================2018-02===================
                            string shortName = "";// Regex.Replace(g.NickName, @"\[[^\]]*?\]", "");                           
                            shortName = function.filtetStingSpecial(g.NickName);
                            //===================2018-02===================


                            shortName = shortName.Replace("'", "").Replace("/", "").Replace("\\", "").Replace("\"", "").Replace(".", "");
                            bool isHanzi = false;
                            for (int ii = 0; ii < shortName.Length; ii++)
                            {
                                if ((int)shortName[ii] > 127)
                                {
                                    //是汉字
                                    isHanzi = true;
                                    break;
                                }
                            }
                            if (isHanzi)
                            {
                                shortName = shortName.Length > 2 ? shortName.Substring(0, 2) : shortName;
                            }
                            else
                            {
                                shortName = shortName.Length > 4 ? shortName.Substring(0, 4) : shortName;
                            }
                          
                            g.NickNameShort = shortName;
                            g.RemarkName = xxs["RemarkName"].ToString();
                            //g.seq = function.middlestring(xxs["HeadImgUrl"].ToString(), "seq=", "&") + xxs["AttrStatus"].ToString()+ xxs["PYInitial"].ToString();
                            g.seq = xxs["NickName"].ToString() + xxs["AttrStatus"].ToString();
                            g.seq = g.seq.Replace("?", "");
                            gro.MemberList.Add(g);
                        }
                    }
                    gro.URLlist = domain + "/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&lang=zh_CN&pass_ticket=" + pass_ticket + "&r=" + function.MilliTime();

                    _grox.Add(gro);
                }
            }

            grox = _grox;
        }

        /// <summary>
        /// 获取群成员列表
        /// </summary>
        public List<GROUP> GETgrouplist(List<string> Date, string uriq)
        {
            List<GROUP> gro = new List<GROUP>();
            foreach (string dt in Date)
            {
                string xs = HttpHelps.Post(dt, uriq, ref cookie, Encoding.UTF8);
                foreach (JObject xxs in JObject.Parse(xs)["ContactList"])
                {
                    ///保存群成员信息
                    GROUP g = new GROUP();
                    g.UserName = xxs["UserName"].ToString();
                    g.NickName = xxs["NickName"].ToString();
                    g.RemarkName = xxs["RemarkName"].ToString();
                    gro.Add(g);
                }
            }
            return gro;
        }

        //所有的群组好友标识
        public List<GROUP> grox;

        //群组列表标识
        private List<string> Group = new List<string>();

        /// <summary>
        /// 获取离线消息推送
        /// </summary>
        public void OfflineMessage()
        {
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxinit?pass_ticket=" + pass_ticket + "&r=" + function.MilliTime() + "&skey=" + skey;
            string Date = "{\"BaseRequest\":{\"Uin\":\"" + wxuin + "\",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}}";
            string s = HttpHelps.Post(Date, uri, ref cookie, Encoding.UTF8);
            //cookie = Regex.Replace(cookie, "^[Domain].*[(GMT)|(GMT,)]$", "");

            cookie = function.Deletetext(cookie, "Domain", "GMT").Replace("Secure,", null);
            webwx_data_ticket = "wxuin=" + wxuin + "; wxsid=" + wxsid + ";  webwx_data_ticket=" + function.middlestring(cookie, "webwx_data_ticket=", ";") + ";";

            Offline = JObject.Parse(s);

            //获取本号参数
            UserName = Offline["User"]["UserName"].ToString();
            NickName = Offline["User"]["NickName"].ToString();
            HeadImage = function.CreateRoundRectImage(function.SoftenImage(new Bitmap(HttpHelps.GetPicture(domain + Offline["User"]["HeadImgUrl"].ToString(), cookie))));
            session = Offline["ContactList"];
            JToken record = Offline["SyncKey"];

            //获取POST秘钥
            SkeyPost = record.ToString();
            //获取GET秘钥
            string str = "";
            foreach (JObject jp in record["List"])
            {
                str = str + jp["Key"].ToString() + "_";
                str = str + jp["Val"].ToString() + "|";
            }
            SkeyGet = str.Substring(0, str.Length - 1);
            FriendsList(true);
        }

        //计算DeviceID秘钥
        public string DeviceID()
        {
            string s = "";
            Random rn = new Random();
            for (int i = 0; i < 15; i++)
            {
                s = s + rn.Next(10).ToString();
            }
            return "e" + s;
        }

        /// <summary>
        /// 委托   加载好友
        /// </summary>
        public delegate void listhy(DataTable dt);

        public event listhy jzhy;

        private DataTable friends = new DataTable();

        /// <summary>
        /// 好友列表
        /// </summary>
        public void FriendsList(bool b)
        {
            friends = new DataTable();
            friends.Columns.Add("UserName");
            friends.Columns.Add("NickName");
            friends.Columns.Add("HeadImgUrl");
            friends.Columns.Add("RemarkName");

            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetcontact?&pass_ticket=" + pass_ticket + "&r=" + function.MilliTime() + "&seq=0&skey=" + skey;
            uri = uri.Replace("http", "https");
            while (true)
            {
                string s = HttpHelps.GetQrh(uri, cookie);

                //删减无用HTML
                while (true)
                {
                    string ls = function.middlestring(s, "<span", "</span>");
                    if (ls != "")
                        s = s.Replace("<span" + ls + "</span>", "");
                    else
                        break;
                }

                Friends = JObject.Parse(s);
                JToken record = Friends["MemberList"];
                //循环加载好友
                Group = new List<string>();
                foreach (JObject jp in record)
                {
                    if (jp["VerifyFlag"].ToString() == "0")
                    {
                        DataRow dr = friends.NewRow();
                        dr[0] = jp["UserName"];
                        dr[1] = jp["NickName"];
                        dr[2] = jp["HeadImgUrl"];
                        dr[3] = jp["RemarkName"];
                        friends.Rows.Add(dr);
                        //头像获取
                        //Image HeadImage =new Bitmap(HttpHelps.GetPicture(domain + jp["HeadImgUrl"].ToString(), cookie));
                        //这里往控件加载头像和好友信息
                    }

                    if (jp["UserName"].ToString().IndexOf("@@") != -1)
                    {
                        Group.Add(jp["UserName"].ToString());
                    }
                }
                if (function.middlestring(s, "Seq\": ", "\n}") == "0")
                { break; }
                else
                {
                    uri = uri.Replace("seq=" + function.middlestring(uri, "seq=", "&") + "&", "seq=" + function.middlestring(s, "Seq\": ", "\n}") + "&");
                }
            }
            GETgroup();

            //数据回调函数
            if (jzhy != null)
                jzhy(friends);
            if (!b)
                return;
            //单线程启动心跳
            var thread = new Thread(FeartBeat);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            var th = new Thread(xh);
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start();
        }

        public bool jieshu = true;

        /// <summary>
        /// 心跳
        /// </summary>
        public void FeartBeat()
        {
            while (jieshu)
            {
                string uri = heartbeat + "qq.com/cgi-bin/mmwebwx-bin/synccheck?r=" + function.MilliTime() + "&skey=" + skey + "&sid=" + wxsid.Replace("+", "%2B") + "&uin=" + wxuin + "&deviceid=" + DeviceID() + "&synckey=" + SkeyGet + "&_=" + tim.ToString();
                tim++;
                string s = HttpHelps.GetQrh(uri, webwx_data_ticket);
                /*string retcode = function.middlestring(s, "retcode:\"", "\"");
                string selector = function.middlestring(s, "selector:\"", "\"");

                if (retcode == "0")
                {
                    if (int.Parse(selector) > 0)
                    {
                        //消息获取
                        BeingPushed(retcode, selector);
                    }
                }*/
                Thread.Sleep(1000);
            }
        }

        private void xh()
        {
            while (jieshu)
            {
                BeingPushed("0", "0");
                function.yanci(1000);
            }
        }

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="retcode"></param>
        /// <param name="selector"></param>
        public void huidiao(string FromUserName, string ToUserName)
        {
            string ls = cookie;
            string sjc = function.MilliTime() + "3568";
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxstatusnotify?lang=zh_CN&pass_ticket=" + pass_ticket;
            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}" +
               "\"Code\":1,\"FromUserName\":\"" + FromUserName + "\",\"ToUserName\":\"" + ToUserName + "\",\"ClientMsgId\":\"" + sjc + "\"}}";

            Date = Date.Replace("\r\n", "").Replace("  ", "");
            string s = HttpHelps.Post(Date, uri, ref ls, Encoding.UTF8);

        }

        private string sing;

        /// <summary>
        /// 查看消息
        /// </summary>
        /// <param name="job"></param>
        public delegate void JObjectEventHandler(string retcode, string selector, JObject job);

        public event JObjectEventHandler job;

        /// <summary>
        /// 查看消息
        /// </summary>
        public void BeingPushed(string retcode, string selector)
        {
            string ls = cookie;
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxsync?sid=" + wxsid + "&skey=" + skey + "&lang=zh_CN&pass_ticket=" + pass_ticket;
            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"},\"SyncKey\":" + SkeyPost + ",\"rr\":23522981}";
            Date = Date.Replace("\r\n", "").Replace("  ", "");
            string s = "";
            s = HttpHelps.Post(Date, uri, ref ls, Encoding.UTF8);
            if (s == "") return;
            Offline = JObject.Parse(s);
            if (Offline["AddMsgList"].ToString() == sing) return;
            sing = Offline["AddMsgList"].ToString();
            //消息处理
            if (job != null)
            {
                job(retcode, selector, Offline);
            }

            JToken record = Offline["SyncKey"];

            //设置POST所需秘钥
            SkeyPost = record.ToString();
            //设置GET所需秘钥
            string str = "";
            foreach (JObject jp in record["List"])
            {
                str = str + jp["Key"].ToString() + "_";
                str = str + jp["Val"].ToString() + "|";
            }
            if (str.Length > 1)
                SkeyGet = str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 加群
        /// </summary>
        /// <param name="retcode"></param>
        /// <param name="selector"></param>
        public void jiaqun(string username, string tousername)
        {
            string ls = cookie;
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxupdatechatroom?fun=invitemember";
            string Date = "{\"InviteMemberList\":\"" + username + "\",\"ChatRoomName\":\"" + tousername + "\",\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}}";
            Date = Date.Replace("\r\n", "").Replace("  ", "");
            string s = "";
            s = HttpHelps.Post(Date, uri, ref ls, Encoding.UTF8);
        }

        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="username"></param>
        public void beizhu(string UserName, string RemarkName)
        {
            string ls = cookie;
            string uri = domain.Replace("http", "https") + "/cgi-bin/mmwebwx-bin/webwxoplog?lang=zh_CN&pass_ticket=" + pass_ticket;
            string Date = "{\"UserName\":\"" + UserName + "\",\"CmdId\":2,\"RemarkName\":\"" + RemarkName + "\",\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}}";
            string s = HttpHelps.Post(Date, uri, ref ls, Encoding.UTF8);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Content"></param>
        public string SendMessage(string Content, string ToUserName)
        {
            //Content = Content.Replace("\\", "\\\\").Replace("\"", "\\\"");
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxsendmsg?pass_ticket=" + pass_ticket;
            string sjc = function.MilliTime() + "3568";
            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," +
"\"Msg\":{\"Type\":1,\"Content\":\"" + Content + "\",\"FromUserName\":\"" + UserName + "\",\"ToUserName\":\"" + ToUserName + "\",\"LocalID\":\"" + sjc + "\",\"ClientMsgId\":\"" + sjc + "\"}}";
            string x = function.middlestring(cookie, "wxloadtime=", ";");
            x = cookie.Replace(x, x + "_expired") + "wxpluginkey=" + function.MilliTime().Substring(0, 10) + ";";
            string s = HttpHelps.Post(Date, uri, ref x, Encoding.UTF8);
            return s;
        }

        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Content"></param>
        public string ErrMsg(string MsgID, string LocalID, string ToUserName)
        {
            string uri = Err + "/cgi-bin/mmwebwx-bin/webwxrevokemsg?lang=zh_CN&" + pass_ticket;
            string x = function.middlestring(cookie, "wxloadtime=", ";");
            x = cookie.Replace(x, x + "_expired") + "wxpluginkey=" + function.MilliTime().Substring(0, 10) + ";";
            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," +
"\"SvrMsgId\":\"" + MsgID + "\",\"ToUserName\":\"" + ToUserName + "\",\"ClientMsgId\":\"" + LocalID + "\"}";
            string s = HttpHelps.Post(Date, uri, ref cookie, Encoding.UTF8);
            return s;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="img"></param>
        public string send(Image img)
        {
            byte[] bt = function.ImageToBytes(img);
            string uploadmediarequest = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," +
                @"""ClientMediaId"":" + function.MilliTime() + ",\"TotalLen\":" + bt.Length.ToString() + ",\"StartPos\":0,\"DataLen\":" + bt.Length.ToString() + ",\"MediaType\":4}";
            Html html = new Html(function.getImageType(img), webwx_data_ticket, uploadmediarequest, pass_ticket);
            string date = html.getdate();
            string str = HttpHelps.HttpUploadFile1(file + "/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json", date, bt);
            return str;
        }

        /// <summary>
        /// 获取图片缩略图
        /// </summary>
        /// <param name="MsgID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Image GetImage(string MsgID, string type)
        {
            if (type == "slave")
                uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID=" + MsgID + "&skey=" + skey + "&type=slave";

            if (type == "slaveImage")
                uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID=" + MsgID + "&skey=" + skey;

            if (type == "location")
                uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetpubliclinkimg?url=xxx&msgid=" + MsgID + "&pictype=location";
            return HttpHelps.GetPicture(uri, cookie);
        }

        /// <summary>
        /// 获取原创表情
        /// </summary>
        /// <param name="MsgID"></param>
        /// <returns></returns>
        public Image GetOriginal(string MsgID)
        {
            Image img = HttpHelps.GetPicture(domain + "/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID=" + MsgID + "&skey=" + skey + "&type=big", cookie);
            return img;
        }

        /// <summary>
        /// 获取语音
        /// </summary>
        /// <param name="MsgID"></param>
        /// <returns></returns>
        public byte[] Getaudio(string MsgID)
        {
            uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetvoice?msgid=" + MsgID + "&skey=" + skey;
            byte[] bs64 = HttpHelps.GetAudio(uri, cookie);
            return bs64;
        }

        /// <summary>
        /// 获取视频
        /// </summary>
        /// <param name="MsgID"></param>
        /// <returns></returns>
        public string Getvideo(string MsgID)
        {
            uri = domain + "/cgi-bin/mmwebwx-bin/webwxgetvideo?msgid=" + MsgID + "&skey=" + skey;
            byte[] bs64 = HttpHelps.HttpDownloadFile(uri, cookie);
            //File.WriteAllBytes();
            return Convert.ToBase64String(bs64);
        }

        /// <summary>
        /// gif发送
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="MediaId"></param>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public string sendgif(string MediaId, string ToUserName)
        {
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxsendemoticon?fun=sys";
            string sjc = function.MilliTime() + "3568";

            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," +
"\"Msg\":{\"Type\":47,\"MediaId\":\"" + MediaId + "\",\"FromUserName\":\"" + UserName + "\",\"ToUserName\":\"" + ToUserName + "\",\"LocalID\":\"" + sjc + "\",\"ClientMsgId\":\"" + sjc + "\"}}";
            string x = function.middlestring(cookie, "wxloadtime=", ";");
            x = cookie.Replace(x, x + "_expired") + "wxpluginkey=" + function.MilliTime().Substring(0, 10) + ";";

            string s = HttpHelps.Post(Date, uri, ref x, Encoding.UTF8);
            return function.middlestring(s, "MsgID\": \"", "\"");
        }

        /// <summary>
        /// 图片发送
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="MediaId"></param>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public string sendImage(string MediaId, string ToUserName)
        {
            if (!ServerCommon.isSendMessage)
            {
                return "zzzz";
            }
            string uri = domain + "/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&lang=zh_CN&pass_ticket=" + pass_ticket;
            string sjc = function.MilliTime() + "3568";

            string Date = "{\"BaseRequest\":{\"Uin\":" + wxuin + ",\"Sid\":\"" + wxsid + "\",\"Skey\":\"" + skey + "\",\"DeviceID\":\"" + DeviceID() + "\"}," +
"\"Msg\":{\"Type\":3,\"MediaId\":\"" + MediaId + "\",\"FromUserName\":\"" + UserName + "\",\"ToUserName\":\"" + ToUserName + "\",\"LocalID\":\"" + sjc + "\",\"ClientMsgId\":\"" + sjc + "\"}}";
            string x = function.middlestring(cookie, "wxloadtime=", ";");
            x = cookie.Replace(x, x + "_expired") + "wxpluginkey=" + function.MilliTime().Substring(0, 10) + ";";

            return HttpHelps.Post(Date, uri, ref x, Encoding.UTF8);
        }
    }
}