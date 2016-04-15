using Grit.Net.Common.Models;
using Grit.Net.Common.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.DAL
{
    public interface IQueryDAL<T> where T : TModel, new()
    {
        T GetBySQL(string cmdText);
        T GetBySQL(string cmdText, object[] parmsValue);
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
        IList<T> GetListBySQL(string cmdText, object[] parmsValue);


        #region  执行SQL
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, object[] parmsValue);
        /// <summary>
        /// 执行SQL返回第一行第一列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText, object[] parmsValue);

        /// <summary>
        /// 执行SQL事务
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
         int ExecuteNonQuery(string[] cmdText, object[][] parmsValue);
        #endregion
    }
}
