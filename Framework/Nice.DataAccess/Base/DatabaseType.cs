namespace Nice.DataAccess
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
        SQLite = 5,
        Odbc = 6,
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
            if (providerName == "SYSTEM.DATA.SQLCLIENT") return DatabaseType.SqlServer;
            else if (providerName == "NPGSQL") return DatabaseType.Postgres;
            else if (providerName == "MYSQL") return DatabaseType.MySQL;
            else if (providerName == "Oracle.ManagedDataAccess.Client") return DatabaseType.Oracle;
            else if (providerName == "SYSTEM.DATA.SQLITE") return DatabaseType.SQLite;
            else if (providerName == "SYSTEM.DATA.ODBC") return DatabaseType.Odbc;
            else return DatabaseType.Unknown;
        }

        internal static string GetStandardInnerName(string providerName)
        {
            providerName = providerName.ToUpper();
            if (providerName == "SYSTEM.DATA.SQLCLIENT") return "MsSql";
            else if (providerName == "NPGSQL") return "Npgsql";
            else if (providerName == "MYSQL") return "MySql";
            else if (providerName == "Oracle.ManagedDataAccess.Client") return "Oracle";
            else if (providerName == "SYSTEM.DATA.SQLITE") return "SQLite";
            else if (providerName == "SYSTEM.DATA.ODBC") return "odbc";
            else return string.Empty;
        }
    }
}
