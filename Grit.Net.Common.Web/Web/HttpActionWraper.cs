using Grit.Net.Common.Attributes.Web;
using Grit.Net.Common.Factory;
using Grit.Net.Common.TypeEx;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Grit.Net.Common.Web
{
    public class HttpActionWraper
    {
        private readonly static Mapping<string, MethodInfo> methodMapping = new Mapping<string, MethodInfo>(64);
        public static void SetAction(Type t, IHttpHandler httpHandler)
        {
            MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo method in methods)
            {
                ActionAttribute attr = method.GetCustomAttribute<ActionAttribute>();
                if (attr != null)
                {
                    string actionName = t.Name + "_" + method.Name;
                    if (!string.IsNullOrEmpty(attr.Name))
                        actionName = t.Name + "_" + attr.Name;
                    methodMapping.Add(actionName, method);
                }
            }
        }

        public static void ExecuteAction(IHttpHandler httpHandler, HttpContext context)
        {
            string path = context.Request.Path;
            int index = path.LastIndexOf('/');
            if (index > 0)
            {
                string actionName = path.Substring(index + 1);
                if (!string.IsNullOrEmpty(actionName))
                {
                    MethodInfo methodInfo = methodMapping[httpHandler.GetType().Name + "_" + actionName];
                    if (methodInfo != null)
                    {
                        ParameterInfo[] pi = methodInfo.GetParameters();
                        object[] parms = null;
                        if (pi.Length > 0)
                            GetParameters(pi, context, ref parms);

                        FastMethodHandler fastInvoker = FastMethodFactory.GetMethodInvoker(methodInfo);
                        object obj = fastInvoker(httpHandler, parms);
                        HttpContextHelper.ResponseJson(context, obj);
                    }
                }
            }
            HttpContextHelper.ResponseUnavailableAction(context);
        }

        private static void GetParameters(ParameterInfo[] parmInfos, HttpContext context, ref  object[] parms)
        {
            Type t = null;
            ParameterInfo pi = null;
            parms = new object[parmInfos.Length];
            for (int i = 0; i < parmInfos.Length; i++)
            {
                pi = parmInfos[i];
                string pvalue = context.Request[pi.Name];
                t = pi.ParameterType;
                if (t.Equals(typeof(string)) || (!t.IsInterface && !t.IsClass))//如果它是值类型,或者String   
                {
                    if (string.IsNullOrWhiteSpace(pvalue))
                        parms[i] = t.IsValueType ? Activator.CreateInstance(t) : null;  
                    else
                        parms[i] = System.Convert.ChangeType(pvalue, t);
                }
                else if (t.Equals(typeof(HttpContext)))
                {
                    parms[i] = context;
                }
                else if (t.IsClass)
                {
                    parms[i] = HttpContextHelper.RequestConvertModel(t, context);
                }
            }
        }
    }
}