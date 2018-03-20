
using System;
using System.Data;
using System.Data.SqlClient;

namespace Nice.DataAccess.MsSql.Provider
{
    /// <summary>
    /// MySql数据库操作的帮助类
    /// </summary>
    public class MsSqlDataProvider : DataProvider
    {
        public MsSqlDataProvider(string _dbConnString)
        {
            dbConnString = _dbConnString;
        }

        public override IDbConnection GetConnection()
        {
            return new SqlConnection(dbConnString);
        }

        public override IDbCommand GetCommand()
        {
            return new SqlCommand();
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
            return new SqlDataAdapter((SqlCommand)command);
        }
        public override IDataParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        public override char GetParameterPrefix()
        {
            return '?';
        }
    }
}
