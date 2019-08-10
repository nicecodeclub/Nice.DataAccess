using Nice.DataAccess.Model.Page;
using System.Collections.Generic;
using System.Data;

namespace Nice.DataAccess.DAL
{
    public interface IQueryDAL<T> where T : new()
    {
        T GetBySQL(string cmdText);
        T GetBySQL(string cmdText, IList<IDataParameter> parms);
        IList<T> GetListBySQL(string cmdText);
        /// <summary>
        /// SQL查询列表
        /// </summary>
        /// <param name="cmdText">SQL</param>
        /// <returns></returns>
        IList<T> GetListBySQL(string cmdText, PageInfo page);
        /// <summary>
        /// SQL查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        IList<T> GetListBySQL(string cmdText, IList<IDataParameter> parms);
        /// <summary>
        /// SQL查询列表
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IList<T> GetListBySQL(string cmdText, IList<IDataParameter> parms, PageInfo page);

        T GetBySQL2(string cmdText, IList<object> parmsValue);
        /// <summary>
        /// SQL查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        IList<T> GetListBySQL2(string cmdText, IList<object> parmsValue);
        /// <summary>
        /// SQL查询列表
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IList<T> GetListBySQL2(string cmdText, IList<object> parmsValue, PageInfo page);
    }

    public interface IQueryDAL
    {

        #region  执行SQL
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, IList<object> parmsValue);
        /// <summary>
        /// 执行SQL返回第一行第一列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText, IList<object> parmsValue);

        /// <summary>
        /// 执行SQL事务
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string[] cmdText, IList<IList<object>> parmsValue);

        /// <summary>
        /// 返回Dataset
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        //DataSet ExecuteDataSet(string cmdText, object[] parmsValue);
        #endregion
    }
}
