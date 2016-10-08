using System;
using System.Collections.Generic;

namespace Grit.Net.Common.IOC
{

    public class Container : IContainer
    {
        
        private readonly Dictionary<MappingKey, Func<object>> mappings;

        public Container()
        {
            mappings = new Dictionary<MappingKey, Func<object>>();
        }


        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="TFrom">源类型</typeparam>
        /// <typeparam name="TTo">目标类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        public void Register(Type from, Type to, string instanceName = null)
        {
            if (to == null)
                throw new ArgumentNullException("to");

            if (!from.IsAssignableFrom(to))
            {
                string errorMessage = string.Format("试图注册该实例错误: '{0}' 不是指定类型 '{1}'",
                    from.FullName, to.FullName);

                throw new InvalidOperationException(errorMessage);
            }

            Func<object> createInstanceDelegate = () => Activator.CreateInstance(to);
            Register(from, createInstanceDelegate, instanceName);
        }


        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <param name="type">源类型</param>
        /// <param name="createInstanceDelegate">创建实例的委托</param>
        /// <param name="instanceName">实例名(可选)</param>
        public void Register<TFrom, TTo>(string instanceName = null) where TTo : TFrom
        {
            Register(typeof(TFrom), typeof(TTo), instanceName);
        }

        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="createInstanceDelegate">创建实例的委托</param>
        /// <param name="instanceName">实例名(可选)</param>
        public void Register(Type type, Func<object> createInstanceDelegate, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (createInstanceDelegate == null)
                throw new ArgumentNullException("createInstanceDelegate");


            var key = new MappingKey(type, instanceName);

            if (mappings.ContainsKey(key))
            {
                const string errorMessageFormat = "已包含当前映射类型 - {0}";
                throw new InvalidOperationException(string.Format(errorMessageFormat, key.ToTraceString()));
            }


            mappings.Add(key, createInstanceDelegate);
        }

        /// <summary>
        /// 检查特定类型/实例名是否已注册
        /// </summary>
        /// <param name="type">注册类型</param>
        /// <param name="instanceName">实例名(可选)</param>
        public void Register<T>(Func<T> createInstanceDelegate, string instanceName = null)
        {
            if (createInstanceDelegate == null)
                throw new ArgumentNullException("createInstanceDelegate");

            Func<object> createInstance = createInstanceDelegate as Func<object>;
            Register(typeof(T), createInstance, instanceName);
        }


        /// <summary>
        /// 检查特定类型/实例名是否已注册
        /// </summary>
        /// <typeparam name="T">注册类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        public bool IsRegistered(Type type, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");


            var key = new MappingKey(type, instanceName);

            return mappings.ContainsKey(key);
        }


        /// <summary>
        /// 检查特定类型/实例名是否已注册
        /// </summary>
        /// <param name="type">注册类型</param>
        /// <param name="instanceName">实例名(可选)</param>
        public bool IsRegistered<T>(string instanceName = null)
        {
            return IsRegistered(typeof(T), instanceName);
        }


        /// <summary>
        /// 解析实例类型
        /// </summary>
        /// <param name="type">注册类型</param>
        /// <param name="instanceName">实例名(可选)</param>
        public object Resolve(Type type, string instanceName = null)
        {
            var key = new MappingKey(type, instanceName);
            Func<object> createInstance;

            if (mappings.TryGetValue(key, out createInstance))
            {
                var instance = createInstance();
                return instance;
            }

            const string errorMessageFormat = "没有发现映射的类型 '{0}'";
            throw new InvalidOperationException(string.Format(errorMessageFormat, type.FullName));
        }


        /// <summary>
        /// 解析实例类型
        /// </summary>
        /// <typeparam name="T">注册类型</typeparam>
        /// <param name="instanceName">实例名(可选)</param>
        public T Resolve<T>(string instanceName = null)
        {
            object instance = Resolve(typeof(T), instanceName);

            return (T)instance;
        }

    }
}