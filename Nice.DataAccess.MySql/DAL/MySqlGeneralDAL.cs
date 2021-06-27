using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;
using Nice.DataAccess.Models;
using System.Text;

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

        protected override string GetPageSql(PageInfo page)
        {
            return string.Format(" ORDER BY {0}.{1} {2} LIMIT {3},{4}; "
                    , ClassSortName, string.IsNullOrEmpty(page.OrderColName) ? IdColomn.ColomnName : page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
        }

        protected override string GetCountFuncName()
        {
            return "COUNT";
        }
    }
}
