using System.Web;
using System.Web.Optimization;

namespace uvrp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-captcha.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fullcalender.css",
                      "~/Content/simditor.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                    "~/Scripts/moment.min.js",
                    "~/Scripts/fullcalendar.min.js",
                    "~/Scripts/custom.js"
            ));
            
            bundles.Add(new Bundle("~/bundles/simditor").Include(
                      "~/Scripts/module.js",
                      "~/Scripts/hotkeys.js",
                      "~/Scripts/uploader.js",
                      "~/Scripts/simditor.js"
            ));
            
            bundles.Add(new Bundle("~/bundles/ckeditor").Include(
                    "~/Scripts/ckeditor/ckeditor.js"
            ));


        }
    }
}
