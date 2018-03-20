using Nice.DataAccess.DAL;

namespace Nice.DataAccess.MySql.DAL
{
    public class MsSqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public MsSqlQueryDAL(string connStrKey) : base(connStrKey) { }
       
    }

}