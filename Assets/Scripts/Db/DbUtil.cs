using System;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

namespace Db
{
    public class DbUtil
    {
        private static string dbPath = Application.persistentDataPath + "/DB/gopl.db";

        private static SqliteConnection dbConn;
        
        // 获取数据库连接
        public static SqliteConnection GetDbConnection()
        {
            try
            {
                dbConn = new SqliteConnection(
                    new SqliteConnectionStringBuilder() { DataSource = dbPath }.ToString());
                dbConn.Open();
                return dbConn;
            }
            catch (Exception e)
            {
                Debug.LogError($"数据库连接异常: {e.Message}");
                return null;
            }
        }
        
        // 查询数据
        public static IDataReader QueryData(string querySQL)
        {
            var dbConn = GetDbConnection();
            IDbCommand readCmnd = dbConn.CreateCommand();
            readCmnd.CommandText = querySQL;
            IDataReader dataReader = readCmnd.ExecuteReader();
            
            return dataReader;
        }
        
        // 插入数据
        public static void InsertData(string insertSQL)
        {
            var dbConn = GetDbConnection();
            IDbCommand cmnd = dbConn.CreateCommand();
            cmnd.CommandText = insertSQL;
            cmnd.ExecuteNonQuery();
        }

        // 关闭连接，释放资源
        public static void CloseDbConn()
        {
            dbConn.Close();
        }
    }
}
