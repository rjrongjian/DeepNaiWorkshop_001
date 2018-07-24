using AI.Bll;
using Dal;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 新一城娱乐系统
{
    public partial class 流水明细 : Form
    {
        GroupMemberInfoWithBocai linshi = null;
        string zse = "";
        public 流水明细(GroupMemberInfoWithBocai gr,string zseq)
        {
            zse = zseq;
            InitializeComponent();
            linshi = gr;
            label1.Text = "昵称：" + gr.GetNickName();
            label2.Text = "备注：" + gr.RemarkName;
            label3.Text = "总下注：" + gr.zongxiazhu.ToString();
            label4.Text = "总盈亏：" + gr.zongyingkui.ToString();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            chaxun(Convert.ToDateTime(开始日期.Text).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(结束日期.Text).ToString("yyyy-MM-dd HH:mm:ss"));
        }
        /// <summary>
        /// 今天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label6_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now.Date;
            DateTime time2 = time1.AddDays(1);
            chaxun(time1.ToString("yyyy-MM-dd 00:00:00"), time2.ToString("yyyy-MM-dd 00:00:00"));
        }
        /// <summary>
        /// 昨天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label5_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now.Date;
            DateTime time2 = time1.AddDays(-1);
            chaxun(time2.ToString("yyyy-MM-dd 00:00:00"), time1.ToString("yyyy-MM-dd 00:00:00"));
        }
        void chaxun(string dt1, string dt2)
        {
            string where = " where seq='" + linshi.Seq + "' ";
            if (comboBox1.Text != "全部")
                where += "and 类型='" + comboBox1.Text + "'";
            where += " and Time BETWEEN '" + dt1 + "' AND '" + dt2 + "'";
            DataTable xzdt = SQL.SELECTdata(where, "liushui_" + zse);
            listView2.Items.Clear();
            if (xzdt.Rows.Count == 0)
                return;
            foreach (DataRow dr in xzdt.Rows)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(dr["Id"].ToString());
                item.SubItems.Add(dr["类型"].ToString());
                item.SubItems.Add(dr["积分"].ToString());
                item.SubItems.Add(dr["剩余积分"].ToString());
                item.SubItems.Add(dr["Time"].ToString());
                item.SubItems.Add(dr["备注"].ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView2.Items.Add(item);
            
            }
        
        }
       

    }
}
