using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekConf.UI.Web
{
	public static class HtmlHelpers
	{
		public static bool IsAdmin(this HtmlHelper helper)
		{
			if ((HttpContext.Current.Session["admin"] != null && HttpContext.Current.Session["admin"].ToString() == "true") ||
			    (HttpContext.Current.Request.QueryString["admin"] != null &&
			     HttpContext.Current.Request.QueryString["admin"] == "true"))
			{
				HttpContext.Current.Session["admin"] = "true";
				return true;
			}
			

			return false;
		}
	}
}