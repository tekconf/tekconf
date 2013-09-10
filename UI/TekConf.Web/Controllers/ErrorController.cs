using System.Web.Mvc;

namespace TekConf.Web.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult NotFound()
		{
			return View();
		}
	}
}