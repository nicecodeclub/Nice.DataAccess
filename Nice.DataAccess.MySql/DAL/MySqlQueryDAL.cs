using Nice.DataAccess.DAL;

namespace Nice.DataAccess.MySql.DAL
{
    public class MySqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public MySqlQueryDAL(string connStrKey) : base(connStrKey) { }
       
    }

}