using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Net.Common.DataAccess
{
    /// <summary>
    /// 连接数据库类型
    /// </summary>
    public enum DatabaseType
    {
        SqlServer = 1,
        Oracle = 2,
        MySQL = 3,
        Postgres = 4,
        Odbc = 5,
        Unknown = 0
    }

    public class DatabaseTypeEx
    {
        /// <summary>
        /// 将驱动器名转化为连接数据库类型
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static DatabaseType Convert(string providerName)
        {
            providerName = providerName.ToUpper();
            if (providerName == "" || providerName == "SYSTEM.DATA.SQLCLIENT") return DatabaseType.SqlServer;
            else if (providerName == "SYSTEM.DATA.ORACLECLIENT") return DatabaseType.Oracle;
            else if (providerName == "MYSQL") return DatabaseType.MySQL;
            else if (providerName == "SYSTEM.DATA.ODBC") return DatabaseType.Odbc;
            else if (providerName == "NPGSQL") return DatabaseType.Postgres;
            else return DatabaseType.Unknown;
        }
        /// <summary>
        /// 连接数据库类型转换为驱动器名
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static string GetProviderName(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.SqlServer) return "System.Data.SqlClient";
            else if (databaseType == DatabaseType.Oracle) return "System.Data.OracleClient";
            else if (databaseType == DatabaseType.MySQL) return "MYSQL";
            else if (databaseType == DatabaseType.Odbc) return "System.Data.Odbc";
            else if (databaseType == DatabaseType.Postgres) return "NPGSQL";
            else return "None";
        }
    }
}
