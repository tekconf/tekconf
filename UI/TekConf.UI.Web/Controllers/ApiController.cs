using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.App_Start;
using TekConf.UI.Web.Models;

namespace TekConf.UI.Web.Controllers
{
	public class ApiController : Controller
	{
		[CompressFilter]
		public ActionResult Index()
		{
			return View();
		}
	}
}
