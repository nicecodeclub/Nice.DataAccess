using Grit.Net.Common.DataAccess;
using Grit.Net.Common.Models;
using Grit.Net.Common.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.DAL
{
    public class QueryDAL<T> : IQueryDAL<T> where T : TModel, new()
    {
        private IQueryDAL<T> dal = null;
        public QueryDAL()
        {
            switch (DataHelper.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    dal = new MSSqlQueryDAL<T>();
                    break;
                case DatabaseType.MySQL:
                    dal = new MySqlQueryDAL<T>();
                    break;
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
        }
        public T GetBySQL(string cmdText)
        {
            return dal.GetBySQL(cmdText);
        }

        public T GetBySQL(string cmdText, object[] parmsValue)
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

        public IList<T> GetListBySQL(string cmdText, object[] parmsValue)
        {
            return dal.GetListBySQL(cmdText, parmsValue);
        }

        #region  执行SQL
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, object[] parmsValue)
        {
            return dal.ExecuteNonQuery(cmdText, parmsValue);
        }
        /// <summary>
        /// 执行SQL返回第一行第一列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, object[] parmsValue)
        {
            return dal.ExecuteNonQuery(cmdText, parmsValue);
        }
        /// <summary>
        /// 执行SQL事务
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parmsValue"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string[] cmdText, object[][] parmsValue)
        {
            return dal.ExecuteNonQuery(cmdText, parmsValue);
        }
        #endregion
    }
}
