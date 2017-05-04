using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Grit.Net.Common.Extensions
{
    /// <summary>
    /// DataTable和List辅助类
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
        /// <summary>  
        /// 将泛型集合类转换成DataTable  
        /// </summary>  
        /// <typeparam name="T">集合项类型</typeparam>  
        /// <param name="list">集合</param>  
        /// <param name="propertyName">需要返回的列的列名</param>  
        /// <returns>数据集(表)</returns>  
        public static DataTable ToDataTable<T>(IList<T> list, string[] propertyNames)
        {

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = typeof(T).GetProperties();
                IList<PropertyInfo> propertyArray = new List<PropertyInfo>(20);

                foreach (var propertyName in propertyNames)
                {
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyName == pi.Name)
                        {
                            propertyArray.Add(pi);
                            result.Columns.Add(pi.Name, pi.PropertyType);
                            break;
                        }
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertyArray)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }


    }
}
