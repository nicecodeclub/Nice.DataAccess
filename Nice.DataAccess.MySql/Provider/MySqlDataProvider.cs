
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Nice.DataAccess.MySql.Provider
{
    /// <summary>
    /// MySql数据库操作的帮助类
    /// </summary>
    public class MySqlDataProvider : DataProvider
    {
        public MySqlDataProvider(string _dbConnString)
        {
            dbConnString = _dbConnString;
        }

        public override IDbConnection GetConnection()
        {
            return new MySqlConnection(dbConnString);
        }

        public override IDbCommand GetCommand()
        {
            return new MySqlCommand();
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
            return new MySqlDataAdapter((MySqlCommand)command);
        }

    }
}
