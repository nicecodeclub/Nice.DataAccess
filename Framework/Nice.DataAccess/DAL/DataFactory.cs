using Nice.DataAccess.Attributes;

using Nice.DataAccess.Models;
using Nice.DataAccess.TypeEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nice.DataAccess.DAL
{
    public class DataFactory<T> where T : TEntity
    {
        public static IGeneralDAL<T> Create(string connStrKey)
        {
            return DataUtil.GetAssembly(connStrKey).CreateInstance(string.Format("{0}`1[[{1}]]", DataUtil.GetSettings(connStrKey).DataFactoryGeneralDAL, typeof(T).AssemblyQualifiedName)
                ,false, BindingFlags.Default, null, new string[] { connStrKey }, null, null) as IGeneralDAL<T>;
        }
    }

    public class QueryFactory<T> where T : new()
    {
        public static IQueryDAL<T> Create(string connStrKey)
        {
            return DataUtil.GetAssembly(connStrKey).CreateInstance(string.Format("{0}`1[[{1}]]", DataUtil.GetSettings(connStrKey).DataFactoryQueryDAL, typeof(T).AssemblyQualifiedName)
                , false, BindingFlags.Default, null, new string[] { connStrKey }, null, null) as IQueryDAL<T>;
        }
    }

    public class QueryFactory
    {
        public static IQueryDAL Create(string connStrKey)
        {
            return DataUtil.GetAssembly(connStrKey).CreateInstance(DataUtil.GetSettings(connStrKey).DataFactoryQueryDAL) as IQueryDAL;
        }
    }

    public class DataEntityFactory
    {
        public static Mapping<string, string> EntityAndTables;
        public static Mapping<string, string> PropertyAndColumns;

        public static void Create(string assemblyName, string namespaceName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                IEnumerable<Type> types = null;
                if (string.IsNullOrEmpty(namespaceName))
                    types = Assembly.Load(assemblyName).GetTypes().Where(t => t.IsClass);
                else
                    types = Assembly.Load(assemblyName).GetTypes().Where(t => t.IsClass && t.Namespace == namespaceName);

                EntityAndTables = new Mapping<string, string>(types.Count());
                PropertyAndColumns = new Mapping<string, string>();
                string typeName = null;
                string tableName = null;
                PropertyInfo[] properties = null;
                string propertyName = null;
                string columnName = null;

                TableAttribute attr = null;
                foreach (Type type in types)
                {
                    attr = type.GetCustomAttribute<TableAttribute>();
                    if (attr == null) continue;
                    typeName = type.Name;
                    tableName = attr.Name;

                    EntityAndTables.Add(typeName.ToUpper(), attr.Name ?? typeName);
                    properties = type.GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        ColumnAttribute attri = pi.GetCustomAttribute<ColumnAttribute>();
                        propertyName = pi.Name;
                        if (attri == null)
                            columnName = propertyName;
                        else
                            columnName = attri.Name;
                        PropertyAndColumns.Add(string.Format("{0}.{1}", typeName, propertyName.ToUpper()), string.Format("{0}.{1}", typeName, columnName));
                    }
                }
            }
        }
    }

}
