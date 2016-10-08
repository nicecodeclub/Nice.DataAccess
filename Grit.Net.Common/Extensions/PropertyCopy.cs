using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Grit.Net.Common.Extensions
{
    public class PropertyCopy<T> where T :new()
    {
        public static T Copy<S>(S source)
        {
            PropertyInfo[] sourceproperties = typeof(S).GetProperties();
            PropertyInfo[] targetproperties = typeof(T).GetProperties();
            T t = new T();
            foreach (PropertyInfo pi in sourceproperties)
            {
                foreach (PropertyInfo tp in targetproperties)
                {
                    if (pi.Name == tp.Name)
                    {
                        tp.SetValue(t, pi.GetValue(source));
                        break;
                    }
                }
            }
            return t;
        }

        ///<summary>
        ///引用类型深度拷贝
        ///</summary>
        ///<typeparam name="T">类型，需要序列化</typeparam>
        ///<param name="RealObject">对象</param>
        ///<returns></returns>
        public static T Clone(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }
}
