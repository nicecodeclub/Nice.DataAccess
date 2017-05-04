using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.Extensions
{
    public class JsonConvertWraper
    {
        private static IsoDateTimeConverter timeConverter = null;

        private static IsoDateTimeConverter GetTimeConverter()
        {
            if (timeConverter == null)
            {
                timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            }
            return timeConverter;
        }
        /// <summary>
        /// JSON序列化对象，时间格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, GetTimeConverter());
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
