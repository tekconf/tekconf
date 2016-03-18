using System.Web;
using System.Web.Optimization;

namespace TekConf.Web
{
    public class BundleConfigOld
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;
            BundleTable.EnableOptimizations = false;


            var jquery = new ScriptBundle("~/bundles/jquery", "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js")
                                        .Include("~/Scripts/jquery-{version}.js");
            jquery.CdnFallbackExpression = "window.jquery";
            bundles.Add(jquery);

            var jqueryUI = new ScriptBundle("~/bundles/jqueryUI", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js")
                                        .Include("~/Scripts/jquery-ui-{version}.js");
            jqueryUI.CdnFallbackExpression = "window.jqueryUI";
            bundles.Add(jqueryUI);

            var jqueryMigrate = new ScriptBundle("~/bundles/jqueryMigrate", "http://code.jquery.com/jquery-migrate-1.1.1.min.js")
                            .Include("~/Scripts/jquery-migrate-{version}.js");
            jqueryMigrate.CdnFallbackExpression = "window.jqueryMigrate";
            bundles.Add(jqueryMigrate);


            bundles.Add(new ScriptBundle("~/js/commonLightbox")
                    .Include(
                //"~/js/lightbox/jquery.lightbox.min.js"
                                    )
            );

            bundles.Add(new ScriptBundle("~/scripts/conferences/detail")
                    .Include(
                //"~/js/typeahead.js"
                                    )
            );

            bundles.Add(new ScriptBundle("~/js/common")
                    .Include(
                                            "~/js/bootstrap.js",
                                            "~/Scripts/jquery.cookie.js",
                                            "~/Scripts/jquery-ui-timepicker-addon.js",
                                            "~/Scripts/jquery-ui-sliderAccess.js",
                                            "~/Scripts/common.js"
                                    )
            );
            bundles.Add(new ScriptBundle("~/js/commonFaq")
                    .Include(
                                            "~/js/bootstrap.js",
                                            "~/js/faq.js"
                                    )
            );


            bundles.Add(new StyleBundle("~/css/home/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/homepage.css"
                                    )
                    );

            bundles.Add(new ScriptBundle("~/js/conferences/index")
                .Include(
                //"~/js/typeahead.js"
                ));
            bundles.Add(new StyleBundle("~/css/conferences/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/features.css",
                                            "~/Content/themes/base/jquery-ui.css",
                                            "~/Content/themes/base/jquery.ui.autocomplete.css"
                //"~/css/typeahead.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/conferences/detail")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/about.css",
                                            "~/Content/themes/base/jquery-ui.css",
                                            "~/css/jquery-ui-timepicker-addon.css"
                //"~/css/typeahead.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/admin/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/plans.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/api/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/faq.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/apps/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/features.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/schedule/index")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/faq.css"
                                    )
                    );

            bundles.Add(new StyleBundle("~/css/session/detail")
                    .Include(
                                            "~/css/bootstrap.css",
                                            "~/css/bootstrap-responsive.css",
                                            "~/css/font-awesome.css",
                                            "~/css/reboot-landing.css",
                                            "~/css/reboot-landing-responsive.css",
                                            "~/css/themes/green/theme.css",
                                            "~/css/pages/about.css"
                                    )
                    );
        }
    }
}