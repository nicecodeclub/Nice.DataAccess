using Grit.Net.Common.Log;
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Grit.Net.Common.Config
{
    /// <summary>
    /// 配置文件管理辅助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 设置AppSetting节点
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool SetAppSettings(string strKey, string strValue)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                string filename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                doc.Load(filename);
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlAttribute att = nodes[i].Attributes["key"];
                    if (att != null && att.Value == strKey)
                    {
                        att = nodes[i].Attributes["value"];
                        att.Value = strValue;
                        break;
                    }
                }
                doc.Save(filename);
                return true;
            }
            catch (XmlException ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
        }
        /// <summary>
        ///  设置多个AppSetting节点
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool SetAppSettings(string[] strKey, string[] strValue)
        {
            try
            {
                if (strKey.Length != strValue.Length)
                    throw new InvalidDataException("键值数量匹配");
                XmlDocument doc = new XmlDocument();
                string filename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                doc.Load(filename);
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 0; i < strKey.Length; i++)
                {
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        XmlAttribute att = nodes[j].Attributes["key"];
                        if (att != null && att.Value == strKey[i])
                        {
                            att = nodes[j].Attributes["value"];
                            att.Value = strValue[i];
                            break;
                        }
                    }
                }
                doc.Save(filename);
                return true;
            }
            catch (XmlException ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加密\解密数据库连接字符串
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="isEncrypt"></param>
        public void EncryptConnectionString(string pathName, bool isEncrypt)
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
        /// <summary>
        /// 修改数据库连接字符串
        /// </summary>
        /// <param name="exePath">可执行文件(.exe)路径</param>
        /// <param name="connStrName">连接字符串名</param>
        /// <param name="connStrValue">字符串值</param>
        /// <param name="providerName">数据库驱动名</param>
        /// <returns></returns>
        public bool SetConnectionString(string exePath, string connStrName, string connStrValue,
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
                LogManager.Write(exePath + ":" + ex.Message);
                return false;
            }
            catch (ConfigurationException ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Write(ex.Message);
                return false;
            }
        }
    }
}
