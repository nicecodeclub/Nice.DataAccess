using Nice.DataAccess.Model.Page;
using Nice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Nice.DataAccess.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public interface IGeneralDAL<T> where T : TEntity
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Insert(T t);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Insert(IList<T> list);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Update(IList<T> list);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t, params Expression<Func<T, object>>[] expressions);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Update(IList<T> list, params Expression<Func<T, object>>[] expressions);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t, IList<string> properties);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Update(IList<T> list, IList<string> properties);
        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool InsertOrUpdate(T t);
        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool InsertOrUpdate(IList<T> list);
        /// <summary>
        /// 插入数据并返回实体,仅支持ID为自增的数据表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool InsertAndGet(T t);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Delete(T t);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="idValue"></param>
        /// <returns></returns>
        bool Delete(object idValue);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Delete(IList<T> list);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        T Get(object id);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetList();
        /// <summary>
        /// 获取实体分页集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetList(PageInfo page);
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetList(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 获取实体分页集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetList(Expression<Func<T, bool>> expression, PageInfo page);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        bool IsExist(string PropertyName, object PropertyValue, object IdValue);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <returns></returns>
        bool VirtualDelete(object IdValue);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool VirtualDelete(T t);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool VirtualDelete(IList<T> list);

    }
}
