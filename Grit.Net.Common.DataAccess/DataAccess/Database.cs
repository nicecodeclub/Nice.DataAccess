//#define Db_MySQL --使用MySQL
//#define Db_PG --使用Postgres
//#define Db_Oracle --使用Oracle
//#define Db_Odbc --使用Odbc
#define Db_MySQL

using System;
using System.Data;
using System.Data.SqlClient;
#if(Db_MySQL)
using MySql.Data.MySqlClient;
#endif
#if (Db_PG)
using Npgsql;
#endif
#if (Db_Oracle)
using System.Data.OracleClient;
#endif
#if (Db_Odbc)
using System.Data.Odbc;
#endif

namespace Grit.Net.Common.DataAccess
{

    /// <summary>
    /// 数据库操作的帮助类
    /// </summary>
    public class Database : DataProvider, IDisposable
    {

        public readonly DatabaseType dataBase;
        private readonly string dbConnString;
        private const int LEVEL = 1;

        public Database(DatabaseType _dataBase, string _dbConnString)
        {
            dataBase = _dataBase;
            dbConnString = _dbConnString;
        }

        public override IDbConnection GetConnection()
        {
            IDbConnection conn = null;
            switch (dataBase)
            {
                case DatabaseType.SqlServer:
                    conn = new SqlConnection(dbConnString);
                    break;
#if(Db_MySQL)
                case DatabaseType.MySQL:
                    conn = new MySqlConnection(dbConnString);
                    break;
#endif
#if (Db_Oracle)
                case DatabaseType.Oracle:
                    conn = new OracleConnection(dbConnString);
                    break;
#endif
#if (Db_PG)
                case DatabaseType.Postgres:
                    conn = new NpgsqlConnection(dbConnString);
                    break;
#endif
#if (Db_Odbc)
                case DatabaseType.Odbc:
                    conn = new OdbcConnection(dbConnString);
                    break;
#endif
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
            return conn;
        }

        public override IDbConnection GetConnection(DatabaseType databaseType, string _dbConnString)
        {
            IDbConnection conn = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    conn = new SqlConnection(_dbConnString);
                    break;
#if(Db_MySQL)
                case DatabaseType.MySQL:
                    conn = new MySqlConnection(_dbConnString);
                    break;
#endif
#if (Db_Oracle)
                case DatabaseType.Oracle:
                    conn = new OracleConnection(dbConnString);
                    break;
#endif
#if (Db_PG)
                case DatabaseType.Postgres:
                    conn = new NpgsqlConnection(dbConnString);
                    break;
#endif
#if (Db_Odbc)
                case DatabaseType.Odbc:
                    conn = new OdbcConnection(dbConnString);
                    break;
#endif
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
            return conn;
        }

        public override IDbCommand GetCommand()
        {

            IDbConnection conn = null;
            switch (dataBase)
            {
                case DatabaseType.SqlServer:
                    conn = new SqlConnection(dbConnString);
                    break;
#if(Db_MySQL)
                case DatabaseType.MySQL:
                    conn = new MySqlConnection(dbConnString);
                    break;
#endif
#if (Db_Oracle)
                case DatabaseType.Oracle:
                    conn = new OracleConnection(dbConnString);
                    break;
#endif
#if (Db_PG)
                case DatabaseType.Postgres:
                    conn = new NpgsqlConnection(dbConnString);
                    break;
#endif
#if (Db_Odbc)
                case DatabaseType.Odbc:
                    conn = new OdbcConnection(dbConnString);
                    break;
#endif
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
            conn.Open();
            return conn.CreateCommand();
        }

        public override void AttachParameters(IDbCommand command, IDataParameter[] dbps)
        {
            command.Parameters.Clear();
            foreach (IDataParameter p in dbps)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        public override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter adp = null;
            switch (dataBase)
            {
                case DatabaseType.SqlServer:
                    adp = new SqlDataAdapter((SqlCommand)command);
                    break;
#if(Db_MySQL)
                case DatabaseType.MySQL:
                    adp = new MySqlDataAdapter((MySqlCommand)command);
                    break;
#endif
#if (Db_Oracle)
                case DatabaseType.Oracle:
                     adp = new OracleDataAdapter((OracleCommand)command);
                    break;
#endif
#if (Db_PG)
                case DatabaseType.Postgres:
                     adp = new NpgsqlDataAdapter((NpgsqlCommand)command);
                    break;
#endif
#if (Db_Odbc)
                case DatabaseType.Odbc:
                   adp = new OdbcDataAdapter((OdbcCommand)command);
                    break;
#endif
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
            return adp;
        }

        #region Dispose
        private bool isDisposed = false;

        /// <summary>
        /// 实现IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如C++）的规范
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary>
        ~Database()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                //if (DbFactory != null)
                //{
                //    DbFactory = null;
                //}
            }
            // 清理非托管资源……

            //让类型知道自己已经被释放
            isDisposed = true;
        }
        #endregion
    }
}
