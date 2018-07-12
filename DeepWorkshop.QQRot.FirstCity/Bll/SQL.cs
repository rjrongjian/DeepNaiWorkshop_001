using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI.Bll
{
    public class KeyVal
    {
        public string Key = "";
        public string Val = "";

        public KeyVal()
        {
        }

        public KeyVal(string k, string v)
        {
            Key = k;
            Val = v;
        }
    }

    public class SQL
    {

        public SQL(string tabelName)
        {
            try
            {
                // 创建数据库
                //SQLiteHelper.CreateDataBase();
                //初始化数据库
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE Friends_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                 'seq' varchar(0),
                                                'NickName' varchar(0),
                                                '推荐人' varchar(0),
                                                '是否入局' varchar(0),
                                                '现有积分' varchar(0),
                                                '总盈亏' varchar(0),
                                                '总下注' varchar(0),
                                                '本地备注' varchar(0),
                                                Time varchar(0));", "Friends_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@"CREATE TABLE NameInt_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                'seq' varchar(0),
                                                'NickName' varchar(0),
                                                '期号' varchar(0),
                                                '下注文本' varchar(0),
                                                '盈亏' varchar(0),
                                                '下注积分' varchar(0),
                                                '结算后积分' varchar(0),
                                                'qd1_0' varchar(0), 'qd1_1' varchar(0), 'qd1_2' varchar(0), 'qd1_3' varchar(0), 'qd1_4' varchar(0),
                                                'qd1_5' varchar(0), 'qd1_6' varchar(0), 'qd1_7' varchar(0), 'qd1_8' varchar(0), 'qd1_9' varchar(0),
                                                'qd2_0' varchar(0), 'qd2_1' varchar(0), 'qd2_2' varchar(0), 'qd2_3' varchar(0), 'qd2_4' varchar(0),
                                                'qd2_5' varchar(0), 'qd2_6' varchar(0), 'qd2_7' varchar(0), 'qd2_8' varchar(0), 'qd2_9' varchar(0),
                                                'qd3_0' varchar(0), 'qd3_1' varchar(0), 'qd3_2' varchar(0), 'qd3_3' varchar(0), 'qd3_4' varchar(0),
                                                'qd3_5' varchar(0), 'qd3_6' varchar(0), 'qd3_7' varchar(0), 'qd3_8' varchar(0), 'qd3_9' varchar(0),
                                                'qd4_0' varchar(0), 'qd4_1' varchar(0), 'qd4_2' varchar(0), 'qd4_3' varchar(0), 'qd4_4' varchar(0),
                                                'qd4_5' varchar(0), 'qd4_6' varchar(0), 'qd4_7' varchar(0), 'qd4_8' varchar(0), 'qd4_9' varchar(0),
                                                'qd5_0' varchar(0), 'qd5_1' varchar(0), 'qd5_2' varchar(0), 'qd5_3' varchar(0), 'qd5_4' varchar(0),
                                                'qd5_5' varchar(0), 'qd5_6' varchar(0), 'qd5_7' varchar(0), 'qd5_8' varchar(0), 'qd5_9' varchar(0),
                                                'd1_0' varchar(0), 'd1_1' varchar(0), 'd1_2' varchar(0), 'd1_3' varchar(0),
                                                'd2_0' varchar(0), 'd2_1' varchar(0), 'd2_2' varchar(0), 'd2_3' varchar(0),
                                                'd3_0' varchar(0), 'd3_1' varchar(0), 'd3_2' varchar(0), 'd3_3' varchar(0),
                                                'd4_0' varchar(0), 'd4_1' varchar(0), 'd4_2' varchar(0), 'd4_3' varchar(0),
                                                'd5_0' varchar(0), 'd5_1' varchar(0), 'd5_2' varchar(0), 'd5_3' varchar(0),
                                                'zh0' varchar(0), 'zh1' varchar(0), 'zh2' varchar(0), 'zh3' varchar(0),
                                                'zhzh0' varchar(0), 'zhzh1' varchar(0), 'zhzh2' varchar(0), 'zhzh3' varchar(0),
                                                'LHH0' varchar(0), 'LHH1' varchar(0), 'LHH2' varchar(0),
                                                Time varchar(0));", "NameInt_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE liushui_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                 'seq' varchar(0),
                                                'NickName' varchar(0),
                                                '期号' varchar(0),
                                                '类型' varchar(0),
                                                '积分' varchar(0),
                                                '剩余积分' varchar(0),
                                                '备注' varchar(0),
                                                Time varchar(0));", "liushui_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE liushuiAct_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                 'seq' varchar(0),
                                                'NickName' varchar(0),
                                                '期号' varchar(0),
                                                '类型' varchar(0),
                                                '实际下注' varchar(0),
                                                '实际中奖' varchar(0), 
                                                '实际下注文本' varchar(0),
                                                '备注' varchar(0),
                                                Time varchar(0));", "liushuiAct_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE liaotian_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                 'seq' varchar(0),
                                                '昵称' varchar(0),
                                                '内容' varchar(0),
                                                Time varchar(0));", "liaotian_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE kaijiang_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                 '期号' varchar(0),
                                                'qd1' varchar(0),
                                               '总下注积分' varchar(0), '盈亏' varchar(0),
                                                Time varchar(0));", "kaijiang_" + tabelName);
                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE chaxun_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                '时间' varchar(0),
                                                Time varchar(0));", "chaxun_" + tabelName);


                SQLiteHelper.ExecuteNonQuery(
@"CREATE TABLE FeiPan_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,                                               
'期号' varchar(0),
'服务器地址' varchar(0),
'提交结束时间' varchar(0),
'当期注额' varchar(0),
'成功提交合计' varchar(0),
'账号剩余金额' varchar(0),
'提交结果' varchar(0),
'qd1_0' varchar(0), 'qd1_1' varchar(0), 'qd1_2' varchar(0), 'qd1_3' varchar(0), 'qd1_4' varchar(0),
'qd1_5' varchar(0), 'qd1_6' varchar(0), 'qd1_7' varchar(0), 'qd1_8' varchar(0), 'qd1_9' varchar(0),
'qd2_0' varchar(0), 'qd2_1' varchar(0), 'qd2_2' varchar(0), 'qd2_3' varchar(0), 'qd2_4' varchar(0),
'qd2_5' varchar(0), 'qd2_6' varchar(0), 'qd2_7' varchar(0), 'qd2_8' varchar(0), 'qd2_9' varchar(0),
'qd3_0' varchar(0), 'qd3_1' varchar(0), 'qd3_2' varchar(0), 'qd3_3' varchar(0), 'qd3_4' varchar(0),
'qd3_5' varchar(0), 'qd3_6' varchar(0), 'qd3_7' varchar(0), 'qd3_8' varchar(0), 'qd3_9' varchar(0),
'qd4_0' varchar(0), 'qd4_1' varchar(0), 'qd4_2' varchar(0), 'qd4_3' varchar(0), 'qd4_4' varchar(0),
'qd4_5' varchar(0), 'qd4_6' varchar(0), 'qd4_7' varchar(0), 'qd4_8' varchar(0), 'qd4_9' varchar(0),
'qd5_0' varchar(0), 'qd5_1' varchar(0), 'qd5_2' varchar(0), 'qd5_3' varchar(0), 'qd5_4' varchar(0),
'qd5_5' varchar(0), 'qd5_6' varchar(0), 'qd5_7' varchar(0), 'qd5_8' varchar(0), 'qd5_9' varchar(0),
'd1_0' varchar(0), 'd1_1' varchar(0), 'd1_2' varchar(0), 'd1_3' varchar(0),
'd2_0' varchar(0), 'd2_1' varchar(0), 'd2_2' varchar(0), 'd2_3' varchar(0),
'd3_0' varchar(0), 'd3_1' varchar(0), 'd3_2' varchar(0), 'd3_3' varchar(0),
'd4_0' varchar(0), 'd4_1' varchar(0), 'd4_2' varchar(0), 'd4_3' varchar(0),
'd5_0' varchar(0), 'd5_1' varchar(0), 'd5_2' varchar(0), 'd5_3' varchar(0),
'zh0' varchar(0), 'zh1' varchar(0), 'zh2' varchar(0), 'zh3' varchar(0),
'zhzh0' varchar(0), 'zhzh1' varchar(0), 'zhzh2' varchar(0), 'zhzh3' varchar(0),
'LHH0' varchar(0), 'LHH1' varchar(0), 'LHH2' varchar(0),
Time varchar(0));", "FeiPan_" + tabelName);

                SQLiteHelper.ExecuteNonQuery(@" CREATE TABLE fuwuqi_" + tabelName + @"(Id Integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                    '类型' varchar(0),
                                                    '服务器地址' varchar(0),
                                                    '用户名' varchar(0), 
                                                    '密码' varchar(0),
                                                    Time varchar(0));", "fuwuqi_" + tabelName);
            }
            catch (Exception ex)
            {
                return;
            }

        }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool tabelbool(string tableName)
        {
            if (tableName == null)
            {
                return false;
            }
            try
            {
                string sql = "SELECT * FROM sqlite_master where type='table' and name='" + tableName + "'";
                if (SQLiteHelper.ExecuteDataRow(sql, null) != null)
                    return true;
            }
            catch (Exception ex) { }
            return false;
        }
        /// <summary>
        /// 表名列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable tabellist()
        {
            string sql = "select name from sqlite_master where type='table' order by name";
            DataTable row = SQLiteHelper.ExecuteDataTable(sql, null);
            return row;
        }

        /// <summary>
        /// 返回记录数
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string tabelint(string str)
        {
            string sql = "select count(*) from " + str;
            return SQLiteHelper.ExecuteDataRow(sql, null)[0].ToString();
        }
        /// <summary>
        /// 新增一行
        /// </summary>
        /// <param name="c"></param>
        public static int INSERT(List<KeyVal> c, string str)
        {
            string Key = "";
            string Val = "";
            foreach (KeyVal j in c)
            {
                Key = Key + j.Key + ",";
                Val = Val + "'" + j.Val + "',";
            }
            string strSql = string.Format(@"INSERT INTO " + str + " ({0}Time)VALUES({1}datetime(CURRENT_TIMESTAMP,'localtime'));", Key, Val);
            return SQLiteHelper.ExecuteNonQuery(strSql);
        }
        /// <summary>
        /// 新增一行
        /// </summary>
        /// <param name="c"></param>
        public static int INSERT(string Key, string Val, string str)
        {
            string strSql = string.Format(@"INSERT INTO " + str + " ({0},Time)VALUES({1},datetime(CURRENT_TIMESTAMP,'localtime'));", Key, Val);
            return SQLiteHelper.ExecuteNonQuery(strSql);
        }
        /// <summary>
        /// 修改一行
        /// </summary>
        /// <param name="c"></param>
        public static int UPDATE(string ID, List<KeyVal> c, string str)
        {
            string Val = "";
            foreach (KeyVal j in c)
            {
                Val += "'" + j.Key + "'=" + "'" + j.Val + "',";
            }
            Val += "Time=datetime(CURRENT_TIMESTAMP,'localtime')";
            string delStr = string.Format(@"UPDATE " + str + " SET {0} where Id ={1}", Val, ID);
            return SQLiteHelper.ExecuteNonQuery(delStr);
        }
        //更新
        public static int UPDATE(string ID, string Key, string Val, string str)
        {
            Val = "'" + Key + "'=" + "'" + Val + "'";
            string delStr = string.Format(@"UPDATE " + str + " SET {0} where Id ={1}", Val, ID);
            return SQLiteHelper.ExecuteNonQuery(delStr);
        }
        //删除
        public static int delete(string name)
        {
            string delStr = string.Format(@"delete from " + name);
            return SQLiteHelper.ExecuteNonQuery(delStr);
        }

        //SELECT RemarkName FROM buddy WHERE SelfUin = '{0}' and buddyUin = '{1}';
        /// <summary>
        /// 查询一行
        /// </summary>
        /// <param name="c"></param>
        public static DataRow SELECTRow(string WHERE, string str)
        {
            string delStr = string.Format(@"SELECT * FROM " + str + " WHERE " + WHERE);
            DataRow row = SQLiteHelper.ExecuteDataRow(delStr, null);
            return row;
        }
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="c"></param>
        public static DataTable SELECTdata(string condition, string str)
        {
            try
            {
                string delStr = string.Format(@"SELECT * FROM " + str + condition);

                DataTable row = SQLiteHelper.ExecuteDataTable(delStr, null);
                return row;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new DataTable();
            }


        }
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="c"></param>
        public static DataTable SELECTdata(string tab, string condition, string str)
        {

            string delStr = string.Format(@"SELECT " + tab + " FROM " + str + " WHERE " + condition);
            DataTable row = SQLiteHelper.ExecuteDataTable(delStr, null);
            return row;
        }


    }
}
