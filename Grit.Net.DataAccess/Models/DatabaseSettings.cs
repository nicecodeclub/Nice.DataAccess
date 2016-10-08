using Grit.Net.Common.Attributes.Config;
using Grit.Net.Common.Config;

namespace Grit.Net.DataAccess
{
    public class DatabaseSettings
    {
        private static string dbConnString = ConfigHelper.GetConnectionString("dbConnString", string.Empty);
        public static string BbConnString
        {
            get
            {
                return dbConnString;
            }
        }
        private static string providerName = ConfigHelper.GetConnProviderName("dbConnString", string.Empty);
        public static string ProviderName
        {
            get
            {
                return providerName;
            }
        }

        private static int commandTimeOut = int.Parse(ConfigHelper.GetAppSettings("CommandTimeOut", "30"));
        public static int CommandTimeOut
        {
            get
            {
                return commandTimeOut;
            }
        }

        private static string dataProviderAssembly= ConfigHelper.GetAppSettings("DataFactory.Provider.Assembly", string.Empty);
        public static string DataProviderAssembly
        {
            get
            {
                return dataProviderAssembly;
            }
        }
        private static string dataProviderTypeName = ConfigHelper.GetAppSettings("DataFactory.Provider.TypeName", string.Empty);
        public static string DataProviderTypeName
        {
            get
            {
                return dataProviderTypeName;
            }
        }

        private static string dataFactoryGeneralDAL = ConfigHelper.GetAppSettings("DataFactory.GeneralDAL.TypeName", string.Empty);
        public static string DataFactoryGeneralDAL
        {
            get
            {
                return dataFactoryGeneralDAL;
            }
        }
        private static string dataFactoryQueryDAL = ConfigHelper.GetAppSettings("DataFactory.QueryDAL.TypeName", string.Empty);
        public static string DataFactoryQueryDAL
        {
            get
            {
                return dataFactoryQueryDAL;
            }
        }

        private static string dataFactoryEntityAssembly = ConfigHelper.GetAppSettings("DataFactory.Entity.Assembly", string.Empty);
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

        private static string dataFactoryEntityNamespace = ConfigHelper.GetAppSettings("DataFactory.Entity.Namespace", string.Empty);
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
