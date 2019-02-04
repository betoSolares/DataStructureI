using System.Web.Optimization;

namespace GamersSpace {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {
            // JavaScript Bundle
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/functions.js"));
        }

    }

}