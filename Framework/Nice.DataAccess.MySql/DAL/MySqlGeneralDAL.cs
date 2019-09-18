using Nice.DataAccess.DAL;
using Nice.DataAccess.Models;

namespace Nice.DataAccess.MySql.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class MySqlGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public MySqlGeneralDAL(string connStrKey) : base(connStrKey) { }
      
        protected override string GetLastIncrementID()
        {
            return "SELECT LAST_INSERT_ID()";
        }
        protected override string GetInsertOrUpdateSql()
        {
            return string.Format("INSERT INTO {0}({1}) VALUES({2}) ON DUPLICATE KEY UPDATE {3} ", TableName, InsertColumnText, InsertColumnValue, SetColumnText);
        }
    }
}
