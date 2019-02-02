using System.Web;
using System.Web.Optimization;

namespace BWYSDPWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery-2.1.3.min.js",
                      "~/Scripts/lib/jquery.mousewheel.min.js",
                      "~/Scripts/lib/jquery.cookie.min.js",
                      "~/Scripts/lib/fastclick.min.js",
                      "~/Scripts/lib/bootstrap.min.js",
                      "~/Scripts/lib/clearmin.min.js",
                      "~/Scripts/lib/home.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/site.css",
                      "~/Content/bootstrap-clearmin.min.css",
                      "~/Content/roboto.css",
                      "~/Content/material-design.css",
                      "~/Content/small-n-flat.css",
                      "~/Content/font-awesome.min.css"
                      ));
        }
    }
}
