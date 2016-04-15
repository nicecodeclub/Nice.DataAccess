using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Grit.Net.Common.Helper
{
    public class XmlHelperEx<T> where T : new()
    {
        /// <summary>
        /// 实体转换为XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EntityToXml(T obj)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement(typeof(T).Name);

            PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            XmlNode xmlNode = null;
            object value = null;
            foreach (PropertyInfo pinfo in propertyInfo)
            {
                if (pinfo == null) continue;

                xmlNode = doc.CreateElement(pinfo.Name);
                Type type = pinfo.PropertyType;
                if (type.IsArray)//如果是数组对象，遍历数组对象字段
                {
                    Array arr = pinfo.GetValue(obj, null) as Array;
                    if (arr == null) continue;

                    XmlNode itemNode = null;
                    foreach (var item in arr)
                    {
                        Type childType = item.GetType();
                        itemNode = doc.CreateElement(childType.Name);
                        object child = Activator.CreateInstance(childType);
                        PropertyInfo[] childPropertyInfos = childType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        XmlNode itemChildNode = null;
                        foreach (PropertyInfo cpinfo in childPropertyInfos)
                        {
                            if (cpinfo == null)
                                continue;
                            itemChildNode = doc.CreateElement(cpinfo.Name);
                            value = cpinfo.GetValue(child, null);
                            if (value == null)
                                itemChildNode.InnerText = string.Empty;
                            else
                                itemChildNode.InnerText = value.ToString();
                            itemNode.AppendChild(itemChildNode);
                        }
                        xmlNode.AppendChild(itemNode);
                    }
                    root.AppendChild(xmlNode);
                }
                else
                {
                    value = pinfo.GetValue(obj, null);
                    if (value == null)
                        xmlNode.InnerText = string.Empty;
                    else
                        xmlNode.InnerText = value.ToString();
                    root.AppendChild(xmlNode);
                }
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        /// <summary>
        /// 实体集合转换为XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EntityToXml(IList<T> obj)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement(typeof(T).Name + "s");

            foreach (T item in obj)
            {
                EntityToXml(doc, root, item);
            }
            doc.AppendChild(root);

            return doc.InnerXml;
        }
        /// <summary>
        /// 实体属性追加到XmlDocument
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        /// <param name="obj"></param>
        private static void EntityToXml(XmlDocument doc, XmlElement root, T obj)
        {
            XmlElement xmlItem = doc.CreateElement(typeof(T).Name);

            PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            XmlNode xmlNode = null;
            foreach (PropertyInfo pinfo in propertyInfo)
            {
                if (pinfo != null)
                {
                    xmlNode = doc.CreateElement(pinfo.Name);
                    string value = string.Empty;
                    if (pinfo.GetValue(obj, null) != null)
                        value = pinfo.GetValue(obj, null).ToString();
                    xmlNode.InnerText = value;
                    xmlItem.AppendChild(xmlNode);
                }
            }
            root.AppendChild(xmlItem);
        }
        /// <summary>
        /// XML转换为实体
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T XmlToEntity(string str)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
            }
            catch (XmlException)
            {
                return default(T);
            }

            if (doc.ChildNodes.Count > 0)
            {
                XmlNode node = doc.ChildNodes[0];
                if (node.Name != typeof(T).Name)
                    return default(T);
                return XmlNodeToEntity(node);
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// XML转换为实体集合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<T> XmlToEntityList(string str)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
            }
            catch (XmlException)
            {
                return null;
            }
            if (doc.ChildNodes.Count != 1)
                return null;
            if (doc.ChildNodes[0].Name.ToLower() != typeof(T).Name.ToLower() + "s")
                return null;

            XmlNode node = doc.ChildNodes[0];

            IList<T> items = new List<T>();

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name.ToLower() == typeof(T).Name.ToLower())
                    items.Add(XmlNodeToEntity(child));
            }

            return items;
        }
        /// <summary>
        /// XmlNode转换为实体
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static T XmlNodeToEntity(XmlNode node)
        {
            T item = new T();

            if (node.NodeType == XmlNodeType.Element)
            {
                XmlElement element = (XmlElement)node;

                PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (XmlAttribute attr in element.Attributes)
                {
                    string attrName = attr.Name.ToLower();
                    string attrValue = attr.Value.ToString();
                    foreach (PropertyInfo pinfo in propertyInfo)
                    {
                        if (pinfo != null)
                        {
                            string name = pinfo.Name.ToLower();
                            Type dbType = pinfo.PropertyType;
                            if (name == attrName)
                            {
                                if (string.IsNullOrEmpty(attrValue))
                                    continue;
                                switch (dbType.ToString())
                                {
                                    case "System.Int32":
                                        pinfo.SetValue(item, System.Convert.ToInt32(attrValue), null);
                                        break;
                                    case "System.Boolean":
                                        pinfo.SetValue(item, System.Convert.ToBoolean(attrValue), null);
                                        break;
                                    case "System.DateTime":
                                        pinfo.SetValue(item, System.Convert.ToDateTime(attrValue), null);
                                        break;
                                    case "System.Decimal":
                                        pinfo.SetValue(item, System.Convert.ToDecimal(attrValue), null);
                                        break;
                                    case "System.Double":
                                        pinfo.SetValue(item, System.Convert.ToDouble(attrValue), null);
                                        break;
                                    default:
                                        pinfo.SetValue(item, attrValue, null);
                                        break;
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            return item;

        }

    }
}