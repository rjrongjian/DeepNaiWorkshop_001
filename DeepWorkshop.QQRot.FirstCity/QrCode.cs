using AI.Bll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI.Dal;
using WindowsFormsApplication4;
using Dal;
using System.Web;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using DeepWorkshop.QQRot.FirstCity.MyModel;

namespace AI
{
    public partial class QrCode : Form
    {
        public QrCode(string title)
        {
            try
            {
                InitializeComponent();
                this.Text = title;
                /*
                webChat = new WebWeChat();
                pictureBox1.Image = function.BytesToImage(HttpHelps.GetQr(webChat.QrCode));
                Thread th = new Thread(new ThreadStart(delegate { webChat.Scanning(); }));
                th.IsBackground = true;
                th.Start();
                webChat.SetImage += new WebWeChat.myHandler(this.Setimg);
                webChat.cl += new WebWeChat.clos(QrCodeClose);
                */
                try
                {
                    SQLiteHelper.CreateDataBase();
                }
                catch (Exception ex) {
                    MyLogUtil.ErrToLog("创建数据库出错，原因："+ex);
                    MessageBox.Show("创建数据库出错");
                }
                try
                {
                    SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE peizhi(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                '账单' varchar(0),
                                                '封盘前' varchar(0),
                                                '封盘' varchar(0),
                                                '开奖' varchar(0),
                                                '实时账单' varchar(0),
                                                '下注' varchar(0),
'自定义1' varchar(0),'自定义2' varchar(0), '自定义3' varchar(0), '自定义4' varchar(0), '自定义5' varchar(0),
'倍数' varchar(0), '最小' varchar(0), '最大' varchar(0),
                                                Time varchar(0));", "peizhi");
                }
                catch (Exception ex) {
                    MyLogUtil.ErrToLog("创建数据库表出错，原因：" + ex);
                    MessageBox.Show("创建数据库表出错");
                }
                DataTable dttabel = SQL.tabellist();
                foreach (DataRow dr in dttabel.Rows)
                {
                    if (dr[0].ToString().IndexOf("Friends_") != -1)
                    {
                        comboBox1.Items.Add(dr[0].ToString().Replace("Friends_", ""));
                    }
                }
                if (comboBox1.Items.Count != 0)
                    comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //public WebWeChat webChat;
        
        /// <summary>
        /// 扫描成功显示头像
        /// </summary>
        /// <param name="img"></param>
        private void Setimg(Image img)
        {
            /*
            // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
            Action<Size> actionDelegate = (x) => { this.pictureBox1.Size = x; };

            pictureBox1.Invoke(actionDelegate, new Size(95, 95));
            Action<Point> locat = (x) => { this.pictureBox1.Location = x; };
            pictureBox1.Invoke(locat, new Point(95, 100));

            pictureBox1.Image = img;

            Action<bool> labe2 = (x) => { this.label2.Visible = x; };
            label2.Invoke(labe2, false);

            Action<bool> labe3 = (x) => { this.label3.Visible = x; };
            label3.Invoke(labe3, true);

            Action<bool> labe4 = (x) => { this.label4.Visible = x; };
            label4.Invoke(labe4, true);
            */
        }
        /// <summary>
        /// 返回重新扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label4_Click(object sender, EventArgs e)
        {
            /*
            pictureBox1.Size = new Size(190, 190);
            pictureBox1.Location = new Point(45, 80);
            webChat = new WebWeChat();
            pictureBox1.Image = function.BytesToImage(HttpHelps.GetQr(webChat.QrCode));
            Thread th = new Thread(new ThreadStart(delegate { webChat.Scanning(); }));
            th.IsBackground = true;
            th.Start();
            webChat.SetImage += new WebWeChat.myHandler(this.Setimg);
            webChat.cl += new WebWeChat.clos(QrCodeClose);
            */
        }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="img"></param>
        private void QrCodeClose()
        {
            /*
            webChat.jzhy += new WebWeChat.listhy(sjxs);
            Action<string> locat = (x) => { label1.Text = x; };
            label1.Invoke(locat, "正在加载数据......");
            //this.Invoke(new AddText(DoWork));
            */
        }
        /// <summary>
        /// 给combox2加载群数据
        /// </summary>
        /// <param name="dt"></param>
        public void sjxs(DataTable dt)
        {
            /*
            Action<string> t = (x) => { comboBox2.Items.Clear(); };
            comboBox2.Invoke(t, "");

            if (webChat.grox == null) return;
            for (int i =0;i< webChat.grox.Count;i++)
            {
                //判断数据库
                Action<string> locat = (x) => { comboBox2.Items.Add(x); comboBox2.SelectedIndex = 0; };
                comboBox2.Invoke(locat, webChat.grox[i].NickName);
                //if (SQL.tabelbool("Friends_" + webChat.grox[i].seq))
            }
            Action<string> txt = (x) => { label1.Text = x; };
            label1.Invoke(txt, "加载完毕,请选择群开始操作！");
            */
        }
        public delegate void AddText();
        private void DoWork()
        {
            /*
           // Form1 qr = new Form1(webChat);
            //qr.Show();
            Hide();
            //Close();
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("请选择qq群和数据库！");
                return;
            }
            //缓存当前选中的QQ群
            CacheData.SelectedGroupIndex = comboBox2.SelectedIndex;

            //webChat.grox[comboBox2.SelectedIndex].seq = comboBox1.Text;
            CacheData.Seq = comboBox1.Text;
            Form1 qr = new Form1(CacheData.CurrentGroupList[comboBox2.SelectedIndex], this.Text);
            CacheData.MainFrom = qr;
            qr.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQL sql = new SQL(textBox1.Text);
            Action<string> locat = (x) => { comboBox1.Items.Add(x); };
            comboBox1.Invoke(locat, textBox1.Text);
            textBox1.Text = "";
            MessageBox.Show("创建成功！");
        }
        /// <summary>
        /// 刷新群列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //获取QQ群列表
            List<GroupInfo> qqQunList = CoolQApiExtend.GetGroupList(CacheData.CoolQApi);
            
            
            //将数据刷新到组件
            RefreshQunListCom(qqQunList);
            //webChat.FriendsList(false);
        }
        /// <summary>
        /// 将群列表数据刷新到控件
        /// </summary>
        /// <param name="qqQunList"></param>
        private void RefreshQunListCom(List<GroupInfo> qqQunList)
        {
            Action<string> t = (x) => { comboBox2.Items.Clear(); };
            comboBox2.Invoke(t, "");

            if (qqQunList == null|| qqQunList.Count==0) return;
            for (int i = 0; i < qqQunList.Count; i++)
            {
                //判断数据库
                Action<string> locat = (x) => { comboBox2.Items.Add(x); comboBox2.SelectedIndex = 0; };
                comboBox2.Invoke(locat, qqQunList[i].GroupName);
                //if (SQL.tabelbool("Friends_" + webChat.grox[i].seq))
            }
            Action<string> txt = (x) => { label1.Text = x; };
            label1.Invoke(txt, "刷新群列表完成，时间："+ DateTime.Now.ToString());
        }
    }
}
