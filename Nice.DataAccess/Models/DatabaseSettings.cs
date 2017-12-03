using Grit.Net.Common.Attributes.Config;
using Grit.Net.Common.Config;

namespace Grit.Net.DataAccess
{
    public class DatabaseSettings
    {
        private static string dbConnString = ConfigManager.GetConnectionString("dbConnString", string.Empty);
        public static string BbConnString
        {
            get
            {
                return dbConnString;
            }
        }
        private static string providerName = ConfigManager.GetConnProviderName("dbConnString", string.Empty);
        public static string ProviderName
        {
            get
            {
                return providerName;
            }
        }

        private static int commandTimeOut = int.Parse(ConfigManager.GetAppSettings("CommandTimeOut", "30"));
        public static int CommandTimeOut
        {
            get
            {
                return commandTimeOut;
            }
        }

        private static string dataProviderAssembly= ConfigManager.GetAppSettings("DataFactory.Provider.Assembly", string.Empty);
        public static string DataProviderAssembly
        {
            get
            {
                return dataProviderAssembly;
            }
        }
        private static string dataProviderTypeName = ConfigManager.GetAppSettings("DataFactory.Provider.TypeName", string.Empty);
        public static string DataProviderTypeName
        {
            get
            {
                return dataProviderTypeName;
            }
        }

        private static string dataFactoryGeneralDAL = ConfigManager.GetAppSettings("DataFactory.GeneralDAL.TypeName", string.Empty);
        public static string DataFactoryGeneralDAL
        {
            get
            {
                return dataFactoryGeneralDAL;
            }
        }
        private static string dataFactoryQueryDAL = ConfigManager.GetAppSettings("DataFactory.QueryDAL.TypeName", string.Empty);
        public static string DataFactoryQueryDAL
        {
            get
            {
                return dataFactoryQueryDAL;
            }
        }

        private static string dataFactoryEntityAssembly = ConfigManager.GetAppSettings("DataFactory.Entity.Assembly", string.Empty);
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

        private static string dataFactoryEntityNamespace = ConfigManager.GetAppSettings("DataFactory.Entity.Namespace", string.Empty);
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
