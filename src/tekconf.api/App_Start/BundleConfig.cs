using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TekConf.Api.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.seed.min.js",
                "~/scripts/bootstrap/bootstrap.min.js",
                "~/scripts/app.seed.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}