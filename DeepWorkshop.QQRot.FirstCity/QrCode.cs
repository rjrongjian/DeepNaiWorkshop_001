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
            catch (Exception ex) {
                CacheData.CoolQApi.AddLog(40, Newbe.CQP.Framework.CoolQLogLevel.Debug, "QrCode 构造方法出错："+ ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
       

        /// <summary>
        /// 进入主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
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

                //加载当前选中群的群员信息
                label1.Text = "正在加载选中群的群员信息";
                MyLogUtil.ToLogFotTest("选中的索引值："+ CacheData.SelectedGroupIndex);
                GroupInfo currentSelectedGroup = CacheData.CurrentGroupList[CacheData.SelectedGroupIndex];
                //MyLogUtil.ToLogFotTest("#####进入主界面前，选中的群："+currentSelectedGroup.GroupName+"____"+currentSelectedGroup.GroupId + "___" + comboBox2.SelectedIndex);
                CoolQApiExtend.GetGroupMemberListAndCache(CacheData.CoolQApi, currentSelectedGroup.GroupId);


                Form1 qr = new Form1(currentSelectedGroup, this.Text);
                CacheData.MainFrom = qr;
                qr.Show();
                Hide();
                MyMemoryUtil.ClearMemory();//释放内存
            }catch(Exception ex)
            {
                CacheData.CoolQApi.AddLog(40, Newbe.CQP.Framework.CoolQLogLevel.Debug, "qrCode类中button3_Click方法出错，异常信息："+ex);
                MyLogUtil.ErrToLog("进入主界面时出现错误，原因:"+ex);
                MessageBox.Show("进入主界面出现错误");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("数据库名不能为空");
                return;
            }
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
            button2.Enabled = false;
            try {
                label1.Text = "正在刷新群列表...";
                //获取QQ群列表
                List<GroupInfo> qqQunList = CoolQApiExtend.GetGroupList(CacheData.CoolQApi);
            
            
                //将群数据刷新到组件
                RefreshQunListCom(qqQunList);
                //说明：加载选中群员的信息转移到了button3_Click

                //webChat.FriendsList(false);
            }
            catch (Exception ex)
            {
                label1.Text = "请重新获取";
                MessageBox.Show("刷新群列表出现异常，原因："+ex.Message);
                MyLogUtil.ErrToLog("刷新群列表出现异常，原因：" + ex);
            }
            button2.Enabled = true;
        }
        /// <summary>
        /// 将群列表数据刷新到控件
        /// </summary>
        /// <param name="qqQunList"></param>
        private void RefreshQunListCom(List<GroupInfo> qqQunList)
        {
            /*
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
            */
            if (qqQunList == null || qqQunList.Count == 0) return;
            comboBox2.Items.Clear();
            for (int i = 0; i < qqQunList.Count; i++)
            {
                comboBox2.Items.Add(qqQunList[i].GroupName);
            }
            comboBox2.SelectedIndex = 0;
            label1.Text = "刷新群列表完成," + DateTime.Now.ToString();
        }

        private void QrCode_FormClosing(object sender, FormClosingEventArgs e)
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
        }

       
    }
}
