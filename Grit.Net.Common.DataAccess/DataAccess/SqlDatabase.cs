using System;
using System.Data;
using System.Data.SqlClient;
using Domain.Common.Config;
using Domain.Common.Log;

namespace Domain.Common.DataAccess
{
    /// <summary>
    /// 数据库操作的帮助类
    /// </summary>
    public class SqlDatabase : DataProvider,IDisposable
    {
        /// <summary>
        /// 获取SqlCommand对象实例，返回SqlCommand
        /// </summary>
        /// <returns>SqlCommand类型数据</returns>
        public static SqlCommand GetCommand()
        {
            SqlConnection conn = new SqlConnection(BaseConfig.Instance.DbConnString);
            conn.Open();
            return conn.CreateCommand();
        }
        /// <summary>
        /// 执行SqlCommand类的ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式，默认CommandType.Text</param>
        /// <returns>返回受影响的行数，失败返回0</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType commandType = CommandType.Text, params SqlParameter[] dbps)
        {
            SqlCommand cmd = GetCommand();
            try
            {
                cmd.CommandText = cmdText;
                if (dbps != null)
                {
                    cmd.Parameters.AddRange(dbps);
                }
                cmd.CommandType = commandType;
                int count = cmd.ExecuteNonQuery();
                return count;
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
                return 0;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        /// <summary>
        /// 执行SqlCommand类的ExecuteScalar方法,返回object
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式，默认CommandType.Text</param>
        /// <returns>返回object类型数据，失败返回null</returns>
        public static object ExecuteScalar(string cmdText, CommandType commandType = CommandType.Text, params SqlParameter[] dbps)
        {
            SqlCommand cmd = GetCommand();
            try
            {
                cmd.CommandText = cmdText;

                if (dbps != null)
                {
                    cmd.Parameters.AddRange(dbps);
                }
                cmd.CommandType = commandType;
                object count = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return count;
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
                return null;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        /// <summary>
        /// 执行SqlCommand类的ExecuteReader方法,返回SqlDataReader
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式，默认CommandType.Text</param>
        /// <returns>返回SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType commandType = CommandType.Text, params SqlParameter[] dbps)
        {
            SqlCommand cmd = GetCommand();
            try
            {
                cmd.CommandText = cmdText;
                if (dbps != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(dbps);
                }
                cmd.CommandType = commandType;
                //CommandBehavior.CloseConnection在执行该命令时，如果关闭关联的 DataReader 对象，则关联的 Connection 对象也将关闭。
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式，默认CommandType.Text</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string cmdText, CommandType commandType = CommandType.Text, params SqlParameter[] dbps)
        {
            SqlCommand cmd = GetCommand();
            try
            {
                cmd.CommandText = cmdText;
                if (dbps != null)
                {
                    cmd.Parameters.AddRange(dbps);
                }
                cmd.CommandType = commandType;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                return ds;
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式，默认CommandType.Text</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string cmdText, CommandType commandType = CommandType.Text, params SqlParameter[] dbps)
        {
            SqlCommand cmd = GetCommand();
            try
            {
                cmd.CommandText = cmdText;
                if (dbps != null)
                {
                    cmd.Parameters.AddRange(dbps);
                }
                cmd.CommandType = commandType;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
                return null;
            }
        }
	    /// <summary>
        /// 批处理 ExecuteNonQuery方法，返回受影响的行数
	    /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
	    /// <returns></returns>
        public static int ExecuteNonQuery(string[] cmdText,SqlParameter[][] dbps)
        {
            SqlConnection conn = new SqlConnection(BaseConfig.Instance.DbConnString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                int count = 0;
                for (int n = 0; n < cmdText.Length; n++)
                {
                    cmd.CommandText = cmdText[n];
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(dbps[n]);
                    count += cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return count;
            }
            catch(Exception ex)
            {
                trans.Rollback();
                ErrorLog.WriteErrorLog(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string parameterName, object value)
        {
            if (value == null)
                value = DBNull.Value;
            return new SqlParameter(parameterName, value);
        }
        #region Dispose
        private bool isDisposed = false;

        /// <summary>
        /// 实现IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如C++）的规范
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary>
        ~SqlDatabase()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                //if (DbFactory != null)
                //{
                //    DbFactory = null;
                //}
            }
            // 清理非托管资源……

            //让类型知道自己已经被释放
            isDisposed = true;
        }
        #endregion
    }
}
