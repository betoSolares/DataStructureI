using System.Web.Optimization;

namespace FarmaPlus {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {
            // CSS bundle
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/Style/normalize.css",
                "~/Style/skeleton.css",
                "~/Style/site.css"));
        }

    }

}