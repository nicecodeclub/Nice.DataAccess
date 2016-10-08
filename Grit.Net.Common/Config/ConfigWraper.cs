using Grit.Net.Common.Attributes.Config;
using Grit.Net.Common.Convert;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Grit.Net.Common.Config
{
    public class ConfigWraper
    {
        public void LoadConfigValue<T>()
        {
            Type t = typeof(T);
            PropertyInfo[] propertys = t.GetProperties();
            string[] appStgs = ConfigurationManager.AppSettings.AllKeys;
            PropertyInfo[] configPropertys = null;
            string[] configkeys = null;
            FilterConfigPropertys(propertys, appStgs, ref configkeys, ref configPropertys);
            PropertyInfo pi = null;
            for (int i = 0; i < configPropertys.Count(); i++)
            {
                pi = configPropertys[i];
                switch (pi.PropertyType.ToString())
                {
                    case "System.Int32":
                        pi.SetValue(t, DataConvert.ToInt(ConfigurationManager.AppSettings[configkeys[i]]));
                        break;
                    case "System.Boolean":
                        pi.SetValue(t, DataConvert.ToBoolean(ConfigurationManager.AppSettings[configkeys[i]]));
                        break;
                    case "System.Decimal":
                        pi.SetValue(t, DataConvert.ToDecimal(ConfigurationManager.AppSettings[configkeys[i]]));
                        break;
                    default:
                        pi.SetValue(t, ConfigurationManager.AppSettings[configkeys[i]]);
                        break;
                }
            }
        }

        public void LoadConfigValue<T>(T t)
        {
            Type type = typeof(T);
            PropertyInfo[] propertys = type.GetProperties();
            string[] appStgs = ConfigurationManager.AppSettings.AllKeys;
            PropertyInfo[] configPropertys = null;
            string[] configkeys = null;
            FilterConfigPropertys(propertys, appStgs, ref configkeys, ref configPropertys);
            PropertyInfo pi = null;
            for (int i = 0; i < configPropertys.Count(); i++)
            {
                pi = configPropertys[i];
                switch (pi.PropertyType.ToString())
                {
                    case "System.Int32":
                        pi.SetValue(t, DataConvert.ToInt(ConfigurationManager.AppSettings[configkeys[i]]), null);
                        break;
                    case "System.Boolean":
                        pi.SetValue(t, DataConvert.ToBoolean(ConfigurationManager.AppSettings[configkeys[i]]), null);
                        break;
                    case "System.Decimal":
                        pi.SetValue(t, DataConvert.ToDecimal(ConfigurationManager.AppSettings[configkeys[i]]), null);
                        break;
                    default:
                        pi.SetValue(t, ConfigurationManager.AppSettings[configkeys[i]], null);
                        break;
                }
            }

        }

        private void FilterConfigPropertys(PropertyInfo[] propertys, string[] appStgs, ref string[] configkeys, ref PropertyInfo[] configPropertys)
        {

            IList<PropertyInfo> propertyArray = new List<PropertyInfo>();
            IList<string> configKeyArray = new List<string>();
            foreach (PropertyInfo pi in propertys)
            {
                foreach (string appstg in appStgs)
                {
                    if (pi.Name.Equals(appstg))
                    {
                        propertyArray.Add(pi);
                        configKeyArray.Add(appstg);
                        break;
                    }
                    else
                    {
                        ConfigKeyAttribute attr = pi.GetCustomAttribute<ConfigKeyAttribute>();
                        if (attr != null && appstg == attr.Name)
                        {
                            propertyArray.Add(pi);
                            configKeyArray.Add(appstg);
                            break;
                        }
                    }
                }
            }
            configkeys = configKeyArray.ToArray();
            configPropertys = propertyArray.ToArray();
        }
    }
}
