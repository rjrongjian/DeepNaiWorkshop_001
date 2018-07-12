using AI.Bll;
using DeepWorkshop.QQRot.FirstCity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication4;

namespace 新一城娱乐系统
{
    public partial class 报盘 : Form
    {
        GROUP _mainGroup = null;
        string zuiJinYiqi = "";

        public 报盘(GROUP gr)
        {
            InitializeComponent();


            if (gr == null)
            {
                _mainGroup = new GROUP();
                _mainGroup.seq = "ABC";
            }
            else
            {
                _mainGroup = gr;
            }



            button4_Click(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lbQiHao.Items.Clear();
            //
            string showText = "";

            //DataTable dtTop = SQL.SELECTdata("期号", "1=1 ORDER BY 期号 DESC LIMIT 1", "kaijiang_" + _mainGroup.seq);
            if (MainPlugin.frmMain != null && MainPlugin.frmMain.FengPan)
            {
                zuiJinYiqi = MainPlugin.frmMain.BenQiQiHao;
                lbQiHao.Items.Add(zuiJinYiqi);
                showText += chaXunXiaZhu(zuiJinYiqi, true);
            }
            else
            {
                zuiJinYiqi = "";
            }



            //DataTable dt = SQL.SELECTdata("期号", "1=1 ORDER BY 期号 DESC LIMIT 3000", "kaijiang_" + _mainGroup.seq);//总下注积分>0

            DataTable dt = SQL.SELECTdata("distinct 期号", "1=1 ORDER BY 期号 DESC LIMIT 3000",
                "(select 期号 from liushui_" + _mainGroup.seq + " union SELECT 期号 FROM kaijiang_" + _mainGroup.seq + " )");

            //
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (zuiJinYiqi != dt.Rows[i][0].ToString())
                {
                    lbQiHao.Items.Add(dt.Rows[i][0].ToString());

                    showText += chaXunXiaZhu(dt.Rows[i][0].ToString());
                }

            }
            textBox2.Text = showText;

        }
        private void lbQiHao_DoubleClick(object sender, EventArgs e)
        {
            if (lbQiHao.Text == "")
                return;
            textBox2.Text = chaXunXiaZhu(lbQiHao.Text);
        }

        string chaXunXiaZhu(string qihao, bool isCCC = false)
        {

            DataTable dtXiaZhu = SQL.SELECTdata(" where 下注文本!='飞盘失败，返还积分' and 下注文本!='回水积分' and 期号='" + qihao + "'", "NameInt_" + _mainGroup.seq);
            StringBuilder jieGuo = new StringBuilder();

            jieGuo.AppendLine(qihao + "期下注统计");
            jieGuo.AppendLine("-----------------------------------------------------------------------------------------------------------------------------");
            jieGuo.AppendLine("         大      小      单      双       0      1       2       3      4        5       6       7       8       9       累计");
            jieGuo.AppendLine("-----------------------------------------------------------------------------------------------------------------------------");

            if ((dtXiaZhu == null || dtXiaZhu.Rows.Count == 0) && isCCC == false)
            {
                jieGuo.AppendLine(
@"球道1                
球道2             
球道3                                              
球道4                                   
球道5                         
累计
龙：0；虎：0；合：0；
和（大：0；小：0；单：0；双：0）
总和组合（大单：0；大双：0；小单：0；小双：0）
");

            }
            else
            {

                xztj xz = getXztjModel(dtXiaZhu.Rows);
                if (isCCC)
                {
                    xz = _mainGroup.xiazhutongji;
                }

                //
                int[] lj = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int io = 0; io < 5; io++)
                {
                    int zongji = 0;

                    jieGuo.Append(String.Format("{0,-4}  ", "球道" + (io + 1) + "  "));
                    for (int x = 0; x < 4; x++)
                    {
                        jieGuo.Append(String.Format("{0,-6}  ", xz.DXDS[io, x].ToString()));
                        zongji += xz.DXDS[io, x];
                        lj[x] += xz.DXDS[io, x];
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        zongji += xz.QD[io, i];
                        lj[4 + i] += xz.QD[io, i];
                        jieGuo.Append(String.Format("{0,-6}  ", xz.QD[io, i].ToString()));
                    }
                    lj[14] += zongji;
                    jieGuo.AppendLine(String.Format("{0,-6}  ", zongji.ToString()));
                }
                ListViewItem ite = new ListViewItem();
                jieGuo.AppendLine("-----------------------------------------------------------------------------------------------------------------------------");

                jieGuo.Append("累计     ");
                for (int i = 0; i < lj.Length; i++)
                {
                    jieGuo.Append(String.Format("{0,-6}  ", lj[i].ToString()));
                }
                jieGuo.AppendLine("");
                jieGuo.AppendLine("龙：" + xz.LHH[0].ToString() + " 虎：" + xz.LHH[1].ToString() + " 合：" + xz.LHH[2].ToString());
                jieGuo.AppendLine("和(大：" + xz.ZHDXDS[0].ToString() + " 小：" + xz.ZHDXDS[1].ToString() + " 单：" + xz.ZHDXDS[2].ToString() + " 双：" + xz.ZHDXDS[3].ToString() + ")");
                jieGuo.AppendLine("总和组合(大单：" + xz.ZHZHDXDS[0].ToString() + " 大双：" + xz.ZHZHDXDS[1].ToString() + " 小单：" + xz.ZHZHDXDS[2].ToString() + " 小双：" + xz.ZHZHDXDS[3].ToString() + ")");

            }
            jieGuo.AppendLine("-----------------------------------------------------------------------------------------------------------------------------");
            jieGuo.AppendLine("");
            jieGuo.AppendLine("");
            jieGuo.AppendLine("");
            jieGuo.AppendLine("");
            return jieGuo.ToString();
        }



        private xztj getXztjModel(DataRowCollection drs)
        {

            xztj xz = new xztj();
            foreach (DataRow dr in drs)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        xz.QD[x, i] += int.Parse(dr["qd" + (x + 1).ToString() + "_" + i.ToString()].ToString());
                    }
                }
                for (int i = 0; i < 5; i++)//大小单双
                {
                    for (int x = 0; x < 4; x++)
                    {
                        xz.DXDS[i, x] += int.Parse(dr["d" + (i + 1).ToString() + "_" + x.ToString()].ToString());
                    }
                }
                for (int i = 0; i < 4; i++)//总和  总和组合  龙虎和
                {
                    xz.ZHDXDS[i] += int.Parse(dr["zh" + i.ToString()].ToString());
                    xz.ZHZHDXDS[i] += int.Parse(dr["zhzh" + i.ToString()].ToString());
                    if (i != 3)
                        xz.LHH[i] += int.Parse(dr["LHH" + i.ToString()].ToString());
                }
            }
            return xz;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;
            textBox2.Text = chaXunXiaZhu(textBox1.Text);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 报盘_Load(object sender, EventArgs e)
        {

        }

        private void 报盘_Activated(object sender, EventArgs e)
        {
            button4_Click(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MainPlugin.frmMain != null && MainPlugin.frmMain.FengPan && !MainPlugin.frmMain.IsFeiPan)
            {
                button4_Click(null, null);

            }
        }






    }



}

