
using Nice.DataAccess.Model.Page;
using Nice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nice.DataAccess.DAL
{
    public class GeneralDAL<T> where T : TEntity, new()
    {
        IGeneralDAL<T> dal = null;
        public GeneralDAL() : this(DataUtil.DefaultConnStringKey) { }

        public GeneralDAL(string connStringKey)
        {
            dal = DataFactory<T>.Create(connStringKey);
        }
        #region 添加 Insert
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Insert(T t)
        {
            return dal.Insert(t);
        }
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Insert(IList<T> list)
        {
            return dal.Insert(list);
        }
        /// <summary>
        /// 插入数据，并获取自增ID
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool InsertAndGet(T t)
        {
            return dal.InsertAndGet(t);
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
            return dal.Update(t);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(IList<T> list)
        {
            return dal.Update(list);
        }
        public bool Update(T t, params Expression<Func<T, object>>[] expressions)
        {
            return dal.Update(t, expressions);
        }

        public bool Update(IList<T> list, params Expression<Func<T, object>>[] expressions)
        {
            return dal.Update(list, expressions);
        }

        #endregion

        #region 添加或更新  InsertOrUpdate
        public bool InsertOrUpdate(T t)
        {
            return dal.InsertOrUpdate(t);
        }

        public bool InsertOrUpdate(IList<T> list)
        {
            return dal.InsertOrUpdate(list);
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
            return dal.Get(IdValue);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return dal.Get(expression);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public IList<T> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="IdValue">主键</param>
        /// <returns></returns>
        public IList<T> GetList(PageInfo page)
        {
            return dal.GetList( page);
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression)
        {
            return dal.GetList(expression);
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression, PageInfo page)
        {
            return dal.GetList(expression, page);
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
            return dal.Delete(t);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idValue">主键</param>
        /// <returns></returns>
        public bool Delete(object idValue)
        {
            return dal.Delete(idValue);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete(IList<T> list)
        {
            return dal.Delete(list);
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
            return dal.VirtualDelete(idValue);
        }

        public bool VirtualDelete(T t)
        {
            return dal.VirtualDelete(t);
        }

        public bool VirtualDelete(IList<T> list)
        {
            return dal.VirtualDelete(list);
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
            return dal.IsExist(PropertyName, PropertyValue, IdValue);
        }
        #endregion
    }
}
