using System.Collections.Generic;
using System.Reflection;

namespace Nice.DataAccess
{
    public class DataUtil
    {
        private const string AssemblyNameBase = "Nice.DataAccess";
        private const int DefaultCommandTimeout = 30;
        private static Dictionary<string, DataFactoryConfig> settingsDic = new Dictionary<string, DataFactoryConfig>();
        private static Dictionary<string, DataHelper> helperDic = new Dictionary<string, DataHelper>();
        private static Dictionary<string, Assembly> dpaDic = new Dictionary<string, Assembly>();
        private static string defaultConnStringKey = "NiceConnString";
        public static string DefaultConnStringKey
        {
            get { return defaultConnStringKey; }
            private set { defaultConnStringKey = value; }
        }
        /// <summary>
        /// 创建数据库操作对象
        /// </summary>
        /// <param name="config"></param>
        public static void Create(DatabaseConfig config)
        {
            Create(config, false);
        }
        /// <summary>
        /// 创建数据库操作对象
        /// </summary>
        /// <param name="config">数据库配置信息</param>
        /// <param name="setDefaultConnStrKey">是否设置为默认的连接字符串</param>
        public static void Create(DatabaseConfig config, bool setDefaultConnStrKey)
        {
            string innerName = DatabaseTypeEx.GetStandardInnerName(config.ProviderName);
            string connStrKey = string.IsNullOrEmpty(config.ConnStrKey) ? defaultConnStringKey : config.ConnStrKey;
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
            if (setDefaultConnStrKey)
            {
                defaultConnStringKey = connStrKey;
            }
        }

        public static DataHelper GetDataHelper()
        {
            return helperDic[defaultConnStringKey];
        }

        public static DataHelper GetDataHelper(string connStrKey)
        {
            return helperDic[connStrKey];
        }
        internal static Assembly GetAssembly(string connStrKey)
        {
            return dpaDic[connStrKey];
        }

        public static DataFactoryConfig GetSettings(string connStrKey)
        {
            return settingsDic[connStrKey];
        }
    }
}
