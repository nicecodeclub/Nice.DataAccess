using MySql.Data.MySqlClient;
using Nice.DataAccess;
using Nice.DataAccess.DAL;

using Nice.DataAccess.Model.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nice.DataAccess.MySql.DAL
{
    public class MySqlQueryDAL<T> : QueryBaseDAL<T>, IQueryDAL<T> where T : new()
    {
        public MySqlQueryDAL(string connStrKey) : base(connStrKey) { }
        protected override IDbDataParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(string.Format("@{0}", parameterName), value);
        }
    }

    //public class MySqlQueryDAL : IQueryDAL
    //{
    //    protected readonly DataHelper DataHelper = null;
    //    public MySqlQueryDAL() : this(DataUtil.DefaultConnStringKey)
    //    {
    //    }
    //    public MySqlQueryDAL(string connStrKey)
    //    {
    //        DataHelper = DataUtil.GetDataHelper(connStrKey);
    //    }

    //    #region  执行SQL
    //    /// <summary>
    //    /// 执行SQL
    //    /// </summary>
    //    /// <param name="cmdText"></param>
    //    /// <param name="parmsValue"></param>
    //    /// <returns></returns>
    //    public int ExecuteNonQuery(string cmdText, object[] parmsValue)
    //    {
    //        IDataParameter[] parms = null;
    //        if (parmsValue != null)
    //            FilterSQLParmeters(ref cmdText, parmsValue, ref parms);

    //        return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms);
    //    }
    //    /// <summary>
    //    /// 执行SQL返回第一行第一列
    //    /// </summary>
    //    /// <param name="cmdText"></param>
    //    /// <param name="parmsValue"></param>
    //    /// <returns></returns>
    //    public object ExecuteScalar(string cmdText, object[] parmsValue)
    //    {
    //        IDataParameter[] parms = null;
    //        if (parmsValue != null)
    //            FilterSQLParmeters(ref cmdText, parmsValue, ref parms);

    //        return DataHelper.ExecuteScalar(cmdText, CommandType.Text, parms);
    //    }
    //    /// <summary>
    //    /// 执行SQL事务
    //    /// </summary>
    //    /// <param name="cmdText"></param>
    //    /// <param name="parmsValue"></param>
    //    /// <returns></returns>
    //    public int ExecuteNonQuery(string[] cmdText, object[][] parmsValue)
    //    {
    //        IDataParameter[] parms = null;
    //        IDataParameter[][] dbps = null;
    //        string text = null;
    //        if (parmsValue != null)
    //        {
    //            dbps = new MySqlParameter[cmdText.Length][];
    //            for (int i = 0; i < cmdText.Length; i++)
    //            {
    //                text = cmdText[i];
    //                if (parmsValue[i] != null)
    //                    FilterSQLParmeters(ref text, parmsValue[i], ref parms);
    //                cmdText[i] = text;
    //                dbps[i] = parms;
    //            }
    //        }
    //        return DataHelper.ExecuteNonQuery(cmdText, dbps);
    //    }

    //    /// <summary>
    //    /// 返回Dataset
    //    /// </summary>
    //    /// <param name="cmdText"></param>
    //    /// <param name="parmsValue"></param>
    //    /// <returns></returns>
    //    //public DataSet ExecuteDataSet(string cmdText, object[] parmsValue)
    //    //{
    //    //    IDataParameter[] parms = null;
    //    //    if (parmsValue != null)
    //    //        FilterSQLParmeters(ref cmdText, parmsValue, ref parms);
    //    //    return DataHelper.ExecuteDataSet(cmdText, CommandType.Text, parms);
    //    //}

    //    /// <summary>
    //    /// 过滤SQL参数
    //    /// </summary>
    //    /// <param name="cmdText"></param>
    //    private void FilterSQLParmeters(ref string cmdText, object[] parmsValue, ref IDataParameter[] parms)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        parms = new MySqlParameter[parmsValue.Length];
    //        char c;
    //        int parmIndex = 0;
    //        for (int i = 0; i < cmdText.Length; i++)
    //        {
    //            c = cmdText[i];
    //            if (c.Equals('?'))
    //            {
    //                sb.AppendFormat("@{0}", parmIndex);
    //                parms[parmIndex] = new MySqlParameter("@" + parmIndex, parmsValue[parmIndex]);
    //                parmIndex++;
    //            }
    //            else
    //                sb.Append(c);
    //        }
    //        cmdText = sb.ToString();
    //    }
    //    #endregion
    //}
}