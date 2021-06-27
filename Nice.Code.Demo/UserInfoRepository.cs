using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nice.Code.Demo
{
    public class UserInfoRepository
    {
        private GeneralDAL<UserInfo> dal = DALManager<UserInfo>.Create();

        public bool Insert(UserInfo entity)
        {
            return dal.Insert(entity);
        }

        public bool InsertOrUpdate(UserInfo entity)
        {
            return dal.InsertOrUpdate(entity);
        }

        public IList<UserInfo> GetList()
        {
            return dal.GetList();
        }

        public IList<UserInfo> GetList(PageInfo page)
        {
            return dal.GetList(page);
        }
        public IList<UserInfo> GetList(Expression<Func<UserInfo, bool>> expression)
        {
            return dal.GetList(expression);
        }
        public IList<UserInfo> GetList(Expression<Func<UserInfo, bool>> expression, PageInfo page)
        {
            return dal.GetList(expression, page);
        }
        public bool Update(UserInfo entity)
        {
            return dal.Update(entity);
        }
        public bool Update(UserInfo entity, params Expression<Func<UserInfo, object>>[] expressions)
        {
            return dal.Update(entity, expressions);
        }

        public bool UpdateState(UserInfo entity)
        {
            return dal.Update(entity, o => o.NState, o => o.ModifyTime);
        }

        public UserInfo Get(string UserId)
        {
            return dal.Get(UserId);
        }

        public UserInfo GetByName(string UserName)
        {
            return dal.Get(o => o.UserName == UserName);
        }

        public bool Delete(string UserId)
        {
            return dal.Delete(UserId);
        }
        public bool Delete(UserInfo entity)
        {
            return dal.Delete(entity);
        }

        public bool VirtualDelete(UserInfo entity)
        {
            return dal.VirtualDelete(entity);
        }

        public bool VirtualDelete(string UserId)
        {
            return dal.VirtualDelete(UserId);
        }

        public bool IsExists(string PropertyName, object PropertyValue, object IdValue)
        {
            return dal.IsExist(PropertyName, PropertyValue, IdValue);
        }

    }
}
