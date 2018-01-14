using MySql.Data.MySqlClient;
using Nice.DataAccess.Attributes;
using Nice.DataAccess.DAL;
using Nice.DataAccess.Emit;
using Nice.DataAccess.Model.Page;
using Nice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nice.DataAccess.MySql.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class MySqlGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public MySqlGeneralDAL(string connStrKey) : base(connStrKey) { }
      
        protected override IDbDataParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(string.Format("@{0}", parameterName), value);
        }

        protected override string GetLastIncrementID()
        {
            return "SELECT LAST_INSERT_ID()";
        }
    }
}
