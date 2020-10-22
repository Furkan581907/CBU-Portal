using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CbuPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "KarsilamaEkrani", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Hata",
              url: "hata/{kod}",
              defaults: new { controller = "Error", action = "Page404", kod = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Admin",
                            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
