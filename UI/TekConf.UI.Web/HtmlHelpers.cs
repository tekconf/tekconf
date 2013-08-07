using System.Web;
using System.Web.Mvc;

namespace TekConf.UI.Web
{
	public static class HtmlHelpers
	{
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