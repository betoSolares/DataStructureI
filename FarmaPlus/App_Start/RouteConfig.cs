﻿using System.Web.Mvc;
using System.Web.Routing;

namespace FarmaPlus {

    public class RouteConfig {

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "EntryPoint",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Employees", action = "Employees", id = UrlParameter.Optional }
            );
        }

    }

}
