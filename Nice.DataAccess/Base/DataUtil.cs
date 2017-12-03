using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace Nice.DataAccess
{
    public class DataUtil
    {
        public const string DefaultConnStringKey = "NiceConnString";
        private const string AssemblyNameBase = "Nice.DataAccess";
        private const int DefaultCommandTimeout = 30;
        private static Dictionary<string, DatabaseSettings> settingsDic = new Dictionary<string, DatabaseSettings>();
        private static Dictionary<string, DataHelper> helperDic = new Dictionary<string, DataHelper>();
        private static Dictionary<string, Assembly> dpaDic = new Dictionary<string, Assembly>();

        public static void Create()
        {
            Create(DefaultConnStringKey);
        }

        public static void Create(string connStrKey)
        {
            ConnectionStringSettings connString = ConfigurationManager.ConnectionStrings[connStrKey];
            string innerName = DatabaseTypeEx.GetStandardInnerName(connString.ProviderName);
            DatabaseSettings settings = new DatabaseSettings();
            settings.ConnString = connString.ConnectionString;
            settings.ProviderName = connString.ProviderName;
            object commandTimeout = ConfigurationManager.AppSettings[connStrKey + ".CommandTimeout"];
            if (commandTimeout == null)
            {
                settings.CommandTimeout = DefaultCommandTimeout;
            }
            else
            {
                settings.CommandTimeout = int.Parse(commandTimeout.ToString());
            }
            settings.DataProviderAssembly = AssemblyNameBase + "." + innerName;
            settings.DataProviderTypeName = settings.DataProviderAssembly + ".Provider." + innerName + "DataProvider";
            settings.DataFactoryGeneralDAL = settings.DataProviderAssembly + ".DAL." + innerName + "GeneralDAL";
            settings.DataFactoryQueryDAL = settings.DataProviderAssembly + ".DAL." + innerName + "QueryDAL";
            settingsDic.Add(connStrKey, settings);
            helperDic.Add(connStrKey, new DataHelper(settings));
            dpaDic.Add(connStrKey, Assembly.Load(settings.DataProviderAssembly));
        }

        public static DataHelper GetDataHelper(string connStrKey)
        {
            return helperDic[connStrKey];
        }
        public static Assembly GetAssembly(string connStrKey)
        {
            return dpaDic[connStrKey];
        }

        public static DatabaseSettings GetSettings(string connStrKey)
        {
            return settingsDic[connStrKey];
        }
    }
}
