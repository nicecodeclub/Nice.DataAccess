
using Nice.DataAccess.Exceptions;
using Nice.DataAccess.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Nice.DataAccess
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class DataHelper
    {
        private DataProvider dataProvider = null;
        private int commandTimeout = 30;

        /// <summary>
        /// 初始化数据库连接参数，在程序启动时初始化
        /// </summary>
        /// <param name="providerName">数据库驱动名</param>
        /// <param name="dbConnString">连接字符串</param>
        public DataHelper(DataFactoryConfig dbSettings)
        {
            object[] args = new object[] { dbSettings.ConnString };
            this.dataProvider = (DataProvider)Assembly.Load(dbSettings.DataProviderAssembly).CreateInstance(dbSettings.DataProviderTypeName, true, BindingFlags.Default, null, args, null, null);
            this.commandTimeout = dbSettings.CommandTimeout;
        }

        /// 判断当前数据库是否成功连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnection()
        {
            IDbConnection conn = null;
            try
            {
                conn = dataProvider.GetConnection();
                conn.Open();
                return true;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        internal IDbConnection GetConnection()
        {
            return dataProvider.GetConnection();
        }
        /// <summary>
        /// 执行ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns>返回受影响的行数，失败返回0</returns>
        public int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数</param>
        /// <returns>返回受影响的行数，失败返回0</returns>
        public int ExecuteNonQuery(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns>返回受影响的行数，失败返回0</returns>
        public int ExecuteNonQuery(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbConnection connection = null;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                if (Transaction.Current != null)
                {
                    cmd.Connection = Transaction.Current.DbTransaction.Connection;
                    cmd.Transaction = Transaction.Current.DbTransaction;
                }
                else
                {
                    connection = dataProvider.GetConnection();
                    cmd.Connection = connection;
                    connection.Open();
                }
                cmd.CommandText = cmdText;
                if (dbps != null && dbps.Count > 0)
                {
                    dataProvider.AttachParameters(cmd, dbps);
                }
                cmd.CommandType = commandType;
                cmd.CommandTimeout = commandTimeout;
                return cmd.ExecuteNonQuery();
            }
            catch (DbException ex)
            {
                throw new DbExcuteException(cmdText, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回object
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns>返回object类型数据，失败返回null</returns>
        public object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回object
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数</param>
        /// <returns>返回object类型数据，失败返回null</returns>
        public object ExecuteScalar(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteScalar(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回object
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns>返回object类型数据，失败返回null</returns>
        public object ExecuteScalar(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbConnection connection = null;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                connection = dataProvider.GetConnection();
                cmd.Connection = connection;
                cmd.CommandText = cmdText;
                if (dbps != null && dbps.Count > 0)
                {
                    dataProvider.AttachParameters(cmd, dbps);
                }
                cmd.CommandType = commandType;
                cmd.CommandTimeout = commandTimeout;
                cmd.Connection.Open();//最晚打开
                return cmd.ExecuteScalar();
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public int ExecuteNonQuery(IList<string> cmdText, IList<IList<IDataParameter>> dbps)
        {
            IDbConnection connection = null;
            IDbTransaction trans = null;
            int n = 0;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                if (Transaction.Current != null)
                {
                    cmd.Connection = Transaction.Current.DbTransaction.Connection;
                    cmd.Transaction = Transaction.Current.DbTransaction;
                }
                else
                {
                    connection = dataProvider.GetConnection();
                    cmd.Connection = connection;
                    connection.Open();
                    trans = connection.BeginTransaction();
                    cmd.Transaction = trans;
                }
                cmd.CommandTimeout = commandTimeout;
                int count = 0;
                for (; n < cmdText.Count; n++)
                {
                    cmd.CommandText = cmdText[n];
                    if (dbps != null && dbps[n] != null)
                        dataProvider.AttachParameters(cmd, dbps[n]);
                    count += cmd.ExecuteNonQuery();

                }
                if (trans != null)
                    trans.Commit();
                return count;
            }
            catch (DbException ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回T
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns>返回指定类型数据，失败返回null</returns>
        public T ExecuteScalar<T>(string cmdText)
        {
            return ExecuteScalar<T>(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回T
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <returns>返回指定类型数据，失败返回null</returns>
        public T ExecuteScalar<T>(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteScalar<T>(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行ExecuteScalar方法,返回T
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns>返回指定类型数据，失败返回null</returns>
        public T ExecuteScalar<T>(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            object value = ExecuteScalar(cmdText, commandType, dbps);
            if (value == null || value == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// 执行ExecuteReader方法,返回SqlDataReader
        /// [调用完成后，请关闭IDataReader对象]
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns>返回SqlDataReader</returns>
        public IDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行ExecuteReader方法,返回SqlDataReader
        /// [调用完成后，请关闭IDataReader对象]
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <returns>返回SqlDataReader</returns>
        public IDataReader ExecuteReader(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteReader(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行ExecuteReader方法,返回SqlDataReader
        /// [调用完成后，请关闭IDataReader对象]
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns>返回SqlDataReader</returns>
        public IDataReader ExecuteReader(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = dataProvider.GetCommand();
                cmd.Connection = dataProvider.GetConnection();
                cmd.CommandText = cmdText;
                if (dbps != null && dbps.Count > 0)
                {
                    dataProvider.AttachParameters(cmd, dbps);
                }
                cmd.CommandTimeout = commandTimeout;
                cmd.CommandType = commandType;
                cmd.Connection.Open();
                //CommandBehavior.CloseConnection在执行该命令时，如果关闭关联的 DataReader 对象，则关联的 Connection 对象也将关闭。
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (DbException ex)
            {
                throw new DbExcuteException(cmdText, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //    cmd.Connection.Dispose();
            //}
        }

        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbConnection connection = null;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                connection = dataProvider.GetConnection();
                cmd.CommandText = cmdText;
                cmd.Connection = connection;
                if (dbps != null && dbps.Count > 0)
                {
                    dataProvider.AttachParameters(cmd, dbps);
                }
                cmd.CommandTimeout = commandTimeout;
                cmd.CommandType = commandType;
                cmd.Connection.Open();
                IDbDataAdapter da = dataProvider.GetDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da = null;
                return ds;
            }
            catch (DbException ex)
            {
                throw new DbExcuteException(cmdText, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行DataSet.Fill,DataTable
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText)
        {
            return ExecuteDataTable(cmdText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行DataSet.Fill,DataTable
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText, IList<IDataParameter> dbps)
        {
            return ExecuteDataTable(cmdText, CommandType.Text, dbps);
        }
        /// <summary>
        /// 执行DataSet.Fill,返回DataSet
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <param name="commandType">执行方式</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbConnection connection = null;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                connection = dataProvider.GetConnection();
                cmd.Connection = connection;
                cmd.CommandText = cmdText;
                if (dbps != null && dbps.Count > 0)
                {
                    dataProvider.AttachParameters(cmd, dbps);
                }
                cmd.CommandType = commandType;
                cmd.CommandTimeout = commandTimeout;
                cmd.Connection.Open();
                IDataAdapter da = dataProvider.GetDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //da.Dispose();
                da = null;
                return ds.Tables[0];
            }
            catch (DbException ex)
            {
                throw new DbExcuteException(cmdText, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 批处理 ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">需要执行的sql语句</param>
        /// <param name="dbps">sql语句需要的参数，没有参数则传入null</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string[] cmdText, IList<IDataParameter>[] dbps)
        {
            IDbConnection connection = null;
            IDbTransaction trans = null;
            int n = 0;
            try
            {
                IDbCommand cmd = dataProvider.GetCommand();
                if (Transaction.Current != null)
                {
                    cmd.Connection = Transaction.Current.DbTransaction.Connection;
                    cmd.Transaction = Transaction.Current.DbTransaction;
                }
                else
                {
                    connection = dataProvider.GetConnection();
                    cmd.Connection = connection;
                    connection.Open();
                    trans = connection.BeginTransaction();
                    cmd.Transaction = trans;
                }
                cmd.CommandTimeout = commandTimeout;
                int count = 0;
                for (; n < cmdText.Length; n++)
                {
                    cmd.CommandText = cmdText[n];
                    if (dbps != null && dbps[n] != null)
                        dataProvider.AttachParameters(cmd, dbps[n]);
                    count += cmd.ExecuteNonQuery();

                }
                if (trans != null && Transaction.Current == null)
                    trans.Commit();
                return count;
            }
            catch (DbException ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw new DbExcuteException(cmdText[n], ex);
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行ExecuteNonQuery方法，返回受影响的行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="trans"></param>
        /// <param name="dbps"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, IDbTransaction trans, CommandType commandType, IList<IDataParameter> dbps)
        {
            IDbConnection connection = trans.Connection;
            IDbCommand cmd = connection.CreateCommand();
            if (dbps != null)
            {
                dataProvider.AttachParameters(cmd, dbps);
            }
            cmd.CommandType = commandType;
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = cmdText;
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string parameterName, object value)
        {
            return dataProvider.CreateParameter(parameterName, value);
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction CreateTransaction()
        {
            IDbConnection connection = dataProvider.GetConnection();
            connection.Open();
            return connection.BeginTransaction();
        }
        public char GetParameterPrefix()
        {
            return dataProvider.GetParameterPrefix();
        }
    }
}
