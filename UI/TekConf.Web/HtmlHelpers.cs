using System.Web;
using System.Web.Mvc;

namespace TekConf.Web
{
	public static class HtmlHelpers
	{
		public static string SocialIcon(this HtmlHelper helper, string provider)
		{
			if (provider == "google")
				return "fa-google-plus";

			if (provider == "microsoft")
				return "fa-windows";

			if (provider == "yahoo")
				return "icon-yahoo";

			return "fa-" + provider;
		}

		public static bool IsAdmin(this HtmlHelper helper)
		{
			if (HttpContext.Current.Request.IsLocal 
					|| HttpContext.Current.User.Identity.Name == "robgibbens@gmail.com" 
					|| HttpContext.Current.User.Identity.Name.ToLower() == "robgibbens"
					|| HttpContext.Current.User.Identity.Name.ToLower() == "qconf" 
					|| (
							HttpContext.Current.Session["admin"] != null 
							&& HttpContext.Current.Session["admin"].ToString() == "true"
							) 
					|| (
							HttpContext.Current.Request.QueryString["admin"] != null 
							&& HttpContext.Current.Request.QueryString["admin"] == "true"
						)
				)
			{
				HttpContext.Current.Session["admin"] = "true";
				return true;
			}
			

			return false;
		}
	}
}