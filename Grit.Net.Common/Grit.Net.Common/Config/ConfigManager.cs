using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace Grit.Net.Common.Config
{
    /// <summary>
    /// 基本配置信息
    /// </summary>
    public class ConfigManager
    {
        private string[] node;//存放配置节点名称
        private string[] value;//存放相应配置文件Value
        private string filepath;
        private static object locker = new object();
        /// <summary>
        /// 配置文件缓存对象
        /// </summary>
        public static ConfigManager Cache;
        /// <summary>
        /// 通过配置文件节点名称访问配置项的值
        /// </summary>
        /// <param name="_key">节点名称</param>
        /// <returns></returns>
        public string this[string _key]
        {
            get
            {
                for (int i = 0; i < node.Length; i++)
                    if (node[i] == _key.ToUpper())
                        return value[i];
                return null;
            }
        }
        /// <summary>
        /// 初始化配置文件信息
        /// </summary>
        public static void Initial(string startupPath)
        {
            Cache = new ConfigManager();
            FileSystemWatcher watcher = new FileSystemWatcher();
            Cache.filepath = startupPath;
            watcher.Path = Cache.filepath;
            watcher.Filter = "*.config";
            watcher.Changed += new FileSystemEventHandler(Cache.OnChanged);
            watcher.EnableRaisingEvents = true;
            Cache.Refresh();
        }

        private void Refresh()
        {
            lock (locker)
            {
                try
                {

                    AppSettingsReader reader = new AppSettingsReader();
                    ConnectionStringSettingsCollection connStrSettings = ConfigurationManager.ConnectionStrings;

                    NameValueCollection appStgs = ConfigurationManager.AppSettings;
                    string[] names = ConfigurationManager.AppSettings.AllKeys;

                    int flag = appStgs.Count;
                    int count = flag + connStrSettings.Count * 2 + 1;
                    node = new string[count];
                    value = new string[count];

                    for (int i = 0; i < flag; i++)
                    {
                        string key = names[i];
                        node[i] = key.ToUpper();
                        value[i] = reader.GetValue(key, typeof(string)).ToString();
                    }

                    for (int i = 0; i < connStrSettings.Count; i++)
                    {
                        node[flag] = connStrSettings[i].Name.ToUpper();
                        value[flag] = connStrSettings[i].ConnectionString;
                        flag++;
                        node[flag] = connStrSettings[i].Name.ToUpper() + "ProviderName".ToUpper();
                        value[flag] = connStrSettings[i].ProviderName;
                        flag++;
                    }
                    //默认数据库访问驱动器
                    node[flag] = "ProviderName".ToUpper();
                    value[flag] = connStrSettings[1].ProviderName;


                }
                catch (ConfigurationErrorsException)
                {
                }
                catch (NullReferenceException)
                {
                }
                catch (IOException)
                {
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 当配置文件修改后，修改缓存中的Value
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Refresh();
        }
    }
}
