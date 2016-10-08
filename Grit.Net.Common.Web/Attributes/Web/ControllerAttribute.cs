using System;

namespace Grit.Net.Common.Attributes.Web
{
    /// <summary>
    /// Controller注解
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        private string route;
        /// <summary>
        /// 路由路径
        /// </summary>
        public string Route
        {
            get
            {
                return route;
            }
            set
            {
                route = value;
            }
        }
    }
}
