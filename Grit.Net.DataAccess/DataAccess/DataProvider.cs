using System.Data;

namespace Grit.Net.DataAccess
{
    public abstract class DataProvider {
        public abstract IDbConnection GetConnection();
        public abstract IDbConnection GetConnection(DatabaseType databaseType, string _dbConnString);
        public abstract IDbCommand GetCommand();
        public abstract void AttachParameters(IDbCommand command, IDataParameter[] dbps);
        public abstract IDbDataAdapter GetDataAdapter(IDbCommand command);
    }
}
