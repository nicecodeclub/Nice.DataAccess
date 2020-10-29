using Nice.DataAccess.Models;

namespace Nice.DataAccess.DAL
{
    public static class DALFactory<T> where T : TEntity, new()
    {
        private static readonly GeneralDAL<T> dal = new GeneralDAL<T>();

        public static GeneralDAL<T> Create()
        {
            return dal;
        }
    }
}
