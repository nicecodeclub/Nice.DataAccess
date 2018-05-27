using System.Collections.Generic;
using System.Reflection;

namespace Nice.DataAccess
{
    public class DataUtil
    {
        public const string DefaultConnStringKey = "NiceConnString";
        private const string AssemblyNameBase = "Nice.DataAccess";
        private const int DefaultCommandTimeout = 30;
        private static Dictionary<string, DataFactoryConfig> settingsDic = new Dictionary<string, DataFactoryConfig>();
        private static Dictionary<string, DataHelper> helperDic = new Dictionary<string, DataHelper>();
        private static Dictionary<string, Assembly> dpaDic = new Dictionary<string, Assembly>();

        //public static void Create()
        //{
        //    Create(DefaultConnStringKey);
        //}
        public static void Create(DatabaseConfig config)
        {
            string innerName = DatabaseTypeEx.GetStandardInnerName(config.ProviderName);
            string connStrKey = string.IsNullOrEmpty(config.ConnStrKey) ? DefaultConnStringKey : config.ConnStrKey;
            DataFactoryConfig settings = new DataFactoryConfig();
            settings.ConnString = config.ConnString;
            settings.ProviderName = config.ProviderName;
            settings.CommandTimeout = config.CommandTimeout <= 0 ? DefaultCommandTimeout : config.CommandTimeout;
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

        public static DataFactoryConfig GetSettings(string connStrKey)
        {
            return settingsDic[connStrKey];
        }
    }
}
