using System.Web;
using System.Web.Optimization;

namespace TekConf.UI.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/common")
                .Include(
                            "~/js/jquery-1.8.2.min.js",
                            "~/js/bootstrap.js"
                        )
            );

            bundles.Add(new ScriptBundle("~/js/commonLightbox")
                .Include(
                            "~/js/jquery-1.8.2.min.js",
                            "~/js/bootstrap.js",
                            "~/js/lightbox/jquery.lightbox.min.js"
                        )
            );

            bundles.Add(new ScriptBundle("~/scripts/conferences/detail")
                .Include(
                            "~/Scripts/jquery-1.8.2.js",
                            "~/Scripts/jquery-ui-1.9.0.js",
                            "~/Scripts/jquery-ui-sliderAccess.js",
                            "~/Scripts/jquery-ui-timepicker-addon.js"
                        )
            );
            bundles.Add(new ScriptBundle("~/js/commonFaq")
                .Include(
                            "~/js/jquery-1.8.2.min.js",
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

            bundles.Add(new StyleBundle("~/css/conferences/index")
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