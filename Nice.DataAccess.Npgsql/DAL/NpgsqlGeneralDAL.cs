using Nice.DataAccess.DAL;
using Nice.DataAccess.Models;
using System.Text;

namespace Nice.DataAccess.Npgsql.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class NpgsqlGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public NpgsqlGeneralDAL(string connStrKey) : base(connStrKey) { }

        protected override string GetLastIncrementID()
        {
            return "RETURNING id";
        }

        protected override string GetInsertOrUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0}({1}) VALUES({2})", TableName, InsertColumnText, InsertColumnValue);
            sb.AppendFormat("ON CONFLICT({0}) DO UPDATE SET {1} ", IdColomn.ColomnName, SetColumnText);
            return sb.ToString();
        }
    }
}
