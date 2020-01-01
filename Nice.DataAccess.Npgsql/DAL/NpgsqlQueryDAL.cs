using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;

namespace Nice.DataAccess.Npgsql.DAL
{
    public class NpgsqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public NpgsqlQueryDAL(string connStrKey) : base(connStrKey) { }

        protected override string GetPageSql(PageInfo page)
        {
            return string.Format(" ORDER BY {0} {1} limit {3} OFFSET {2}; ", page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
        }
    }

}