using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AI.Bll
{
    public class xztj
    {
        /// <summary>
        /// 球道
        /// </summary>
        public int[,] QD = new int[5, 10] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        /// <summary>
        /// 大小单双
        /// </summary>
        public int[,] DXDS = new int[5, 4]//0大  1小 2单  3双
        { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

        /// <summary>
        /// 总和大小单双
        /// </summary>
        public int[] ZHDXDS = new int[] { 0, 0, 0, 0 };// 0总大 1总小  2总单 3总双

        /// <summary>
        /// 总和组合大小单双
        /// </summary>
        public int[] ZHZHDXDS = new int[] { 0, 0, 0, 0 };// 0大单 1大双  2小单 3小双

        /// <summary>
        /// 龙虎和
        /// </summary>
        public int[] LHH = new int[] { 0, 0, 0 };  //0 龙 Q1>Q5  1虎 Q1<Q5  3和 Q1=Q5
    }
    public class kjtj
    {
        public string qihao = "";
        public string kjhm = "";
        public int[] QD = new int[] { 0, 0, 0, 0, 0 };


        //0大  1小 2单  3双
        public bool[,] DXDS = new bool[5, 4] 
       {
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false }
       };
        public bool[] ZH = new bool[] { false, false, false, false };// 0总大 1总小  2总单 3总双

        public bool[] ZHzh = new bool[] { false, false, false, false };// 0大单 1大双  2小单 3小双

        public bool[] LHH = new bool[] { false, false, false }; //0 龙 Q1>Q5  1虎 Q1<Q5  3和 Q1=Q5

    }
    public class lsxe
    {
        public int id = 0;
        public string name = "";
        public string gz = "";
        public int xe = 0;
        public int dqxz = 0;
    }
    public class guize
    {
        public List<lsxe> cshxe = new List<lsxe>();
        public guize()
        {
            for (int i = 0; i < 25; i++)
            {
                lsxe x = new lsxe();
                x.id = i;
                if (i < 4 && i > 13)
                { x.xe = 6000; }
                else
                { x.xe = 3000; if (i > 22)x.xe = 6000; }

                if (i == 0)
                { x.name = "大"; x.gz = "5,6,7,8,9"; }
                if (i == 1)
                { x.name = "小"; x.gz = "1,2,3,4,0"; }
                if (i == 2)
                { x.name = "单"; x.gz = "1,3,5,7,9"; }
                if (i == 3)
                { x.name = "双"; x.gz = "2,4,6,8,0"; }
                if (i == 4)
                { x.name = "0"; x.gz = "0"; }
                if (i == 5)
                { x.name = "1"; x.gz = "1"; }
                if (i == 6)
                { x.name = "2"; x.gz = "2"; }
                if (i == 7)
                { x.name = "3"; x.gz = "3"; }
                if (i == 8)
                { x.name = "4"; x.gz = "4"; }
                if (i == 9)
                { x.name = "5"; x.gz = "5"; }
                if (i == 10)
                { x.name = "6"; x.gz = "6"; }
                if (i == 11)
                { x.name = "7"; x.gz = "7"; }
                if (i == 12)
                { x.name = "8"; x.gz = "8"; }
                if (i == 13)
                { x.name = "9"; x.gz = "9"; }
                if (i == 14)
                { x.name = "和数大"; x.gz = "和数>=23"; }
                if (i == 15)
                { x.name = "和数小"; x.gz = "和数<=22"; }
                if (i == 16)
                { x.name = "和数单"; x.gz = "和数%2==1"; }
                if (i == 17)
                { x.name = "和数双"; x.gz = "和数%2==0"; }
                if (i == 18)
                { x.name = "和数大单"; x.gz = "和数>22 and 和数%2==1"; }
                if (i == 19)
                { x.name = "和数大双"; x.gz = "和数>23 and 和数%2==0"; }
                if (i == 20)
                { x.name = "和数小单"; x.gz = "和数<23 and 和数%2==1"; }
                if (i == 21)
                { x.name = "和数小双"; x.gz = "和数<23 and 和数%2==0"; }
                if (i == 22)
                { x.name = "龙"; x.gz = "球1>球5"; }
                if (i == 23)
                { x.name = "虎"; x.gz = "球1<球5"; }
                if (i == 24)
                { x.name = "和【合】"; x.gz = "球1==球5"; }

                cshxe.Add(x);
            }
            DataTable det = SQLiteHelper.ExecuteDataTable("select * from peizhi where Id=2", null);
            if (det.Rows.Count == 0)
            {
                string cot = "";
                foreach (lsxe ox in cshxe)
                {
                    cot += ox.xe.ToString() + "|";
                }
                SQL.INSERT("账单", "'" + cot + "'", "peizhi");
            }
            else
            {
                string[] sz = det.Rows[0][1].ToString().Split('|');
                for (int p = 0; p < sz.Length; p++)
                {
                    cshxe[p].xe = int.Parse(sz[p]);
                    if (p == 21)
                        return;
                }
            }
        }
        public bool sava()
        {
            string cot = "";
            foreach (lsxe ox in cshxe)
            {
                cot += ox.xe.ToString() + "|";
            }
            if (SQL.UPDATE("2", "账单", cot, "peizhi") == 1)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 赔率
    /// </summary>
    public class peilv
    {
        /// <summary>
        /// 球道
        /// </summary>
        public decimal[,] QD = new decimal[5, 10] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        /// <summary>
        /// 大小单双
        /// </summary>
        public decimal[,] DXDS = new decimal[5, 4]//0大  1小 2单  3双
        { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

        /// <summary>
        /// 总和大小单双
        /// </summary>
        public decimal[] ZHDXDS = new decimal[] { 0, 0, 0, 0 };// 0总大 1总小  2总单 3总双

        /// <summary>
        /// 总和组合大小单双
        /// </summary>
        public decimal[] ZHZHDXDS = new decimal[] { 0, 0, 0, 0 };// 0大单 1大双  2小单 3小双

        /// <summary>
        /// 龙虎和
        /// </summary>
        public decimal[] LHH = new decimal[] { 0, 0, 0 };  //0 龙 Q1>Q5  1虎 Q1<Q5  3和 Q1=Q5
    }

    /// <summary>
    /// 飞盘提交参数名称
    /// </summary>
    public class feiPanCanShu
    {
        /// <summary>
        /// 球道
        /// </summary>
        public string[,] QD = new string[5, 10] {{ "", "", "", "", "", "", "", "", "", "" },
       { "", "", "", "", "", "", "", "", "", "" },
       { "", "", "", "", "", "", "", "", "", "" },
       { "", "", "", "", "", "", "", "", "", "" },
       { "", "", "", "", "", "", "", "", "", "" }};

        /// <summary>
        /// 大小单双
        /// </summary>
        public string[,] DXDS = new string[5, 4]//0大  1小 2单  3双
        { { "", "", "", "" }, { "", "", "", "" }, { "", "", "", "" }, { "", "", "", "" }, { "", "", "", "" } };

        /// <summary>
        /// 总和大小单双
        /// </summary>
        public string[] ZHDXDS = new string[] { "", "", "", "" };// 0总大 1总小  2总单 3总双

        /// <summary>
        /// 总和组合大小单双
        /// </summary>
        public string[] ZHZHDXDS = new string[] { "", "", "", "" };// 0大单 1大双  2小单 3小双

        /// <summary>
        /// 龙虎和
        /// </summary>
        public string[] LHH = new string[] { "", "", "" };  //0 龙 Q1>Q5  1虎 Q1<Q5  3和 Q1=Q5
    }

    public class feiPanJieGuo
    {
        public bool isSuccess = false;

        public string errorMessage = "";

        public string serverUrl = "";

        public string xiaZhu = "";

        public string yuE = "";

        /// <summary>
        /// 球道
        /// </summary>
        public bool[,] QD = new bool[5, 10] {{ false, false, false, false, false, false, false, false, false, false },
       { false, false, false, false, false, false, false, false, false, false },
       { false, false, false, false, false, false, false, false, false, false },
       { false, false, false, false, false, false, false, false, false, false },
       { false, false, false, false, false, false, false, false, false, false }};

        //0大  1小 2单  3双
        public bool[,] DXDS = new bool[5, 4] 
       {
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false }
       };
        /// <summary>
        /// 总和大小单双
        /// </summary>
        public bool[] ZHDXDS = new bool[] { false, false, false, false };// 0总大 1总小  2总单 3总双
        /// <summary>
        /// 总和组合大小单双
        /// </summary>
        public bool[] ZHZHDXDS = new bool[] { false, false, false, false };// 0大单 1大双  2小单 3小双
        /// <summary>
        /// 龙虎和
        /// </summary>
        public bool[] LHH = new bool[] { false, false, false }; //0 龙 Q1>Q5  1虎 Q1<Q5  3和 Q1=Q5


    }
}
