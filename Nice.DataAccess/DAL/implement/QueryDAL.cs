

using Nice.DataAccess.Model.Page;
using System.Collections.Generic;

namespace Nice.DataAccess.DAL
{
    public class QueryDAL<T> : IQueryDAL<T> where T : new()
    {
        private IQueryDAL<T> dal = null;
        public QueryDAL() : this(DataUtil.DefaultConnStringKey) { }

        public QueryDAL(string connStringKey)
        {
            dal = QueryFactory<T>.Create(connStringKey);
        }

        public T GetBySQL(string cmdText)
        {
            return dal.GetBySQL(cmdText);
        }

        public T GetBySQL(string cmdText, IList<object> parmsValue)
        {
            return dal.GetBySQL(cmdText, parmsValue);
        }

        public IList<T> GetListBySQL(string cmdText)
        {
            return dal.GetListBySQL(cmdText);
        }

        public IList<T> GetListBySQL(string cmdText, PageInfo page)
        {
            return dal.GetListBySQL(cmdText, page);
        }

        public IList<T> GetListBySQL(string cmdText, IList<object> parmsValue)
        {
            return dal.GetListBySQL(cmdText, parmsValue);
        }
        public IList<T> GetListBySQL(string cmdText, IList<object> parmsValue, PageInfo page)
        {
            return dal.GetListBySQL(cmdText, parmsValue, page);
        }

    }

    public class QueryDAL
    {
        private IQueryDAL dal = null;

        public QueryDAL() : this(DataUtil.DefaultConnStringKey) { }

        public QueryDAL(string connStringKey)
        {
            dal = QueryFactory.Create(connStringKey);
        }
        #region  执行SQL
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, IList<object> parmsValue)
        {
            return dal.ExecuteNonQuery(cmdText, parmsValue);
        }
        /// <summary>
        /// 执行SQL返回第一行第一列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, IList<object> parmsValue)
        {
            return dal.ExecuteScalar(cmdText, parmsValue);
        }
        /// <summary>
        /// 执行SQL事务
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string[] cmdText, IList<IList<object>> parmsValue)
        {
            return dal.ExecuteNonQuery(cmdText, parmsValue);
        }

        /// <summary>
        /// 执行SQL，返回DataSet
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        //public  DataSet ExecuteDataSet(string cmdText, object[] parmsValue)
        //{
        //    return dal.ExecuteDataSet(cmdText, parmsValue);
        //}
        #endregion
    }
}
