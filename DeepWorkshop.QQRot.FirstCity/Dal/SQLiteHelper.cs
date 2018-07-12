using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data.SQLite;
using System.Data;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using www_52bang_site_enjoy.MyTool;

namespace Dal
{
    public class SQLiteHelper
    {
        private static string databaseName = MySystemUtil.GetDllRoot()+ "DATA.db3";

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseName">数据库文件路径</param>
        public static void CreateDataBase()
        {
            MyLogUtil.ToLogFotTest("建立sqllite数据库时的文件路径："+databaseName);
            if (!File.Exists(databaseName))
            {
                SQLiteConnection.CreateFile(databaseName);
            }
        }

        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection GetSQLiteConnection()
        {
            #region 方法一
            SQLiteConnectionStringBuilder connStr = new SQLiteConnectionStringBuilder();
            connStr.DataSource = databaseName;
            connStr.Password = "lipanfeng";
            connStr.Pooling = true;
            return new SQLiteConnection(connStr.ToString());
            #endregion

            #region 方法二
            //return new SQLiteConnection(string.Format("Data Source={0};password=lipanfeng", databaseName));
            #endregion
        }

        /// <summary>
        /// 匹配参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, string cmdText, params object[] p)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 30;
            if (p != null)
            {
                foreach (object item in p)
                {
                    cmd.Parameters.AddWithValue(string.Empty, item);
                }
            }
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strSql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = GetSQLiteConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable();

                    sda.Fill(dt);
                    return dt;
                }
            }

        }
        /// <summary>
        /// 返回第一行
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataRow ExecuteDataRow(string cmdText, SQLiteParameter[] parameters)
        {
            DataTable dt = ExecuteDataTable(cmdText, parameters);
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// 执行非查询操作 返回受影响的行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, params object[] p)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = GetSQLiteConnection())
            {
                try
                {
                    PrepareCommand(cmd, conn, cmdText, p);
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行非查询操作 返回受影响的行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, string tbName, params object[] p)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = GetSQLiteConnection())
            {
                try
                {
                    PrepareCommand(cmd, conn, cmdText, p);
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex.Message == string.Format("SQLite error\r\ntable {0} already exists", tbName))
                    {
                        return 0;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 返回SQLiteDataReader
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteReader(string cmdText, params object[] p)
        {
            SQLiteConnection conn = GetSQLiteConnection();
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, conn, cmdText, p);
                SQLiteDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception)
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 返回结果集的首行首列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params object[] p)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = GetSQLiteConnection())
            {
                PrepareCommand(cmd, conn, cmdText, p);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 分页获取DataSet
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cmdText"></param>
        /// <param name="countText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataSet ExecutePager(ref int recordCount, int pageIndex, int pageSize, string cmdText, string countText, params object[] p)
        {
            if (recordCount < 0)
                recordCount = int.Parse(ExecuteScalar(countText, p).ToString());
            DataSet ds = new DataSet();
            SQLiteCommand command = new SQLiteCommand();
            using (SQLiteConnection connection = GetSQLiteConnection())
            {
                PrepareCommand(command, connection, cmdText, p);
                SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, "result");
            }
            return ds;
        }
    }
}
