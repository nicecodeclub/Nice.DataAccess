using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Nice.DataAccess.Exceptions
{
    [Serializable]
    public sealed class DbExcuteException : DbException
    {
        private string sql;
        public string Sql
        {
            get { return sql; }
            private set { sql = value; }
        }
        public DbExcuteException() { }
        public DbExcuteException(string sql, DbException ex)
           : base(ex.Message, ex)
        {
            this.sql = sql;
        }

    }
}
