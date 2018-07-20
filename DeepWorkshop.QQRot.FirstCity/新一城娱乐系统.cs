using AI.Bll;
using AI.Dal;
using Bll;
using Dal;
using DeepWorkshop.QQRot.FirstCity;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using DeepWorkshop.QQRot.FirstCity.Validate;
using Newbe.CQP.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using 新一城娱乐系统;
using 新一城娱乐系统.FeiPan;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public static string ShouQuanServer = "http://193.112.91.139";

        private WebWeChat _qrWebWeChat = null;
        public GroupInfo _group = null;
        //public GROUP _group = null;
        private guize _guiZe = new guize();

        private int _chengYuanShuLiang = 0;

        private int _zongJiFen = 0;

        private string _offLine;

        private bool _qiuDao1 = true;

        private bool _qiuDao2 = true;

        private bool _qiuDao3 = true;

        private bool _qiuDao4 = true;

        private bool _qiuDao5 = true;

        private bool _qiuDaoHe = true;

        private int _qiuDaosl = 5;

        private int _eDuMax = 0;

        private int _lsEDu = 0;

        private kjtj _kaiJiangData = new kjtj();


        private kjtj _shangQiKaiJiangData = new kjtj();


        private bool _fengPan = false;

        private DateTime _buShi = DateTime.Now.AddYears(-1);

        private bool _fengPanQian = false;

        private object obj = new object();

        private MouseEventArgs _aoEvent = null;

        //private bool _jianTin = false;

        private bool _isFeiPan = false;

        //结算是否进1
        private bool _isJieSuanJinYi = false;
        private bool _isJieSuanJinYi_zj = false;


        private bool _isFaSongXiaZhu = false;

        private feiPanJieGuo _feiDanJieGuoData = new feiPanJieGuo();

        public Thread _dgvThread;
        private int _feiDanFengPanMiao
        {

            get
            {

                if (ConfigHelper.GetAppConfig("FengPanFeiDan") == null)
                {
                    return 30;
                }
                else
                {
                    return int.Parse(ConfigHelper.GetAppConfig("FengPanFeiDan"));
                }
            }
        }

        public string BenQiQiHao
        {
            get { return textBox1.Text; }
        }


        public bool FengPan
        {
            get { return _fengPan; }
        }

        public bool IsFeiPan
        {
            get { return _isFeiPan; }
        }
        
        public Form1( GroupInfo qz, string title)
        {
            InitializeComponent();
            //初始化当前登陆的qq
            CacheData.LoginQQ = CacheData.CoolQApi.GetLoginQQ();
            CacheData.LoginNick = CacheData.CoolQApi.GetLoginNick();
            MyLogUtil.ToLogFotTest(CacheData.LoginQQ + "_" + CacheData.LoginNick);
            try
            {
                if (ConfigHelper.GetAppConfig("qd") != null)
                {
                    textBox29.Text = ConfigHelper.GetAppConfig("qd");
                    textBox30.Text = ConfigHelper.GetAppConfig("dq");
                    textBox31.Text = ConfigHelper.GetAppConfig("zh");
                    textBox32.Text = ConfigHelper.GetAppConfig("zhzh");
                    textBox33.Text = ConfigHelper.GetAppConfig("lhh");

                    checkBox4.Checked = ConfigHelper.GetAppConfig("账单zd").Equals("1");
                    checkBox5.Checked = ConfigHelper.GetAppConfig("封盘前zd").Equals("1");
                    checkBox6.Checked = ConfigHelper.GetAppConfig("封盘zd").Equals("1");
                    checkBox7.Checked = ConfigHelper.GetAppConfig("开奖zd").Equals("1");
                    checkBox8.Checked = ConfigHelper.GetAppConfig("下注zd").Equals("1");
                }
            }
            catch (Exception ex) { }
            foreach (lsxe ox in _guiZe.cshxe)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(ox.name);
                item.SubItems.Add(ox.gz);
                item.SubItems.Add(ox.xe.ToString());
                item.SubItems.Add(ox.id.ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView5.Items.Add(item);
            }
            this.Text = title;
            label8.Text = "当前操作群：" + qz.GroupName;
            _group = qz;
            //_qrWebWeChat = webchat;
            Control.CheckForIllegalCrossThreadCalls = false;

            //此方法已做适配，见MessageArrival(long fromGroup, long fromQq, string msg)
            //_qrWebWeChat.job += new WebWeChat.JObjectEventHandler(MessageArrival);

            状态栏.Text = "单击启动监听开始获取群消息！";

            //_group.MemberList = _qrWebWeChat.GETgrouplist(_group.DATAlist, _group.URLlist);
            _dgvThread = new Thread(dgv2);//获取群成员列表
            _dgvThread.Start();
            // GamePlayer
            timer1.Start();//开奖倒计时
            timer2.Start();//测试网速

            //更新记录期号加载
            DateTime time1 = DateTime.Now.Date;
            DateTime time2 = time1.AddDays(1);
            DataTable deset = SQLiteHelper.ExecuteDataTable("select 期号  from kaijiang_" + CacheData.Seq + " where Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + time2.ToString("yyyy-MM-dd 00:00:00") + "'", null);
            foreach (DataRow dr in deset.Rows)
            {
                comboBox5.Items.Add(dr[0].ToString());
            }


            //文本消息配置加载
            DataTable det = SQLiteHelper.ExecuteDataTable("select * from peizhi where Id=1", null);
            if (det.Rows.Count == 0)
            {
                SQL.INSERT(
"账单,封盘前,封盘,开奖,实时账单,下注,自定义1,自定义2,自定义3,自定义4,自定义5,倍数,最小,最大",
"'" + textBox12.Text + "','" + textBox13.Text + "|" + comboBox1.Text + "','" + textBox14.Text + "|" + comboBox6.Text + "','" + textBox15.Text +
"','" + textBox16.Text + "','" + textBox17.Text + "','" + textBox18.Text + "','" + textBox19.Text + "','" + textBox20.Text +
"','" + textBox21.Text + "','" + textBox22.Text + "','" + textBox24.Text + "','" + textBox25.Text + "','" + textBox26.Text + "'"
                , "peizhi");
            }
            if (det.Rows.Count == 1)
            {
                try
                {
                    textBox12.Text = det.Rows[0]["账单"].ToString();

                    textBox13.Text = det.Rows[0]["封盘前"].ToString().Split('|')[0];
                    textBox14.Text = det.Rows[0]["封盘"].ToString();

                    textBox15.Text = det.Rows[0]["开奖"].ToString();
                    textBox16.Text = det.Rows[0]["实时账单"].ToString();
                    textBox17.Text = det.Rows[0]["下注"].ToString();

                    textBox18.Text = det.Rows[0]["自定义1"].ToString();
                    textBox19.Text = det.Rows[0]["自定义2"].ToString();
                    textBox20.Text = det.Rows[0]["自定义3"].ToString();
                    textBox21.Text = det.Rows[0]["自定义4"].ToString();
                    textBox22.Text = det.Rows[0]["自定义5"].ToString();

                    textBox24.Text = det.Rows[0]["倍数"].ToString();
                    textBox25.Text = det.Rows[0]["最小"].ToString();
                    textBox26.Text = det.Rows[0]["最大"].ToString();
                    //修复数组越界，表中首条数据只有账单字段有值，所以这些字段无需做此分割操作
                    comboBox1.Text = String.IsNullOrWhiteSpace(det.Rows[0]["封盘前"].ToString())?"":det.Rows[0]["封盘前"].ToString().Split('|')[1];
                    comboBox6.Text = String.IsNullOrWhiteSpace(det.Rows[0]["封盘"].ToString()) ?"":det.Rows[0]["封盘"].ToString().Split('|')[1];
                }
                catch (Exception ex) {
                    MyLogUtil.ErrToLog("初始化主窗口出错，原因："+ex);
                    MessageBox.Show(ex.Message);
                }
            }

            MainPlugin.frmMain = this;

        }

        public delegate void AddHandler(string a, GROUP b, string MsgId);


        ///正在修改
        /// <summary>
        /// 组装博彩信息到群成员列表，并展示(此方法成立的前提是已经获取了群成员列表)
        /// </summary>
        public void dgv2()
        {
            try { 
            lvChengYuanJiFen.Items.Clear();
            if (_group == null) return;
            DataTable dt = SQL.SELECTdata("", " Friends_" + CacheData.Seq);
            //2017-05-04 08:45:40
            DateTime time1 = DateTime.Now.Date;
            DateTime time2 = time1.AddDays(1);
            DataTable deset = SQLiteHelper.ExecuteDataTable("select seq,sum(盈亏)  from NameInt_" + CacheData.Seq + " where Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + time2.ToString("yyyy-MM-dd 00:00:00") + "' group by seq", null);

            foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = jp.Seq;
                item.SubItems.Add(jp.GroupMemberBaseInfo.NickName);
                DataRow[] dR = dt.Select("seq='" + jp.Seq + "'");
                DataRow dr = dt.NewRow();
                if (dR.Length == 0)
                {
                    string nickname = jp.GroupMemberBaseInfo.NickName.Replace(",", "").Replace("/", "");
                    //注意昵称中包含很多特殊字符，这里昵称中的非汉字字符下划线过滤掉以便存储到数据库
                    SQL.INSERT("seq,NickName,是否入局,现有积分,总盈亏,总下注", "'" + jp.Seq + "','" + MyRegexUtil.RemoveSpecialCharacters(nickname) + "','否','0','0','0'", " Friends_" + CacheData.Seq);

                    dr[4] = "否";
                    dr[5] = "0";
                    dr[6] = "0";
                    // dr = SQL.SELECTdata(" WHERE seq='" + jp.seq + "'", 1, _group.seq).Rows[0];
                }
                else
                {
                    dr = dR[0];
                }
                MyLogUtil.ToLogFotTest("查看此用户："+jp.GroupMemberBaseInfo.Number+"的数据库存储的备注"+ dr["本地备注"].ToString());
                try
                {
                    item.SubItems.Add(dr["本地备注"].ToString());
                }
                catch (Exception ex)
                {
                    MyLogUtil.ToLogFotTest("看看本地备注名称"+ jp.RemarkName);
                    item.SubItems.Add(jp.RemarkName);//备注
                }
                item.SubItems.Add(jp.GroupMemberBaseInfo.NickName);

                item.SubItems.Add(dr[3].ToString());//推荐人
                item.SubItems.Add(dr[4].ToString());//是否入局
                if (dr[4].ToString() == "是")
                    jp.sfrj = true;

                jp.bendibeizhu = dr["本地备注"].ToString();

                item.SubItems.Add(dr[5].ToString());//积分
                jp.zongjifen = int.Parse(dr[5].ToString());

                _zongJiFen = _zongJiFen + int.Parse(dr[5].ToString());
                item.SubItems.Add(dr[6].ToString());//总战绩
                try//今日战绩
                {
                    jp.zongxiazhu = int.Parse(dr[7].ToString());
                }
                catch (Exception ex) { jp.zongxiazhu = 0; }

                try//今日战绩
                {
                    item.SubItems.Add(deset.Select("seq='" + jp.Seq + "'")[0][1].ToString());
                }
                catch (Exception ex) { item.SubItems.Add("0"); }

                item.SubItems.Add("");//本期下注
                item.SubItems.Add(jp.Seq);
                item.SubItems.Add(_chengYuanShuLiang.ToString());
                item.SubItems.Add(""+jp.GroupMemberBaseInfo.Number);//多加一个字段，代表此会员的qq号 index=11
                item.SubItems.Remove(item.SubItems[0]);
                //if (jp.UserName == _qrWebWeChat.UserName)
                //   continue;
                lvChengYuanJiFen.Items.Add(item);
                jp.Id = _chengYuanShuLiang;
                _chengYuanShuLiang++;
            }
            label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + _zongJiFen.ToString();
            }catch(Exception ex)
            {
                MessageBox.Show("加载群成员列表失败，原因：" + ex.Message);//待删
                MyLogUtil.ErrToLog("加载群成员列表失败，原因："+ex);
            }


        }

        /// 废弃
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="retcode"></param>
        /// <param name="selector"></param>
        /// <param name="Offline"></param>
        private void MessageArrival(string retcode, string selector, JObject Offline)
        {
            
            if (_offLine == Offline.ToString())
            {
                return;
            }
            
            /*
                        _offLine = Offline.ToString();
                        foreach (JObject msg in Offline["AddMsgList"])
                        {
                            //获取消息的基本参数
                            string FromUserName = msg["FromUserName"].ToString();
                            string ToUserName = msg["ToUserName"].ToString();

                            GROUP cy = null;
                            string Content = msg["Content"].ToString();
                            string MsgId = msg["MsgId"].ToString();
                            string MsgType = msg["MsgType"].ToString();
                            if (FromUserName == _group.UserName)
                            {
                                string usnm = "@" + function.middlestring(Content, "@", ":<br/>");
                                cy = getname(usnm);
                                Content = msg["Content"].ToString().Replace(usnm + ":<br/>", "").Replace("\n", "");
                            }
                            if (ToUserName == _group.UserName)
                                cy = getname(FromUserName);
                            if (cy == null || !_jianTin)
                                continue;
                            //下面处理消息类型
                            if (MsgType == "1" || MsgType == "10000")
                            {
                                //
                                if (ServerCommon.isLogWechat)
                                {
                                    function.logWx("收到消息: " + Content);
                                }

                                //
                                string SubMsgType = msg["SubMsgType"].ToString();
                                //接收到好友纯文本消息
                                if (SubMsgType == "0" || SubMsgType == "10000")
                                {
                                    jzxx(cy, Content, MsgId);
                                    xxcl(Content.Trim(), cy, MsgId);
                                    //new AddHandler(xxcl).BeginInvoke(Content.Trim(), cy, MsgId, null, null);
                                }

                            }
                            //图片
                            if (MsgType == "3")
                                jzxx(cy, "[图片]", MsgId);
                            //语音
                            if (MsgType == "34")
                                jzxx(cy, "[语音]", MsgId);
                            //视频
                            if (MsgType == "43")
                                jzxx(cy, "[视频]", MsgId);
                            //原创表情
                            if (MsgType == "47")
                                jzxx(cy, "[表情]", MsgId);


                            //告诉服务器  我收到了消息
                            _qrWebWeChat.huidiao(ToUserName, FromUserName);
                        }
                        */

        }

        /// <summary>
        /// 接收的群消息处理（QQ版本）
        /// </summary>
        /// <param name="retcode"></param>
        /// <param name="selector"></param>
        /// <param name="Offline"></param>
        public void MessageArrival(long fromGroup, long fromQq, string msg)
        {
            //当被监听且是选中的qq群时候，才会收集信息
            if (CacheData.IsJianTing&& fromGroup==_group.GroupId)
            {
                MessageInfo message = MyMessageUtil.ConvertMessage(msg);
                if (message.MessageType == 1)//普通纯文本消息
                {
                    //
                    if (ServerCommon.isLogWechat)
                    {
                        function.logWx("收到消息: " + msg);
                    }


                    jzxx(_group, fromQq, message.MessageContent, "-9998");//由于没调用酷q方法，没有返回值
                    xxcl(msg.Trim(), _group, fromQq, "-9998");//由于没调用酷q方法，没有返回值
                                                  //new AddHandler(xxcl).BeginInvoke(Content.Trim(), cy, MsgId, null, null);

                }
                else//含图片或语音或视频或表情的消息
                {
                    jzxx(_group, fromQq, message.MessageContent, "-9998");//由于没调用酷q方法，没有返回值
                }
            }
            

        }
        ///正在改
        ///注意：此方法中_group 代表群 gr应该代表群员
        /// <summary>
        /// 收到群里用户的纯文本消息开始分析是否下单，上分等操作
        /// </summary>
        /// <param name="conter"></param>
        /// <param name="gr"></param>
        /// <param name="msgid"></param>
        private void xxcl(string conter, GroupInfo gr,long fromQQ, string msgid)
        {
            //当前说话的qq的详情
            GroupMemberInfoWithBocai groupMember = CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, fromQQ);
            if (conter.IndexOf("上") == 0 || conter.IndexOf("下") == 0 || conter.IndexOf("回") == 0)
            {
                playPointSound();
            }

            if (conter == "撤销" || conter == "取消")
            {
                string str = CoolQCode.At(fromQQ);//酷q中@某人
                if (!groupMember.sfrj)
                {
                    str += " 请先入局！";
                    
                    send(gr.GroupId, str);//酷q发送群消息
                    jzxx(_group, str, msgid);
                    return;
                }
                if (_fengPan == false)
                {
                    //
                    int benQixiaZhu = groupMember.benqixiazhu;

                    groupMember.conter = "";
                    groupMember.zongjifen += groupMember.benqixiazhu;
                    _eDuMax -= groupMember.benqixiazhu;
                    groupMember.benqixiazhu = 0;
                    groupMember.xiazhutongji = new xztj();
                    lvChengYuanJiFen.Items[groupMember.Id].SubItems[8].Text = "";

                    lvChengYuanJiFen.Items[groupMember.Id].SubItems[5].Text = groupMember.zongjifen.ToString();
                    str += " 撤单成功！";//+ gr.zongjifen.ToString();

                    //
                    _zongJiFen += groupMember.benqixiazhu;
                    label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + _zongJiFen.ToString();

                }
                else
                {
                    str += " 已锁盘，取消失败！";
                }
                
                send(gr.GroupId, str);//酷q发送群消息
                jzxx(_group, str, msgid);


            }
            if (conter == "查")
            {
                if (!groupMember.sfrj)
                {
                    string st = CoolQCode.At(fromQQ);//酷q中@某人
                    st += " 请先入局！";
                    send(gr.GroupId, st);//酷q发送群消息
                    jzxx(_group, st, msgid);
                    return;
                }
                string str = CoolQCode.At(fromQQ) + "\n余量：" + groupMember.zongjifen.ToString() + "\n当前攻击:" + groupMember.conter;
                send(gr.GroupId, str);//酷q发送群消息
                jzxx(_group, str, msgid);
                //查分
                return;
            }
            if (conter == "查流水" && cbChaLiuShui.Checked)
            {
                if (!groupMember.sfrj)
                {
                    string st = CoolQCode.At(fromQQ);//酷q中@某人
                    st += " 请先入局！";
                    send(gr.GroupId, st);//酷q发送群消息
                    jzxx(_group, st, msgid);
                    return;
                }
                //
                DateTime time1 = DateTime.Now.Date;
                DateTime time2 = time1.AddDays(1);
                string strSql = "select sum(实际下注),sum(实际中奖) from liushuiAct_" + CacheData.Seq
                    + " where Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + time2.ToString("yyyy-MM-dd 00:00:00")
                    + "' and seq='" + groupMember.Seq
                    + "' group by seq";
                DataTable dtAct = SQLiteHelper.ExecuteDataTable(strSql, null);


                string strSql2 = "select 期号,实际中奖 from liushuiAct_" + CacheData.Seq
                 + " where 实际中奖>0 and Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '"
                 + time2.ToString("yyyy-MM-dd 00:00:00")
                 + "' and seq='" + groupMember.Seq + "'"
                 ;
                DataTable dtAct2 = SQLiteHelper.ExecuteDataTable(strSql2, null);

                //
                string sjxz = "";
                string sjzj = "";
                string xdcg = "";
                foreach (DataRow row in dtAct2.Rows)
                {
                    xdcg += row[0] + "，+" + row[1] + "\n";
                }
                //
                if (dtAct.Rows.Count > 0)
                {
                    sjxz = dtAct.Rows[0][0].ToString();
                    sjzj = dtAct.Rows[0][1].ToString();
                }
                //
                string str = string.Format("{0} {1}：\n【使用分数：{2}】\n【剩余：{3}】\n【今日得分：{4}】\n【今日流水：{5}】\n【下单成功期：】\n{6}",
                        CoolQCode.At(fromQQ),
                        DateTime.Now.ToString("MM-dd HH:mm:ss"),
                       groupMember.benqixiazhu,//本期下注
                       groupMember.zongjifen,
                       sjzj,
                       sjxz,
                       xdcg);

                send(gr.GroupId, str);//酷q发送群消息
                jzxx(_group, str, msgid);


                //查分
                return;
            }
            if (conter.IndexOf("查") == 0 || conter.IndexOf("上") == 0)
            {
                if (!groupMember.sfrj)
                {
                    string st = CoolQCode.At(fromQQ);//酷q中@某人
                    st += " 请先入局！";
                    send(gr.GroupId, st);//酷q发送群消息
                    jzxx(_group, st, msgid);
                    return;
                }
                int result = 0;
                if (int.TryParse(conter.Replace("查", "").Replace("上", "").Replace("分", ""), out result) == true)
                {
                    if (result != 0)
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(groupMember.GroupMemberBaseInfo.NickName);
                        item.SubItems.Add(groupMember.RemarkName);
                        item.SubItems.Add(groupMember.Id.ToString());
                        item.SubItems.Add("QQ");
                        item.SubItems.Add(result.ToString());
                        item.SubItems.Remove(item.SubItems[0]);
                        listView3.Items.Add(item);
                    }
                }
                //上分
                return;
            }
            if (conter == "回" || conter == "下")
            {
                if (!groupMember.sfrj)
                {
                    string st = CoolQCode.At(fromQQ);//酷q中@某人
                    st += " 请先入局！";
                    send(gr.GroupId, st);//酷q发送群消息
                    jzxx(_group, st, msgid);
                    return;
                }
                if (groupMember.zongjifen > 0)
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(groupMember.GroupMemberBaseInfo.NickName);
                    item.SubItems.Add(groupMember.RemarkName);
                    item.SubItems.Add(groupMember.Id.ToString());
                    item.SubItems.Add("QQ");
                    item.SubItems.Add(groupMember.zongjifen.ToString());
                    item.SubItems.Remove(item.SubItems[0]);
                    listView4.Items.Add(item);
                }
                else
                {
                    string messageId = send(gr.GroupId, CoolQCode.At(fromQQ) + " 能量不足，拒绝下分");//酷q发送群消息
                    //jzxx(getname(_qrWebWeChat.UserName), CoolQCode.At(fromQQ) + " 能量不足，拒绝下分", ""+messageId);
                    jzxx(gr, CoolQCode.At(fromQQ) + " 能量不足，拒绝下分",  messageId);
                }
                return;
            }
            if (conter.IndexOf("回") == 0 || conter.IndexOf("下") == 0)
            {
                if (!groupMember.sfrj)
                {
                    string st = CoolQCode.At(fromQQ);
                    st += " 请先入局！";
                    //send(st, _group.UserName);
                    send(gr.GroupId, st);//酷q发送群消息
                    jzxx(_group, st, msgid);
                    return;
                }
                int result;
                if (int.TryParse(conter.Replace("回", "").Replace("分", "").Replace("下", ""), out result) == true)
                {
                    if (result != 0)
                    {
                        if (result > groupMember.zongjifen)
                        {
                            string messageIdTemp = send(gr.GroupId, CoolQCode.At(fromQQ) + " 能量不足，拒绝下分");//酷q发送群消息
                            jzxx(_group, CoolQCode.At(fromQQ) + " 能量不足，拒绝下分",  messageIdTemp);
                            return;
                        }
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(groupMember.GroupMemberBaseInfo.NickName);
                        item.SubItems.Add(groupMember.RemarkName);
                        item.SubItems.Add(groupMember.Id.ToString());
                        item.SubItems.Add("QQ");
                        item.SubItems.Add(result.ToString());
                        item.SubItems.Remove(item.SubItems[0]);
                        listView4.Items.Add(item);
                    }
                }
                //下分首字是
                //球道、单球（单、双、大、小）、总和（单、双、大、小）、总和组合（大单、大双、小单、小双）、龙、虎、合
                return;
            }
            if (!groupMember.sfrj)
                return;

            if (_qiuDaosl != 0 && _fengPan == false)
            {
                MyLogUtil.ToLogFotTest("下注之前的数据："+conter);
                conter = conter.Replace("/ ", "/")
                            .Replace("/  ", "/")
                            .Replace("/  ", "/")
                            .Replace("/   ", "/")
                            .Replace(" /", "/")
                            .Replace("  /", "/")
                            .Replace("  /", "/")
                            .Replace("  /", "/");
                MyLogUtil.ToLogFotTest("下注之前准备用空格分隔的数据：" + conter);
                if (xiazhu(conter.Split(' '), gr, groupMember) == 1)
                {
                    string messageIdTemp = send(gr.GroupId, CoolQCode.At(fromQQ) + "不满足攻击条件");//酷q发送群消息
                    jzxx(_group, CoolQCode.At(fromQQ) + "不满足攻击条件", ""+messageIdTemp);
                }
            }
            
            
        }

        ///正在改
        /// <summary>
        /// 增加下注
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="x">数据表（指定球道中的几号球下注）</param>
        /// <param name="y">下注积分积分</param>
        /// <returns></returns>
        private bool add(GroupInfo gr,GroupMemberInfoWithBocai groupMember, ref int x, int y, int id)
        {
            ///groupMember.lszjf 49
            ///groupMember.lsxzjf = 0;
            ///此时的下注积分y为5
            ///此时的配置
            ///textBox24 下注积分必须为此值的倍数 1
            ///textBox25 最小下注额 5
            ///textBox26 最大下注额 100000
            int totalXiazu = groupMember.benqixiazhu;

            if (_guiZe.cshxe[id].dqxz > _guiZe.cshxe[id].xe)
                return false;
            if (y % int.Parse(textBox24.Text) != 0)//整数倍
            {
                return false;
            }
            if (int.Parse(textBox25.Text) > y)//最小额度
            {
                return false;
            }
            //此代码废弃
            if (int.Parse(textBox26.Text) <= _eDuMax)//最大额度 totalXiazu+y   每次下注成功后这个_eDuMax才会更新，所以第一次没有控制住
            {
                return false;
            }
            if (groupMember.lszjf < 1)
                return false;
            if (groupMember.lszjf < y)
            {
                lock (obj)
                {
                    _zongJiFen -= groupMember.lszjf;
                    _lsEDu += groupMember.lszjf;
                    _guiZe.cshxe[id].dqxz += groupMember.lszjf;
                }
                x += groupMember.lszjf;
                groupMember.lsxzjf += groupMember.lszjf;
                groupMember.lszjf = 0;
            }
            else
            {
                lock (obj)
                {
                    _zongJiFen -= y;
                    _lsEDu += y;
                    _guiZe.cshxe[id].dqxz += y;
                }
                x += y;
                groupMember.lsxzjf += y;
                groupMember.lszjf -= y;
            }
            
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contstring"></param>
        /// <param name="gr"></param>
        /// <returns></returns>
        private int xiazhu(string[] contstring, GroupInfo gr,GroupMemberInfoWithBocai groupMember)
        {
            MyLogUtil.ToLogFotTest("测试下注，需要的模拟数据，groupMember.zongjifen:" + groupMember.zongjifen + "___groupMember.benqixiazhu:" + groupMember.benqixiazhu);
            xztj lsxz = new xztj();
            try
            {
                string con = "";
                groupMember.lszjf = groupMember.zongjifen;
                groupMember.lsxzjf = 0;
                _lsEDu = 0;
                foreach (string xz in contstring)
                {
                    string[] fz = xz.Split('/');

                    if (con.Length > 0)
                    {
                        con += ";";
                    }

                    #region 大小单双

                    if (fz.Length == 2)
                    {
                        // 单/20
                        int wz = -1;
                        if (fz[0] == "大")
                            wz = 0;
                        if (fz[0] == "小")
                            wz = 1;
                        if (fz[0] == "单")
                            wz = 2;
                        if (fz[0] == "双")
                            wz = 3;

                        if (wz != -1)
                        {
                            if (_qiuDaoHe)
                            {
                                if (!add(gr, groupMember, ref  lsxz.ZHDXDS[wz], int.Parse(fz[1]), 14 + wz))
                                    return 1;
                                con += xz;
                                continue;
                            }
                            if (_qiuDaosl == 1)
                            {
                                if (_qiuDao1)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[0, wz], int.Parse(fz[1]), wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao2)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[1, wz], int.Parse(fz[1]), wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao3)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[2, wz], int.Parse(fz[1]), wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao4)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[3, wz], int.Parse(fz[1]), wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao5)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[4, wz], int.Parse(fz[1]), wz)) return 1;
                                }
                                else
                                    return 5;
                                con += xz;
                                continue;
                            }
                        }

                        //   大单/20
                        wz = -1;
                        if (fz[0] == "大单")
                            wz = 0;
                        if (fz[0] == "大双")
                            wz = 1;
                        if (fz[0] == "小单")
                            wz = 2;
                        if (fz[0] == "小双")
                            wz = 3;
                        if (wz != -1 && _qiuDaoHe)
                        {
                            if (!add(gr, groupMember, ref  lsxz.ZHZHDXDS[wz], int.Parse(fz[1]), 18 + wz)) return 1;
                            con += xz;
                            continue;
                        }
                        // 0123456789/20
                        if (int.TryParse(fz[0], out wz))
                        {
                            if (_qiuDaosl == 5)
                                continue;
                            for (int i = 0; i < fz[0].Length; i++)
                            {
                                wz = int.Parse(fz[0].Substring(i, 1));
                                if (_qiuDao1)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.QD[0, wz], int.Parse(fz[1]), 4 + wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao2)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.QD[1, wz], int.Parse(fz[1]), 4 + wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao3)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.QD[2, wz], int.Parse(fz[1]), 4 + wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao4)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.QD[3, wz], int.Parse(fz[1]), 4 + wz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao5)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.QD[4, wz], int.Parse(fz[1]), 4 + wz)) return 1;
                                }
                                else
                                    return 5;
                            }
                            con += xz;
                            continue;
                        }
                    }

                    #endregion 大小单双

                    #region 单球
                    ///13/13579/5 分割后 数组的长度为3
                    if (fz.Length == 3)
                    {
                        //判断此次下注是否超过限额
                        if (fz[0].Length * fz[1].Length * int.Parse(fz[2]) > int.Parse(textBox26.Text))
                        {
                            string msgid = send(gr.GroupId, CoolQCode.At(groupMember.GroupMemberBaseInfo.Number) + "失败，不符合攻击条件！");//酷q发送群消息                                                                                             //if (msgid != "")
                            jzxx(_group, CoolQCode.At(groupMember.GroupMemberBaseInfo.Number) + "失败，不符合攻击条件！", msgid);
                            continue;
                        }


                        int wz = -1;
                        // 万2/0123456789/20
                        if (int.TryParse(fz[1], out wz))
                        {
                            ///验证球道是不是指定的球道编号,例如336/5/3 就不能通过
                            for (int i = 0; i < fz[0].Length; i++)
                            {
                                if ("12345万千百十个".IndexOf(fz[0].Substring(i, 1)) == -1)
                                    return 2;
                            }
                            for (int i = 0; i < fz[1].Length; i++)///fz[1]=wz=13579
                            {
                                wz = int.Parse(fz[1].Substring(i, 1));///1
                                if (fz[0].IndexOf("1") != -1 || fz[0].IndexOf("万") != -1)
                                    if (_qiuDao1)///表示在球道1下注
                                    {
                                        if (!add(gr, groupMember, ref  lsxz.QD[0, wz], int.Parse(fz[2]), 4 + wz)) return 1;
                                    }
                                    else
                                        return 5;
                                if (fz[0].IndexOf("2") != -1 || fz[0].IndexOf("千") != -1)
                                    if (_qiuDao2)
                                    {
                                        if (!add(gr, groupMember, ref  lsxz.QD[1, wz], int.Parse(fz[2]), 4 + wz)) return 1;
                                    }
                                    else
                                        return 5;
                                if (fz[0].IndexOf("3") != -1 || fz[0].IndexOf("百") != -1)
                                    if (_qiuDao3)
                                    {
                                        if (!add(gr, groupMember, ref  lsxz.QD[2, wz], int.Parse(fz[2]), 4 + wz)) return 1;
                                    }
                                    else
                                        return 5;
                                if (fz[0].IndexOf("4") != -1 || fz[0].IndexOf("十") != -1)
                                    if (_qiuDao4)
                                    {
                                        if (!add(gr, groupMember, ref  lsxz.QD[3, wz], int.Parse(fz[2]), 4 + wz)) return 1;
                                    }
                                    else
                                        return 5;
                                if (fz[0].IndexOf("5") != -1 || fz[0].IndexOf("个") != -1)
                                    if (_qiuDao5)
                                    {
                                        if (!add(gr, groupMember, ref  lsxz.QD[4, wz], int.Parse(fz[2]), 4 + wz)) return 1;
                                    }
                                    else
                                        return 5;
                            }
                            con += xz;
                            continue;
                        }
                        //   万1/单/20
                        wz = -1;
                        if (fz[1] == "大")
                            wz = 0;
                        if (fz[1] == "小")
                            wz = 1;
                        if (fz[1] == "单")
                            wz = 2;
                        if (fz[1] == "双")
                            wz = 3;

                        if (wz != -1)
                        {
                            for (int i = 0; i < fz[0].Length; i++)
                            {
                                if ("12345万千百十个".IndexOf(fz[0].Substring(i, 1)) == -1)
                                    return 2;
                            }
                            if (fz[0].IndexOf("1") != -1 || fz[0].IndexOf("万") != -1)
                                if (_qiuDao1)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[0, wz], int.Parse(fz[2]), wz)) return 1;
                                }
                                else
                                    return 5;
                            if (fz[0].IndexOf("2") != -1 || fz[0].IndexOf("千") != -1)
                                if (_qiuDao2)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[1, wz], int.Parse(fz[2]), wz)) return 1;
                                }
                                else
                                    return 5;
                            if (fz[0].IndexOf("3") != -1 || fz[0].IndexOf("百") != -1)
                                if (_qiuDao3)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[2, wz], int.Parse(fz[2]), wz)) return 1;
                                }
                                else
                                    return 5;
                            if (fz[0].IndexOf("4") != -1 || fz[0].IndexOf("十") != -1)
                                if (_qiuDao4)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[3, wz], int.Parse(fz[2]), wz)) return 1;
                                }
                                else
                                    return 5;
                            if (fz[0].IndexOf("5") != -1 || fz[0].IndexOf("个") != -1)
                                if (_qiuDao5)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[4, wz], int.Parse(fz[2]), wz)) return 1;
                                }
                                else
                                    return 5;
                            con += xz;
                            continue;
                        }
                    }

                    #endregion 单球

                    #region 大单

                    string[] dq = null;
                    int wzz = -1;
                    int dszh = 0;
                    while (true)
                    {
                        if (xz.IndexOf("大单") != -1)
                        {
                            dq = xz.Split(new string[] { "大单" }, StringSplitOptions.None);
                            wzz = 0; break;
                        }
                        if (xz.IndexOf("大双") != -1)
                        {
                            dq = xz.Split(new string[] { "大双" }, StringSplitOptions.None);
                            wzz = 1; break;
                        }
                        if (xz.IndexOf("小单") != -1)
                        {
                            dq = xz.Split(new string[] { "小单" }, StringSplitOptions.None);
                            wzz = 2; break;
                        }
                        if (xz.IndexOf("小双") != -1)
                        {
                            dq = xz.Split(new string[] { "小双" }, StringSplitOptions.None);
                            wzz = 3; break;
                        }
                        dszh = 1;
                        if (xz.IndexOf("大") != -1)
                        {
                            dq = xz.Split('大');
                            wzz = 0; break;
                        }
                        if (xz.IndexOf("小") != -1)
                        {
                            dq = xz.Split('小');
                            wzz = 1; break;
                        }
                        if (xz.IndexOf("单") != -1)
                        {
                            dq = xz.Split('单');
                            wzz = 2; break;
                        }
                        if (xz.IndexOf("双") != -1)
                        {
                            dq = xz.Split('双');
                            wzz = 3; break;
                        }
                        break;
                    }

                    if (dq != null && dq.Length == 2)
                    {
                        if (dq[0] == "")
                        {
                            if (_qiuDaoHe)
                            {
                                if (dszh == 0)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.ZHZHDXDS[wzz], int.Parse(dq[1]), 18 + wzz)) return 1;
                                }
                                else
                                    if (!add(gr, groupMember, ref  lsxz.ZHDXDS[wzz], int.Parse(dq[1]), 14 + wzz)) return 1;
                                con += xz;
                                continue;
                            }
                            if (_qiuDaosl == 1 && dszh == 1)
                            {
                                if (_qiuDao1)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[0, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao2)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[1, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao3)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[2, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao4)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[3, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                                if (_qiuDao5)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[4, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                                con += xz;
                                continue;
                            }
                        }
                        else if (dszh == 1)
                        {
                            for (int i = 0; i < dq[0].Length; i++)
                            {
                                if ("12345万千百十个".IndexOf(dq[0].Substring(i, 1)) == -1)
                                    return 2;
                            }
                            if (dq[0].IndexOf("1") != -1 || dq[0].IndexOf("万") != -1)
                                if (_qiuDao1)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[0, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                            if (dq[0].IndexOf("2") != -1 || dq[0].IndexOf("千") != -1)
                                if (_qiuDao2)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[1, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                            if (dq[0].IndexOf("3") != -1 || dq[0].IndexOf("百") != -1)
                                if (_qiuDao3)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[2, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                            if (dq[0].IndexOf("4") != -1 || dq[0].IndexOf("十") != -1)
                                if (_qiuDao4)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[3, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                            if (dq[0].IndexOf("5") != -1 || dq[0].IndexOf("个") != -1)
                                if (_qiuDao5)
                                {
                                    if (!add(gr, groupMember, ref  lsxz.DXDS[4, wzz], int.Parse(dq[1]), wzz)) return 1;
                                }
                                else
                                    return 5;
                            con += xz;
                            continue;
                        }
                    }

                    #endregion 大单。。

                    if (xz.IndexOf("龙") == 0)
                    {
                        if (!add(gr, groupMember, ref  lsxz.LHH[0], int.Parse(xz.Replace("龙", "")), 22)) return 1;
                        con += xz;
                        continue;
                    }
                    if (xz.IndexOf("虎") == 0)
                    {
                        if (!add(gr, groupMember, ref  lsxz.LHH[1], int.Parse(xz.Replace("虎", "")), 23)) return 1;
                        con += xz;
                        continue;
                    }
                    if (xz.IndexOf("和") == 0 || xz.IndexOf("合") == 0)
                    {
                        if (!add(gr, groupMember, ref  lsxz.LHH[2], int.Parse(xz.Replace("合", "").Replace("和", "")), 24)) return 1;
                        con += xz;
                        continue;
                    }


                    return 2;
                }//循环尾


                //这里写下注成功
                if (lsxz != new xztj())
                {
                    _eDuMax += _lsEDu;
                    for (int u = 0; u < 10; u++)
                    {
                        for (int x = 0; x < 5; x++)
                            groupMember.xiazhutongji.QD[x, u] += lsxz.QD[x, u];
                    }
                    for (int u = 0; u < 4; u++)
                    {
                        groupMember.xiazhutongji.DXDS[0, u] += lsxz.DXDS[0, u];
                        groupMember.xiazhutongji.DXDS[1, u] += lsxz.DXDS[1, u];
                        groupMember.xiazhutongji.DXDS[2, u] += lsxz.DXDS[2, u];
                        groupMember.xiazhutongji.DXDS[3, u] += lsxz.DXDS[3, u];
                        groupMember.xiazhutongji.DXDS[4, u] += lsxz.DXDS[4, u];

                        groupMember.xiazhutongji.ZHDXDS[u] += lsxz.ZHDXDS[u];
                        groupMember.xiazhutongji.ZHZHDXDS[u] += lsxz.ZHZHDXDS[u];
                    }
                    groupMember.xiazhutongji.LHH[0] += lsxz.LHH[0];
                    groupMember.xiazhutongji.LHH[1] += lsxz.LHH[1];
                    groupMember.xiazhutongji.LHH[2] += lsxz.LHH[2];

                    groupMember.zongjifen = groupMember.lszjf;
                    groupMember.conter += (con + ";");
                    groupMember.benqixiazhu += groupMember.lsxzjf;

                    //
                    lvChengYuanJiFen.Items[groupMember.Id].SubItems[8].Text = groupMember.conter;
                    lvChengYuanJiFen.Items[groupMember.Id].SubItems[5].Text = groupMember.zongjifen.ToString();
                    SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注",
                                "'" + groupMember.GroupMemberBaseInfo.NickName + "','"
                                + groupMember.Seq + "','"
                                + _kaiJiangData.qihao
                                + "','下注','"
                                + groupMember.lsxzjf.ToString()
                                + "','" + groupMember.zongjifen.ToString()
                                + "','" + groupMember.conter
                                + "'", " liushui_" + CacheData.Seq);
                    label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + (_zongJiFen - groupMember.lsxzjf).ToString();
                    //加本期合计
                    for (int u = 0; u < 10; u++)
                    {
                        for (int x = 0; x < 5; x++)
                            _group.xiazhutongji.QD[x, u] += lsxz.QD[x, u];
                    }
                    for (int u = 0; u < 4; u++)
                    {
                        _group.xiazhutongji.DXDS[0, u] += lsxz.DXDS[0, u];
                        _group.xiazhutongji.DXDS[1, u] += lsxz.DXDS[1, u];
                        _group.xiazhutongji.DXDS[2, u] += lsxz.DXDS[2, u];
                        _group.xiazhutongji.DXDS[3, u] += lsxz.DXDS[3, u];
                        _group.xiazhutongji.DXDS[4, u] += lsxz.DXDS[4, u];

                        _group.xiazhutongji.ZHDXDS[u] += lsxz.ZHDXDS[u];
                        _group.xiazhutongji.ZHZHDXDS[u] += lsxz.ZHZHDXDS[u];
                    }
                    _group.xiazhutongji.LHH[0] += lsxz.LHH[0];
                    _group.xiazhutongji.LHH[1] += lsxz.LHH[1];
                    _group.xiazhutongji.LHH[2] += lsxz.LHH[2];

                    _group.zongjifen = groupMember.lszjf;
                    _group.conter += (con + ";");
                    _group.benqixiazhu += groupMember.lsxzjf;
                    //
                    return 0;
                }
                return 2;
            }
            catch (Exception ex)
            {
                string msgid = send(gr.GroupId, CoolQCode.At(groupMember.GroupMemberBaseInfo.Number)+ "攻击格式错误！");//酷q发送群消息
                //if (msgid != "")
                jzxx(_group, CoolQCode.At(groupMember.GroupMemberBaseInfo.Number) + " 攻击格式错误！", msgid);
                return 3;
            }
            
        }

        /// <summary>
        /// 增加消息,以软件端发送的消息 存到数据库时seq 代表库名，昵称 代表群名
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="Content"></param>
        private void jzxx(GroupInfo gr, string Content, string msgid)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add(gr.GroupName);//群名
            item.SubItems.Add("群号："+gr.GroupId);//群号
            item.SubItems.Add(Content);//发送的群信息
            item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            item.SubItems.Add(msgid);//发送群消息返回的结果
            item.SubItems.Add("");//暂时不知道传什么
            item.SubItems.Remove(item.SubItems[0]);
            lvQunXiaoXi.Items.Insert(0, item);
            List<KeyVal> zcs = new List<KeyVal>();//保存聊天记录
            KeyVal zcs1 = new KeyVal("seq", CacheData.Seq);
            zcs.Add(zcs1);
            KeyVal zcs2 = new KeyVal("昵称", gr.GroupName);//群名
            zcs.Add(zcs2);
            KeyVal zcs3 = new KeyVal("内容", Content);
            zcs.Add(zcs3);
            try
            {
                SQL.INSERT(zcs, " liaotian_" + CacheData.Seq);
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 增加消息,以软件端发送的消息 存到数据库时seq 代表库名，昵称 代表群名
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="Content"></param>
        private void jzxx(GroupInfo gr,long CurrentLoginQQ, string Content, string msgid)
        {
            GroupMemberInfoWithBocai groupMember = CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic,CurrentLoginQQ);
            if (groupMember == null)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add("非当前群群员"+ CurrentLoginQQ);//群名
                item.SubItems.Add("非当前群群员");//群号
                item.SubItems.Add(Content);//发送的群信息
                item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(msgid);//发送群消息返回的结果
                item.SubItems.Add("" + CurrentLoginQQ);
                item.SubItems.Remove(item.SubItems[0]);
                lvQunXiaoXi.Items.Insert(0, item);
                List<KeyVal> zcs = new List<KeyVal>();//保存聊天记录
                KeyVal zcs1 = new KeyVal("seq", "非当前群群员" + CurrentLoginQQ);
                zcs.Add(zcs1);
                KeyVal zcs2 = new KeyVal("昵称", "非当前群群员");//群名
                zcs.Add(zcs2);
                KeyVal zcs3 = new KeyVal("内容", Content);
                zcs.Add(zcs3);
                MyLogUtil.ErrToLog("监听的群" + gr.GroupId + "中收到了一个非此群的群员" + CurrentLoginQQ + "的信息");
                try
                {
                    SQL.INSERT(zcs, " liaotian_" + CacheData.Seq);
                }
                catch (Exception ex) { }
            }
            else
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(groupMember.GroupMemberBaseInfo.NickName);//昵称
                item.SubItems.Add(groupMember.bendibeizhu);//备注 原先为groupMember.RemarkName
                item.SubItems.Add(Content);//发送的群信息
                item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(msgid);//发送群消息返回的结果
                item.SubItems.Add("" + CurrentLoginQQ);
                item.SubItems.Remove(item.SubItems[0]);
                lvQunXiaoXi.Items.Insert(0, item);
                List<KeyVal> zcs = new List<KeyVal>();//保存聊天记录
                KeyVal zcs1 = new KeyVal("seq", groupMember.Seq);
                zcs.Add(zcs1);
                KeyVal zcs2 = new KeyVal("昵称", groupMember.GroupMemberBaseInfo.NickName);//群名
                zcs.Add(zcs2);
                KeyVal zcs3 = new KeyVal("内容", Content);
                zcs.Add(zcs3);
                try
                {
                    SQL.INSERT(zcs, " liaotian_" + CacheData.Seq);
                }
                catch (Exception ex) { }
            }
            
            
        }

        /// <summary>
        /// 发送文字
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private string send( long groupId,string msg)
        {
            
            //
            if (ServerCommon.isLogWechat)
            {
                function.logWx("发送文字: " + msg);
            }

            //
            string xx = ""+CacheData.CoolQApi.SendGroupMsg(groupId,msg);//发送群消息总会有返回值
            
            return "";
        }

        ///废弃
        /// <summary>
        /// 获取当前登录软件的群成员的信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private GROUP getname(string UserName)
        {
            /*
            foreach (GROUP Item in _group.MemberList)
            {
                if (Item.UserName == UserName)
                {
                    return Item;
                }
            }
            */
            return null;
            
        }

        /// 正在适配，好像这个方法没有被调用
        /// 暂时忽略
        /// <summary>
        /// 发图发字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fatu(object sender, EventArgs e)
        {
            /*
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = fileDialog.FileName;
                FileStream fs = File.OpenRead(path); //OpenRead
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度
                Byte[] image = new Byte[filelength]; //建立一个字节数组
                fs.Read(image, 0, filelength); //按字节流读取
                System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
                fs.Close();
                Bitmap bit = new Bitmap(result);
                string xx = _qrWebWeChat.send(bit);
                xx = xx.Replace(" ", "");
                string MediaId = function.middlestring(xx, "MediaId\":\"", "\"");
                string msgid;
                if (function.getImageType(bit) == "GIF")
                    msgid = _qrWebWeChat.sendgif(MediaId, _group.UserName);
                else
                    msgid = _qrWebWeChat.sendImage(MediaId, _group.UserName);
                if (msgid != "")
                    jzxx(_group, "[图片]", msgid);
            }
            // _qrWebWeChat.jiaqun(username, listView2.CheckedItems[i].SubItems[1].Text);
            */
        }
        /// 适配完成
        /// <summary>
        /// 向指定群发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            
            if (textBox9.Text == "")
                return;
            string msgid = send(_group.GroupId,textBox9.Text);
            //if (msgid != "") //代表发送成功
            jzxx(_group, textBox9.Text, msgid);
            textBox9.Text = "";
            
        }

        /// <summary>
        /// 勾选球道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qiudao1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                if (cb.Name == "qiudao1")
                { _qiuDao1 = true; _qiuDaosl++; }
                if (cb.Name == "qiudao2")
                { _qiuDao2 = true; _qiuDaosl++; }
                if (cb.Name == "qiudao3")
                { _qiuDao3 = true; _qiuDaosl++; }
                if (cb.Name == "qiudao4")
                { _qiuDao4 = true; _qiuDaosl++; }
                if (cb.Name == "qiudao5")
                { _qiuDao5 = true; _qiuDaosl++; }
                if (cb.Name == "qiudaohe")
                    _qiuDaoHe = true;
            }
            else
            {
                if (cb.Name == "qiudao1")
                { _qiuDao1 = false; _qiuDaosl--; }
                if (cb.Name == "qiudao2")
                { _qiuDao2 = false; _qiuDaosl--; }
                if (cb.Name == "qiudao3")
                { _qiuDao3 = false; _qiuDaosl--; }
                if (cb.Name == "qiudao4")
                { _qiuDao4 = false; _qiuDaosl--; }
                if (cb.Name == "qiudao5")
                { _qiuDao5 = false; _qiuDaosl--; }
                if (cb.Name == "qiudaohe")
                    _qiuDaoHe = false;
            }
            listView5.Items.Clear();
            if (_qiuDaosl == 0 && _qiuDaoHe)
                return;

            for (int i = 0; i < _guiZe.cshxe.Count; i++)
            {
                if (i > 13 && !_qiuDaoHe)
                    return;
                if (i < 14 && !_qiuDaoHe && _qiuDaosl == 0)
                    return;
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(_guiZe.cshxe[i].name);
                item.SubItems.Add(_guiZe.cshxe[i].gz);
                item.SubItems.Add(_guiZe.cshxe[i].xe.ToString());
                item.SubItems.Add(_guiZe.cshxe[i].id.ToString());
                listView5.Items.Add(item);
            }
        }

        /// <summary>
        /// 开奖倒计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //
            string q = "";
            DateTime dtNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string fz = dtNow.Minute.ToString();
            string fen = fz.Substring(fz.Length - 1, 1);
            string miao = dtNow.Second.ToString();
            int ms = (int.Parse(fen) * 60) + int.Parse(miao);
            int daojishi = 0;
            if (dtNow.Hour >= 2 && dtNow.Hour <= 6)
            {
                return;
            }

            if (dtNow.Hour >= 6 && dtNow.Hour < 10)
            {
                if (_buShi.Year == DateTime.Now.AddYears(-1).Year)
                {
                    string qihao = DateTime.Now.ToString("yyyyMMdd") + "024";
                    qihao = resetQiHao(qihao);

                    if (qihao != "")
                    {
                        _kaiJiangData.qihao = qihao;
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(_kaiJiangData.qihao);
                        item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        item.SubItems.Remove(item.SubItems[0]);
                        lvQunXiaoXi.Items.Insert(0, item);
                        textBox1.Text = qihao;
                        label18.Text = "期号：" + _kaiJiangData.qihao;
                    }
                    _buShi = Convert.ToDateTime(_buShi.AddYears(1).ToString("yyyy-MM-dd 10:00:00"));
                }
                DateTime kjdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                开奖时间.Value = dtNow.AddMinutes(5 - int.Parse(fen));
                daojishi = ((10 - dtNow.Hour) * 60 * 60) - (dtNow.Minute * 60) - (dtNow.Second);
            }
            else
            {
                //划分5分钟
                if ((dtNow.Hour >= 22 || dtNow.Hour < 2) && int.Parse(fen) < 5)
                {
                    if (_buShi.Year == DateTime.Now.AddYears(-1).Year)
                    {
                        string qihao = DateTime.Now.ToString("yyyyMMdd");
                        if (dtNow.Hour >= 22)
                        {
                            string ls = ((((dtNow.Hour - 22) * 60 + dtNow.Minute) / 5) + 97).ToString();
                            if (ls != "120")
                                ls = "0" + ls;
                            qihao = DateTime.Now.ToString("yyyyMMdd") + ls;
                        }
                        if (dtNow.Hour < 2)
                        {
                            string ls = (((dtNow.Hour * 60 + dtNow.Minute) / 5) + 1).ToString();
                            if (ls != "120")
                                ls = "0" + ls;
                            qihao = DateTime.Now.ToString("yyyyMMdd") + ls;
                        }

                        qihao = resetQiHao(qihao);
                        _kaiJiangData.qihao = qihao;// (long.Parse(qihao) + 1).ToString();

                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(_kaiJiangData.qihao);
                        item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        item.SubItems.Remove(item.SubItems[0]);
                        lvQunXiaoXi.Items.Insert(0, item);
                        textBox1.Text = qihao;
                        label18.Text = "期号：" + _kaiJiangData.qihao;

                        _buShi = dtNow.AddMinutes(5 - int.Parse(fen));
                        _buShi = Convert.ToDateTime(_buShi.ToString("yyyy-MM-dd HH:mm:00"));
                    }
                    DateTime kjdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    开奖时间.Value = dtNow.AddMinutes(5 - int.Parse(fen));
                    daojishi = 300 - ms;
                }
                else
                {
                    if (_buShi.Year == DateTime.Now.AddYears(-1).Year)
                    {
                        string qihao = DateTime.Now.ToString("yyyyMMdd");
                        if (dtNow.Hour > 2 && dtNow.Hour < 22)
                        {
                            string ls = ((((dtNow.Hour - 10) * 60 + dtNow.Minute) / 10) + 25).ToString();
                            qihao = DateTime.Now.ToString("yyyyMMdd") + ls;
                        }
                        if (dtNow.Hour >= 22)
                        {
                            string ls = ((((dtNow.Hour - 22) * 60 + dtNow.Minute) / 5) + 97).ToString();
                            if (ls != "120")
                                ls = "0" + ls;
                            qihao = DateTime.Now.ToString("yyyyMMdd") + ls;
                        }
                        if (dtNow.Hour < 2)
                        {
                            string ls = (((dtNow.Hour * 60 + dtNow.Minute) / 5) + 1).ToString();
                            if (ls != "120")
                                ls = "0" + ls;
                            qihao = DateTime.Now.ToString("yyyyMMdd") + ls;
                        }
                        qihao = resetQiHao(qihao);
                        _kaiJiangData.qihao = qihao;// (long.Parse(qihao) + 1).ToString();
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(_kaiJiangData.qihao);
                        item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        item.SubItems.Remove(item.SubItems[0]);
                        lvQunXiaoXi.Items.Insert(0, item);
                        textBox1.Text = qihao;
                        label18.Text = "期号：" + _kaiJiangData.qihao;

                        _buShi = dtNow.AddMinutes(10 - int.Parse(fen));
                        _buShi = Convert.ToDateTime(_buShi.ToString("yyyy-MM-dd HH:mm:00"));
                    }
                    DateTime kjdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    开奖时间.Value = dtNow.AddMinutes(10 - int.Parse(fen));
                    daojishi = 600 - ms;
                }
            }
            开奖倒计时.Text = "开奖倒计时：" + daojishi.ToString();
            //if (_fengPan && daojishi % 25 == 0)
            //{
            //    fasong("目前是锁盘阶段\r\n攻击/撤销暂时无效", true);
            //}


            int kjq = 0;
            //封盘，开奖前n秒封盘并发送消息
            if (int.TryParse(function.middlestring(comboBox6.Text, "前", "秒"), out kjq))
            {
                if (daojishi < kjq && !_fengPan)
                {
                    _fengPan = true;
                    if (checkBox6.Checked)
                    {
                        fasong(textBox14.Text, false);
                    }
                    else { }
                }
            }
            int fpq = 0;
            //封盘前，开奖前n秒发送消息
            if (int.TryParse(function.middlestring(comboBox1.Text, "前", "秒"), out fpq))
            {
                if (daojishi < fpq + kjq && !_fengPanQian)
                {
                    _fengPanQian = true;
                    if (checkBox5.Checked)
                    {
                        fasong(textBox13.Text, false);
                    }
                    else { }
                }
            }

            //==========封盘后ns飞单==========
            if (daojishi < (kjq - _feiDanFengPanMiao) && _fengPan && !_isFeiPan)
            {
                //
                FeiPan();
                _isFeiPan = true;
            }

            //倒计时发送下注明细
            if (daojishi <= 40 && _isFeiPan && _isFaSongXiaZhu == false)
            {
                string xzmx = "";
                int groupNum = 0;
                foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
                {
                    string xzCon = jp.conter;
                    //重新算下注下注
                    if (_feiDanJieGuoData.isSuccess == false)
                    {
                        xzCon = getXiaZhuTongJiCon(jp.xiazhutongji, _feiDanJieGuoData);
                    }
                    //
                    if (jp.conter != "")
                    {
                        xzmx += jp.NickNameShort + ":" + xzCon + "  ";
                        groupNum++;
                        if (groupNum % 2 == 0)
                        {
                            xzmx += "\n";
                        }
                    }
                }
                xzmx = textBox17.Text.Replace("{下注明细}", xzmx).Replace("{期号}", _kaiJiangData.qihao);

                fasong(xzmx, false);
                _isFaSongXiaZhu = true;
            }

            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) > _buShi)
            {
                #region 开奖

                string err = "";
                timer1.Stop();
                log("进入开奖");
                开奖倒计时.Text = "开奖倒计时：正在开奖......";
                if (comboBox3.Text == "超级API")
                {
                    while (true)
                    {
                        try
                        {
                            q = HttpHelps.Get(Encoding.Default, ShouQuanServer + "/api/lottery/get/1", ref q);
                        }
                        catch (Exception ex) { continue; }
                        string qihao = function.middlestring(q, "Sn\":\"", "\"");
                        qihao = resetQiHao(qihao);
                        err += qihao + "  ";
                        string number = function.middlestring(q, "Numbers\":\"", "\"");
                        err += number + "  ";
                        string dateline = function.middlestring(q, "DateLine\":\"", "\"");
                        err += dateline;
                        if (dateline != "" && number != "" && qihao != "")
                        {
                            DateTime dti = Convert.ToDateTime(dateline);
                            try
                            {
                                if (dti >= _buShi && dti < _buShi.AddMinutes(2))
                                {
                                    textBox1.Text = qihao;
                                    textBox2.Text = number;
                                    开奖时间.Value = Convert.ToDateTime(dateline);

                                    _kaiJiangData.qihao = qihao;
                                    label18.Text = "期号：" + _kaiJiangData.qihao;
                                    _kaiJiangData.kjhm = number;
                                    string[] sz = _kaiJiangData.kjhm.Split(',');
                                    for (int i = 0; i < 5; i++)
                                    {
                                        _kaiJiangData.QD[i] = int.Parse(sz[i]);
                                    }
                                    log("开奖获取信息：" + _kaiJiangData.qihao + "," + _kaiJiangData.kjhm + "," + dateline);
                                    break;
                                }
                            }
                            catch (Exception ex) { }
                        }
                        DateTime ls = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        err += ls.ToString("yyyy-MM-dd HH:mm:ss") + "  " + _buShi.AddMinutes(2).ToString("yyyy-MM-dd HH:mm:ss") + "\n";
                        if (ls > _buShi.AddMinutes(2))
                        {
                            log("开奖超时");
                            MessageBox.Show("API开奖超时,请手动开奖,日志;" + err);
                            return;
                        }
                        function.yanci(5000);
                    }
                }
                else
                {
                    Random rd = new Random(int.Parse(function.MilliTime()));
                    _kaiJiangData.QD = new int[] { rd.Next(10), rd.Next(10), rd.Next(10), rd.Next(10), rd.Next(10) };
                    _kaiJiangData.kjhm = _kaiJiangData.QD[0].ToString() + "," + _kaiJiangData.QD[1].ToString() + "," + _kaiJiangData.QD[2].ToString() + "," + _kaiJiangData.QD[3].ToString() + "," + _kaiJiangData.QD[4].ToString();
                }
                try
                {
                    kaiJiang();
                    log("核算开奖完毕");
                }
                catch (Exception ex) { log("核算开奖失败"); }

                try
                {
                    string[] zjxx = jieSuan().Split(new string[] { "||||||" }, StringSplitOptions.None);
                    string kjwb = textBox15.Text.Replace("{期号}", _kaiJiangData.qihao).Replace("{开奖号码}", _kaiJiangData.kjhm).Replace("{中奖详细}", zjxx[0]);
                    fasong(kjwb, false);
                    log("结算积分完毕");
                    kjwb = textBox12.Text.Replace("{期号}", _kaiJiangData.qihao).Replace("{开奖号码}", _kaiJiangData.kjhm).Replace("{本期账单}", zjxx[1]);
                    if (checkBox4.Checked)
                    {
                        fasong(kjwb, false);
                    }
                    else { }
                }
                catch (Exception ex) { log("核算开奖失败"); }


                //
                _kaiJiangData = new kjtj();

                _fengPan = false;
                _fengPanQian = false;
                _isFeiPan = false;
                _isFaSongXiaZhu = false;
                _buShi = DateTime.Now.AddYears(-1);
                _eDuMax = 0;
                foreach (lsxe ox in _guiZe.cshxe)
                    ox.dqxz = 0;


                log("开始下一轮倒计时");
                timer1.Start();
                if (checkBox11.Checked)
                {
                    MessageBox.Show("同步已完成!");
                }

                #endregion 开奖
            }
            
        }



        /// <summary>
        /// 手动开奖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            开奖倒计时.Text = "开奖倒计时：正在开奖......";
            timer1.Stop();
            _fengPan = true;
            _kaiJiangData.kjhm = textBox2.Text;
            string[] sz = _kaiJiangData.kjhm.Split(',');
            for (int i = 0; i < 5; i++)
            {
                _kaiJiangData.QD[i] = int.Parse(sz[i]);
            }

            //计算开奖
            kaiJiang();
            try
            {
                string[] zjxx = jieSuan().Split(new string[] { "||||||" }, StringSplitOptions.None);
                string kjwb = textBox15.Text.Replace("{期号}", _kaiJiangData.qihao).Replace("{开奖号码}", _kaiJiangData.kjhm).Replace("{中奖详细}", zjxx[0]);
                fasong(kjwb, false);

                kjwb = textBox12.Text.Replace("{期号}", _kaiJiangData.qihao).Replace("{开奖号码}", _kaiJiangData.kjhm).Replace("{本期账单}", zjxx[1]);
                fasong(kjwb, false);
            }
            catch (Exception ex) { }
            _kaiJiangData = new kjtj();
            _fengPan = false;
            _isFeiPan = false;
            _eDuMax = 0;
            _buShi = DateTime.Now.AddYears(-1);
            timer1.Start();
            if (checkBox11.Checked)
                MessageBox.Show("同步已完成!");
        }

        /// <summary>
        /// 开奖
        /// </summary>
        private void kaiJiang()
        {
            try
            {
                int zonghe = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (_kaiJiangData.QD[i] % 2 == 0)
                        _kaiJiangData.DXDS[i, 3] = true;//双
                    else
                        _kaiJiangData.DXDS[i, 2] = true;//单

                    if (_kaiJiangData.QD[i] < 5)
                        _kaiJiangData.DXDS[i, 1] = true; //小
                    else
                        _kaiJiangData.DXDS[i, 0] = true; //大

                    zonghe += _kaiJiangData.QD[i];
                }
                if (zonghe % 2 == 0)
                    _kaiJiangData.ZH[3] = true;//双
                else
                    _kaiJiangData.ZH[2] = true;//单

                if (zonghe < 23)
                    _kaiJiangData.ZH[1] = true; //小
                else
                    _kaiJiangData.ZH[0] = true; //大

                if (zonghe >= 23)//大
                    if (zonghe % 2 != 0)//单
                        _kaiJiangData.ZHzh[0] = true;
                    else//双
                        _kaiJiangData.ZHzh[1] = true;
                else
                    if (zonghe % 2 != 0)//单
                        _kaiJiangData.ZHzh[2] = true;
                    else//双
                        _kaiJiangData.ZHzh[3] = true;
                if (_kaiJiangData.QD[0] > _kaiJiangData.QD[4])
                    _kaiJiangData.LHH[0] = true;
                if (_kaiJiangData.QD[0] < _kaiJiangData.QD[4])
                    _kaiJiangData.LHH[1] = true;
                if (_kaiJiangData.QD[0] == _kaiJiangData.QD[4])
                    _kaiJiangData.LHH[2] = true;
            }
            catch (Exception ex) { log("开奖失败"); }
        }

        /// <summary>
        /// 开奖结算积分，返回中奖详细和本期账单
        /// </summary>
        private string jieSuan()
        {

            
            string zjxx = "";
            string strzd = "";
            //
            _shangQiKaiJiangData = _kaiJiangData;

            feiPanJieGuo fpJieGuo = _feiDanJieGuoData;

            if (fpJieGuo.isSuccess)
            {
                fpJieGuo = ServerCommon.SetFeiPanJieGuo(fpJieGuo, true);
            }
            //_kaiJiangData  本期开奖结果


            int groupNum = 0;
            int groupNumB = 0;
            foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
            {


                try
                {
                    string strXiaZhuWenBen = "";
                    if (jp.conter == "")
                    {
                        //积分返还
                        jp.shangQiXiaZhu = jp.xiazhutongji;
                        jp.xiazhutongji = new xztj();
                        continue;
                    }

                    List<KeyVal> xiaZhuData = new List<KeyVal>();
                    int qd = 0;
                    int dxds = 0;
                    int zh = 0;
                    int zhzh = 0;
                    int lhh = 0;

                    //实际中奖
                    int zj_qd = 0;
                    int zj_dxds = 0;
                    int zj_zh = 0;
                    int zj_zhzh = 0;
                    int zj_lhh = 0;
                    jp.shangQiXiaZhu = new xztj();

                    #region 统计积分
                    for (int i = 0; i < 10; i++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            KeyVal c = new KeyVal("qd" + (x + 1).ToString() + "_" + i.ToString(), jp.xiazhutongji.QD[x, i].ToString());
                            xiaZhuData.Add(c);
                        }
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        qd += jp.xiazhutongji.QD[x, _kaiJiangData.QD[x]];//球道

                        if (jp.xiazhutongji.QD[x, _kaiJiangData.QD[x]] > 0)
                        {
                            if (_feiDanJieGuoData.isSuccess || _feiDanJieGuoData.QD[x, _kaiJiangData.QD[x]])
                            {
                                strXiaZhuWenBen += (x + 1).ToString() + "/" + _kaiJiangData.QD[x].ToString()
                                    + "/" + jp.xiazhutongji.QD[x, _kaiJiangData.QD[x]].ToString() + ";";
                            }
                        }
                        //实际中奖
                        if (fpJieGuo.QD[x, _kaiJiangData.QD[x]])
                        {
                            zj_qd += jp.xiazhutongji.QD[x, _kaiJiangData.QD[x]];
                        }
                    }

                    for (int i = 0; i < 5; i++)//大小单双
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (_kaiJiangData.DXDS[i, x] && jp.xiazhutongji.DXDS[i, x] > 0)
                            {
                                dxds += jp.xiazhutongji.DXDS[i, x];
                                if (_feiDanJieGuoData.isSuccess || _feiDanJieGuoData.DXDS[i, x])
                                {

                                    strXiaZhuWenBen += (i + 1).ToString() + "/";
                                    if (x == 0) strXiaZhuWenBen += "大";
                                    if (x == 1) strXiaZhuWenBen += "小";
                                    if (x == 2) strXiaZhuWenBen += "单";
                                    if (x == 3) strXiaZhuWenBen += "双";
                                    strXiaZhuWenBen += "/" + jp.xiazhutongji.DXDS[i, x].ToString() + ";";
                                }
                                //实际中奖
                                if (fpJieGuo.DXDS[i, x])
                                {
                                    zj_dxds += jp.xiazhutongji.DXDS[i, x];
                                }
                            }
                            KeyVal c = new KeyVal("d" + (i + 1).ToString() + "_" + x.ToString(), jp.xiazhutongji.DXDS[i, x].ToString());
                            xiaZhuData.Add(c);
                        }
                    }
                    for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                    {
                        if (_kaiJiangData.ZH[i])
                        {
                            zh += jp.xiazhutongji.ZHDXDS[i];
                            if (jp.xiazhutongji.ZHDXDS[i] > 0)
                            {
                                if (_feiDanJieGuoData.isSuccess || _feiDanJieGuoData.ZHDXDS[i])
                                {
                                    if (i == 0) strXiaZhuWenBen += "总大";
                                    if (i == 1) strXiaZhuWenBen += "总小";
                                    if (i == 2) strXiaZhuWenBen += "总单";
                                    if (i == 3) strXiaZhuWenBen += "总双";
                                    strXiaZhuWenBen += jp.xiazhutongji.ZHDXDS[i].ToString() + ";";
                                }
                            }
                            //实际中奖
                            if (fpJieGuo.ZHDXDS[i])
                            {
                                zj_zh += jp.xiazhutongji.ZHDXDS[i]; ;
                            }

                        }
                        if (_kaiJiangData.ZHzh[i])
                        {
                            zhzh += jp.xiazhutongji.ZHZHDXDS[i];
                            if (jp.xiazhutongji.ZHZHDXDS[i] > 0)
                            {
                                if (_feiDanJieGuoData.isSuccess || _feiDanJieGuoData.ZHZHDXDS[i])
                                {
                                    if (i == 0) strXiaZhuWenBen += "大单";
                                    if (i == 1) strXiaZhuWenBen += "大双";
                                    if (i == 2) strXiaZhuWenBen += "小单";
                                    if (i == 3) strXiaZhuWenBen += "小双";
                                    strXiaZhuWenBen += jp.xiazhutongji.ZHZHDXDS[i].ToString() + ";";
                                }
                            }
                            //实际中奖
                            if (fpJieGuo.ZHZHDXDS[i])
                            {
                                zj_zhzh += jp.xiazhutongji.ZHZHDXDS[i];
                            }

                        }
                        if (i != 3)
                        {
                            if (_kaiJiangData.LHH[i])
                            {
                                lhh += jp.xiazhutongji.LHH[i];
                                if (jp.xiazhutongji.LHH[i] > 0)
                                {
                                    if (_feiDanJieGuoData.isSuccess || _feiDanJieGuoData.LHH[i])
                                    {
                                        if (i == 0) strXiaZhuWenBen += "龙";
                                        if (i == 1) strXiaZhuWenBen += "虎";
                                        if (i == 2) strXiaZhuWenBen += "合";
                                        strXiaZhuWenBen += jp.xiazhutongji.LHH[i].ToString() + ";";
                                    }
                                }
                                //实际中奖
                                if (fpJieGuo.LHH[i])
                                {
                                    zj_lhh += jp.xiazhutongji.LHH[i]; ;
                                }
                            }
                            KeyVal l = new KeyVal("LHH" + i.ToString(), jp.xiazhutongji.LHH[i].ToString());
                            xiaZhuData.Add(l);
                        }
                        KeyVal c = new KeyVal("zh" + i.ToString(), jp.xiazhutongji.ZHDXDS[i].ToString());
                        xiaZhuData.Add(c);

                        KeyVal c1 = new KeyVal("zhzh" + i.ToString(), jp.xiazhutongji.ZHZHDXDS[i].ToString());
                        xiaZhuData.Add(c1);

                    }

                    #endregion


                    #region 增加成员数据

                    if (strXiaZhuWenBen != "")
                    {
                        strzd += jp.NickNameShort + "  " + strXiaZhuWenBen + "  ";
                        groupNum++;
                        if (groupNum % 2 == 0)
                        {
                            strzd += "\n";
                        }
                    }

                    //开奖后积分

                    double zjjf = 0;//未减去飞盘失败的
                    int zj_xiazhu = fpJieGuo.isSuccess ? jp.benqixiazhu : getXiaZhuTongJi(jp.xiazhutongji, fpJieGuo);//实际下注


                    double zj_zjjf = 0;//实际提交中奖
                    try
                    {
                        zjjf = (qd * Convert.ToDouble(textBox29.Text))
                            + (dxds * Convert.ToDouble(textBox30.Text))
                            + (zh * Convert.ToDouble(textBox31.Text))
                            + (zhzh * Convert.ToDouble(textBox32.Text))
                            + (lhh * Convert.ToDouble(textBox33.Text));

                        zj_zjjf = (zj_qd * Convert.ToDouble(textBox29.Text))
                         + (zj_dxds * Convert.ToDouble(textBox30.Text))
                         + (zj_zh * Convert.ToDouble(textBox31.Text))
                         + (zj_zhzh * Convert.ToDouble(textBox32.Text))
                         + (zj_lhh * Convert.ToDouble(textBox33.Text));

                        //
                        if (Math.Round(zjjf, 0) > zjjf)
                        {
                            _isJieSuanJinYi = true;
                        }
                        zjjf = Math.Round(zjjf, 0);

                        //
                        if (Math.Round(zj_zjjf, 0) > zj_zjjf)
                        {
                            _isJieSuanJinYi_zj = true;
                        }
                        zj_zjjf = Math.Round(zj_zjjf, 0);
                    }
                    catch (Exception ex)
                    {
                        log(jp.GroupMemberBaseInfo.NickName + "赔率未设置，结算失败");
                    }
                    jp.benqiyingkui = (int)zjjf - jp.benqixiazhu;
                    jp.zongyingkui += jp.benqiyingkui;
                    jp.zongxiazhu += jp.benqixiazhu;
                    if (zjjf != 0)//奖金流水保存
                    {
                        lock (obj)
                        {
                            _zongJiFen += (int)zjjf;
                        }

                        if (_feiDanJieGuoData.isSuccess)
                        {
                            zjxx += jp.NickNameShort + "  +" + zjjf.ToString().PadLeft(5, '0') + "积分  ";
                        }
                        else
                        {
                            zjxx += jp.NickNameShort + "  +" + zj_zjjf.ToString().PadLeft(5, '0') + "积分  ";
                        }
                        groupNumB++;
                        if (groupNumB % 2 == 0)
                        {
                            zjxx += "\n";
                        }
                        jp.zongjifen += (int)zjjf;

                        //增加成员数据
                        SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注",
                            "'" + jp.GroupMemberBaseInfo.NickName + "','"
                            + jp.Seq + "','"
                            + _kaiJiangData.qihao
                            + "','奖金','"
                            + ((int)zjjf).ToString()
                            + "','" + jp.zongjifen
                            + "',''",
                            " liushui_" + CacheData.Seq);



                        label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + (_zongJiFen + zjjf).ToString();
                    }

                    //成功下注
                    SQL.INSERT("NickName,seq,期号,类型,实际下注,实际中奖,实际下注文本,备注",
                            "'" + jp.GroupMemberBaseInfo.NickName + "','"
                            + jp.Seq + "','"
                            + _kaiJiangData.qihao + "','','"
                            + zj_xiazhu.ToString() + "','"
                            + zj_zjjf
                            + "','',''",
                            " liushuiAct_" + CacheData.Seq);

                    KeyVal seq = new KeyVal("seq", jp.Seq);
                    xiaZhuData.Add(seq);
                    KeyVal NN = new KeyVal("NickName", jp.GroupMemberBaseInfo.NickName);
                    xiaZhuData.Add(NN);
                    KeyVal qh = new KeyVal("期号", _kaiJiangData.qihao);
                    xiaZhuData.Add(qh);
                    KeyVal xzwb = new KeyVal("下注文本", jp.conter);
                    xiaZhuData.Add(xzwb);
                    KeyVal yk = new KeyVal("盈亏", jp.benqiyingkui.ToString());
                    xiaZhuData.Add(yk);
                    KeyVal xzjf = new KeyVal("下注积分", jp.benqixiazhu.ToString());
                    xiaZhuData.Add(xzjf);
                    KeyVal jsjf = new KeyVal("结算后积分", jp.zongjifen.ToString());
                    xiaZhuData.Add(jsjf);

                    //
                    SQL.INSERT(xiaZhuData, " NameInt_" + CacheData.Seq);
                    #endregion



                    #region 统计积分

                    //更新用户总积分
                    string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where seq='" + jp.Seq + "'",
                        "'现有积分'='" + jp.zongjifen.ToString() + "','总盈亏'='" + jp.zongyingkui.ToString() + "','总下注'='"
                        + jp.zongxiazhu.ToString() + "'");

                    SQLiteHelper.ExecuteNonQuery(delStr);


                    //显示更新后积分，获取统计
                    _group.zongyingkui += jp.zongyingkui;
                    _group.benqixiazhu += jp.benqixiazhu;
                    lvChengYuanJiFen.Items[jp.Id].SubItems[5].Text = jp.zongjifen.ToString();
                    lvChengYuanJiFen.Items[jp.Id].SubItems[6].Text = jp.zongyingkui.ToString();
                    DateTime time1 = DateTime.Now.Date;
                    DateTime time2 = time1.AddDays(1);
                    try
                    {
                        DataTable deset = SQLiteHelper.ExecuteDataTable("select sum(盈亏)  from NameInt_" + CacheData.Seq + " where seq='" + jp.Seq
                            + "' and Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + time2.ToString("yyyy-MM-dd 00:00:00")
                            + "' group by seq", null);
                        lvChengYuanJiFen.Items[jp.Id].SubItems[7].Text = deset.Rows[0][0].ToString();
                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion

                    MyValidate.T();

                    log(jp.GroupMemberBaseInfo.NickName + "清空下注数据，更新总积分");

                    lvChengYuanJiFen.Items[jp.Id].SubItems[8].Text = "";//本期下注
                    lvChengYuanJiFen.Items[jp.Id].SubItems[6].Text = jp.zongyingkui.ToString();//总积分

                    jp.benqixiazhu = 0;
                    jp.benqiyingkui = 0;
                    jp.conter = "";
                    //
                    jp.shangQiXiaZhu = jp.xiazhutongji;
                    jp.xiazhutongji = new xztj();


                }
                catch (Exception ex)
                {
                    log(jp.GroupMemberBaseInfo.NickName + "玩家结算失败");
                }
            }

            //群合计
            _group.xiazhutongji = new xztj();

            //
            List<KeyVal> zcs = new List<KeyVal>();//保存开奖信息
            KeyVal zcs1 = new KeyVal("期号", _kaiJiangData.qihao);
            zcs.Add(zcs1);
            KeyVal zcs2 = new KeyVal("qd1", _kaiJiangData.kjhm);
            zcs.Add(zcs2);

            KeyVal zcs7 = new KeyVal("总下注积分", _group.benqixiazhu.ToString());
            zcs.Add(zcs7);
            KeyVal zcs8 = new KeyVal("盈亏", _group.zongyingkui.ToString());
            zcs.Add(zcs8);


            SQL.INSERT(zcs, " kaijiang_" + CacheData.Seq);

            //
            DateTime tim1 = DateTime.Now.Date;
            DateTime tim2 = tim1.AddDays(1);
            DataTable dest = SQLiteHelper.ExecuteDataTable("select 期号  from kaijiang_" + CacheData.Seq + " where Time BETWEEN '" + tim1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + tim2.ToString("yyyy-MM-dd 00:00:00") + "'", null);


            comboBox5.Items.Clear();
            foreach (DataRow dr in dest.Rows)
            {
                comboBox5.Items.Add(dr[0].ToString());
            }

            //
            feiPanFanHuan();



            return zjxx + "||||||" + strzd;
            
        }

        /// <summary>
        /// 测试赔率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                new 测试赔率().Show();
            }
        }

        /// 正在处理
        /// <summary>
        /// 报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox2.Checked)
            {
                new 报盘(_group).Show();
            }
            
        }

        /// <summary>
        /// 报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            new 报表(CacheData.Seq, textBox23.Text).Show();
        }

        /// <summary>
        /// 一键修改备注和入局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            
            string rj = "否";
            string Val = "";
            if (checkBox12.Checked)
                rj = "是";
            Val = "'是否入局'='" + rj + "'";
            if (txtTuiJianRen.Text != "")
                Val += ",'推荐人'='" + txtTuiJianRen.Text + "'";
            if (textBox27.Text != "")
                Val += ",'本地备注'='" + textBox27.Text + "'";

            string where = "";
            int m = lvChengYuanJiFen.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (lvChengYuanJiFen.CheckedItems[i].Checked)
                {
                    if (txtTuiJianRen.Text != "")
                        lvChengYuanJiFen.CheckedItems[i].SubItems[3].Text = txtTuiJianRen.Text;

                    if (textBox27.Text != "")
                        lvChengYuanJiFen.CheckedItems[i].SubItems[1].Text = textBox27.Text;
                    if (rj == "是")
                        /*getname(lvChengYuanJiFen.CheckedItems[i].SubItems[13].Text).sfrj = true;*/
                        CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, Convert.ToInt64(lvChengYuanJiFen.CheckedItems[i].SubItems[11].Text)).sfrj = true;
                    if (rj == "否")
                        /*getname(lvChengYuanJiFen.CheckedItems[i].SubItems[2].Text).sfrj = false;*/
                        //MyLogUtil.ToLogFotTest("测试数据："+ lvChengYuanJiFen.CheckedItems[i].SubItems[11].Text);
                        CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, Convert.ToInt64(lvChengYuanJiFen.CheckedItems[i].SubItems[11].Text)).sfrj = false;
                    lvChengYuanJiFen.CheckedItems[i].SubItems[4].Text = rj;
                    where = where + "seq like '" + lvChengYuanJiFen.CheckedItems[i].SubItems[9].Text + "' or ";
                }

                foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
                {
                    MyLogUtil.ToLogFotTest("一键修改,会员seq值：" + jp.Seq + ",lvChengYuanJiFen值：" + lvChengYuanJiFen.CheckedItems[i].SubItems[9].Text);
                    if (jp.Seq == lvChengYuanJiFen.CheckedItems[i].SubItems[9].Text)
                    {
                        
                        jp.bendibeizhu = textBox27.Text;
                        break;
                    }
                }
            }

            if (where != "")
            {
                where = where.Remove(where.Length - 4, 4);
                string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where " + where, Val);
                SQLiteHelper.ExecuteNonQuery(delStr);
            }
            
        }

        /// <summary>
        /// 更新记录  上分下分下注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            
            DialogResult d = MessageBox.Show("更新纪录？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            string leixing = comboBox4.Text;
            string qihao = comboBox5.Text;
            string beizhu = textBox10.Text;
            string jfen = textBox11.Text;
            if (string.IsNullOrWhiteSpace(jfen))
            {
                return;
            }
            int m = lvChengYuanJiFen.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (lvChengYuanJiFen.CheckedItems[i].Checked)
                {
                    if (leixing == "上分")
                    {
                        CacheData.GroupMemberInfoList[int.Parse(lvChengYuanJiFen.CheckedItems[i].SubItems[10].Text)].zongjifen += int.Parse(jfen);
                        lock (obj)
                        {
                            _zongJiFen += int.Parse(jfen);
                        }
                    }
                    if (leixing == "下分")
                    {
                        CacheData.GroupMemberInfoList[int.Parse(lvChengYuanJiFen.CheckedItems[i].SubItems[10].Text)].zongjifen -= int.Parse(jfen);
                        lock (obj)
                        {
                            _zongJiFen -= int.Parse(jfen);
                        }
                    }
                    lvChengYuanJiFen.CheckedItems[i].SubItems[5].Text = CacheData.GroupMemberInfoList[int.Parse(lvChengYuanJiFen.CheckedItems[i].SubItems[10].Text)].zongjifen.ToString();
                    SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注", "'" + lvChengYuanJiFen.CheckedItems[i].SubItems[0].Text + "','" + lvChengYuanJiFen.CheckedItems[i].SubItems[9].Text + "','" + qihao + "','" + leixing + "','" + CacheData.GroupMemberInfoList[int.Parse(lvChengYuanJiFen.CheckedItems[i].SubItems[10].Text)].zongjifen.ToString() + "','" + jfen + "','" + beizhu + "'", " liushui_" + CacheData.Seq);

                    string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where seq='" + lvChengYuanJiFen.CheckedItems[i].SubItems[9].Text + "'", "'现有积分'='" + lvChengYuanJiFen.CheckedItems[i].SubItems[5].Text + "'");
                    SQLiteHelper.ExecuteNonQuery(delStr);
                    // listView2.CheckedItems[i].Remove();
                }
            }
            
        }


        /// <summary>
        /// 同意上分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {

            DialogResult d = MessageBox.Show("同意上分？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            string cont = "";
            int m = listView3.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (listView3.CheckedItems[i].Checked)
                {
                    int id = int.Parse(listView3.CheckedItems[i].SubItems[2].Text);
                    int jf = int.Parse(listView3.CheckedItems[i].SubItems[4].Text);
                    CacheData.GroupMemberInfoList[id].zongjifen += jf;
                    SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注", "'" + listView3.CheckedItems[i].SubItems[0].Text + "','" + CacheData.GroupMemberInfoList[id].Seq + "','" + _kaiJiangData.qihao + "','上分','" + jf.ToString() + "','" + CacheData.GroupMemberInfoList[id].zongjifen + "',''", " liushui_" + CacheData.Seq);
                    cont += CoolQCode.At(CacheData.GroupMemberInfoList[id].GroupMemberBaseInfo.Number);
                    cont += " 上分成功\n余量:" + CacheData.GroupMemberInfoList[id].zongjifen + "\n";

                    string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where seq='" + CacheData.GroupMemberInfoList[id].Seq + "'", "'现有积分'='" + CacheData.GroupMemberInfoList[id].zongjifen + "'");
                    SQLiteHelper.ExecuteNonQuery(delStr);
                    lvChengYuanJiFen.Items[id].SubItems[5].Text = CacheData.GroupMemberInfoList[id].zongjifen.ToString();
                    listView3.CheckedItems[i].Remove();
                    m = listView3.CheckedItems.Count;
                    i = -1;
                    lock (obj)
                    {
                        _zongJiFen += jf;
                    }
                }
            }
            if (cont != "")
            {
                //jzxx(getname(_qrWebWeChat.UserName), cont, send(cont, _group.UserName));
                jzxx(_group, cont, send(_group.GroupId, cont));
            }
            
        }

        /// <summary>
        /// 拒绝上分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button17_Click(object sender, EventArgs e)
        {
            
            DialogResult d = MessageBox.Show("拒绝上分？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            string cont = "";
            int m = listView3.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (listView3.CheckedItems[i].Checked)
                {
                    cont += CoolQCode.At(Convert.ToInt64(listView3.CheckedItems[i].SubItems[12].Text)) + " 上分失败\n余量:\n";
                    listView3.CheckedItems[i].Remove();
                    m = listView3.CheckedItems.Count;
                    i = -1;
                }
            }
            if (cont != "")
            {
                jzxx(_group, cont, send(_group.GroupId, cont));
            }
            
        }

        /// <summary>
        /// 同意下分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button19_Click(object sender, EventArgs e)
        {
            
            DialogResult d = MessageBox.Show("同意下分？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            string cont = "";
            int m = listView4.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (listView4.CheckedItems[i].Checked)
                {
                    int id = int.Parse(listView4.CheckedItems[i].SubItems[2].Text);
                    int jf = int.Parse(listView4.CheckedItems[i].SubItems[4].Text);
                    if (CacheData.GroupMemberInfoList[id].zongjifen - jf >= 0)
                    {
                        CacheData.GroupMemberInfoList[id].zongjifen -= jf;
                        SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注", "'" + listView4.CheckedItems[i].SubItems[0].Text + "','" + CacheData.GroupMemberInfoList[id].Seq + "','" + _kaiJiangData.qihao + "','下分','" + jf.ToString() + "','" + CacheData.GroupMemberInfoList[id].zongjifen + "',''", " liushui_" + CacheData.Seq);
                        cont += CoolQCode.At(CacheData.GroupMemberInfoList[id].GroupMemberBaseInfo.Number) + " 下分成功\n余量:" + CacheData.GroupMemberInfoList[id].zongjifen;

                        string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where seq='" + CacheData.GroupMemberInfoList[id].Seq + "'", "'现有积分'='" + CacheData.GroupMemberInfoList[id].zongjifen + "'");
                        SQLiteHelper.ExecuteNonQuery(delStr);
                        lvChengYuanJiFen.Items[id].SubItems[5].Text = CacheData.GroupMemberInfoList[id].zongjifen.ToString();
                        listView4.CheckedItems[i].Remove();
                        m = listView4.CheckedItems.Count;
                        i = -1;
                        lock (obj)
                        {
                            _zongJiFen -= jf;
                        }
                    }
                    else//说明下分的数据中有重复或者不合法数据，此次下分失败
                    {
                        cont += CoolQCode.At(CacheData.GroupMemberInfoList[id].GroupMemberBaseInfo.Number) + "当前余额不足以此次下分";
                        listView4.CheckedItems[i].Remove();
                        m = listView4.CheckedItems.Count;
                        i = -1;
                    }
                    
                }
            }
            if (cont != "")
            {
                jzxx(_group, cont, send(_group.GroupId, cont));
            }
            
        }

        private void lvChengYuanJiFen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            new 流水明细(CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, Convert.ToInt64(lvChengYuanJiFen.SelectedItems[0].SubItems[11].Text)), CacheData.Seq).Show();
            
        }

        /// <summary>
        /// 拒绝下分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button18_Click(object sender, EventArgs e)
        {
            
            DialogResult d = MessageBox.Show("拒绝下分？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            string cont = "";
            int m = listView4.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (listView4.CheckedItems[i].Checked)
                {
                    cont += CoolQCode.At(Convert.ToInt64(listView3.CheckedItems[i].SubItems[12].Text)) + " 上分失败\n余量:";
                    listView4.CheckedItems[i].Remove();
                    m = listView4.CheckedItems.Count;
                    i = -1;
                }
            }
            if (cont != "")
            {
                jzxx(_group, cont, send( _group.GroupId, cont));
            }
            
        }

        bool isShowServerExpire = false;

        private Thread pingQQThread;
        /// <summary>
        /// 测速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {

            if (pingQQThread == null || pingQQThread.ThreadState != ThreadState.Running)
            {
                pingQQThread = new Thread(pingQQ);
                pingQQThread.Start();
            }

        }
        private void pingQQ()
        {
            try
            {
                //服务器用户过期强制退出
                if (DateTime.Now > 新一城娱乐系统.FeiPan.ServerCommon.serverExpire && isShowServerExpire == false)
                {
                    isShowServerExpire = true;
                    CacheData.IsJianTing = false;
                    MessageBox.Show("用户到期，系统停止监听消息，程序将自动退出。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //程序完全退出
                    System.Environment.Exit(0);
                }

                Ping ping = new Ping();
                PingReply pr = ping.Send("wx.qq.com");
                label27.Text = "当前延迟：" + pr.RoundtripTime.ToString();
                lock (obj)
                {
                    label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + _zongJiFen.ToString();
                }
                SystemSleepManagement.ResetSleepTimer(true);
            }
            catch (Exception ex) { label27.Text = "当前延迟：9999"; }

        }


        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            if (but.Name == "button9")
                foreach (ListViewItem it in lvChengYuanJiFen.Items)
                    it.Checked = true;
            if (but.Name == "button10")
                foreach (ListViewItem it in lvChengYuanJiFen.Items)
                    it.Checked = false;
            if (but.Name == "button15")
                foreach (ListViewItem it in listView3.Items)
                    it.Checked = true;
            if (but.Name == "button20")
                foreach (ListViewItem it in listView4.Items)
                    it.Checked = true;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("清空除玩家积分外的所有数据？", "提示", MessageBoxButtons.OKCancel);
            if (d != DialogResult.OK)
                return;
            SQL.delete("kaijiang_" + CacheData.Seq);
            SQL.delete("liaotian_" + CacheData.Seq);
            SQL.delete("liushui_" + CacheData.Seq);
            SQL.delete("NameInt_" + CacheData.Seq);
            MessageBox.Show("清除完毕！");
        }

        /// <summary>
        /// 实时账单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string xzmx = textBox16.Text.Replace("{实时账单}", sszhangdan());
            fasong(xzmx, false);
        }

        public static Encoding _encoding = System.Text.Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 实时账单
        /// </summary>
        /// <returns></returns>
        private string sszhangdan()
        {
            
            string zd = "";
            
            int groupNum = 0;
            foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
            {
                groupNum++;
                //string str3 = "";
                if (!string.IsNullOrWhiteSpace(jp.bendibeizhu))
                {
                    //byte[] bytes = _encoding.GetBytes(str);
                    //str3 = _encoding.GetString(bytes, 0, 4);
                    zd += jp.bendibeizhu;
                    zd += ":" + jp.conter + jp.zongjifen;
                }
                else
                {
                    //byte[] bytes = _encoding.GetBytes(str2);
                    zd += jp.NickNameShort;
                    zd += ":" + jp.conter + jp.zongjifen;
                }
                if (groupNum % 2 == 0)
                {
                    zd += "\n";
                }
                else
                {
                    zd += "\n";
                }
            }
            
            return zd;
        }

        /// <summary>
        /// 发送自定义消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button21_Click(object sender, EventArgs e)
        {
            string xzmx = "";
            Button but = (Button)sender;
            if (but.Text == "发送消息1")
                xzmx = textBox18.Text;
            if (but.Text == "发送消息2")
                xzmx = textBox19.Text;
            if (but.Text == "发送消息3")
                xzmx = textBox20.Text;
            if (but.Text == "发送消息4")
                xzmx = textBox21.Text;
            if (but.Text == "发送消息5")
                xzmx = textBox22.Text;
            if (xzmx == "")
                return;
            fasong(xzmx, false);
        }

        /// 正在改
        /// <summary>
        /// 发送文字到QQ群
        /// </summary>
        /// <param name="xzmx">要发送的文字信息</param>
        /// <param name="b">酷q中gif、jpg用同一个方法，此参数作废</param>
        public void fasong(string xzmx, bool b)
        {
            if (checkBox3.Checked)//系统设置->图片模式
            {
                Image image = function.TextToBitmap(xzmx, Color.Black, Color.White);
                //返回的路径是酷q中指定的路径
                String imgPath = MyImageUtil.Save(image);
                int msgid = -9999;
                if (!string.IsNullOrWhiteSpace(imgPath))//生成图片成功
                {
                    //发送群消息,信息为图片
                    msgid = CacheData.CoolQApi.SendGroupMsg(_group.GroupId, CoolQCode.Image(imgPath));
                }
               
                
                jzxx(_group, "[图]" + xzmx, ""+msgid);
            }
            else
            {
                string msgid = send(_group.GroupId, xzmx);
                jzxx(_group,CacheData.LoginQQ, xzmx, msgid);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button26_Click(object sender, EventArgs e)
        {
            ConfigHelper.UpdateAppConfig("qd", textBox29.Text);
            ConfigHelper.UpdateAppConfig("dq", textBox30.Text);
            ConfigHelper.UpdateAppConfig("zh", textBox31.Text);
            ConfigHelper.UpdateAppConfig("zhzh", textBox32.Text);
            ConfigHelper.UpdateAppConfig("lhh", textBox33.Text);

            ConfigHelper.UpdateAppConfig("账单zd", checkBox4.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("封盘前zd", checkBox5.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("封盘zd", checkBox6.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("开奖zd", checkBox7.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("下注zd", checkBox8.Checked ? "1" : "0");

            List<KeyVal> cs = new List<KeyVal>();
            KeyVal c1 = new KeyVal(); c1.Key = "账单"; c1.Val = textBox12.Text; cs.Add(c1);
            KeyVal c2 = new KeyVal(); c2.Key = "封盘前"; c2.Val = textBox13.Text + "|" + comboBox1.Text; cs.Add(c2);
            KeyVal c3 = new KeyVal(); c3.Key = "封盘"; c3.Val = textBox14.Text + "|" + comboBox6.Text; cs.Add(c3);
            KeyVal c4 = new KeyVal(); c4.Key = "开奖"; c4.Val = textBox15.Text; cs.Add(c4);
            KeyVal c5 = new KeyVal(); c5.Key = "实时账单"; c5.Val = textBox16.Text; cs.Add(c5);
            KeyVal c6 = new KeyVal(); c6.Key = "下注"; c6.Val = textBox17.Text; cs.Add(c6);

            KeyVal c8 = new KeyVal(); c8.Key = "自定义1"; c8.Val = textBox18.Text; cs.Add(c8);
            KeyVal c9 = new KeyVal(); c9.Key = "自定义2"; c9.Val = textBox19.Text; cs.Add(c9);
            KeyVal c10 = new KeyVal(); c10.Key = "自定义3"; c10.Val = textBox20.Text; cs.Add(c10);
            KeyVal c11 = new KeyVal(); c11.Key = "自定义4"; c11.Val = textBox21.Text; cs.Add(c11);
            KeyVal c12 = new KeyVal(); c12.Key = "自定义5"; c12.Val = textBox22.Text; cs.Add(c12);

            KeyVal c13 = new KeyVal(); c13.Key = "倍数"; c13.Val = textBox24.Text; cs.Add(c13);
            KeyVal c14 = new KeyVal(); c14.Key = "最小"; c14.Val = textBox25.Text; cs.Add(c14);
            KeyVal c15 = new KeyVal(); c15.Key = "最大"; c15.Val = textBox26.Text; cs.Add(c15);
            if (SQL.UPDATE("1", cs, "peizhi") == 1)
            {
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        private void lvChengYuanJiFen_MouseMove(object sender, MouseEventArgs e)
        {
            _aoEvent = e;
        }

        private void lvChengYuanJiFen_MouseHover(object sender, EventArgs e)
        {
            ListViewItem lv = this.lvChengYuanJiFen.GetItemAt(_aoEvent.X, _aoEvent.Y);

            if (lv != null)
            {
                int wh = 0;
                for (int i = 0; i < lvChengYuanJiFen.Columns.Count; i++)
                {
                    wh += lvChengYuanJiFen.Columns[i].Width;
                    if (wh > _aoEvent.X)
                    {
                        if (lv.SubItems[i].Text != "")
                        {
                            toolTip1.Show(lv.SubItems[i].Text, lvChengYuanJiFen, new Point(_aoEvent.X, _aoEvent.Y), 1000);
                            toolTip1.Active = true;
                        }
                        return;
                    }
                }
            }
        }

        private void lvQunXiaoXi_MouseHover(object sender, EventArgs e)
        {
            ListViewItem lv = this.lvQunXiaoXi.GetItemAt(_aoEvent.X, _aoEvent.Y);

            if (lv != null)
            {
                int wh = 0;
                for (int i = 0; i < lvQunXiaoXi.Columns.Count; i++)
                {
                    wh += lvQunXiaoXi.Columns[i].Width;
                    if (wh > _aoEvent.X)
                    {
                        if (lv.SubItems[i].Text != "")
                        {
                            toolTip1.Show(lv.SubItems[i].Text, lvQunXiaoXi, new Point(_aoEvent.X, _aoEvent.Y), 1000);
                            toolTip1.Active = true;
                        }
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 下注修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            
            if (textBox8.Text == "")
                return;
            int m = lvChengYuanJiFen.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (lvChengYuanJiFen.CheckedItems[i].Checked)
                {
                    xiazhu(textBox8.Text.Split(' '),_group,CacheData.GroupMemberInfoList[int.Parse(lvChengYuanJiFen.CheckedItems[i].SubItems[10].Text)]);
                }
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int x = 0;
            if (!int.TryParse(textBox28.Text, out x))
                return;
            int m = listView5.CheckedItems.Count;
            for (int i = 0; i < m; i++)
            {
                if (listView5.CheckedItems[i].Checked)
                {
                    _guiZe.cshxe[int.Parse(listView5.CheckedItems[i].SubItems[3].Text)].xe = x;
                    listView5.CheckedItems[i].SubItems[2].Text = x.ToString();
                }
            }
            _guiZe.sava();
        }
        /// 正在改
        /// <summary>
        /// 是否启动监听群员发送的消息
        /// 不启动的话，不会监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            if (CacheData.IsJianTing)
            {
                CacheData.IsJianTing = false;
                button13.Text = "启动监听";
                状态栏.Text = "已关闭监听";
            }
            else
            {
                button13.Text = "取消监听";
                状态栏.Text = "正在监听群消息.....";
                CacheData.IsJianTing = true;
                fasong("开始！", false);
            }
        }

        /// <summary>
        /// 抓取一期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string q = "";
            q = HttpHelps.Get(Encoding.Default, ShouQuanServer + "/api/lottery/get/1", ref q);

            string qihao = function.middlestring(q, "Sn\":\"", "\"");
            qihao = resetQiHao(qihao);
            string number = function.middlestring(q, "Numbers\":\"", "\"");
            string dateline = function.middlestring(q, "DateLine\":\"", "\"");

            if (dateline != "" && number != "" && qihao != "")
            {
                textBox1.Text = qihao;
                textBox2.Text = number;
                开奖时间.Value = Convert.ToDateTime(dateline);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lvChengYuanJiFen.Focus();
            foreach (ListViewItem it in lvChengYuanJiFen.Items)
            {
                it.Selected = false;
            }
            foreach (ListViewItem it in lvChengYuanJiFen.Items)
            {
                if (textBox3.Text != "" && it.SubItems[0].Text != "")
                {
                    if (it.SubItems[0].Text.IndexOf(textBox3.Text) != -1)
                    {
                        it.Selected = true;
                    }
                }
                if (textBox4.Text != "" && it.SubItems[1].Text != "")
                {
                    if (it.SubItems[1].Text.IndexOf(textBox4.Text) != -1)
                    {
                        it.Selected = true;
                    }
                }
                if (textBox6.Text != "" && it.SubItems[3].Text != "")
                {
                    if (it.SubItems[3].Text.IndexOf(textBox6.Text) != -1)
                    {
                        it.Selected = true;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否关闭窗口", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;

                }
                else
                {
                    //_qrWebWeChat.jieshu = false;
                    //程序完全退出
                    System.Environment.Exit(0);
                }
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("主窗口退出时出现异常，原因："+ex);
            }
            
            
        }

        private void log(string str)
        {
            FileStream fs = new FileStream(MySystemUtil.GetDllRoot() + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + str);
            sw.Close();
            fs.Close();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                btSendMsg.Enabled = false;
            }
            else
            {
                btSendMsg.Enabled = true;
            }
        }

        //账单按钮
        private void button5_Click(object sender, EventArgs e)
        {
            开奖倒计时.Text = "开奖倒计时：正在开奖......";
            timer1.Stop();
            _fengPan = true;
            _kaiJiangData.kjhm = textBox2.Text;
            string[] sz = _kaiJiangData.kjhm.Split(',');
            for (int i = 0; i < 5; i++)
            {
                _kaiJiangData.QD[i] = int.Parse(sz[i]);
            }
            kaiJiang();
            try
            {
                string[] zjxx = jieSuan().Split(new string[] { "||||||" }, StringSplitOptions.None);
                //===================2018-02===================
                string kjwb = textBox12.Text.Replace("{期号}", _kaiJiangData.qihao)
                                        .Replace("{开奖号码}", _kaiJiangData.kjhm)
                                        .Replace("{本期账单}", zjxx[1])
                                         .Replace("{实时账单}", sszhangdan());
                //===================2018-02===================
                fasong(kjwb, false);
            }
            catch (Exception ex) { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                button27.Enabled = false;
            }
            else
            {
                button27.Enabled = true;
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            fasong(textBox13.Text, false);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                button28.Enabled = false;
            }
            else
            {
                button28.Enabled = true;
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            fasong(textBox14.Text, false);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                button29.Enabled = false;
            }
            else
            {
                button29.Enabled = true;
            }
        }

        //开奖按钮
        private void button29_Click(object sender, EventArgs e)
        {
            开奖倒计时.Text = "开奖倒计时：正在开奖......";
            timer1.Stop();
            _fengPan = true;
            _kaiJiangData.kjhm = textBox2.Text;
            string[] sz = _kaiJiangData.kjhm.Split(',');
            for (int i = 0; i < 5; i++)
            {
                _kaiJiangData.QD[i] = int.Parse(sz[i]);
            }
            kaiJiang();
            try
            {
                //===================2018-02===================
                string[] zjxx = jieSuan().Split(new string[] { "||||||" }, StringSplitOptions.None);
                string kjwb = textBox15.Text
                    .Replace("{期号}", _kaiJiangData.qihao)
                    .Replace("{开奖号码}", _kaiJiangData.kjhm)
                    .Replace("{中奖详细}", zjxx[0])
                     .Replace("{实时账单}", sszhangdan());
                //===================2018-02===================
                fasong(kjwb, false);
            }
            catch (Exception ex) { }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                button30.Enabled = false;
            }
            else
            {
                button30.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button30_Click(object sender, EventArgs e)
        {
            
            string xzmx = "";
            int groupNum = 0;
            foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
            {
                if (jp.conter != "")
                {

                    xzmx += jp.NickNameShort + ":" + jp.conter + "  ";
                    groupNum++;
                    if (groupNum % 2 == 0)
                    {
                        xzmx += "\n";
                    }
                }
            }
            xzmx = textBox17.Text.Replace("{下注明细}", xzmx).Replace("{期号}", _kaiJiangData.qihao);
            fasong(xzmx, false);
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }




        private 飞盘 frmFeiPan;
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            //FeiPan();
            if (checkBox9.Checked)
            {
                if (frmFeiPan == null || frmFeiPan.IsDisposed)
                {
                    frmFeiPan = new 飞盘(this);
                }
                frmFeiPan.Show();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FeiPan()
        {
            
            _feiDanJieGuoData = new feiPanJieGuo();
            _feiDanJieGuoData.isSuccess = true;


            if (frmFeiPan != null && !frmFeiPan.IsDisposed && frmFeiPan.IsStart)
            {
                if (frmFeiPan.IsZhiYingZhiKuiStop())
                {
                    function.log("止亏止盈...");
                    frmMessageTimer frmMessage = new frmMessageTimer("已达到止盈/止亏设置的条件！");
                    frmMessage.Show();
                    //止盈止亏不飞单
                    return true;
                }


                xztj mXZTJ = new xztj();

                foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
                {
                    if (jp.conter == "")
                    {
                        continue;
                    }

                    #region 合计成员积分
                    for (int i = 0; i < 10; i++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            mXZTJ.QD[x, i] += jp.xiazhutongji.QD[x, i];
                        }
                    }
                    for (int i = 0; i < 5; i++)//大小单双
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            mXZTJ.DXDS[i, x] += jp.xiazhutongji.DXDS[i, x];
                        }
                    }
                    for (int i = 0; i < 3; i++)//龙虎和
                    {
                        mXZTJ.LHH[i] += jp.xiazhutongji.LHH[i];
                    }

                    for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                    {
                        mXZTJ.ZHDXDS[i] += jp.xiazhutongji.ZHDXDS[i];
                        mXZTJ.ZHZHDXDS[i] += jp.xiazhutongji.ZHZHDXDS[i];
                    }
                    #endregion

                }
                //
                _feiDanJieGuoData = frmFeiPan.StartXiaZhu(mXZTJ, _kaiJiangData.qihao);



                try
                { }
                catch (Exception ex)
                {
                    log("飞盘失败" + ex.Message);
                }
            }
            
            return true;
        }

        /// <summary>
        /// 飞盘失败返还积分
        /// </summary>
        private string feiPanFanHuan()
        {
           

            string zjxx = "";
            string strzd = "";
            
           feiPanJieGuo fpJieGuo = _feiDanJieGuoData;


           //_shangQiKaiJiangData  上期开奖结果
           //加下注积分，减中奖积分

           if (fpJieGuo.isSuccess == true)
           {
               return "";
           }



           foreach (GroupMemberInfoWithBocai jp in CacheData.GroupMemberInfoList)
           {


               List<KeyVal> xiaZhuData = new List<KeyVal>();
               int xiaZhuJifen = 0;//下注返还

               int qd = 0;
               int dxds = 0;
               int zh = 0;
               int zhzh = 0;
               int lhh = 0;

               #region 统计积分
               for (int i = 0; i < 10; i++)
               {
                   for (int x = 0; x < 5; x++)
                   {
                       if (fpJieGuo.QD[x, i] == false)
                       {
                           xiaZhuJifen += jp.shangQiXiaZhu.QD[x, i];
                           KeyVal c = new KeyVal("qd" + (x + 1).ToString() + "_" + i.ToString(), (-jp.shangQiXiaZhu.QD[x, i]).ToString());
                           xiaZhuData.Add(c);
                       }
                       else
                       {
                           KeyVal c = new KeyVal("qd" + (x + 1).ToString() + "_" + i.ToString(), "0");
                           xiaZhuData.Add(c);
                       }
                   }
               }
               for (int x = 0; x < 5; x++)
               {
                   if (fpJieGuo.QD[x, _shangQiKaiJiangData.QD[x]] == false)
                   {
                       qd += jp.shangQiXiaZhu.QD[x, _shangQiKaiJiangData.QD[x]];//球道
                   }
               }

               //大小单双
               for (int i = 0; i < 5; i++)
               {
                   for (int x = 0; x < 4; x++)
                   {
                       if (fpJieGuo.DXDS[i, x] == false)
                       {
                           xiaZhuJifen += jp.shangQiXiaZhu.DXDS[i, x];
                           if (_shangQiKaiJiangData.DXDS[i, x] && jp.shangQiXiaZhu.DXDS[i, x] > 0)
                           {
                               dxds += jp.shangQiXiaZhu.DXDS[i, x];
                           }
                           KeyVal c = new KeyVal("d" + (i + 1).ToString() + "_" + x.ToString(), (-jp.shangQiXiaZhu.DXDS[i, x]).ToString());
                           xiaZhuData.Add(c);
                       }
                       else
                       {
                           KeyVal c = new KeyVal("d" + (i + 1).ToString() + "_" + x.ToString(), "0");
                           xiaZhuData.Add(c);
                       }
                   }
               }

               //总和  总和组合  龙虎和
               for (int i = 0; i < 4; i++)
               {
                   if (fpJieGuo.ZHDXDS[i] == false)
                   {
                       xiaZhuJifen += jp.shangQiXiaZhu.ZHDXDS[i];
                       if (_shangQiKaiJiangData.ZH[i])
                       {
                           zh += jp.shangQiXiaZhu.ZHDXDS[i];
                           KeyVal c = new KeyVal("zh" + i.ToString(), (-jp.shangQiXiaZhu.ZHDXDS[i]).ToString());
                           xiaZhuData.Add(c);
                       }
                       else
                       {
                           KeyVal c = new KeyVal("zh" + i.ToString(), "0");
                           xiaZhuData.Add(c);
                       }
                   }
                   else
                   {
                       KeyVal c = new KeyVal("zh" + i.ToString(), "0");
                       xiaZhuData.Add(c);
                   }


                   if (fpJieGuo.ZHZHDXDS[i] == false)
                   {
                       xiaZhuJifen += jp.shangQiXiaZhu.ZHZHDXDS[i];
                       if (_shangQiKaiJiangData.ZHzh[i])
                       {
                           zhzh += jp.shangQiXiaZhu.ZHZHDXDS[i];
                           KeyVal c1 = new KeyVal("zhzh" + i.ToString(), (-jp.shangQiXiaZhu.ZHZHDXDS[i]).ToString());
                           xiaZhuData.Add(c1);
                       }
                       else
                       {
                           KeyVal c1 = new KeyVal("zhzh" + i.ToString(), "0");
                           xiaZhuData.Add(c1);
                       }
                   }
                   else
                   {
                       KeyVal c1 = new KeyVal("zhzh" + i.ToString(), "0");
                       xiaZhuData.Add(c1);
                   }


                   if (i != 3)
                   {
                       if (fpJieGuo.LHH[i] == false)
                       {

                           xiaZhuJifen += jp.shangQiXiaZhu.LHH[i];
                           if (_shangQiKaiJiangData.LHH[i])
                           {
                               lhh += jp.shangQiXiaZhu.LHH[i];
                               KeyVal l = new KeyVal("LHH" + i.ToString(), (-jp.shangQiXiaZhu.LHH[i]).ToString());
                               xiaZhuData.Add(l);
                           }
                           else
                           {
                               KeyVal l = new KeyVal("LHH" + i.ToString(), "0");
                               xiaZhuData.Add(l);
                           }

                       }
                       else
                       {
                           KeyVal l = new KeyVal("LHH" + i.ToString(), "0");
                           xiaZhuData.Add(l);
                       }
                   }


               }

               #endregion


               #region 增加成员数据


               //开奖后积分
               double zjjf = 0;
               try
               {
                   zjjf = (qd * Convert.ToDouble(textBox29.Text))
                       + (dxds * Convert.ToDouble(textBox30.Text))
                       + (zh * Convert.ToDouble(textBox31.Text))
                       + (zhzh * Convert.ToDouble(textBox32.Text))
                       + (lhh * Convert.ToDouble(textBox33.Text));


                   double zjjfTemp = Math.Round(zjjf, 0);
                   if (zjjfTemp < zjjf && _isJieSuanJinYi)
                   {
                       zjjf += 1;
                   }
                   else if (zjjfTemp > zjjf && _isJieSuanJinYi == false)
                   {
                       zjjf -= 1;
                   }

                   //
                   zjjf = Math.Round(zjjf, 0);

               }
               catch (Exception ex)
               {
                   log(jp.GroupMemberBaseInfo.NickName + "赔率未设置，结算失败");
               }
               if (xiaZhuJifen > 0)
               {
                   int benqiyingkui = (-(int)zjjf + xiaZhuJifen);//反向
                   jp.zongyingkui += benqiyingkui;//加下注
                   jp.zongxiazhu += -xiaZhuJifen;
                   if (benqiyingkui != 0)
                   {
                       //奖金流水保存
                       lock (obj)
                       {
                           _zongJiFen += benqiyingkui;
                       }

                       jp.zongjifen += benqiyingkui;

                       //增加成员数据
                       SQL.INSERT("NickName,seq,期号,类型,积分,剩余积分,备注",
                           "'" + jp.GroupMemberBaseInfo.NickName + "','" + jp.Seq + "','" + _shangQiKaiJiangData.qihao + "','飞盘失败返还','" + benqiyingkui.ToString() + "','" + jp.zongjifen + "',''",
                           " liushui_" + CacheData.Seq);
                       label1.Text = "成员数量：" + _chengYuanShuLiang.ToString() + "    总积分： " + _zongJiFen.ToString();
                   }

                   KeyVal seq = new KeyVal("seq", jp.Seq);
                   xiaZhuData.Add(seq);
                   KeyVal NN = new KeyVal("NickName", jp.GroupMemberBaseInfo.NickName);
                   xiaZhuData.Add(NN);
                   KeyVal qh = new KeyVal("期号", _shangQiKaiJiangData.qihao);
                   xiaZhuData.Add(qh);
                   KeyVal xzwb = new KeyVal("下注文本", "飞盘失败，返还积分");
                   xiaZhuData.Add(xzwb);
                   KeyVal yk = new KeyVal("盈亏", benqiyingkui.ToString());
                   xiaZhuData.Add(yk);
                   KeyVal xzjf = new KeyVal("下注积分", (-xiaZhuJifen).ToString());
                   xiaZhuData.Add(xzjf);
                   KeyVal jsjf = new KeyVal("结算后积分", jp.zongjifen.ToString());
                   xiaZhuData.Add(jsjf);

                   //
                   SQL.INSERT(xiaZhuData, " NameInt_" + CacheData.Seq);




                   #region 统计积分

                   //更新用户总积分
                   string delStr = string.Format(@"UPDATE Friends_" + CacheData.Seq + " SET {0} where seq='" + jp.Seq + "'",
                       "'现有积分'='" + jp.zongjifen.ToString() + "','总盈亏'='" + jp.zongyingkui.ToString() + "','总下注'='"
                       + jp.zongxiazhu.ToString() + "'");

                   SQLiteHelper.ExecuteNonQuery(delStr);


                   //显示更新后积分，获取统计
                   _group.zongyingkui += jp.zongyingkui;
                   _group.benqixiazhu += jp.benqixiazhu;
                   lvChengYuanJiFen.Items[jp.Id].SubItems[5].Text = jp.zongjifen.ToString();
                   lvChengYuanJiFen.Items[jp.Id].SubItems[6].Text = jp.zongyingkui.ToString();
                   DateTime time1 = DateTime.Now.Date;
                   DateTime time2 = time1.AddDays(1);
                   try
                   {
                       DataTable deset = SQLiteHelper.ExecuteDataTable("select sum(盈亏)  from NameInt_" + CacheData.Seq + " where seq='" + jp.Seq
                           + "' and Time BETWEEN '" + time1.ToString("yyyy-MM-dd 00:00:00") + "' AND '" + time2.ToString("yyyy-MM-dd 00:00:00")
                           + "' group by seq", null);
                       lvChengYuanJiFen.Items[jp.Id].SubItems[7].Text = deset.Rows[0][0].ToString();
                   }
                   catch (Exception ex)
                   {
                   }

                   #endregion


                   lvChengYuanJiFen.Items[jp.Id].SubItems[6].Text = jp.zongyingkui.ToString();//总积分
               }
               try
               { }
               catch (Exception ex)
               {
                   log(jp.GroupMemberBaseInfo.NickName + "玩家返还结算失败");
               }

               #endregion
           }
          

            return zjxx + "||||||" + strzd;
        }



        private void playPointSound()
        {

            try
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = "tmpe/point.wav";
                player.Load();
                player.Play();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 刷新群列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click_1(object sender, EventArgs e)
        {

            GroupInfo currentSelectedGroup = CacheData.CurrentGroupList[CacheData.SelectedGroupIndex];
            //MyLogUtil.ToLogFotTest("#####进入主界面后，刷新群员列表选中的群：" + currentSelectedGroup.GroupName + "____" + currentSelectedGroup.GroupId+"___"+ comboBox2.SelectedIndex);
            CoolQApiExtend.GetGroupMemberListAndCache(CacheData.CoolQApi, currentSelectedGroup.GroupId);

            if (_dgvThread == null || _dgvThread.ThreadState != ThreadState.Running)
            {
                _chengYuanShuLiang = 0;
                _dgvThread = new Thread(dgv2);
                _dgvThread.Start();
            }

            //new Thread(dgv2).Start();
            
        }
        /// <summary>
        /// 期号不足位数补0
        /// </summary>
        /// <param name="qihao"></param>
        /// <returns></returns>
        public string resetQiHao(string qihao)
        {
            try
            {
                if (qihao.Length == 10)
                {
                    string d = qihao.Substring(0, 8);
                    string num = qihao.Substring(8, qihao.Length - 8);
                    return d + num.PadLeft(3, '0');
                }
                else if (qihao.Length == 12)
                {
                    string d = qihao.Substring(0, 8);
                    string num = qihao.Substring(8, qihao.Length - 8);
                    int numI = int.Parse(num);
                    return d + numI.ToString().PadLeft(3, '0');
                }
                else
                {
                    return qihao;
                }
            }
            catch
            {
                return qihao;
            }
        }

        /// <summary>
        /// 实际下注
        /// </summary>
        /// <param name="xzTongJi"></param>
        /// <param name="fpJieGuo"></param>
        /// <returns></returns>
        public int getXiaZhuTongJi(xztj xzTongJi, feiPanJieGuo fpJieGuo)
        {
            int xiaZhu = 0;

            #region 统计积分
            //单球
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (xzTongJi.QD[i, x] > 0 && fpJieGuo.QD[i, x] == true)
                    {
                        xiaZhu += xzTongJi.QD[i, x];
                    }
                }
            }

            //大小单双
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (xzTongJi.DXDS[i, x] > 0 && fpJieGuo.DXDS[i, x] == true)
                    {
                        xiaZhu += xzTongJi.DXDS[i, x];
                    }
                }
            }

            //总和  总和组合  龙虎和
            for (int i = 0; i < 4; i++)
            {
                if (xzTongJi.ZHDXDS[i] > 0 && fpJieGuo.ZHDXDS[i] == true)
                {

                    xiaZhu += xzTongJi.ZHDXDS[i];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (xzTongJi.ZHZHDXDS[i] > 0 && fpJieGuo.ZHZHDXDS[i] == true)
                {
                    xiaZhu += xzTongJi.ZHZHDXDS[i];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (xzTongJi.LHH[i] > 0 && fpJieGuo.LHH[i] == true)
                {
                    xiaZhu += xzTongJi.LHH[i];
                }
            }

            #endregion

            return xiaZhu;
        }

        public string getXiaZhuTongJiCon(xztj xzTongJi, feiPanJieGuo fpJieGuo)
        {
            string xiaZhuCon = "";

            #region 统计积分
            //单球
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (xzTongJi.QD[i, x] > 0 && fpJieGuo.QD[i, x] == true)
                    {
                        xiaZhuCon += (i + 1).ToString() + "/" + x + "/" + xzTongJi.QD[i, x] + ";";
                    }
                }
            }

            //大小单双
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (xzTongJi.DXDS[i, x] > 0 && fpJieGuo.DXDS[i, x] == true)
                    {
                        xiaZhuCon += (i + 1).ToString();
                        if (x == 0) xiaZhuCon += "大";
                        if (x == 1) xiaZhuCon += "小";
                        if (x == 2) xiaZhuCon += "单";
                        if (x == 3) xiaZhuCon += "双";
                        xiaZhuCon += xzTongJi.DXDS[i, x] + ";";
                    }
                }
            }

            //总和  总和组合  龙虎和
            for (int i = 0; i < 4; i++)
            {
                if (xzTongJi.ZHDXDS[i] > 0 && fpJieGuo.ZHDXDS[i] == true)
                {
                    if (i == 0) xiaZhuCon += "大";
                    if (i == 1) xiaZhuCon += "小";
                    if (i == 2) xiaZhuCon += "单";
                    if (i == 3) xiaZhuCon += "双";
                    xiaZhuCon += xzTongJi.ZHDXDS[i] + ";";
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (xzTongJi.ZHZHDXDS[i] > 0 && fpJieGuo.ZHZHDXDS[i] == true)
                {
                    if (i == 0) xiaZhuCon += "大单";
                    if (i == 1) xiaZhuCon += "大双";
                    if (i == 2) xiaZhuCon += "小单";
                    if (i == 3) xiaZhuCon += "小双";
                    xiaZhuCon += xzTongJi.ZHZHDXDS[i] + ";";
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (xzTongJi.LHH[i] > 0 && fpJieGuo.LHH[i] == true)
                {
                    if (i == 0) xiaZhuCon += "龙";
                    if (i == 1) xiaZhuCon += "虎";
                    if (i == 2) xiaZhuCon += "和";
                    xiaZhuCon += xzTongJi.LHH[i] + ";";
                }
            }

            #endregion

            return xiaZhuCon;
        }
    }
}