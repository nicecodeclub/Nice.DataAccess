using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Web;


namespace Grit.Net.Common.Web
{
    public class HttpContextHelper<T> where T : new()
    {
        /// <summary>
        /// 请求参数转换成Model
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T RequestConvertModel(HttpContext context)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            T obj = new T();
            foreach (PropertyInfo pi in properties)
            {
                string result = context.Request.Params[pi.Name];
                if (!string.IsNullOrEmpty(result))
                {
                    pi.SetValue(obj, System.Convert.ChangeType(result, pi.PropertyType), null);
                }
            }
            return obj;
        }

        /// <summary>
        /// 打印输出JSON
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public static void ResponseJson(HttpContext context, T result)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }
    }

    public class HttpContextHelper
    {
        /// <summary>
        /// 打印输出JSON
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public static void ResponseJson(HttpContext context, object result)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }
        /// <summary>
        /// 返回成功请求
        /// </summary>
        /// <param name="context"></param>
        public static void ResponseSuccess(HttpContext context, string describe = null)
        {
            HttpReplyResult result = new HttpReplyResult();
            result.result = true;
            if (describe == null)
                result.describe = "请求成功";
            else
                result.describe = describe;
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }
        /// <summary>
        /// 返回无效的Action参数请求结果
        /// </summary>
        /// <param name="context"></param>
        public static void ResponseUnavailableAction(HttpContext context)
        {
            HttpReplyResult result = new HttpReplyResult();
            result.describe = "无效的Action参数 ,Action parameter is invalid !!!";
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        /// <summary>
        /// 返回无效的Action参数请求结果
        /// </summary>
        /// <param name="context"></param>
        public static void ResponseRedirect(HttpContext context, string url)
        {
            HttpReplyResult result = new HttpReplyResult();
            result.result = true;
            result.redirect = true;
            result.redirectURL = url;
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        /// <summary>
        /// 请求参数转换成Model
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object RequestConvertModel(Type type, HttpContext context)
        {
            PropertyInfo[] properties = type.GetProperties();
            object obj = Activator.CreateInstance(type);
            foreach (PropertyInfo pi in properties)
            {
                string result = context.Request.Params[pi.Name];
                if (!string.IsNullOrEmpty(result))
                {
                    pi.SetValue(obj, System.Convert.ChangeType(result, pi.PropertyType), null);
                }
            }
            return obj;
        }
    }
}
