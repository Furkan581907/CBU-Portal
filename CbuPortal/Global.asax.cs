using CbuPortal.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CbuPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 500)
        //    {
        //        Response.Redirect("Error/PageError.cshtml");
        //    }
        //    else if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 400)
        //    {
        //        Response.Redirect("Error/PageError.cshtml");
        //    }
        //    else
        //    {
        //        Response.Redirect("Error/PageError.cshtml");
        //    }
        //}
    }
}
