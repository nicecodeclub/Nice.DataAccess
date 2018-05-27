using Nice.DataAccess.DAL;
using Nice.DataAccess.Models;

namespace Nice.DataAccess.SQLite.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class SQLiteGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public SQLiteGeneralDAL(string connStrKey) : base(connStrKey) { }
      
        protected override string GetLastIncrementID()
        {
            return "SELECT LAST_INSERT_ID()";
        }
    }
}
