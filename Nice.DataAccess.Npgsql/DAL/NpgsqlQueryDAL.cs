using Nice.DataAccess.DAL;

namespace Nice.DataAccess.Npgsql.DAL
{
    public class NpgsqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public NpgsqlQueryDAL(string connStrKey) : base(connStrKey) { }
    }

}