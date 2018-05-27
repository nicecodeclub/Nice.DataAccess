using Nice.DataAccess.DAL;

namespace Nice.DataAccess.SQLite.DAL
{
    public class SQLiteQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public SQLiteQueryDAL(string connStrKey) : base(connStrKey) { }
       
    }

}