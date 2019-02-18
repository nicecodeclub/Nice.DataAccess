using Nice.DataAccess.Attributes;
using Nice.DataAccess.Exceptions;
using Nice.DataAccess.Model.Page;
using Nice.DataAccess.Models;
using Nice.DataAccess.TypeEx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace Nice.DataAccess.DAL
{
    public abstract class BaseDAL<T> where T : TEntity, new()
    {
        #region 字段 Field
        /// <summary>
        /// 类名
        /// </summary>
        protected string ClassName;
        /// <summary>
        /// 表名
        /// </summary>
        protected string TableName;
        /// <summary>
        /// ID信息
        /// </summary>
        protected IdColomn IdColomn;
        /// <summary>
        /// 
        /// </summary>
        protected FilterValid filterValid;
        /// <summary>
        /// 类型属性集合
        /// </summary>
        protected PropertyInfo[] tableProperties;
        /// <summary>
        /// 属性名与数据库字段名对应
        /// </summary>
        protected Mapping<string, string> propertyAndColumn;
        protected const string flagWhere = " WHERE ";
        protected const string flagFrom = " FROM ";
        protected const string flagAnd = " AND ";
        protected string GetColumnText;//获取列数据列名
        protected string SetColumnText;//设置列数据列名
        protected string InsertColumnText;//插入列数据列名
        protected readonly DataHelper DataHelper = null;

        #endregion

        #region 构造函数 constructor
        protected BaseDAL(string connStrKey)
        {
            this.DataHelper = DataUtil.GetDataHelper(connStrKey);
            GetPropertyInfo();
        }
        #endregion

        #region 过滤属性和字段名信息
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

            StringBuilder sbGetColumns = new StringBuilder(50);
            StringBuilder sbSetColumns = new StringBuilder(50);
            StringBuilder sbInsertColumn = new StringBuilder(50);
            IList<PropertyInfo> propertyArray = new List<PropertyInfo>();
            GetPropertyInfo(properties, propertyArray, sbGetColumns, sbSetColumns, sbInsertColumn);

            tableProperties = propertyArray.ToArray();
            GetColumnText = sbGetColumns.ToString().Trim(',');
            SetColumnText = sbSetColumns.ToString().Trim(',');
            InsertColumnText = sbInsertColumn.ToString().Trim(',');
            if (IdColomn == null)
                throw new IdNotImplementedException();
        }

        /// <summary>
        /// 获取类型属性和字段名信息
        /// </summary>

        private void GetPropertyInfo(PropertyInfo[] properties, IList<PropertyInfo> propertyArray, StringBuilder sbGetColumns, StringBuilder sbSetColumns, StringBuilder sbInsertColumn)
        {
            string propertyName = null;
            string columnName = null;
            foreach (PropertyInfo pi in properties)
            {
                propertyArray.Add(pi);
                //过滤暂存、附加字段
                if (pi.GetCustomAttribute<TransientAttribute>() != null) continue;

                ColumnAttribute attri = pi.GetCustomAttribute<ColumnAttribute>();
                propertyName = pi.Name;
                if (attri == null || attri.Name == null)
                    columnName = propertyName;
                else
                    columnName = attri.Name;
                propertyAndColumn.Add(propertyName.ToUpper(), columnName);
                sbGetColumns.AppendFormat("{0} {1},", columnName, propertyName);

                IdAttribute idAttri = pi.GetCustomAttribute<IdAttribute>();

                if (idAttri == null)
                {
                    sbInsertColumn.AppendFormat(" {0},", columnName);
                    if (attri == null || !attri.IsReadOnly)
                        sbSetColumns.AppendFormat(" {0}={1}{2},", columnName, DataHelper.GetParameterPrefix(), propertyName);
                    GetParamFilterValid(pi, columnName);
                }
                else //主键
                {
                    IdColomn = new IdColomn();
                    IdColomn.IdProperty = pi;
                    IdColomn.ColomnName = columnName;
                    IdColomn.GenerateType = idAttri.GenerateType;
                    if (idAttri.GenerateType == IdGenerateType.Assign)
                    {
                        sbInsertColumn.AppendFormat(" {0},", columnName);
                    }
                }
            }
        }

        protected void GetParamFilterValid(PropertyInfo pi, string columnName)
        {
            if (filterValid != null) return;
            ValidAttribute validAttri = pi.GetCustomAttribute<ValidAttribute>();
            if (validAttri != null)
            {
                filterValid = new FilterValid();
                filterValid.ParamFilterValid = DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), pi.Name), validAttri.Available);
                filterValid.ParamFilterInValid = DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), pi.Name), validAttri.Invalid);
                filterValid.ValidColumnName = columnName;
                filterValid.PropertyName = pi.Name;
            }
        }

        #endregion

        #region 抽象方法 abstract
        protected abstract string GetLastIncrementID();
        #endregion

        #region 公共
        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="propertys"></param>

        private void PrepareInsert(T t, StringBuilder sbColumn, IList<IDataParameter> parms)
        {
            PropertyInfo pi = null;
            for (int i = 0; i < tableProperties.Length; i++)
            {
                pi = tableProperties[i];
                if (IdColomn.IdProperty.Name == pi.Name && IdColomn.GenerateType != IdGenerateType.Assign)
                    continue;
                sbColumn.AppendFormat("{0}{1},", DataHelper.GetParameterPrefix(), pi.Name);
                parms.Add(DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), pi.Name), pi.GetValue(t)));
            }
        }
        private void PrepareUpdate(T t, out string cmdText, IList<IDataParameter> parms)
        {
            string IdColomnName = IdColomn.ColomnName;
            object IdValue = IdColomn.IdProperty.GetValue(t, null);
            string IdPropertyName = IdColomn.IdProperty.Name;
            parms.Add(DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdPropertyName), IdValue));
            PropertyInfo pi = null;
            for (int i = 0; i < tableProperties.Length; i++)
            {
                pi = tableProperties[i];
                if (pi.Name != IdPropertyName)
                {
                    ColumnAttribute attri = pi.GetCustomAttribute<ColumnAttribute>();
                    if (attri == null || !attri.IsReadOnly)
                    {
                        parms.Add(DataHelper.CreateParameter(DataHelper.GetParameterPrefix() + pi.Name, pi.GetValue(t)));
                    }
                }
            }
            cmdText = string.Format("UPDATE {0} SET {1} WHERE {2}={3}{4}", TableName, SetColumnText, IdColomnName, DataHelper.GetParameterPrefix(), IdPropertyName);
        }
        private void PrepareUpdate(T t, StringBuilder cmdText, IList<string> properties, IList<IDataParameter> parms)
        {
            IList<PropertyInfo> filterProperties = new List<PropertyInfo>();
            PrepareUpdateSql(properties, cmdText, filterProperties);
            PrepareParameters(t, filterProperties, parms);
        }
        private void PrepareUpdateSql(IList<string> properties, StringBuilder cmdText, IList<PropertyInfo> filterProperties)
        {
            string propertyName = null;
            PropertyInfo pi = null;
            for (int i = 0; i < properties.Count; i++)
            {
                propertyName = properties[i];
                cmdText.AppendFormat("{0}={1}{2}{3}", propertyAndColumn[propertyName.ToUpper()], DataHelper.GetParameterPrefix(), propertyName, i == properties.Count - 1 ? "" : ",");
                for (int j = 0; j < tableProperties.Length; j++)
                {
                    pi = tableProperties[j];
                    if (pi.Name == propertyName)
                    {
                        filterProperties.Add(pi);
                        break;
                    }
                }
            }
        }
        private void PrepareParameters(T t, IList<PropertyInfo> properties, IList<IDataParameter> parms)
        {
            PropertyInfo pi = null;
            for (int i = 0; i < properties.Count; i++)
            {
                pi = properties[i];
                parms.Add(DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), pi.Name), pi.GetValue(t, null)));
            }
        }
        private void PrepareDelete(object idValue, out string cmdText, out IDataParameter[] parms)
        {
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            parms = new IDataParameter[] { DataHelper.CreateParameter(IdParameterName, idValue) };
            cmdText = string.Format("delete from {0} where {1}={2}", TableName, IdColomn.ColomnName, IdParameterName);
        }

        private void PrepareVirtualDelete(object idValue, out string cmdText, out IDataParameter[] parms)
        {
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            parms = new IDataParameter[] { DataHelper.CreateParameter(IdParameterName, idValue), filterValid.ParamFilterInValid };
            cmdText = string.Format("UPDATE {0} SET {1}={2}{3} WHERE {4}={5}", TableName, filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName, IdColomn.ColomnName, IdParameterName);
        }

        private T Get(StringBuilder cmdText, IList<IDataParameter> parms)
        {
            DataTable dt = DataHelper.ExecuteDataTable(cmdText.ToString(), CommandType.Text, parms.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                T t = new T();
                object value = null;
                DataRow dr = dt.Rows[0];
                PropertyInfo[] querypropertys = null;
                FilterProperty(dt, tableProperties, ref querypropertys);
                foreach (PropertyInfo pi in querypropertys)
                {
                    value = dr[pi.Name];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, value, null);
                    }
                }
                return t;
            }
            return default(T);
        }

        private IList<T> GetList(DataTable dt)
        {
            IList<T> result = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                T t = default(T);
                object value = null;
                result = new List<T>();
                PropertyInfo[] querypropertys = null;
                FilterProperty(dt, tableProperties, ref querypropertys);
                foreach (DataRow dr in dt.Rows)
                {
                    t = new T();
                    foreach (PropertyInfo pi in querypropertys)
                    {
                        value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                    result.Add(t);
                }
            }
            return result;
        }

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
                    sb.AppendFormat("{0}{1}", DataHelper.GetParameterPrefix(), parmIndex);
                    parms[parmIndex] = DataHelper.CreateParameter(DataHelper.GetParameterPrefix() + parmIndex.ToString(), parmsValue[parmIndex]);
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
                    sb.AppendFormat("{0}{1}", DataHelper.GetParameterPrefix(), parmIndex);
                    parms[parmIndex] = DataHelper.CreateParameter(DataHelper.GetParameterPrefix() + parmIndex.ToString(), parmsValue[parmIndex]);
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

        private void ExpressionHandling(StringBuilder cmdText, Expression<Func<T, bool>> expression, IList<IDataParameter> parms)
        {
            IList<DataParameter> parameters = new List<DataParameter>();
            (new ExpressionHandler(DataHelper, cmdText, parameters)).Execute(expression);
            DataParameter parameter = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                parameter = parameters[i];
                parms.Add(DataHelper.CreateParameter(parameter.ParameterName, parameter.Value));
            }
        }
        #endregion

        #region 添加 Insert
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Insert(T t)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder sbColumn = new StringBuilder(50);
            PrepareInsert(t, sbColumn, parms);
            string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2})", TableName, InsertColumnText, sbColumn.ToString().TrimEnd(','));
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
            StringBuilder sbColumn = null;
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i];
                sbColumn = new StringBuilder(50);
                parms = new List<IDataParameter>();
                PrepareInsert(t, sbColumn, parms);
                dbps[i] = parms.ToArray();
                cmdText[i] = string.Format("INSERT INTO {0}({1}) VALUES({2})", TableName, InsertColumnText, sbColumn.ToString().TrimEnd(','));
            }
            return DataHelper.ExecuteNonQuery(cmdText, dbps) > 0;
        }
        /// <summary>
        /// 插入数据，并获取自增ID
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool InsertAndGet(T t)
        {
            if (IdColomn.GenerateType != IdGenerateType.Increment)
                throw new NotImplementedException("不支持非自增列对象获取创建ID");
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder sbColumn = new StringBuilder(50);
            PrepareInsert(t, sbColumn, parms);
            string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2});{3};", TableName, InsertColumnText, sbColumn.ToString().TrimEnd(','), GetLastIncrementID());
            object obj = DataHelper.ExecuteScalar(cmdText, CommandType.Text, parms.ToArray());
            if (obj != null && obj != DBNull.Value)
            {
                IdColomn.IdProperty.SetValue(t, obj);
                return true;
            }
            return false;
        }
        #endregion

        #region 更新 Update
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(T t)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            string cmdText = null;
            PrepareUpdate(t, out cmdText, parms);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms.ToArray()) > 0;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(IList<T> list)
        {
            string[] cmdText = new string[list.Count];
            IDataParameter[][] dbps = new IDataParameter[list.Count()][];
            IList<IDataParameter> parms = null;
            string sql = null;
            T t = null;
            for (int i = 0; i < list.Count; i++)
            {
                t = list[i];
                parms = new List<IDataParameter>();
                PrepareUpdate(t, out sql, parms);
                dbps[i] = parms.ToArray();
                cmdText[i] = sql;
            }
            return DataHelper.ExecuteNonQuery(cmdText, dbps) > 0;
        }
        public bool Update(T t, IList<string> properties)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat(" UPDATE {0} SET ", TableName);
            PrepareUpdate(t, cmdText, properties, parms);
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            cmdText.AppendFormat(" WHERE {0}={1}", IdColomn.ColomnName, IdParameterName);
            object IdValue = IdColomn.IdProperty.GetValue(t, null);
            parms.Add(DataHelper.CreateParameter(IdParameterName, IdValue));

            return DataHelper.ExecuteNonQuery(cmdText.ToString(), CommandType.Text, parms.ToArray()) > 0;
        }

        public bool Update(IList<T> list, IList<string> properties)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder cmdSql = new StringBuilder();
            IList<PropertyInfo> filterProperties = new List<PropertyInfo>();
            cmdSql.Append(" UPDATE {0} SET ");
            PrepareUpdateSql(properties, cmdSql, filterProperties);
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            cmdSql.AppendFormat(" WHERE {0}={1}", IdColomn.ColomnName, IdParameterName);

            string strSql = cmdSql.ToString();
            string[] cmdText = new string[list.Count];
            IDataParameter[][] dbps = new IDataParameter[list.Count()][];

            T t = null;
            List<IDataParameter> dbs = new List<IDataParameter>();
            for (int i = 0; i < list.Count; i++)
            {
                t = list[i];
                cmdText[i] = strSql;
                dbs.Clear();
                PrepareParameters(t, filterProperties, dbs);
                object IdValue = IdColomn.IdProperty.GetValue(t, null);
                dbs.Add(DataHelper.CreateParameter(IdParameterName, IdValue));
                dbps[i] = dbs.ToArray();
            }
            return DataHelper.ExecuteNonQuery(cmdText.ToString(), CommandType.Text, parms.ToArray()) > 0;
        }

        #endregion

        #region 添加或更新  InsertOrUpdate
        public bool InsertOrUpdate(T t)
        {
            throw new NotImplementedException();
        }

        public bool InsertOrUpdate(IList<T> list)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 获取 Get

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public T Get(object IdValue)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            StringBuilder cmdText = new StringBuilder(50);
            cmdText.AppendFormat(" SELECT {0} FROM {1} {2} WHERE {2}.{3}={4}", GetColumnText, TableName, ClassName, IdColomn.ColomnName, IdParameterName);
            parms.Add(DataHelper.CreateParameter(IdParameterName, IdValue));
            if (filterValid != null)
            {
                cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms.Add(filterValid.ParamFilterValid);
            }
            return Get(cmdText, parms);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            IList<IDataParameter> parms = new List<IDataParameter>();
            StringBuilder cmdText = new StringBuilder(32);
            cmdText.AppendFormat(" SELECT {0} FROM {1} WHERE ", GetColumnText, TableName);
            ExpressionHandling(cmdText, expression, parms);
            if (filterValid != null)
            {
                cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms.Add(filterValid.ParamFilterValid);
            }
            return Get(cmdText, parms);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public IList<T> GetList()
        {
            StringBuilder cmdText = new StringBuilder(20);
            cmdText.AppendFormat("SELECT {0} FROM {1} {2}", GetColumnText, TableName, ClassName);
            IDataParameter[] parms = null;
            if (filterValid != null)
            {
                cmdText.AppendFormat(" WHERE {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms = new IDataParameter[] { filterValid.ParamFilterValid };
            }
            DataTable dt = DataHelper.ExecuteDataTable(cmdText.ToString(), CommandType.Text, parms);

            return GetList(dt);
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
            IDataParameter[] parms = null;
            if (filterValid != null)
            {
                cmdText.AppendFormat(" WHERE {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms = new IDataParameter[] { filterValid.ParamFilterValid };
            }
            if (page != null)
            {
                cmdText.AppendFormat(" ORDER BY {0}.{1} {2} LIMIT {3},{4}; "
                    , ClassName, string.IsNullOrEmpty(page.OrderColName) ? IdColomn.ColomnName : page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
                cmdText.AppendFormat(" SELECT COUNT(1) FROM {0}", TableName);
                if (filterValid != null)
                {
                    cmdText.AppendFormat(" WHERE {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                }
                cmdText.AppendFormat(";");
            }
            DataSet ds = DataHelper.ExecuteDataSet(cmdText.ToString(), CommandType.Text, parms);
            if (ds != null && ds.Tables.Count == 2)
            {
                DataTable dt = ds.Tables[0];
                result = GetList(dt);
                page.TotalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            }
            return result;
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression)
        {
            StringBuilder cmdText = new StringBuilder(32);
            IList<IDataParameter> parms = new List<IDataParameter>();
            cmdText.AppendFormat("SELECT {0} FROM {1} WHERE ", GetColumnText, TableName);
            ExpressionHandling(cmdText, expression, parms);
            if (filterValid != null)
            {
                cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms.Add(filterValid.ParamFilterValid);
            }
            DataTable dt = DataHelper.ExecuteDataTable(cmdText.ToString(), CommandType.Text, parms.ToArray());

            return GetList(dt);
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression, PageInfo page)
        {
            IList<T> result = null;
            StringBuilder cmdText = new StringBuilder(100);
            IList<IDataParameter> parms = new List<IDataParameter>();
            cmdText.AppendFormat("SELECT {0} FROM {1} WHERE ", GetColumnText, TableName);
            StringBuilder whereText = new StringBuilder();
            ExpressionHandling(whereText, expression, parms);
            cmdText.Append(whereText);
            if (filterValid != null)
            {
                cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms.Add(filterValid.ParamFilterValid);
            }
            if (page != null)
            {
                cmdText.AppendFormat(" ORDER BY {0} {1} LIMIT {2},{3}; "
                    , string.IsNullOrEmpty(page.OrderColName) ? IdColomn.ColomnName : page.OrderColName, page.OrderStr, page.StartIndex, page.PageSize);
                cmdText.AppendFormat(" SELECT COUNT(1) FROM {0} WHERE {1}", TableName, whereText);
                if (filterValid != null)
                {
                    cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                }
                cmdText.AppendFormat(";");
            }
            DataSet ds = DataHelper.ExecuteDataSet(cmdText.ToString(), CommandType.Text, parms.ToArray());
            if (ds != null && ds.Tables.Count == 2)
            {
                DataTable dt = ds.Tables[0];
                result = GetList(dt);
                page.TotalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            }
            return result;
        }
        #endregion

        #region 物理删除 Delete
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Delete(T t)
        {
            object idValue = IdColomn.IdProperty.GetValue(t, null);
            return Delete(idValue);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idValue">主键</param>
        /// <returns></returns>
        public bool Delete(object idValue)
        {
            string cmdText = null;
            IDataParameter[] parms = null;
            PrepareDelete(idValue, out cmdText, out parms);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete(IList<T> list)
        {
            T t;
            string[] cmdText = new string[list.Count];
            IDataParameter[][] dbps = new IDataParameter[list.Count][];
            string sql = null;
            IDataParameter[] parms = null;
            object idValue = null;
            for (int i = 0; i < list.Count; i++)
            {
                t = list[i];
                idValue = IdColomn.IdProperty.GetValue(t, null);
                PrepareDelete(idValue, out sql, out parms);
                cmdText[i] = sql;
                dbps[i] = parms;
            }
            return DataHelper.ExecuteNonQuery(cmdText, dbps) > 0;
        }

        #endregion

        #region 逻辑删除 VirtualDelete
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="idValue">主键</param>
        /// <returns></returns>
        public bool VirtualDelete(object idValue)
        {
            string cmdText = null;
            IDataParameter[] parms = null;
            PrepareVirtualDelete(idValue, out cmdText, out parms);
            return DataHelper.ExecuteNonQuery(cmdText, CommandType.Text, parms) > 0;
        }

        public bool VirtualDelete(T t)
        {
            object idValue = IdColomn.IdProperty.GetValue(t, null);
            return VirtualDelete(t);
        }

        public bool VirtualDelete(IList<T> list)
        {
            T t;
            string[] cmdText = new string[list.Count];
            IDataParameter[][] dbps = new IDataParameter[list.Count][];
            string sql = null;
            IDataParameter[] parms = null;
            object idValue = null;
            for (int i = 0; i < list.Count; i++)
            {
                t = list[i];
                idValue = IdColomn.IdProperty.GetValue(t, null);
                PrepareVirtualDelete(idValue, out sql, out parms);
                cmdText[i] = sql;
                dbps[i] = parms;
            }
            return DataHelper.ExecuteNonQuery(cmdText, dbps) > 0;
        }
        #endregion

        #region 扩展  extension (是否存在某个属性)
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="PropertyName">属性名</param>
        /// <param name="PropertyValue">属性值</param>
        /// <returns></returns>
        public bool IsExist(string PropertyName, object PropertyValue, object IdValue)
        {
            string colunmName = propertyAndColumn[PropertyName.ToUpper()];
            if (string.IsNullOrEmpty(colunmName)) throw new NotImplementedException("指定属性名不存在");
            IList<IDataParameter> parms = new List<IDataParameter>();
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            parms.Add(DataHelper.CreateParameter(string.Format("{0}{1}", DataHelper.GetParameterPrefix(), colunmName), PropertyValue));
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select {0} from {1} where {2}={3}", IdColomn.ColomnName, TableName, colunmName, IdParameterName);
            if (filterValid != null)
            {
                cmdText.AppendFormat(" AND {0}={1}{2}", filterValid.ValidColumnName, DataHelper.GetParameterPrefix(), filterValid.PropertyName);
                parms.Add(filterValid.ParamFilterValid);
            }
            object obj = DataHelper.ExecuteScalar(cmdText.ToString(), CommandType.Text, parms.ToArray());
            if (obj != null && !obj.Equals(IdValue))
                return true;
            return false;
        }
        #endregion
    }
}
