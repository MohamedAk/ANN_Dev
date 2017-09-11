using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Optimization;

namespace Anzu.AnnPortal.Web.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = false;
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
                 "~/Content/vendors/custom-scrollbar/jquery.mCustomScrollbar.min.css",
                 "~/Content/vendors/font-awesome/css/font-awesome-animation.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/app/app.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/app/app.js",
                "~/app/dashboard/dashboard.js",
                "~/app/common/error/error.js",
                "~/app/settings/settings.js",
                "~/app/admin/admin.js",
                "~/app/services/practice.service.js",
                "~/app/services/breast-implant.service.js",
                "~/app/services/procedure-filter-level.service.js",
                "~/app/admin/practice-management/practice-management.js",
                "~/app/admin/asap-user-management/asap-user-management.js",
                "~/app/admin/procedure-filter-meta-data/procedure-filter-meta-data.js",
                "~/app/admin/procedure-filter-mappings/procedure-filter-mappings.js",
                "~/app/admin/user-management/user-management.js",
                "~/app/admin/user-management/user-service.js",
                "~/app/components/navbar/navbar.js",
                "~/app/components/slide-menu/slide-menu.js",
                  "~/app/services/navbar.service.js"));

#if !DEBUG

            System.Web.Optimization.BundleTable.EnableOptimizations = false;
            if (ConfigurationManager.AppSettings["enableBundling"] == "true")
            {
                System.Web.Optimization.BundleTable.EnableOptimizations = false;
            }
            if (ConfigurationManager.AppSettings["enableMinification"] != "false")
            {
                foreach (var bundle in BundleTable.Bundles)
                {
                    bundle.Transforms.Clear();
                }
            }
                       
#endif
        }
    }
}
