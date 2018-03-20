using Nice.DataAccess.DAL;
using Nice.DataAccess.Models;

namespace Nice.DataAccess.MsSql.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class MsSqlGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public MsSqlGeneralDAL(string connStrKey) : base(connStrKey) { }
      
        protected override string GetLastIncrementID()
        {
            return "SELECT SCOPE_IDENTITY()";
        }
    }
}
