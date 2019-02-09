using System.Web.Optimization;

namespace GamersSpace {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {
            // CSS Bundle
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/Style/normalize.css",
                "~/Style/skeleton.css",
                "~/Style/site.css"));
        }

    }

}