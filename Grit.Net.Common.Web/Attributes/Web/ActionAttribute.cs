using System;

namespace Grit.Net.Common.Attributes.Web
{
    /// <summary>
    /// HttpHandler下Action名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : Attribute
    {
        private string name;
        /// <summary>
        /// Action标识
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
