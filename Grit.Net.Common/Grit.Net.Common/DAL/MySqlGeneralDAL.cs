using Grit.Net.Common.Attributes;
using Grit.Net.Common.DataAccess;
using Grit.Net.Common.Exceptions;
using Grit.Net.Common.Factory;
using Grit.Net.Common.Models;
using Grit.Net.Common.Models.Page;
using Grit.Net.Common.TypeEx;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Grit.Net.Common.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class MySqlGeneralDAL<T> : IGeneralDAL<T> where T : TEntity, new()
    {
        /// <summary>
        /// 类名
        /// </summary>
        private string ClassName;
        /// <summary>
        /// 表名
        /// </summary>
        private string TableName;
        /// <summary>
        /// 主键列名
        /// </summary>
        private string IdColomnName;
        /// <summary>
        /// 主键属性
        /// </summary>
        private PropertyInfo IdProperty;

        /// <summary>
        /// ID生成类型
        /// </summary>
        private IdGenerateType thisIdGenerateType;

        /// <summary>
        /// 类型属性集合
        /// </summary>
        private PropertyInfo[] tableProperties;

        /// <summary>
        /// 属性名与数据库字段名对应
        /// </summary>
        private Mapping<string, string> propertyAndColumn;

        private MySqlParameter ParamFilterValid = null;
        private MySqlParameter ParamFilterInValid = null;
        private string ValidColumnName = null;
        private const string flagWhere = " WHERE ";
        private const string flagFrom = " FROM ";
        private const string flagAnd = " AND ";
        private string GetColumnText;//获取列数据列名
        private string SetColumnText;//设置列数据列名
        private string InsertColumnText;//插入列数据列名

        public MySqlGeneralDAL()
        {
            GetPropertyInfo();
        }

        private void GetPropertyInfo()
        {
            Type type = typeof(T);
            ClassName = type.Name;
            PropertyInfo[] properties = type.GetProperties();

            //表名
            TableAttribute tableAttri = type.GetCustomAttribute<TableAttribute>();
            if (tableAttri == null)
                TableName = ClassName;
            else
                TableName = tableAttri.Name;

            //属性与字段名对应
            propertyAndColumn = new Mapping<string, string>(properties.Length);

            string propertyName = null;
            string columnName = null;
            StringBuilder sbGetColumns = new StringBuilder(50);
            StringBuilder sbSetColumns = new StringBuilder(50);
            StringBuilder sbInsertColumn = new StringBuilder(50);

            IList<PropertyInfo> propertyArray = new List<PropertyInfo>();

            foreach (PropertyInfo pi in properties)
            {
                propertyArray.Add(pi);
                //过滤暂存、附加字段
                if (pi.GetCustomAttribute<TransientAttribute>() != null) continue;

                ColumnAttribute attri = pi.GetCustomAttribute<ColumnAttribute>();
                propertyName = pi.Name;
                if (attri == null)
                    columnName = propertyName;
                else
                    columnName = attri.Name;
                propertyAndColumn.Add(propertyName.ToUpper(), columnName);
                sbGetColumns.AppendFormat("{0}.{1} {2},", ClassName, columnName, propertyName);

                IdAttribute idAttri = pi.GetCustomAttribute<IdAttribute>();

                if (idAttri == null)
                {
                    sbInsertColumn.AppendFormat(" {0},", columnName);
                    if (attri == null || !attri.IsReadOnly)
                        sbSetColumns.AppendFormat(" {0}=@{1},", columnName, propertyName);

                    if (ValidColumnName == null)
                    {
                        ValidAttribute validAttri = pi.GetCustomAttribute<ValidAttribute>();
                        if (validAttri != null)
                        {
                            ParamFilterValid = new MySqlParameter(string.Format("@{0}", pi.Name), validAttri.Available);
                            ParamFilterInValid = new MySqlParameter(string.Format("@{0}", pi.Name), validAttri.Invalid);
                            ValidColumnName = columnName;
                        }
                    }
                }
                else //主键
                {
                    if (attri == null)
                        IdColomnName = pi.Name;
                    else
                        IdColomnName = attri.Name;
                    IdProperty = pi;
                    thisIdGenerateType = idAttri.GenerateType;
                    if (thisIdGenerateType == IdGenerateType.Assign)
                    {
                        sbInsertColumn.AppendFormat(" {0},", columnName);
                        sbSetColumns.AppendFormat(" {0}=@{1},", columnName, propertyName);
                    }

                }

            }
            propertyAndColumn.TrimEmpty();
            tableProperties = propertyArray.ToArray();
            GetColumnText = sbGetColumns.ToString().Trim(',');
            SetColumnText = sbSetColumns.ToString().Trim(',');
            InsertColumnText = sbInsertColumn.ToString().Trim(',');
            if (IdColomnName == null)
                throw new IdNotImplementedException();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public T Get(object IdValue)
        {
            IList<MySqlParameter> parms = new List<MySqlParameter>();
            StringBuilder cmdText = new StringBuilder(50);
            cmdText.AppendFormat(" SELECT {0} FROM {1} {2} WHERE {2}.{3}=@{3}", GetColumnText, TableName, ClassName, IdColomnName);
            parms.Add(new MySqlParameter("@" + IdColomnName, IdValue));
            if (ParamFilterValid != null)
            {
                cmdText.AppendFormat(" AND {0}=@{0}", ValidColumnName);
                parms.Add(ParamFilterValid);
            }

            DataTable dt = DataHelper.ExecuteDataTable(cmdText.ToString(), CommandType.Text, parms.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                T t = new T();
                object value = null;
                DataRow dr = dt.Rows[0];
                PropertyInfo[] querypropertys = null;
                FilterProperty(dt, tableProperties, ref querypropertys);
                SetValueDelegate setter = null;
                foreach (PropertyInfo pi in querypropertys)
                {
                    value = dr[pi.Name];
                    if (value != DBNull.Value)
                    {
                        //pi.SetValue(t, value, null);
                        setter = FastMethodFactory.CreatePropertySetter(pi);
                        setter(t, value);
                    }
                }
                return t;
            }
            return default(T);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public IList<T> GetList()
        {
            IList<T> result = null;
            StringBuilder cmdText = new StringBuilder(20);
            cmdText.AppendFormat("SELECT {0} FROM {1} {2}", GetColumnText, TableName, ClassName);
            IDbDataParameter[] parms = null;
            if (ParamFilterValid != null)
            {
                cmdText.AppendFormat(" WHERE {0}=@{0}", ValidColumnName);
                parms = new MySqlParameter[] { ParamFilterValid };
            }

            DataTable dt = DataHelper.ExecuteDataTable(cmdText.ToString(), CommandType.Text, parms);
            if (dt != null && dt.Rows.Count > 0)
            {
                T t = default(T);
                object value = null;
                result = new List<T>();
                PropertyInfo[] querypropertys = null;
                FilterProperty(dt, tableProperties, ref querypropertys);
                SetValueDelegate setter = null;
                foreach (DataRow dr in dt.Rows)
                {
                    t = new T();
                    foreach (PropertyInfo pi in querypropertys)
                    {
                        value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            //pi.SetValue(t, value, null);
                            setter = FastMethodFactory.CreatePropertySetter(pi);
                            setter(t, value);
                        }
                    }
                    result.Add(t);
                }
            }
            return result;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public IList<T> GetList(PageInfo page)
        {
            IList<T> result = null;

            StringBuilder cmdText = new StringBuilder(100);
            cmdText.AppendFormat("SELECT {0} FROM {1} {2}", GetColumnText, TableName, ClassName);
            IDbDataParameter[] parms = null;
            if (ParamFilterValid != null)
            {
                cmdText.AppendFormat(" WHERE {0}=@{0}", ValidColumnName);
                parms = new MySqlParameter[] { ParamFilterValid };
            }
            if (page != null)
            {
                cmdText.AppendFormat(" ORDER BY {0}.{1} {2} LIMIT {3},{4}; "
                    , ClassName, string.IsNullOrEmpty(page.OrderColName) ? IdColomnName : page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
                cmdText.AppendFormat(" SELECT COUNT(1) FROM {0}", TableName);
                if (ParamFilterValid != null)
                {
                    cmdText.AppendFormat(" WHERE {0}=@{0}", ValidColumnName);
                    parms = new MySqlParameter[] { ParamFilterValid };
                }
                cmdText.AppendFormat(";");
            }

            DataSet ds = DataHelper.ExecuteDataSet(cmdText.ToString(), CommandType.Text, parms);
            if (ds != null && ds.Tables.Count == 2)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    T t = default(T);
                    object value = null;
                    result = new List<T>();
                    PropertyInfo[] querypropertys = null;
                    FilterProperty(dt, tableProperties, ref querypropertys);
                    SetValueDelegate setter = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        t = new T();
                        foreach (PropertyInfo pi in querypropertys)
                        {
                            value = dr[pi.Name];
                            if (value != DBNull.Value)
                            {
                                //pi.SetValue(t, value, null);
                                setter = FastMethodFactory.CreatePropertySetter(pi);
                                setter(t, value);
                            }
                        }
                        result.Add(t);
                    }
                }
                page.TotalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            }

            return result;
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Insert(T t)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder sbColumn = new StringBuilder(50);
            PropertyInfo pi = null;
            GetValueDelegate getter = null;
            for (int i = 0; i < tableProperties.Length; i++)
            {
                pi = tableProperties[i];
                if (pi.Name == IdProperty.Name && thisIdGenerateType != IdGenerateType.Assign)
                    continue;
                sbColumn.AppendFormat("@{0},", pi.Name);
                //parms.Add(new MySqlParameter("@" + pi.Name, pi.GetValue(t)));
                getter = FastMethodFactory.CreatePropertyGetter(pi);
                parms.Add(new MySqlParameter("@" + pi.Name, getter(t, null)));
            }

            string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2})", TableName, InsertColumnText, sbColumn.ToString().Trim(','));
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms.ToArray()) > 0;
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Insert(IList<T> list)
        {
            string[] cmdText = new string[list.Count];
            IDataParameter[][] dbps = new IDataParameter[list.Count()][];
            IList<IDataParameter> parms = null;

            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i];
                StringBuilder sbColumn = new StringBuilder(50);
                PropertyInfo pi = null;
                parms = new List<IDataParameter>();
                for (int j = 0; j < tableProperties.Length; j++)
                {
                    pi = tableProperties[j];
                    if (pi.Name == IdProperty.Name && thisIdGenerateType != IdGenerateType.Assign)
                        continue;
                    sbColumn.AppendFormat("@{0},", pi.Name);
                    parms.Add(new MySqlParameter("@" + pi.Name, pi.GetValue(t)));
                }
                dbps[i] = parms.ToArray();
                cmdText[i] = string.Format("INSERT INTO {0}({1}) VALUES({2})", TableName, InsertColumnText, sbColumn.ToString().Trim(','));
            }
            return DataHelper.ExecuteNonQuery(cmdText, dbps) > 0;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(T t)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            object IdValue = IdProperty.GetValue(t, null);
            parms.Add(new MySqlParameter("@" + IdColomnName, IdValue));
            PropertyInfo pi = null;
            GetValueDelegate getter = null;
            for (int i = 0; i < tableProperties.Length; i++)
            {
                pi = tableProperties[i];
                if (pi.Name != IdColomnName)
                {
                    ColumnAttribute attri = pi.GetCustomAttribute<ColumnAttribute>();
                    if (attri == null || !attri.IsReadOnly)
                    {
                        //parms.Add(new MySqlParameter("@" + pi.Name, pi.GetValue(t)));
                        getter = FastMethodFactory.CreatePropertyGetter(pi);
                        parms.Add(new MySqlParameter("@" + pi.Name, getter(t, null)));
                    }
                }
            }
            string cmdText = string.Format("UPDATE {0} SET {1} WHERE {2}=@{3}", TableName, SetColumnText, IdColomnName, IdProperty.Name);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms.ToArray()) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Delete(T t)
        {
            object value = IdProperty.GetValue(t, null);
            IDataParameter[] parms = new MySqlParameter[] { new MySqlParameter("@" + IdColomnName, value) };
            string cmdText = string.Format("delete from {0} where {1}=@{1}", TableName, IdColomnName);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idValue">主键</param>
        /// <returns></returns>
        public bool Delete(object idValue)
        {
            IDataParameter[] parms = new MySqlParameter[] { new MySqlParameter("@" + IdColomnName, idValue) };
            string cmdText = string.Format("delete from {0} where {1}=@{1}", TableName, IdColomnName);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms) > 0;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="PropertyName">属性名</param>
        /// <param name="PropertyValue">属性值</param>
        /// <returns></returns>
        public bool IsExist(string PropertyName, object PropertyValue, object IdValue)
        {
            string colunmName = propertyAndColumn[PropertyName.ToUpper()];
            if (string.IsNullOrEmpty(colunmName)) throw new NotImplementedException();

            IList<MySqlParameter> parms = new List<MySqlParameter>();
            parms.Add(new MySqlParameter("@" + colunmName, PropertyValue));

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select {0} from {1} where {2}=@{2}", IdColomnName, TableName, colunmName);
            if (ParamFilterValid != null)
            {
                cmdText.AppendFormat(" AND {0}=@{0}", ValidColumnName);
                parms.Add(ParamFilterValid);
            }

            object obj = DataHelper.ExecuteScalar(cmdText.ToString(), CommandType.Text, parms.ToArray());
            if (obj != null && !obj.Equals(IdValue))
                return true;
            return false;
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="PropertyValue">属性值</param>
        /// <returns></returns>
        public bool VirtualDelete(object IdValue)
        {
            IDataParameter[] parms = new MySqlParameter[] { new MySqlParameter("@" + IdColomnName, IdValue), ParamFilterInValid };
            string cmdText = string.Format("UPDATE {0} SET {2}=@{2} WHERE {1}=@{1}", TableName, IdColomnName, ValidColumnName);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms) > 0;
        }

        public bool InsertAndGet(T t)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder sbColumn = new StringBuilder(50);
            PropertyInfo pi = null;
            GetValueDelegate getter = null;
            for (int i = 0; i < tableProperties.Length; i++)
            {
                pi = tableProperties[i];
                if (pi.Name == IdProperty.Name && thisIdGenerateType != IdGenerateType.Assign)
                    continue;
                sbColumn.AppendFormat("@{0},", pi.Name);
                //parms.Add(new MySqlParameter("@" + pi.Name, pi.GetValue(t)));
                getter = FastMethodFactory.CreatePropertyGetter(pi);
                parms.Add(new MySqlParameter("@" + pi.Name, getter(t, null)));
            }

            string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2});SELECT LAST_INSERT_ID();", TableName, InsertColumnText, sbColumn.ToString().Trim(','));
            object obj = DataHelper.ExecuteScalar(cmdText, CommandType.Text, parms.ToArray());
            if (obj != null && obj != DBNull.Value)
            {
                SetValueDelegate setter = FastMethodFactory.CreatePropertySetter(IdProperty);
                setter(t, System.Convert.ChangeType(obj, IdProperty.PropertyType));
            
                return true;
            }
            return false;


        }
        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="propertys"></param>
        private void FilterProperty(DataTable dt, PropertyInfo[] propertys, ref PropertyInfo[] querypropertys)
        {
            IList<PropertyInfo> propertyArray = new List<PropertyInfo>(20);

            foreach (PropertyInfo pi in propertys)
            {
                if (dt.Columns.Contains(pi.Name))
                {
                    propertyArray.Add(pi);
                }
            }

            querypropertys = propertyArray.ToArray();
        }

        /// <summary>
        /// 将属性名转换为表字段名
        /// </summary>
        private void PropertyToColumnName(string strWhere, StringBuilder sb)
        {

            int indexFlag = strWhere.IndexOf('=');
            string nameAndColumn = strWhere.Substring(0, indexFlag).Trim();
            sb.Append(' ');
            sb.Append(propertyAndColumn[nameAndColumn]);

            string nextNameAndColumn = strWhere.Substring(indexFlag).Trim();
            indexFlag = nextNameAndColumn.IndexOf(flagAnd);

            sb.Append('=');
            if (indexFlag == -1)
            {
                sb.Append(nextNameAndColumn.Trim().TrimStart('='));
                return;
            }
            else
            {
                sb.Append(nextNameAndColumn.Substring(0, indexFlag).Trim().TrimStart('='));
                sb.Append(flagAnd);
                PropertyToColumnName(nextNameAndColumn, sb);
            }
        }
        /// <summary>
        /// 将属性名转换为表字段名
        /// </summary>
        private void PropertyToColumnName(string strWhere, StringBuilder sb, object[] parmsValue, IDataParameter[] parms, ref int parmIndex)
        {

            int indexFlag = strWhere.IndexOf('=');
            string columnName = strWhere.Substring(0, indexFlag).Trim();
            sb.Append(' ');
            sb.Append(propertyAndColumn[columnName]);

            string nextColumnName = strWhere.Substring(indexFlag).Trim();
            indexFlag = nextColumnName.IndexOf(flagAnd);
            string columnValue = null;
            sb.Append('=');
            if (indexFlag == -1)
            {
                columnValue = nextColumnName.Trim().TrimStart('=');
                if (columnValue.Equals("?"))
                {
                    sb.AppendFormat("@{0}", parmIndex);
                    parms[parmIndex] = new MySqlParameter("@" + parmIndex, parmsValue[parmIndex]);
                    parmIndex++;
                }
                else
                {
                    sb.Append(columnValue);
                }
                return;
            }
            else
            {
                columnValue = nextColumnName.Substring(0, indexFlag).Trim().TrimStart('=');
                nextColumnName = nextColumnName.Substring(indexFlag + flagAnd.Length);
                if (columnValue.Equals("?"))
                {
                    sb.AppendFormat("@{0}", parmIndex);
                    parms[parmIndex] = new MySqlParameter("@" + parmIndex, parmsValue[parmIndex]);
                    parmIndex++;
                }
                else
                {
                    sb.Append(columnValue);
                }
                sb.Append(flagAnd);
                PropertyToColumnName(nextColumnName, sb, parmsValue, parms, ref parmIndex);
            }
        }

    }
}
