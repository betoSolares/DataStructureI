using System.Web.Optimization;

namespace Album {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {
            // CSS bundle
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                                        "~/Styles/bulma.min.css"));
        }

    }

}