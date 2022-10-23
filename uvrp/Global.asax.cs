using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using uvrp.Controllers;

namespace uvrp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

//        protected void Application_Error(object sender, EventArgs e)
//        {
//            var exception = Server.GetLastError();
//
//            // Process 404 HTTP errors
//            var httpException = exception as HttpException;
//            if (httpException != null && httpException.GetHttpCode() == 404)
//            {
//                Response.Clear();
//                Server.ClearError();
//                Response.TrySkipIisCustomErrors = true;
//
//                IController controller = new HomeController();
//
//                var routeData = new RouteData();
//                routeData.Values.Add("controller", "Home");
//                routeData.Values.Add("action", "PageNotFound");
//
//                var requestContext = new RequestContext(
//                    new HttpContextWrapper(Context), routeData);
//                controller.Execute(requestContext);
//            }
//        }
//        
        
    }
}
