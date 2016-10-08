using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.IOC
{
    public interface IContainer
    {
        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="instanceName">实例名(可选)</param>
        void Register(Type from, Type to, string instanceName = null);


        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="TFrom">源类型</typeparam>
        /// <typeparam name="TTo">目标类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        void Register<TFrom, TTo>(string instanceName = null) where TTo : TFrom;


        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <param name="type">源类型</param>
        /// <param name="createInstanceDelegate">创建实例的委托</param>
        /// <param name="instanceName">实例名(可选)</param>
        void Register(Type type, Func<object> createInstanceDelegate, string instanceName = null);

        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="createInstanceDelegate">创建实例的委托</param>
        /// <param name="instanceName">实例名(可选)</param>
        void Register<T>(Func<T> createInstanceDelegate, string instanceName = null);


        /// <summary>
        /// 检查特定类型/实例名是否已注册
        /// </summary>
        /// <param name="type">注册类型</param>
        /// <param name="instanceName">实例名(可选)</param>
  
        bool IsRegistered(Type type, string instanceName = null);


        /// <summary>
        /// 检查特定类型/实例名是否已注册
        /// </summary>
        /// <typeparam name="T">注册类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        bool IsRegistered<T>(string instanceName = null);


        /// <summary>
        /// 解析实例类型
        /// </summary>
        /// <param name="type">注册类型</param>
        /// <param name="instanceName">实例名(可选)</param>
        object Resolve(Type type, string instanceName = null);


        /// <summary>
        /// 解析实例类型
        /// </summary>
        /// <typeparam name="T">注册类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        T Resolve<T>(string instanceName = null);
    }
}
