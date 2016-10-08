using Grit.Net.Common.DataAccess;
using Grit.Net.Common.Models;
using Grit.Net.Common.Models.Page;
using System;
using System.Collections.Generic;

namespace Grit.Net.Common.DAL
{
    public class GeneralDAL<T> where T : TEntity, new()
    {
        IGeneralDAL<T> dal = null;
        public GeneralDAL()
        {
            switch (DataHelper.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    dal = new MSSqlGeneralDAL<T>();
                    break;
                case DatabaseType.MySQL:
                    dal = new MySqlGeneralDAL<T>();
                    break;
                default:
                    throw new InvalidOperationException("无效的数据库连接驱动！");
            }
        }

        public bool Delete(T t)
        {
            return dal.Delete(t);
        }
        public bool Delete(object idValue)
        {
            return dal.Delete(idValue);
        }

        public T Get(object id)
        {
            return dal.Get(id);
        }

        public IList<T> GetList()
        {
            return dal.GetList();
        }

        public IList<T> GetList(PageInfo page)
        {
            return dal.GetList(page);
        }

        public bool Insert(T t)
        {
            return dal.Insert(t);
        }

        public bool Insert(IList<T> list)
        {
            return dal.Insert(list);
        }

        public bool IsExist(string PropertyName, object PropertyValue, object IdValue)
        {
            return dal.IsExist(PropertyName, PropertyValue, IdValue);
        }

        public bool Update(T t)
        {
            return dal.Update(t);
        }

        public bool VirtualDelete(object IdValue)
        {
            return dal.VirtualDelete(IdValue);
        }

        public bool InsertAndGet(T t)
        {
            return dal.InsertAndGet(t);
        }
    }
}
