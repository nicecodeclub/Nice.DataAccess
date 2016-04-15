namespace System.Web.Routing
{
    public class HttpHandlerRoute : IRouteHandler
    {
        private String _virtualPath = null;
        private IHttpHandler _handler = null;

        public HttpHandlerRoute(String virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public HttpHandlerRoute(IHttpHandler handler)
        {
            _handler = handler;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler result;
            if (_handler == null)
            {
                result = (IHttpHandler)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(IHttpHandler));
            }
            else
            {
                result = _handler;
            }
            return result;
        }
    }

    public static class RoutingExtensions
    {
        public static void MapHttpHandlerRoute(this RouteCollection routes, string routeName, string routeUrl, string physicalFile, RouteValueDictionary defaults = null, RouteValueDictionary constraints = null)
        {
            var route = new Route(routeUrl, defaults, constraints, new HttpHandlerRoute(physicalFile));
            RouteTable.Routes.Add(routeName, route);
        }

        public static void MapHttpHandlerRoute(this RouteCollection routes, string routeName, string routeUrl, IHttpHandler handler, RouteValueDictionary defaults = null, RouteValueDictionary constraints = null)
        {
            var route = new Route(routeUrl, defaults, constraints, new HttpHandlerRoute(handler));
            RouteTable.Routes.Add(routeName, route);
        }
    }
}