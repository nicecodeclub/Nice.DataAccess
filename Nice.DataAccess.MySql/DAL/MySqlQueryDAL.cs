using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;

namespace Nice.DataAccess.MySql.DAL
{
    public class MySqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public MySqlQueryDAL(string connStrKey) : base(connStrKey) { }

        protected override string GetPageSql(PageInfo page)
        {
            return string.Format(" ORDER BY {0} {1} LIMIT {2},{3}; ", page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
        }
    }

}