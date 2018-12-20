using Nice.DataAccess;
using Nice.DataAccess.DAL;
using Nice.DataAccess.Model.Page;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nice.Code.Demo
{
    public class UserInfoRepository
    {
        private static GeneralDAL<UserInfo> dal = new GeneralDAL<UserInfo>();

        public bool Insert(UserInfo entity)
        {
            return dal.Insert(entity);
        }

        public IList<UserInfo> GetList()
        {
            return dal.GetList();
        }

        public IList<UserInfo> GetList(PageInfo page)
        {
            return dal.GetList(page);
        }

        public bool Update(UserInfo entity)
        {
            return dal.Update(entity);
        }
        public bool Update(UserInfo entity, IList<string> properties)
        {
            return dal.Update(entity, properties);
        }

        public bool UpdateState(UserInfo entity)
        {
            return dal.Update(entity, new string[] { "NState", "ModifyTime" });
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

        public bool VirtualDelete(string UserId)
        {
            return dal.VirtualDelete(UserId);
        }

        public bool IsExists(string PropertyName, object PropertyValue, object IdValue)
        {
            return dal.IsExist(PropertyName, PropertyValue, IdValue);
        }

        public bool UpdateErrorTest(UserInfo userInfo)
        {
            return DataUtil.GetDataHelper().ExecuteNonQuery(string.Format("update tbl_user_info set UserName='{1}' where UserIdd={0}", userInfo.UserName, userInfo.UserId)) > 0;
        }
    }
}
