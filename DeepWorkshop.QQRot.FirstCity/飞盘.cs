using AI.Bll;
using Bll;
using Dal;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication4;
using 新一城娱乐系统.FeiPan;

namespace 新一城娱乐系统
{
    public partial class 飞盘 : Form
    {
        #region 变量

        private Thread connectThread;
        public Form1 frmMain = null;
        private String Seq;//当群信息为空的时候，启动此Seq

        private GroupInfo _mainGroup;

        private const string serShunFeng = "http://mem1.paeghe214.dqbpkj.com:88/";

        private const string serYongLi = "http://w88.ukk556.com/#53888";

        public bool IsStart
        {
            get
            {
                return cbStart.Checked;
            }
        }

        #endregion 变量

        #region 配置

        public 飞盘(Form1 formPar)
        {
            InitializeComponent();

            this.frmMain = formPar;
            if (formPar == null)
            {
                _mainGroup = new GroupInfo();
                Seq = "ABC";
            }
            else
            {
                /*_mainGroup = frmMain._group;*/
            }
            if (ServerCommon.isDebug == false)
            {
                button2.Visible = false;
            }


        }

        private void 飞盘_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            #region 初始化配置

            cbFengPanFeiDan.Items.Clear();
            cbTryConNum.Items.Clear();

            for (int i = 1; i < 46; i++)
            {
                cbFengPanFeiDan.Items.Add(i);
            }
            for (int i = 1; i < 11; i++)
            {
                cbTryConNum.Items.Add(i);
            }
            //
            cmbSer.Items.Clear();
            cmbSer.Items.Add("顺丰");
            cmbSer.Items.Add("永利");
            //
            LoadServerData();

            //
            try
            {
                #region 配置

                if (ConfigHelper.GetAppConfig("FengPanFeiDan") != null)
                {
                    //
                    cbFengPanFeiDan.Text = ConfigHelper.GetAppConfig("FengPanFeiDan");
                    cbFailSound.Checked = ConfigHelper.GetAppConfig("FailSound") == "1";
                    //
                    cbIsTryCon.Checked = ConfigHelper.GetAppConfig("IsTryCon") == "1";
                    cbTryConNum.Text = ConfigHelper.GetAppConfig("TryConNum");
                    //
                    cbTouZhuRecordNum.Text = ConfigHelper.GetAppConfig("TouZhuRecordNum");
                    //
                    cbIsZhiYing.Checked = ConfigHelper.GetAppConfig("IsZhiYing") == "1";
                    cbZhiYingNum.Text = ConfigHelper.GetAppConfig("ZhiYingNum");
                    cbIsZhiKui.Checked = ConfigHelper.GetAppConfig("IsZhiKui") == "1";
                    cbZhiKuiNum.Text = ConfigHelper.GetAppConfig("ZhiKuiNum");
                }
                else
                {
                    cbFengPanFeiDan.Text = "30";
                    cbTryConNum.Text = "3";
                    cbTouZhuRecordNum.Text = "500";
                }

                #endregion 配置
            }
            catch (Exception ex)
            {
                function.log("读取配置错误" + ex.Message);
            }

            #endregion 初始化配置
        }

        private void LoadServerData()
        {
            //加载服务器
            lvSerState.Items.Clear();
            DataTable dtServer = SQLiteHelper.ExecuteDataTable("select * from fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq), null);
            if (dtServer.Rows.Count > 0)
            {
                foreach (DataRow row in dtServer.Rows)
                {
                    ListViewItem serItem = lvSerState.Items.Add(row["ID"].ToString());
                    serItem.SubItems.Add(row["类型"].ToString());
                    serItem.SubItems.Add(row["服务器地址"].ToString());
                    serItem.SubItems.Add(row["用户名"].ToString());
                    serItem.SubItems.Add("停止");
                    serItem.SubItems.Add("删除");
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveConfig_Click(object sender, EventArgs e)
        {
            //
            ConfigHelper.UpdateAppConfig("FengPanFeiDan", cbFengPanFeiDan.Text);
            ConfigHelper.UpdateAppConfig("FailSound", cbFailSound.Checked ? "1" : "0");
            //
            ConfigHelper.UpdateAppConfig("IsTryCon", cbIsTryCon.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("TryConNum", cbTryConNum.Text);
            //
            ConfigHelper.UpdateAppConfig("TouZhuRecordNum", cbTouZhuRecordNum.Text);
            //
            ConfigHelper.UpdateAppConfig("IsZhiYing", cbIsZhiYing.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("ZhiYingNum", cbZhiYingNum.Text);
            ConfigHelper.UpdateAppConfig("IsZhiKui", cbIsZhiKui.Checked ? "1" : "0");
            ConfigHelper.UpdateAppConfig("ZhiKuiNum", cbZhiKuiNum.Text);
        }

        #endregion 配置

        #region 连接

        private void button2_Click(object sender, EventArgs e)
        {
            //ServerFeiPan.login("http://w88.ukk556.com/#53888", "ffgggg7788", "jun1357@", "永利");

            Text = function.filtetStingSpecial("567567567桀#@$R#FDFDsdasd桀༺ི田野༻ྀ=͟͟͞͞恩");
        }



        private void trmCheckConnection_Tick(object sender, EventArgs e)
        {
            if (frmMain != null)
            {
                //ShowTipOnWindow("开始提交下注");
                //lblState.Text = frmMain.FengPan ? "aaa" : "bbb";
            }
            bool isSuccess = false;

            if (cbStart.Checked)
            {
                isSuccess = ServerFeiPan.checkLogin();
            }
            if (isSuccess == true)
            {
                ServerFeiPan.getYuE();
                lblState.Text = "使用中";
            }
            else
            {
                lblState.Text = "已停止";
            }
            lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
            lblServer.Text = ServerFeiPan.FeidanUrl;
            lblName.Text = ServerFeiPan.LoginName;

        }


        private void trmChaXun_Tick(object sender, EventArgs e)
        {

            //查询最新
            chaXunZuiXin();
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string leixin = cmbSer.Text;

            if (leixin == "顺丰")
            {
                ServerFeiPan.login(serShunFeng, txtUserName.Text, txtUserPass.Text, "顺丰");
            }
            else if (leixin == "永利")
            {
                ServerFeiPan.login(serYongLi, txtUserName.Text, txtUserPass.Text, "永利");
            }

            if (cbIsTryCon.Checked && ServerFeiPan.IsLoginSuccess == false)
            {
                MessageBox.Show("登录失败");
            }
            if (ServerFeiPan.IsLoginSuccess)
            {
                lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
                lblServer.Text = ServerFeiPan.FeidanUrl;
                lblName.Text = ServerFeiPan.LoginName;
                lblState.Text = "使用中";
            }
            else
            {
                lblState.Text = "链接失败";
            }

            //已登录
            function.log("已经登录 【btnLogin】" + ServerFeiPan.ServerType);
        }

        #endregion 连接

        #region 连接服务器函数

        /// <summary>
        /// 下注
        /// </summary>
        public feiPanJieGuo StartXiaZhu(xztj xztj, string qiHao)
        {
            feiPanJieGuo fpjgData = new feiPanJieGuo();
            fpjgData.serverUrl = "";
            fpjgData.yuE = "0";
            lblYuE.Text = "0";
            lblServer.Text = "";
            lblName.Text = "";
            lblState.Text = "链接中";

            if (cbStart.Checked == true)
            {
                string useServer = "";
                //尝试下注
                if (ServerFeiPan.checkLogin() == false)
                {
                    ServerFeiPan.loginAgain();
                }
                if (ServerFeiPan.checkLogin() == true)
                {
                    if (ServerFeiPan.ServerType.Equals("顺丰"))
                    {
                        fpjgData.isSuccess = true;
                        fpjgData.serverUrl += ServerFeiPan.FeidanUrl + ";";
                        fpjgData = ServerFeiPan.xiaZhu_shunfen(xztj, qiHao, fpjgData);
                    }
                    else if (ServerFeiPan.ServerType.Equals("永利"))
                    {
                        fpjgData.isSuccess = true;
                        fpjgData.serverUrl += ServerFeiPan.FeidanUrl + ";";
                        fpjgData = ServerFeiPan.xiaZhu_yongli(xztj, qiHao, fpjgData);
                    }
                    useServer = ServerFeiPan.ServerType + ServerFeiPan.LoginName;
                }
                foreach (ListViewItem item in lvSerState.Items)
                {
                    item.SubItems[4].Text = "已停止";
                }
                //重新遍历服务器列表
                if (fpjgData.isSuccess == false)
                {
                    //加载服务器
                    DataTable dtServer = SQLiteHelper.ExecuteDataTable("select * from fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq), null);
                    if (dtServer.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtServer.Rows)
                        {
                            string _serverType = row["类型"].ToString();
                            string _serverUrl = row["服务器地址"].ToString();
                            string _userName = row["用户名"].ToString();
                            string _userPass = row["密码"].ToString();
                            if (_serverType.Equals(_serverType + _userName))
                            {
                                function.log("提交失败：" + _serverType + _userName);
                                continue;
                            }
                            if (_serverType.Equals("顺丰"))
                            {
                                ServerFeiPan.login(_serverUrl, _userName, _userPass, _serverType);
                                if (ServerFeiPan.IsLoginSuccess == false && cbIsTryCon.Checked)
                                {
                                    frmMessageTimer frmMessage = new frmMessageTimer("服务器" + _serverUrl + "不能使用");
                                    frmMessage.Show();
                                    Application.DoEvents();
                                }

                                if (ServerFeiPan.IsLoginSuccess == true)
                                {
                                    fpjgData = ServerFeiPan.xiaZhu_shunfen(xztj, qiHao, fpjgData);
                                }

                            }
                            else if (_serverType.Equals("永利"))
                            {
                                ServerFeiPan.login(_serverUrl, _userName, _userPass, _serverType);

                                if (ServerFeiPan.IsLoginSuccess == false && cbIsTryCon.Checked)
                                {
                                    frmMessageTimer frmMessage = new frmMessageTimer("服务器" + _serverUrl + "不能使用");
                                    frmMessage.Show();
                                    Application.DoEvents();
                                }

                                if (ServerFeiPan.IsLoginSuccess == true)
                                {
                                    fpjgData = ServerFeiPan.xiaZhu_yongli(xztj, qiHao, fpjgData);
                                }

                            }

                            //
                            if (ServerFeiPan.IsLoginSuccess)
                            {
                                lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
                                lblServer.Text = ServerFeiPan.FeidanUrl;
                                lblName.Text = ServerFeiPan.LoginName;
                                lblState.Text = "使用中";
                            }
                            else
                            {
                                lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
                                lblServer.Text = ServerFeiPan.FeidanUrl;
                                lblName.Text = ServerFeiPan.LoginName;
                                lblState.Text = "已停止";
                            }
                            foreach (ListViewItem item in lvSerState.Items)
                            {
                                if (item.SubItems[2].Text.Equals(_serverUrl))
                                {
                                    item.SubItems[4].Text = ServerFeiPan.IsLoginSuccess ? "使用中" : "已停止";
                                }
                            }
                            //
                            if (fpjgData.isSuccess)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                fpjgData = ServerCommon.SetFeiPanJieGuo(fpjgData, true);
                fpjgData.errorMessage = "未开启";
                //关闭直接返回
                return fpjgData;
            }

            if (fpjgData.isSuccess == false)
            {
                playFaileSound();

                if (cbStart.Checked && ServerFeiPan.IsLoginSuccess == false)
                {
                    frmMessageTimer frmMessage = new frmMessageTimer("盘口可能坏了，请检查是否可用？");
                    frmMessage.Show();
                    cbStart.Checked = false;
                }
            }


            //for (int i = 0; i < 4; i++)//总和组合
            //{
            //    fpjgData.ZHZHDXDS[i] = true;
            //}

            //
            //lblYuE.Text = fpjgData.yuE;

            //插入
            feiPanJieGuoInsert(xztj, fpjgData, qiHao);

            //查询最新
            chaXunZuiXin();

            return fpjgData;
            //
            //string result = HttpHelps.Post("", _feidanUrl + "/user/cql_cqsc_lm.aspx?t=all",
            //     _webCookie, Encoding.Default);
        }

        private void feiPanJieGuoInsert(xztj mXZTJ, feiPanJieGuo fpJieGuo, string qiHao)
        {
            #region 插入数据

            List<KeyVal> xiaZhuData = new List<KeyVal>();
            int qd = 0;
            int dxds = 0;
            int zh = 0;
            int zhzh = 0;
            int lhh = 0;
            string sVal = "";
            int totalXiazu = 0;
            //球道
            for (int i = 0; i < 10; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (fpJieGuo.QD[x, i])
                    {
                        qd += mXZTJ.QD[x, i];
                        sVal = mXZTJ.QD[x, i].ToString();
                    }
                    else
                    {
                        sVal = mXZTJ.QD[x, i] == 0 ? "0" : fpJieGuo.errorMessage;
                    }

                    KeyVal c = new KeyVal("qd" + (x + 1).ToString() + "_" + i.ToString(), sVal);
                    xiaZhuData.Add(c);
                    //
                    totalXiazu += mXZTJ.QD[x, i];
                }
            }
            //大小单双
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (fpJieGuo.DXDS[i, x])
                    {
                        dxds += mXZTJ.DXDS[i, x];
                        sVal = mXZTJ.DXDS[i, x].ToString();
                    }
                    else
                    {
                        sVal = mXZTJ.DXDS[i, x] == 0 ? "0" : fpJieGuo.errorMessage;
                    }
                    KeyVal c = new KeyVal("d" + (i + 1).ToString() + "_" + x.ToString(), sVal);
                    xiaZhuData.Add(c);
                    //
                    totalXiazu += mXZTJ.DXDS[i, x];
                }
            }
            //总和  总和组合
            for (int i = 0; i < 4; i++)
            {
                if (fpJieGuo.ZHDXDS[i])
                {
                    zh += mXZTJ.ZHDXDS[i];
                    sVal = mXZTJ.ZHDXDS[i].ToString();
                }
                else
                {
                    sVal = mXZTJ.ZHDXDS[i] == 0 ? "0" : fpJieGuo.errorMessage;
                }
                KeyVal c = new KeyVal("zh" + i.ToString(), sVal);
                xiaZhuData.Add(c);
                //
                totalXiazu += mXZTJ.ZHDXDS[i];

                //
                if (fpJieGuo.ZHZHDXDS[i])
                {
                    zhzh += mXZTJ.ZHZHDXDS[i];
                    sVal = mXZTJ.ZHZHDXDS[i].ToString();
                }
                else
                {
                    sVal = mXZTJ.ZHZHDXDS[i] == 0 ? "0" : fpJieGuo.errorMessage;
                }
                KeyVal c1 = new KeyVal("zhzh" + i.ToString(), sVal);
                xiaZhuData.Add(c1);
                //
                totalXiazu += mXZTJ.ZHZHDXDS[i];
            }
            // 龙虎和
            for (int i = 0; i < 3; i++)
            {
                if (fpJieGuo.LHH[i])
                {
                    lhh += mXZTJ.LHH[i];
                    sVal = mXZTJ.LHH[i].ToString();
                }
                else
                {
                    sVal = mXZTJ.LHH[i] == 0 ? "0" : fpJieGuo.errorMessage;
                }
                KeyVal l = new KeyVal("LHH" + i.ToString(), sVal);
                xiaZhuData.Add(l);
                //
                totalXiazu += mXZTJ.LHH[i];
            }

            //开奖后积分
            double zjjf = 0;
            zjjf = qd + dxds + zh + zhzh + lhh;

            #endregion 插入数据

            #region 增加数据

            KeyVal qh = new KeyVal("期号", qiHao);
            xiaZhuData.Add(qh);

            KeyVal fwq = new KeyVal("服务器地址", fpJieGuo.serverUrl);
            xiaZhuData.Add(fwq);

            KeyVal jssj = new KeyVal("提交结束时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            xiaZhuData.Add(jssj);

            KeyVal tjhj = new KeyVal("成功提交合计", zjjf.ToString());
            xiaZhuData.Add(tjhj);

            KeyVal dqze = new KeyVal("当期注额", totalXiazu.ToString());
            xiaZhuData.Add(dqze);

            KeyVal syje = new KeyVal("账号剩余金额", fpJieGuo.yuE);
            xiaZhuData.Add(syje);

            KeyVal jg = new KeyVal("提交结果", fpJieGuo.isSuccess ? "提交成功" : fpJieGuo.errorMessage);
            xiaZhuData.Add(jg);
            //
            SQL.INSERT(xiaZhuData, " FeiPan_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq));

            #endregion 增加数据
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void lvSerState_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem lvi = lvSerState.GetItemAt(e.X, e.Y);
                if (lvi != null)
                {
                    ListViewItem.ListViewSubItem lvSub = lvi.GetSubItemAt(e.X, e.Y);
                    if (lvSub != null)
                    {
                        int subIndex = lvi.SubItems.IndexOf(lvSub);

                        if (subIndex == 5)
                        {
                            string sId = lvi.SubItems[0].Text;

                            //删除
                            string strSql = string.Format(@"DELETE FROM fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq) + " where ID='" + sId + "'");
                            SQLiteHelper.ExecuteNonQuery(strSql);

                            //
                            MessageBox.Show("删除成功");
                            LoadServerData();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("删除失败");
            }
        }

        /// <summary>
        /// 开启登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbStart_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStart.Checked)
            {
                if (connectThread == null || connectThread.ThreadState != ThreadState.Running)
                {
                    connectThread = new Thread(TryConnect);
                    connectThread.Start();
                }
            }
        }

        public void TryConnect()
        {



            if (ServerFeiPan.checkLogin() == true)
            {
                //已登录
                function.log("已经登录 【TryConnect】");
                return;
            }


            //加载服务器
            DataTable dtServer = SQLiteHelper.ExecuteDataTable("select * from fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq), null);
            if (dtServer.Rows.Count > 0)
            {
                foreach (DataRow row in dtServer.Rows)
                {
                    string _serverType = row["类型"].ToString();
                    string _serverUrl = row["服务器地址"].ToString();
                    string _userName = row["用户名"].ToString();
                    string _userPass = row["密码"].ToString();

                    //
                    System.Windows.Forms.ListViewItem.ListViewSubItem subItem = null;

                    foreach (ListViewItem item in lvSerState.Items)
                    {
                        if (item.SubItems[2].Text.Equals(_serverUrl))
                        {
                            subItem = item.SubItems[4];
                        }
                    }
                    subItem.Text = "链接中..";
                    if (subItem != null)
                    {
                        if (_serverType.Equals("顺丰"))
                        {
                            ServerFeiPan.login(_serverUrl, _userName, _userPass, _serverType);
                        }
                        else if (_serverType.Equals("永利"))
                        {
                            ServerFeiPan.login(_serverUrl, _userName, _userPass, _serverType);
                        }
                        //
                        subItem.Text = ServerFeiPan.IsLoginSuccess ? "使用中" : "已停止";
                        if (ServerFeiPan.IsLoginSuccess)
                        {
                            lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
                            lblServer.Text = ServerFeiPan.FeidanUrl;
                            lblName.Text = ServerFeiPan.LoginName;
                            lblState.Text = "使用中";
                            break;
                        }
                        else
                        {
                            lblYuE.Text = ServerFeiPan.KeYongYuE.ToString();
                            lblServer.Text = ServerFeiPan.FeidanUrl;
                            lblName.Text = ServerFeiPan.LoginName;
                            lblState.Text = "已停止";
                        }
                    }

                    function.log("测试登录 【TryConnect】" + _serverType);

                }
            }


            //
            if (cbStart.Checked && ServerFeiPan.IsLoginSuccess == false)
            {
                playFaileSound();
                frmMessageTimer frmMessage = new frmMessageTimer("盘口可能坏了，请检查是否可用？");
                frmMessage.Show();
                cbStart.Checked = false;
            }


        }

        /// <summary>
        /// 止亏止盈
        /// </summary>
        /// <returns></returns>
        public bool IsZhiYingZhiKuiStop()
        {
            bool isStop = false;
            decimal zhiYing = 0;
            decimal zhiKui = 0;
            try
            {
                zhiYing = int.Parse(cbZhiYingNum.Text);
                zhiKui = int.Parse(cbZhiKuiNum.Text);

                if (cbIsZhiYing.Checked && ServerFeiPan.KeYongYuE > zhiYing)
                {
                    isStop = true;
                }
                else if (cbIsZhiKui.Checked && ServerFeiPan.KeYongYuE < zhiKui)
                {
                    isStop = true;
                }
                else
                {
                    isStop = false;
                }
            }
            catch
            {
                isStop = false;
            }
            function.log("止盈止亏：" + (isStop ? "停止" : "不停止")
                + " " + (cbIsZhiYing.Checked ? "1" : "0") + " " + zhiYing
                 + " " + (cbIsZhiKui.Checked ? "1" : "0") + " " + zhiKui
                );
            return isStop;
        }
        #endregion 连接服务器函数

        #region 操作

        private void llBaoPan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new 报盘(_mainGroup).Show();
        }

        /// <summary>
        /// 保存服务器密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            //
            int ind = cmbSer.SelectedIndex;
            try
            {
                if (ind > -1)
                {
                    string leixin = cmbSer.Text;
                    string serUrl = "";

                    //if (leixin == "顺丰")
                    //{
                    //    serUrl = "http://mem1.paeghe214.dqbpkj.com:88/";
                    //}
                    //else if (leixin == "永利")
                    //{
                    //    serUrl = "http://w88.ukk556.com/#53888";
                    //}
                    serUrl = txtServerUrl.Text;

                    List<KeyVal> xiaZhuData = new List<KeyVal>();
                    KeyVal seq = new KeyVal("类型", leixin);
                    xiaZhuData.Add(seq);
                    KeyVal NN = new KeyVal("服务器地址", serUrl);
                    xiaZhuData.Add(NN);
                    KeyVal qh = new KeyVal("用户名", txtUserName.Text);
                    xiaZhuData.Add(qh);
                    KeyVal xzwb = new KeyVal("密码", txtUserPass.Text);
                    xiaZhuData.Add(xzwb);

                    //
                    string tabelint = SQL.tabelint(" fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq) + " where 类型='" + leixin + "'");
                    int count = int.Parse(tabelint);
                    if (count > 0 && 1 == 2)
                    {
                        //更新用户总积分
                        string strSql = string.Format(@"UPDATE fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq) + " SET {0} where 类型='" + leixin + "'",
                            "'服务器地址'='" + serUrl + "','用户名'='" + txtUserName.Text + "','密码'='" + txtUserPass.Text + "'");
                        SQLiteHelper.ExecuteNonQuery(strSql);
                    }
                    else
                    {
                        SQL.INSERT(xiaZhuData, " fuwuqi_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq));
                    }

                    //
                    MessageBox.Show("保存成功");
                    LoadServerData();
                }
                else
                {
                    //
                    MessageBox.Show("先选择一个服务器");
                }
            }
            catch
            {
                MessageBox.Show("保存失败");
            }
        }

        /// <summary>
        /// 获取密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //加载服务器
            //int ind = cmbSer.SelectedIndex;

            //if (ind > -1)
            //{
            //    string leixin = (ind + 1).ToString();
            //    DataTable dtServer = SQLiteHelper.ExecuteDataTable("select * from fuwuqi_" + _mainGroup.seq +
            //                " where 类型='" + leixin + "'", null);
            //    if (dtServer.Rows.Count > 0)
            //    {
            //        DataRow row = dtServer.Rows[0];
            //        txtUserName.Text = row["用户名"].ToString();
            //        txtUserPass.Text = row["密码"].ToString();
            //    }
            //}
        }

        /// <summary>
        /// 查询最新飞盘
        /// </summary>
        private void chaXunZuiXin()
        {
            lvFeidanList.Items.Clear();

            ListViewItem item;

            DataTable dtFeipan = SQL.SELECTdata(" WHERE 1=1 ORDER BY id DESC", "FeiPan_" + (String.IsNullOrWhiteSpace(Seq) ? CacheData.Seq : Seq));
            if (dtFeipan.Rows.Count > 0)
            {
                DataRow dr = dtFeipan.Rows[0];
                string heji = "";
                int sum = 0;
                string vals = "";
                int vali = 0;
                for (int i = 0; i < 5; i++)
                {
                    item = new ListViewItem("球道" + (i + 1));
                    sum = 0;
                    vals = "";
                    vali = 0;

                    for (int j = 0; j < 4; j++)
                    {
                        //大小单双
                        vals = dr["d" + (i + 1) + "_" + j].ToString();
                        item.SubItems.Add(vals);
                        //
                        int.TryParse(vals, out vali);
                        sum += vali;
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        //球道
                        vals = dr["qd" + (i + 1) + "_" + j].ToString();
                        item.SubItems.Add(vals);
                        //
                        int.TryParse(vals, out vali);
                        sum += vali;
                        if (i == 2 && j == 6)
                        {

                        }
                    }
                    item.SubItems.Add(sum.ToString());
                    lvFeidanList.Items.Add(item);
                }
                //
                item = new ListViewItem("累计");
                for (int j = 0; j < 4; j++)
                {
                    sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        //大小单双
                        vals = dr["d" + (i + 1) + "_" + j].ToString();
                        //
                        int.TryParse(vals, out vali);
                        sum += vali;
                    }

                    item.SubItems.Add(sum.ToString());
                }
                for (int j = 0; j < 10; j++)
                {
                    sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        //球道
                        vals = dr["qd" + (i + 1) + "_" + j].ToString();
                        //
                        int.TryParse(vals, out vali);
                        sum += vali;
                    }
                    item.SubItems.Add(sum.ToString());
                }
                lvFeidanList.Items.Add(item);


                //
                heji += "龙：" + dr["LHH0"] + " 虎：" + dr["LHH1"] + " 合：" + dr["LHH2"] + "\r\n";
                heji += "和(大：" + dr["ZH0"] + " 小：" + dr["ZH1"] + " 单：" + dr["ZH2"] + " 双：" + dr["ZH3"] + ")\r\n";
                heji += "总和组合(大单：" + dr["ZHZH0"] + " 大双：" + dr["ZHZH1"] + " 小单：" + dr["ZHZH2"] + " 小双：" + dr["ZHZH3"] + ")\r\n\r\n";

                lblHeJi.Text = heji;
                lblDanQianZhuE.Text = dr["当期注额"].ToString();
                lblTiJiaoE.Text = dr["成功提交合计"].ToString();
                lblGridTitle.Text = dr["期号"].ToString() + "（期号）下注提交明细";
            }
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="text"></param>
        private void ShowTipOnWindow(string text)
        {
            this.Text = text;
        }

        private void playFaileSound()
        {
            if (cbFailSound.Checked)
            {
                try
                {
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = "tmpe/faile.wav";
                    player.Load();
                    player.Play();
                }
                catch
                {

                }

            }

        }

        private void 飞盘_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.frmMain != null)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        #endregion 操作




    }

    #region 获取分析赔率大小单双序号

    //大小单双序号
    //for (int i = 0; i < 5; i++)
    //{
    //    for (int j = 0; j < 4; j++)
    //    {
    //        //2_11 2_12 3_13 3_14
    //        //5_25 5_26 6_27 6_28
    //        //...
    //        int a = 0;
    //        if (j < 2)
    //        {
    //            a = 2 + i * 3;
    //        }
    //        else
    //        {
    //            a =3 + i * 3;
    //        }
    //        int b = 11 + 14 * i + j;
    //        xunhao = a.ToString() + "_" + b.ToString() ;
    //        string peilvNote = function.middlestring(resultPeilv, "<m_" + xunhao + ">", "</m_" + xunhao + ">");
    //        serPeilv.DXDS[i, j] = decimal.Parse(peilvNote);
    //    }
    //}

    #endregion 获取分析赔率大小单双序号
}