using Grit.Net.Common.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace Grit.Net.Common.Config
{
    /// <summary>
    /// 配置文件管理辅助类
    /// </summary>
    public static class ConfigManager
    { /// <summary>
      /// 设置AppSetting节点
      /// </summary>
      /// <param name="strKey">Key</param>
      /// <param name="strValue">Value</param>
        public static bool SetWebAppSettings(string applicationPath, string strKey, string strValue)
        {
            Configuration config = null;
            try
            {
                //打开配置文件及相关配置节
                config = WebConfigurationManager.OpenWebConfiguration(applicationPath);//打开配置文件及相关配置节
            }
            catch (ConfigurationErrorsException) { }
            return SetAppSettings(config, new Dictionary<string, string>() { { strKey, strValue } });
        }
        /// <summary>
        /// 设置AppSetting节点
        /// </summary>
        /// <param name="config">Configuration实例</param>
        /// <param name="dic">Key/Value字典</param>
        public static bool SetWebAppSettings(string applicationPath, Dictionary<string, string> dic)
        {
            Configuration config = null;
            try
            {
                config = WebConfigurationManager.OpenWebConfiguration(applicationPath);//打开配置文件及相关配置节
            }
            catch (ConfigurationErrorsException) { }
            return SetAppSettings(config, dic);
        }
        /// <summary>
        /// 设置AppSetting节点
        /// </summary>
        /// <param name="config">Configuration实例</param>
        /// <param name="dic">Key/Value字典</param>
        public static bool SetExeAppSettings(Dictionary<string, string> dic)
        {
            Configuration config = null;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch (ConfigurationErrorsException) { }
            return SetAppSettings(config, dic);
        }

        /// <summary>
        /// 设置AppSetting节点
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strValue">Value</param>
        public static bool SetExeAppSettings(string strKey, string strValue)
        {
            Configuration config = null;
            try
            {
                //打开配置文件及相关配置节
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch (ConfigurationErrorsException) { }
            return SetAppSettings(config, new Dictionary<string, string>() { { strKey, strValue } });
        }

        /// <summary>
        /// 设置AppSetting节点
        /// </summary>
        /// <param name="config">Configuration实例</param>
        /// <param name="dic">Key/Value字典</param>
        public static bool SetAppSettings(Configuration config, Dictionary<string, string> dic)
        {
            if (config == null) return false;
            try
            {
                AppSettingsSection appSettings = config.GetSection("appSettings") as AppSettingsSection;
                bool IsChange = false;
                if (dic != null && dic.Count > 0)
                {
                    foreach (var item in dic)
                    {
                        var settings = appSettings.Settings[item.Key];
                        if (settings != null && settings.Value != item.Value)
                        {
                            settings.Value = item.Value;
                            IsChange = true;
                        }
                        else if (settings == null)
                        {
                            appSettings.Settings.Add(item.Key, item.Value);
                            IsChange = true;
                        }
                    }
                }
                if (!IsChange) return true;
                //清除缓存
                ConfigurationManager.RefreshSection("appSettings");
                config.Save();
                config = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetAppSettings(string strKey, string strDefault)
        {
            string strValue = ConfigurationManager.AppSettings[strKey];
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return strDefault;
            }
            return strValue;
        }
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConnectionString(string strKey, string defaultValue)
        {
            ConnectionStringSettings ConnString = ConfigurationManager.ConnectionStrings[strKey];
            if (ConnString == null || string.IsNullOrEmpty(ConnString.ConnectionString))
                return defaultValue;
            return ConnString.ConnectionString;
        }
        public static string GetConnProviderName(string strKey, string defaultValue)
        {
            ConnectionStringSettings ConnString = ConfigurationManager.ConnectionStrings[strKey];
            if (ConnString == null || string.IsNullOrEmpty(ConnString.ProviderName))
                return defaultValue;
            return ConnString.ProviderName;
        }

        /// <summary>
        /// 修改数据库连接字符串
        /// </summary>
        /// <param name="exePath">可执行文件(.exe)路径</param>
        /// <param name="connStrName">连接字符串名</param>
        /// <param name="connStrValue">字符串值</param>
        /// <param name="providerName">数据库驱动名</param>
        /// <returns></returns>
        public static bool SetConnectionString(string exePath, string connStrName, string connStrValue,
            string providerName)
        {
            try
            {
                ConnectionStringSettings conn = new ConnectionStringSettings();
                conn.ConnectionString = connStrValue;
                conn.Name = connStrName;
                conn.ProviderName = providerName;
                Configuration conStart = ConfigurationManager.OpenExeConfiguration(exePath);

                ConnectionStringsSection conSection = conStart.GetSection("connectionStrings") as ConnectionStringsSection;
                if (conSection == null) throw new InvalidOperationException("配置文件错误！");
                if (conSection.ConnectionStrings[connStrName] != null)
                {
                    conSection.ConnectionStrings.Remove(connStrName);
                }
                conStart.ConnectionStrings.ConnectionStrings.Add(conn);
                conStart.Save();
                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                Logging.Error(exePath + ":" + ex.Message);
                return false;
            }
            catch (ConfigurationException ex)
            {
                Logging.Error(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 加密\解密数据库连接字符串
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="isEncrypt"></param>
        public static void EncryptConnectionString(string pathName, bool isEncrypt)
        {
            // Define the Dpapi provider name.
            string strProvider = "DataProtectionConfigurationProvider";
            // string strProvider = "RSAProtectedConfigurationProvider";

            Configuration configuration = null;
            ConnectionStringsSection dbConnSection = null;
            // Open the configuration file and retrieve 
            // the connectionStrings section.
            // For Web!
            // oConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            // For Windows! Takes the executable file name without the config extension.
            configuration = ConfigurationManager.OpenExeConfiguration(pathName);

            if (configuration != null)
            {
                bool blnChanged = false;

                dbConnSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;

                if (dbConnSection != null)
                {
                    if ((!(dbConnSection.ElementInformation.IsLocked)) && (!(dbConnSection.SectionInformation.IsLocked)))
                    {
                        if (isEncrypt)
                        {
                            if (!(dbConnSection.SectionInformation.IsProtected))
                            {
                                blnChanged = true;
                                // Encrypt the section.
                                dbConnSection.SectionInformation.ProtectSection(strProvider);
                            }
                        }
                        else
                        {
                            if (dbConnSection.SectionInformation.IsProtected)
                            {
                                blnChanged = true;
                                // Remove encryption.
                                dbConnSection.SectionInformation.UnprotectSection();
                            }
                        }
                    }

                    if (blnChanged)
                    {
                        dbConnSection.SectionInformation.ForceSave = true;
                        configuration.Save();
                    }
                }
            }

        }

    }
}
