using System.Web.Optimization;

namespace Health {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {
            // CSS Bundle
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/Styles/normalize.css",
                "~/Styles/skeleton.css",
                "~/Styles/site.css"));
            // JS Bundle
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/jquery-min.js",
                "~/Scripts/site.js",
                "~/Scripts/style.js"));
        }
        
    }

}
