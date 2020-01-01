using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;

namespace Nice.DataAccess.MySql.DAL
{
    public class MsSqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public MsSqlQueryDAL(string connStrKey) : base(connStrKey) { }
        protected override string GetPageSql(PageInfo page)
        {
            return string.Format(" ORDER BY {0} {1} OFFSET {2} ROW FETCH NEXT {3} ROW ONLY ", page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
        }
    }

}