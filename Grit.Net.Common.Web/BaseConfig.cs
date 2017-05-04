using Grit.Net.Common.Attributes.Config;
using Grit.Net.Common.Config;

namespace Grit.Net.Common.Web
{
    public class BaseConfig
    {
        static BaseConfig()
        {
            (new ConfigWraper()).LoadConfigValue<BaseConfig>();
        }
        private static string webControllerAssembly;
        private static string webControllerNamespace;
        private static string dataFactoryEntityAssembly;
        private static string dataFactoryEntityNamespace;

        [ConfigKey(Name = "Web.Controller.Assembly")]
        public static string WebControllerAssembly
        {
            get
            {
                return webControllerAssembly;
            }

            set
            {
                webControllerAssembly = value;
            }
        }

        [ConfigKey(Name = "Web.Controller.Namespace")]
        public static string WebControllerNamespace
        {
            get
            {
                return webControllerNamespace;
            }

            set
            {
                webControllerNamespace = value;
            }
        }
       
        [ConfigKey(Name = "Data.Factory.Entity.Assembly")]
        public static string DataFactoryEntityAssembly
        {
            get
            {
                return dataFactoryEntityAssembly;
            }

            set
            {
                dataFactoryEntityAssembly = value;
            }
        }
        /// <summary>
        /// Data.Factory.Entity.Namespace
        /// </summary>

        [ConfigKey(Name = "Data.Factory.Entity.Namespace")]
        public static string DataFactoryEntityNamespace
        {
            get
            {
                return dataFactoryEntityNamespace;
            }

            set
            {
                dataFactoryEntityNamespace = value;
            }
        }
    }
}
