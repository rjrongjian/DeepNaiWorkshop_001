using AI;
using AI.Bll;
using AI.Dal;
using Bll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 注册机;

namespace 新一城娱乐系统
{
    public partial class login : Form
    {

        public static string ShouQuanServer = "http://193.112.91.139";


        public login()
        {
            InitializeComponent();
            SystemSleepManagement.PreventSleep(true);
            textBox3.Text = softReg.GetMNum();
            if (ConfigHelper.GetAppConfig("sfjz") == "是")
            {
                checkBox1.Checked = true;
                textBox1.Text = ConfigHelper.GetAppConfig("user");
                textBox2.Text = ConfigHelper.GetAppConfig("pass");
            }
        }
        SoftReg softReg = new SoftReg();
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string s = HttpHelps.Post("Account=" + textBox1.Text + "&Password=" + textBox2.Text + "&Machine=" + textBox3.Text + "&AuthCode=" + softReg.GetRNum(), 
               ShouQuanServer+ "/api/auth/login", ref ck, Encoding.UTF8);
            string zt = function.middlestring(s, "Status\":", ",");
            if (zt == "")
            {
                MessageBox.Show("连接服务器失败！");
            }
            if (zt == "0")
            {
                if (checkBox1.Checked)
                {
                    ConfigHelper.UpdateAppConfig("sfjz", "是");
                    ConfigHelper.UpdateAppConfig("user", textBox1.Text);
                    ConfigHelper.UpdateAppConfig("pass", textBox2.Text);
                }
                //
                string expire = function.middlestring(s, "Expire\":\"", "\"");
                DateTime dtExpire = new DateTime();
                DateTime.TryParse(expire, out dtExpire);
                新一城娱乐系统.FeiPan.ServerCommon.serverExpire = dtExpire;
                //
                MessageBox.Show("登录成功，本软件仅供娱乐，禁止用于赌博，否则后果自负。");
                //new QrCode("新一城娱乐系统V1.0 （有效期至：" + expire + "） 登录用户：" + textBox1.Text + "  （本软件仅供娱乐，禁止用于赌博，否则后果自负）").Show();
                this.Hide();
            }
            if (zt == "-1")
                MessageBox.Show("非法数据！");
            if (zt == "1")
                MessageBox.Show("用户名或密码错误");
            if (zt == "2")
                MessageBox.Show("非授权客户端");
            if (zt == "3")
                MessageBox.Show("授权过期");
        }
        string ck = "";
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string s = HttpHelps.Post("Account=" + textBox1.Text + "&Password=" + textBox2.Text + "&Machine=" + textBox3.Text,
                  ShouQuanServer + "/api/auth/regclient", ref ck, Encoding.UTF8);
            if (s == "")
            {
                MessageBox.Show("连接服务器失败！");
            }
            string zt = function.middlestring(s, "Status\":", ",");
            if (zt == "0")
                MessageBox.Show("注册成功->已向管理员发送授权申请！");
            if (zt == "-1")
                MessageBox.Show("非法数据！");
            if (zt == "1")
                MessageBox.Show("数据验证错误！");
            if (zt == "2")
                MessageBox.Show("帐号已存在！");
        }


        /*http://api.kaijiangtong.com/lottery/?name=cqssc&format=xml&uid=776619&token=fab1b9946276941fd218473332a69235b5fd71aa&num=1
            
         * //开奖
         * 
         * http://www.tzkhxxjs.com:81/User/List
         *
         * 获取开奖号码，最后面的参数是数量 1-30
        http://115.159.147.110:81/api/lottery/get/10

        用户注册
        http://115.159.147.110:81/api/auth/regclient
        POST参数
        Account 帐号
        Password 密码
        Machine 机器码

        返回结果
        { "status": 0, "message": "注册成功!"}
        状态码
        -1 非法数据
        0 成功
        1 数据验证错误
        2 帐号已存在，重名

        用户登录
        http://115.159.147.110:81/api/auth/login
        POST参数
        Account 帐号
        Password 密码
        Machine 机器码
        AuthCode 授权码
        状态码
        -1 非法数据
        0 成功
        1 用户名或密码错误
        2 非授权客户端，机器码或授权码不匹配
        3 授权过期

        修改密码
        http://115.159.147.110:81/api/auth/changepassword
        POST参数
        account 帐号
        oldpass 原密码
        newpass 新密码*/
    }
}
