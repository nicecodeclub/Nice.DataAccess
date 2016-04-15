using Grit.Net.Common.Attributes.Web;
using Grit.Net.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace Grit.Net.Common.Web
{
    public class HttpHandlerRouteWraper
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string assemblyName = BaseConfig.WebControllerAssembly;
            string namespaceName = BaseConfig.WebControllerNamespace;
            if (string.IsNullOrEmpty(assemblyName))//|| string.IsNullOrEmpty(namespaceName)
                throw new ConfigNotImplementException();
            RegisterRoutes(routes, assemblyName, namespaceName);
        }
        public static void RegisterRoutes(RouteCollection routes, string assemblyName, string namespaceName)
        {
            IEnumerable<Type> types = null;

            Assembly assembly = Assembly.Load(assemblyName);
            if (string.IsNullOrEmpty(namespaceName))
                types = assembly.GetTypes().Where(t => t.IsClass);
            else
                types = assembly.GetTypes().Where(t => t.IsClass && t.Namespace == namespaceName);
            //string assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));
            ControllerAttribute attr = null;
            foreach (Type t in types)
            {
                attr = t.GetCustomAttribute<ControllerAttribute>();
                if (attr != null)
                {
                    string routeUrl = null;
                    if (string.IsNullOrEmpty(attr.Route))
                        routeUrl = namespaceName.Substring(assemblyName.Length).Replace('.', '/') + "/" + t.Name;
                    else
                        routeUrl = attr.Route;
                    IHttpHandler handler = (IHttpHandler)assembly.CreateInstance(t.FullName);
                    HttpActionWraper.SetAction(t, handler);
                    routes.MapHttpHandlerRoute(t.Name, routeUrl + "/{action}", handler);
                }
            }
        }

    }
}