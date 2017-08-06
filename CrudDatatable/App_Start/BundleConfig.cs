using System.Web;
using System.Web.Optimization;

namespace CrudDatatable
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

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                       "~/Scripts/bootstrap-datepicker.js",
                       "~/Scripts/toastr.js",
                       "~/Scripts/bootbox.js",
                       "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       "~/Content/toastr.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/bootstrap-datepicker3.css",
                       "~/Content/DataTables/css/dataTables.bootstrap.css",
                       "~/Content/DataTables/css/buttons.dataTables.css",
                       "~/Content/DataTables/css/buttons.bootstrap.css",
                       "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                       "~/Scripts/DataTables/jquery.dataTables.js",
                       "~/Scripts/DataTables/dataTables.jqueryui.js",
                       "~/Scripts/DataTables/dataTables.bootstrap.js",
                       "~/Scripts/DataTables/dataTables.buttons.js",
                       "~/Scripts/jszip.js",
                       "~/Scripts/DataTables/buttons.pdfmake.js",
                       "~/Scripts/DataTables/buttons.html5.js",
                       "~/Scripts/DataTables/buttons.flash.js",
                       "~/Scripts/DataTables/buttons.print.js",
                       "~/Scripts/MyScripts/jquery.dataTables.columnFilter.js"));
        }
    }
}
