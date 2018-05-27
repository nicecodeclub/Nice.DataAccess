
using System;
using System.Data;
using System.Data.SQLite;

namespace Nice.DataAccess.SQLite.Provider
{
    /// <summary>
    /// SQLite数据库操作的帮助类
    /// </summary>
    public class SQLiteDataProvider : DataProvider
    {
        public SQLiteDataProvider(string _dbConnString)
        {
            dbConnString = _dbConnString;
        }

        public override IDbConnection GetConnection()
        {
            return new SQLiteConnection(dbConnString);
        }

        public override IDbCommand GetCommand()
        {
            return new SQLiteCommand();
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
            return new SQLiteDataAdapter((SQLiteCommand)command);
        }
        public override IDataParameter CreateParameter(string parameterName, object value)
        {
            return new SQLiteParameter(parameterName, value);
        }

        public override char GetParameterPrefix()
        {
            return '$';
        }
    }
}
