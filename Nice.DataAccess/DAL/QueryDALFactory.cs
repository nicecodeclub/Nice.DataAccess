namespace Nice.DataAccess.DAL
{
    public class QueryDALFactory<T> where T : new()
    {
        private static readonly QueryDAL<T> dal = new QueryDAL<T>();

        public static QueryDAL<T> Create()
        {
            return dal;
        }
    }
}
