using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.Attributes.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigKeyAttribute: Attribute
    {
        private string name;
        /// <summary>
        /// 表名
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

        public object defaultValue;
        public object DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }
        }

    }
}
