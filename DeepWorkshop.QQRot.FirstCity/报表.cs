using AI.Bll;
using Dal;
using DeepWorkshop.QQRot.FirstCity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApplication4;

namespace 新一城娱乐系统
{
    public partial class 报表 : Form
    {
        private string seq = "";
        private double bili = 0;

        public 报表(string gr, string huishui)
        {
            InitializeComponent();
            seq = gr;
            bili = Convert.ToDouble(huishui);
            label13.Text = "";
        }

        /// <summary>
        /// 玩家查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string sj1 = 玩家日期始.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sj2 = 玩家日期末.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string str = "select * from Friends_" + seq;
            if (textBox1.Text != "")
                str += " where NickName like '%" + textBox1.Text + "%'";
            DataTable deset = SQLiteHelper.ExecuteDataTable(str, null);

            listView2.Items.Clear();
            foreach (DataRow dr in deset.Rows)
            {
                ListViewItem item = new ListViewItem();

                item.SubItems.Add(dr["seq"].ToString());
                item.SubItems.Add(dr["NickName"].ToString());
                item.SubItems.Add(dr["现有积分"].ToString());
                item.SubItems.Add(dr["总盈亏"].ToString());
                item.SubItems.Add(dr["总下注"].ToString());
                try
                {
                    string strAA = "select sum(积分) from liushui_" + seq + " where seq like '" + dr["seq"].ToString() +
                        "' and 类型 like '上分' and  Time BETWEEN '" + sj1 + "' AND '" + sj2 + "'";
                    DataTable set = SQLiteHelper.ExecuteDataTable(strAA, null);

                    item.SubItems.Add(set.Rows[0][0].ToString());

                    strAA = "select sum(积分) from liushui_" + seq + " where seq like '" + dr["seq"].ToString() +
                        "' and 类型 like '下分' and  Time BETWEEN '" + sj1 + "' AND '" + sj2 + "'";
                    set = SQLiteHelper.ExecuteDataTable(strAA, null);
                    item.SubItems.Add(set.Rows[0][0].ToString());
                }
                catch (Exception ex)
                {
                }
                item.SubItems.Remove(item.SubItems[0]);
                listView2.Items.Add(item);
            }
        }

        /// <summary>
        /// 玩家导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.InitialDirectory = "E:\\";
            sfd.FileName = "玩家统计";
            sfd.Filter = "xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
                FileName = sfd.FileName;
            else
                return;

            ExcelEdit Edit = new ExcelEdit();
            Edit.InsertTable(LVtoTB(listView2), "Sheet1", 2, 1, true);
            Edit.SaveAs(FileName);
            Edit.Close();
        }

        /// <summary>
        /// 期号查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string sj1 = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sj2 = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string str = "select * from kaijiang_" + seq + " where Time BETWEEN '" + sj1 + "' AND '" + sj2 + "'";
            listView1.Items.Clear();
            DataTable deset = SQLiteHelper.ExecuteDataTable(str, null);
            foreach (DataRow dr in deset.Rows)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(dr["期号"].ToString());
                item.SubItems.Add(dr["qd1"].ToString());
                item.SubItems.Add(dr["盈亏"].ToString());
                item.SubItems.Add(dr["总下注积分"].ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView1.Items.Add(item);
            }
        }

        /// <summary>
        /// 期号导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string FileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.InitialDirectory = "E:\\";
            sfd.FileName = "期号数据";
            sfd.Filter = "xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
                FileName = sfd.FileName;
            else
                return;
            ExcelEdit Edit = new ExcelEdit();
            Edit.InsertTable(LVtoTB(listView1), "Sheet1", 2, 1, true);
            Edit.SaveAs(FileName);
            Edit.Close();
        }

        /// <summary>
        /// 流水明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            string sj1 = dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sj2 = dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string str = "select * from liushui_" + seq + " where Time BETWEEN '" + sj1 + "' AND '" + sj2 + "' ";
            if (comboBox1.Text != "全部")
            {
                str += "and 类型 like '" + comboBox1.Text + "' ";
            }
            if (textBox3.Text != "")
            {
                str += "and 期号 like '" + textBox3.Text + "' ";
            }
            if (textBox2.Text != "")
            {
                str += "and NickName like '%" + textBox2.Text + "%' ";
            }
            DataTable deset = SQLiteHelper.ExecuteDataTable(str, null);
            listView5.Items.Clear();
            foreach (DataRow dr in deset.Rows)
            {
                ListViewItem item = new ListViewItem();
                //item.SubItems.Add(dr["Id"].ToString());
                item.SubItems.Add(dr["seq"].ToString());
                item.SubItems.Add(dr["NickName"].ToString());
                item.SubItems.Add(dr["期号"].ToString());
                item.SubItems.Add(dr["Time"].ToString());
                item.SubItems.Add(dr["类型"].ToString());
                item.SubItems.Add(dr["积分"].ToString());
                item.SubItems.Add(dr["剩余积分"].ToString());
                item.SubItems.Add(dr["备注"].ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView5.Items.Add(item);
            }
        }

        /// <summary>
        /// 聊天记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            string sj1 = dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sj2 = dateTimePicker8.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string str = "select * from liaotian_" + seq + " where Time BETWEEN '" + sj1 + "' AND '" + sj2 + "' ";
            if (textBox4.Text != "")
            {
                str += "and 昵称 like '%" + textBox4.Text + "%' ";
            }
            listView6.Items.Clear();
            DataTable deset = SQLiteHelper.ExecuteDataTable(str, null);
            foreach (DataRow dr in deset.Rows)
            {
                ListViewItem item = new ListViewItem();
                //item.SubItems.Add(dr["Id"].ToString());
                item.SubItems.Add(dr["seq"].ToString());
                item.SubItems.Add(dr["昵称"].ToString());
                item.SubItems.Add(dr["Time"].ToString());
                item.SubItems.Add(dr["内容"].ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView6.Items.Add(item);
            }
        }

        private DataTable LVtoTB(ListView lv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text);
            }
            for (int i = 0; i < lv.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < lv.Columns.Count; j++)
                {
                    dr[j] = lv.Items[i].SubItems[j].Text;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 流水明细导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            string FileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.InitialDirectory = "E:\\";
            sfd.FileName = "流水明细";
            sfd.Filter = "xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
                FileName = sfd.FileName;
            else
                return;
            ExcelEdit Edit = new ExcelEdit();
            Edit.InsertTable(LVtoTB(listView5), "Sheet1", 2, 1, true);
            Edit.SaveAs(FileName);
            Edit.Close();
        }

        /// <summary>
        /// 聊天记录导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            string FileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.InitialDirectory = "E:\\";
            sfd.FileName = "聊天记录";
            sfd.Filter = "xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
                FileName = sfd.FileName;
            else
                return;
            ExcelEdit Edit = new ExcelEdit();
            Edit.InsertTable(LVtoTB(listView6), "Sheet1", 2, 1, true);
            Edit.SaveAs(FileName);
            Edit.Close();
        }

        /// <summary>
        /// 回水导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            string FileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.InitialDirectory = "E:\\";
            sfd.FileName = "回水统计";
            sfd.Filter = "xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
                FileName = sfd.FileName;
            else
                return;
            ExcelEdit Edit = new ExcelEdit();
            Edit.InsertTable(LVtoTB(listView4), "Sheet1", 2, 1, true);
            Edit.SaveAs(FileName);
            Edit.Close();
        }

        /// <summary>
        /// 回水查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            //===================2018-02-08===================
            //在属性窗口中改 主窗体的 textBox23 的 modifiers 属性为Public
            if (MainPlugin.frmMain != null && MainPlugin.frmMain._group != null)
            {
                try
                {
                    bili = Convert.ToDouble(MainPlugin.frmMain.textBox23.Text);
                }
                catch
                {
                    frmMessageTimer frmMessage = new frmMessageTimer("比例设置错误！");
                    frmMessage.Show();
                }
            }
            //===================2018-02-08===================

            string sj1 = dateTimePicker6.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sj2 = dateTimePicker7.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string str = "select * from NameInt_" + seq + " where Time BETWEEN '" + sj1 + "' AND '" + sj2 + "' group by seq";
            DataTable deset = SQLiteHelper.ExecuteDataTable(str, null);
            int q = 0;

            //
            decimal showDD = 0;
            decimal showDD_HS = 0;
            decimal showDXDS = 0;
            decimal showDXDS_HS = 0;
            decimal showZ = 0;
            decimal showZ_HS = 0;

            //
            listView4.Items.Clear();
            foreach (DataRow dr in deset.Rows)
            {
                if (dr[1].ToString() == "")
                    continue;
                string s = "select * from NameInt_" + seq + " where seq like '" + dr[1].ToString() + "' and Time BETWEEN '" + sj1 + "' AND '" + sj2 + "'";
                DataTable datat = SQLiteHelper.ExecuteDataTable(s, null);
                int qd = 0;
                int dxds = 0;
                int zh = 0;
                int zhzh = 0;
                int lhh = 0;

                foreach (DataRow drB in datat.Rows)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            qd += int.Parse(drB["qd" + (x + 1).ToString() + "_" + i.ToString()].ToString());
                        }
                    }
                    for (int i = 0; i < 5; i++)//大小单双
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            dxds += int.Parse(drB["d" + (i + 1).ToString() + "_" + x.ToString()].ToString());
                        }
                    }
                    for (int i = 0; i < 4; i++)//总和  总和组合
                    {
                        zh += int.Parse(drB["zh" + i.ToString()].ToString());
                        zhzh += int.Parse(drB["zhzh" + i.ToString()].ToString());
                    }
                    for (int i = 0; i < 3; i++)//龙虎和
                    {
                        lhh += int.Parse(drB["LHH" + i.ToString()].ToString());
                    }
                }
                if (qd.ToString().Length >= int.Parse(textBox5.Text))
                {
                    q++;
                    string tagS = dr[1].ToString() + "/" + dateTimePicker6.Value.ToString("yyyy-MM-dd") + "/" + dateTimePicker7.Value.ToString("yyyy-MM-dd");

                    ListViewItem item = new ListViewItem();
                    //item.SubItems.Add(q.ToString());
                    item.SubItems.Add(dr["seq"].ToString());
                    item.SubItems.Add(dr["NickName"].ToString());
                    //单点
                    item.SubItems.Add(qd.ToString());
                    item.SubItems.Add((qd * bili / 100).ToString());
                    //大小单双
                    int zonghe = dxds + zh + zhzh + lhh;
                    item.SubItems.Add(zonghe.ToString());
                    item.SubItems.Add((zonghe * bili / 100).ToString());
                    //总下注
                    item.SubItems.Add((qd + zonghe).ToString());
                    //总回水
                    item.SubItems.Add(((qd + zonghe) * bili / 100).ToString());

                    item.SubItems.Add("确认回水");
                    item.SubItems.Remove(item.SubItems[0]);

                    item.Tag = tagS;

                    //
                    string sqlCCC = "select count(1) from Chaxun_" + seq + " where 时间='" + tagS + "' ";
                    DataRow sCCC = SQLiteHelper.ExecuteDataRow(sqlCCC, null);
                    int iCCCCount = int.Parse(sCCC[0].ToString());

                    if (iCCCCount > 0)
                    {
                        item.BackColor = System.Drawing.Color.Yellow;

                        showDD += qd;
                        showDXDS += zonghe;
                        showZ += qd + zonghe;
                        showDD_HS += (decimal)(qd * bili / 100);
                        showDXDS_HS += (decimal)(zonghe * bili / 100);
                        showZ_HS += (decimal)((qd + zonghe) * bili / 100);
                    }
                    listView4.Items.Add(item);
                }
            }
            label13.Text = String.Format("当前选中发放详情：\n 单点总下注：{0}  单点回水：{1}      大小单双总下注：{2}  大小单双回水：{3}      总下注：{4}  总回水：{5}  ",
                 showDD.ToString(), showDD_HS.ToString(), showDXDS.ToString(), showDXDS_HS.ToString(), showZ.ToString(), showZ_HS.ToString());
        }

        /// <summary>
        /// 回水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView4_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            ListViewItem lvi = listView4.GetItemAt(e.X, e.Y);
            if (lvi != null)
            {
                ListViewItem.ListViewSubItem lvSub = lvi.GetSubItemAt(e.X, e.Y);
                if (lvSub != null)
                {
                    int subIndex = lvi.SubItems.IndexOf(lvSub);

                    if (subIndex == 8)
                    {
                        string[] sTags = lvi.Tag.ToString().Split('/');

                        string sId = sTags[0];
                        string sql = "select 结算后积分 from NameInt_" + seq + " where seq='" + sId + "' order by id desc limit 1";
                        DataRow sZJF = SQLiteHelper.ExecuteDataRow(sql, null);
                        int iZJF = int.Parse(sZJF[0].ToString());

                        string huishuiZong = lvi.SubItems[3].Text;
                        string nickname = lvi.SubItems[1].Text;
                        int zengjiaJiFen = (int)decimal.Parse(huishuiZong);

                        //
                        List<KeyVal> xiaZhuData = new List<KeyVal>();

                        #region 统计积分

                        for (int i = 0; i < 10; i++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                KeyVal c = new KeyVal("qd" + (x + 1).ToString() + "_" + i.ToString(), "0");
                                xiaZhuData.Add(c);
                            }
                        }
                        //大小单双
                        for (int i = 0; i < 5; i++)
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                KeyVal c = new KeyVal("d" + (i + 1).ToString() + "_" + x.ToString(), "0");
                                xiaZhuData.Add(c);
                            }
                        }
                        //总和  总和组合  龙虎和
                        for (int i = 0; i < 4; i++)
                        {
                            KeyVal c = new KeyVal("zh" + i.ToString(), "0");
                            xiaZhuData.Add(c);
                            KeyVal c1 = new KeyVal("zhzh" + i.ToString(), "0");
                            xiaZhuData.Add(c1);
                            if (i != 3)
                            {
                                KeyVal l = new KeyVal("LHH" + i.ToString(), "0");
                                xiaZhuData.Add(l);
                            }
                        }

                        #endregion 统计积分

                        KeyVal seqA = new KeyVal("seq", sId);
                        xiaZhuData.Add(seqA);
                        KeyVal NN = new KeyVal("NickName", nickname);
                        xiaZhuData.Add(NN);
                        KeyVal qh = new KeyVal("期号", "");
                        xiaZhuData.Add(qh);
                        KeyVal xzwb = new KeyVal("下注文本", "回水积分");
                        xiaZhuData.Add(xzwb);
                        KeyVal yk = new KeyVal("盈亏", zengjiaJiFen.ToString());
                        xiaZhuData.Add(yk);
                        KeyVal xzjf = new KeyVal("下注积分", "0");
                        xiaZhuData.Add(xzjf);
                        KeyVal jsjf = new KeyVal("结算后积分", (iZJF + zengjiaJiFen).ToString());
                        xiaZhuData.Add(jsjf);

                        //
                        SQL.INSERT(xiaZhuData, " NameInt_" + seq);

                        //更新用户总积分
                        string delStr = string.Format(@"UPDATE Friends_" + seq + " SET {0} where seq='" + sId + "'",
                            "'现有积分'='" + (iZJF + zengjiaJiFen).ToString() + "'");

                        SQLiteHelper.ExecuteNonQuery(delStr);



                        List<KeyVal> chaxunData = new List<KeyVal>();
                        KeyVal shijian = new KeyVal("时间", lvi.Tag.ToString());
                        chaxunData.Add(shijian);
                        //
                        SQL.INSERT(chaxunData, " Chaxun_" + seq);

                        //===================2018-02===================
                        //在属性窗口中改 主窗体的lvChengYuanJiFen 的 modifiers 属性为Public
                        try
                        {
                            if (MainPlugin.frmMain != null && MainPlugin.frmMain._group != null)
                            {
                                foreach (GROUP jp in MainPlugin.frmMain._group.MemberList)
                                {
                                    if (jp.seq == sId)
                                    {
                                        int zongjifens = MainPlugin.frmMain._group.zongjifen;
                                        jp.zongjifen += zengjiaJiFen;

                                        for (int i = 0; i < MainPlugin.frmMain.lvChengYuanJiFen.Items.Count; i++)
                                        {
                                            var dsf = MainPlugin.frmMain.lvChengYuanJiFen.Items[i].SubItems[0].Text;
                                            string d = MainPlugin.frmMain.lvChengYuanJiFen.Items[i].Tag.ToString();
                                            if (d == sId)
                                            {
                                                MainPlugin.frmMain.lvChengYuanJiFen.Items[i].SubItems[5].Text = (iZJF + zengjiaJiFen).ToString();//总积分
                                            }

                                        }

                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            function.log("回水错误 " + ex.Message);
                        }
                        //===================2018-02===================
                        //
                        MessageBox.Show("回水成功");
                    }
                }
            }
            try
            { }
            catch
            {
                MessageBox.Show("回水失败");
            }
            */
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}