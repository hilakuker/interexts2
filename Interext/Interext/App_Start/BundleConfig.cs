using Interext.App_Start;
using System.Web;
using System.Web.Optimization;

namespace Interext
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new ScriptBundle("~/bundles/google").Include(
  "~/Scripts/jquery-{version}.js",
  "~/Scripts/jquery-ui-{version}.js",
  "~/Scripts/jquery-{version}.js",
  "~/Scripts/jquery.unobtrusive*",
  "~/Scripts/googlemaps.js",
  "~/Scripts/bootstrap.js",
  "~/Scripts/respond.js",
  "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-{version}.js",
                      "~/Scripts/jquery-ui-timepicker-addon"));

            bundles.Add(new ScriptBundle("~/bundles/contrastJs").Include(
                      "~/Scripts/contrast/contrast-global-plugins.js",
                      "~/Scripts/contrast/contrast.js",
                      "~/Scripts/contrast/megafolio/js/megafolio-init.js",
                      "~/Scripts/hoverIntent.js",
                      "~/Scripts/main.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/eventPageJs").Include(
                        "~/Scripts/event-create.js"));

            bundles.Add(new ScriptBundle("~/bundles/timePicker").Include( 
                        "~/Scripts/tinybox.js",
                        "~/Scripts/timepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/colorPickerJs").Include(
                        "~/Scripts/colorpicker/colorpicker.js",
                        "~/Scripts/colorpicker/layout.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/shared-css").Include(
                "~/Content/css/shared/main.css",
                        "~/Content/css/shared/flipping-text.css",
                       "~/Content/css/shared/font-awesome.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/site-css").Include(
                       "~/Content/css/site/main.css",
                        "~/Content/css/site/events-wall.css",
                         "~/Content/css/site/file-uploader-button.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/jquery-ui-style").Include(
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery-ui-timepicker-addon.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/eventsPageCss").Include(
                "~/Content/contrast/eventspage.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/profilePageCss").Include(
                "~/Content/profilepage.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/eventPageCss").Include(
                "~/Content/css/site/eventpage-template.css",
                "~/Content/css/site/eventpage-more.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/signinPageCss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-signin.css",
                "~/Content/zocial/zocial.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/colorPickerCss").Include(
                            "~/Content/css/site/colorpicker.css"
                                 ));
        }
    }
}
