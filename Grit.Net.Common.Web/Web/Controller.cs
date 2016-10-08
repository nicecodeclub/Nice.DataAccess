using Grit.Net.Common.Config;
using System.Web;
using System.Web.SessionState;

namespace Grit.Net.Common.Web
{
    /// <summary>
    /// Controller 控制器
    /// </summary>
    public class Controller : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpActionWraper.ExecuteAction(this, context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}