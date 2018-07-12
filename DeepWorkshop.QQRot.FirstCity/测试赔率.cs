using AI.Bll;
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
    public partial class 测试赔率 : Form
    {
        public 测试赔率()
        {
            InitializeComponent();
        }
        bool qd1 = true;
        bool qd2 = true;
        bool qd3 = true;
        bool qd4 = true;
        bool qd5 = true;
        bool He = true;
        int qdsl = 5;
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
                { qd1 = true; qdsl++; }
                if (cb.Name == "qiudao2")
                { qd2 = true; qdsl++; }
                if (cb.Name == "qiudao3")
                { qd3 = true; qdsl++; }
                if (cb.Name == "qiudao4")
                { qd4 = true; qdsl++; }
                if (cb.Name == "qiudao5")
                { qd5 = true; qdsl++; }
                if (cb.Name == "qiudaohe")
                    He = true;
            }
            else
            {
                if (cb.Name == "qiudao1")
                { qd1 = false; qdsl--; }
                if (cb.Name == "qiudao2")
                { qd2 = false; qdsl--; }
                if (cb.Name == "qiudao3")
                { qd3 = false; qdsl--; }
                if (cb.Name == "qiudao4")
                { qd4 = false; qdsl--; }
                if (cb.Name == "qiudao5")
                { qd5 = false; qdsl--; }
                if (cb.Name == "qiudaohe")
                    He = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            zjf = 0;
            string[] sz = textBox1.Text.Split(' ');
            listView2.Items.Clear();
            foreach(string str in sz)
            {
                xiazhu(str.Split(' '));
            }
            label4.Text = "下注总额：" + zjf.ToString();
        }
        int zjf = 0;
        void add(ref int x, int y)
        {
            x += y;
            zjf += y;
        }
        void xiazhu(string[] contstring)
        {
            xztj lsxz = new xztj();
            try
            {
                foreach (string xz in contstring)
                {
                    string[] fz = xz.Split('/');

                    if (fz.Length == 2)
                    {
                        //   单/20
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
                            if (He)
                            {
                                add(ref  lsxz.ZHDXDS[wz], int.Parse(fz[1]));
                                continue;
                            }
                            if (qdsl == 1)
                            {
                                if (qd1)
                                    add(ref  lsxz.DXDS[0, wz], int.Parse(fz[1]));
                                if (qd2)
                                    add( ref  lsxz.DXDS[1, wz], int.Parse(fz[1]));
                                if (qd3)
                                    add(ref  lsxz.DXDS[2, wz], int.Parse(fz[1]));
                                if (qd4)
                                    add( ref  lsxz.DXDS[3, wz], int.Parse(fz[1]));
                                if (qd5)
                                    add(ref  lsxz.DXDS[4, wz], int.Parse(fz[1]));
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
                        if (wz != -1 && He)
                        {
                            add( ref  lsxz.ZHZHDXDS[wz], int.Parse(fz[1]));
                            continue;
                        }
                        // 0123456789/20
                        if (int.TryParse(fz[0], out wz))
                        {
                            for (int i = 0; i < fz[0].Length; i++)
                            {
                                wz = int.Parse(fz[0].Substring(i, 1));
                                if (qd1)
                                    add( ref  lsxz.QD[0, wz], int.Parse(fz[1]));
                                if (qd2)
                                    add(ref  lsxz.QD[1, wz], int.Parse(fz[1]));
                                if (qd3)
                                    add( ref  lsxz.QD[2, wz], int.Parse(fz[1]));
                                if (qd4)
                                    add( ref  lsxz.QD[3, wz], int.Parse(fz[1]));
                                if (qd5)
                                    add( ref  lsxz.QD[4, wz], int.Parse(fz[1]));
                            }
                            continue;
                        }

                    }
                    if (fz.Length == 3)
                    {
                        int wz = -1;
                        // 万2/0123456789/20
                        if (int.TryParse(fz[1], out wz))
                        {
                            for (int i = 0; i < fz[0].Length; i++)
                            {
                                if ("12345万千百十个".IndexOf(fz[0].Substring(i, 1)) == -1)
                                    return;
                            }
                            for (int i = 0; i < fz[1].Length; i++)
                            {
                                wz = int.Parse(fz[1].Substring(i, 1));
                                if (fz[0].IndexOf("1") != -1 || fz[0].IndexOf("万") != -1)
                                    if (qd1)
                                        add(ref  lsxz.QD[0, wz], int.Parse(fz[2]));
                                if (fz[0].IndexOf("2") != -1 || fz[0].IndexOf("千") != -1)
                                    if (qd2)
                                        add( ref  lsxz.QD[1, wz], int.Parse(fz[2]));
                                if (fz[0].IndexOf("3") != -1 || fz[0].IndexOf("百") != -1)
                                    if (qd3)
                                        add( ref  lsxz.QD[2, wz], int.Parse(fz[2]));
                                if (fz[0].IndexOf("4") != -1 || fz[0].IndexOf("十") != -1)
                                    if (qd4)
                                        add( ref  lsxz.QD[3, wz], int.Parse(fz[2]));
                                if (fz[0].IndexOf("5") != -1 || fz[0].IndexOf("个") != -1)
                                    if (qd5)
                                        add( ref  lsxz.QD[4, wz], int.Parse(fz[2]));
                            }
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
                                    return;
                            }
                            if (fz[0].IndexOf("1") != -1 || fz[0].IndexOf("万") != -1)
                                if (qd1)
                                    add(ref  lsxz.DXDS[0, wz], int.Parse(fz[2]));
                            if (fz[0].IndexOf("2") != -1 || fz[0].IndexOf("千") != -1)
                                if (qd2)
                                    add(ref  lsxz.DXDS[1, wz], int.Parse(fz[2]));
                            if (fz[0].IndexOf("3") != -1 || fz[0].IndexOf("百") != -1)
                                if (qd3)
                                    add(ref  lsxz.DXDS[2, wz], int.Parse(fz[2]));
                            if (fz[0].IndexOf("4") != -1 || fz[0].IndexOf("十") != -1)
                                if (qd4)
                                    add(ref  lsxz.DXDS[3, wz], int.Parse(fz[2]));
                            if (fz[0].IndexOf("5") != -1 || fz[0].IndexOf("个") != -1)
                                if (qd5)
                                    add(ref  lsxz.DXDS[4, wz], int.Parse(fz[2]));
                            continue;
                        }
                    }
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
                            if (He)
                            {
                                if (dszh == 0)
                                    add(ref  lsxz.ZHZHDXDS[wzz], int.Parse(dq[1]));
                                else
                                    add(ref  lsxz.ZHDXDS[wzz], int.Parse(dq[1]));
                                continue;
                            }
                            if (qdsl == 1 && dszh == 1)
                            {
                                if (qd1)
                                    add(ref  lsxz.DXDS[0, wzz], int.Parse(dq[1]));
                                if (qd2)
                                    add(ref  lsxz.DXDS[1, wzz], int.Parse(dq[1]));
                                if (qd3)
                                    add(ref  lsxz.DXDS[2, wzz], int.Parse(dq[1]));
                                if (qd4)
                                    add(ref  lsxz.DXDS[3, wzz], int.Parse(dq[1]));
                                if (qd5)
                                    add(ref  lsxz.DXDS[4, wzz], int.Parse(dq[1]));
                                continue;
                            }
                        }
                        else if (dszh == 1)
                        {
                            for (int i = 0; i < dq[0].Length; i++)
                            {
                                if ("12345万千百十个".IndexOf(dq[0].Substring(i, 1)) == -1)
                                    return;
                            }
                            if (dq[0].IndexOf("1") != -1 || dq[0].IndexOf("万") != -1)
                                if (qd1)
                                    add(ref  lsxz.DXDS[0, wzz], int.Parse(dq[1]));
                            if (dq[0].IndexOf("2") != -1 || dq[0].IndexOf("千") != -1)
                                if (qd2)
                                    add(ref  lsxz.DXDS[1, wzz], int.Parse(dq[1]));
                            if (dq[0].IndexOf("3") != -1 || dq[0].IndexOf("百") != -1)
                                if (qd3)
                                    add(ref  lsxz.DXDS[2, wzz], int.Parse(dq[1]));
                            if (dq[0].IndexOf("4") != -1 || dq[0].IndexOf("十") != -1)
                                if (qd4)
                                    add(ref  lsxz.DXDS[3, wzz], int.Parse(dq[1]));
                            if (dq[0].IndexOf("5") != -1 || dq[0].IndexOf("个") != -1)
                                if (qd5)
                                    add(ref  lsxz.DXDS[4, wzz], int.Parse(dq[1]));
                            continue;
                        }
                    }
                    if (xz.IndexOf("龙") == 0)
                    {
                        add(ref  lsxz.LHH[0], int.Parse(xz.Replace("龙", "")));
                        continue;
                    }
                    if (xz.IndexOf("虎") == 0)
                    {
                        add(ref  lsxz.LHH[0], int.Parse(xz.Replace("虎", "")));
                        continue;
                    }
                    if (xz.IndexOf("和") == 0 || xz.IndexOf("合") == 0)
                    {
                        add(ref  lsxz.LHH[0], int.Parse(xz.Replace("合", "").Replace("和", "")));
                        continue;
                    }
                    return;
                }//循环尾
                //这里写下注成功
                if (lsxz != new xztj())
                {
                    string[] sz = textBox2.Text.Split(',');
                    for (int i = 0; i < 5; i++)
                    {
                        kaijiangdata.QD[i] = int.Parse(sz[i]);
                    }
                    kaijiang();
                    jiesuan(lsxz, contstring[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("下注格式错误！");
            }
        }
        kjtj kaijiangdata = new kjtj();
        /// <summary>
        /// 开奖
        /// </summary>
        void kaijiang()
        {
            int zonghe = 0;
            for (int i = 0; i < 5; i++)
            {
                if (kaijiangdata.QD[i] % 2 == 0)
                    kaijiangdata.DXDS[i, 3] = true;//双
                else
                    kaijiangdata.DXDS[i, 2] = true;//单

                if (kaijiangdata.QD[i] < 5)
                    kaijiangdata.DXDS[i, 1] = true; //小
                else
                    kaijiangdata.DXDS[i, 0] = true; //大

                zonghe += kaijiangdata.QD[i];
            }
            if (zonghe % 2 == 0)
                kaijiangdata.ZH[3] = true;//双
            else
                kaijiangdata.ZH[2] = true;//单

            if (zonghe < 23)
                kaijiangdata.ZH[1] = true; //小
            else
                kaijiangdata.ZH[0] = true; //大

            if (zonghe >= 23)//大
                if (zonghe % 2 != 0)//单
                    kaijiangdata.ZHzh[0] = true;
                else//双
                    kaijiangdata.ZHzh[1] = true;
            else
                if (zonghe % 2 != 0)//单
                    kaijiangdata.ZHzh[2] = true;
                else//双
                    kaijiangdata.ZHzh[3] = true;
            if (kaijiangdata.QD[0] > kaijiangdata.QD[4])
                kaijiangdata.LHH[0] = true;
            if (kaijiangdata.QD[0] < kaijiangdata.QD[4])
                kaijiangdata.LHH[1] = true;
            if (kaijiangdata.QD[0] == kaijiangdata.QD[4])
                kaijiangdata.LHH[2] = true;
        }
        /// <summary>
        /// 结算积分
        /// </summary>
        void jiesuan(xztj xiazhutongji,string cont)
        {
            int qd = 0; int dxds = 0; int zh = 0; int zhzh = 0; int lhh = 0;
            for (int x = 0; x < 5; x++)
            {
                qd += xiazhutongji.QD[x, kaijiangdata.QD[x]];//球道
                if (xiazhutongji.QD[x, kaijiangdata.QD[x]] > 0)
                {
                    string str = (x+1).ToString() + "/" + kaijiangdata.QD[x].ToString() + "/" + xiazhutongji.QD[x, kaijiangdata.QD[x]].ToString();
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(str);
                    item.SubItems.Add((xiazhutongji.QD[x, kaijiangdata.QD[x]] * 9.71).ToString());
                    item.SubItems.Remove(item.SubItems[0]);
                    listView2.Items.Add(item);
                }
            }
            for (int i = 0; i < 5; i++)//大小单双
            {
                for (int x = 0; x < 4; x++)
                {
                    if (kaijiangdata.DXDS[i, x])
                    {
                        dxds += xiazhutongji.DXDS[i, x];
                        if (xiazhutongji.DXDS[i, x] > 0)
                        {
                            string str = (i + 1).ToString() + "/";
                            if (x == 0) str += "大"; if (x == 1) str += "小"; if (x == 2) str += "单"; if (x == 3) str += "双";
                            str +="/"+ xiazhutongji.DXDS[i, x].ToString();
                            ListViewItem item = new ListViewItem();
                            item.SubItems.Add(str);
                            item.SubItems.Add((xiazhutongji.DXDS[i, x] * 1.94).ToString());
                            item.SubItems.Remove(item.SubItems[0]);
                            listView2.Items.Add(item);
                        }
                    }
                }
            }
            for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
            {
                if (kaijiangdata.ZH[i])
                {
                    zh += xiazhutongji.ZHDXDS[i];
                    if (xiazhutongji.ZHDXDS[i] > 0)
                    {
                        string str = "";
                        if (i == 0) str += "总大"; if (i == 1) str += "总小"; if (i == 2) str += "总单"; if (i == 3) str += "总双";
                        str += xiazhutongji.ZHDXDS[i].ToString();
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(str);
                        item.SubItems.Add((xiazhutongji.ZHDXDS[i] * 1.94).ToString());
                        item.SubItems.Remove(item.SubItems[0]);
                        listView2.Items.Add(item);
                    }
                }
                if (kaijiangdata.ZHzh[i])
                {
                    zhzh += xiazhutongji.ZHZHDXDS[i];
                    if (xiazhutongji.ZHZHDXDS[i] > 0)
                    {
                        string str = "";
                        if (i == 0) str += "大单"; if (i == 1) str += "大双"; if (i == 2) str += "小单"; if (i == 3) str += "小双";
                        str += xiazhutongji.ZHZHDXDS[i].ToString();
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(str);
                        item.SubItems.Add((xiazhutongji.ZHZHDXDS[i] * 1.94).ToString());
                        item.SubItems.Remove(item.SubItems[0]);
                        listView2.Items.Add(item);
                    }
                }
                if (i != 3)
                {
                    if (kaijiangdata.LHH[i])
                    {
                        lhh += xiazhutongji.LHH[i];
                        if (xiazhutongji.LHH[i] > 0)
                        {
                            string str = "";
                            if (i == 0) str += "龙"; if (i == 1) str += "虎"; if (i == 2) str += "合"; 
                            str += xiazhutongji.LHH[i].ToString();
                            ListViewItem item = new ListViewItem();
                            item.SubItems.Add(str);
                            item.SubItems.Add((xiazhutongji.LHH[i] * 1.94).ToString());
                            item.SubItems.Remove(item.SubItems[0]);
                            listView2.Items.Add(item);
                        }
                    }
                }
            }
            return;
            double zjjf = (qd * 9.71) + (dxds * 1.94) + (zh * 1.94) + (zhzh * 1.94) + (lhh * 1.94);
            if (zjjf != 0)//奖金流水保存
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(cont);
                item.SubItems.Add(zjjf.ToString());
                item.SubItems.Remove(item.SubItems[0]);
                listView2.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
