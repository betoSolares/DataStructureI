using System.Web.Mvc;
using System.Web.Routing;

namespace Health {

    public class RouteConfig {

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "LoadInventory",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Inventory", action = "LoadInventory", id = UrlParameter.Optional });
        }

    }

}
