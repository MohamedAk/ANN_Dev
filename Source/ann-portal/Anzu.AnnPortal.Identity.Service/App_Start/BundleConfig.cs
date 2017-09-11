using System.Web;
using System.Web.Optimization;

namespace Anzu.AnnPortal.Identity.Service
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                     "~/Content/vendors/jquery/dist/jquery.js",
                     "~/Content/vendors/lodash/lodash.min.js",
                     "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                     "~/Content/vendors/angular/angular.js",
                     "~/Content/vendors/angular-aria/angular-aria.js",
                     "~/Content/vendors/angular-material/angular-material.js",
                     "~/Content/vendors/angular-resource/angular-resource.js",
                     "~/Content/vendors/angular-cookies/angular-cookies.js",
                     "~/Content/vendors/angular-sanitize/angular-sanitize.js",
                     "~/Content/vendors/angular-mocks/angular-mocks.js",
                     "~/Content/vendors/angular-ui-router/release/angular-ui-router.js",
                     "~/Content/vendors/angular-animate/angular-animate.js",
                     "~/Content/vendors/angular-ui-router.stateHelper/statehelper.js",
                     "~/Content/vendors/ng-sticky-element/dist/ng-sticky-element.min.js",
                     "~/Content/vendors/toastr/toaster.js",
                     "~/Content/vendors/kendo/js/kendo.web.min.js",
                     "~/Content/vendors/bootstrap/dist/js/ui-bootstrap-tpls-1.3.1.min.js",
                     "~/Content/vendors/custom-scrollbar/jquery.mCustomScrollbar.js"));

            bundles.Add(new StyleBundle("~/Content/vendor").Include(
                 "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                 "~/Content/vendors/angular-material/angular-material.css",
                 "~/Content/vendors/toastr/toaster.css",
                 "~/Content/vendors/kendo/styles/kendo.common-material.min.css",
                 "~/Content/vendors/kendo/styles/kendo.material.min.css",
                 "~/Content/vendors/ng-sticky-element/dist/ng-sticky-element.min.css",
                 "~/Content/vendors/custom-scrollbar/jquery.mCustomScrollbar.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/app.css"));
        }
    }
}
