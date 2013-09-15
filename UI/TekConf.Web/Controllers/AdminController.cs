using System.Web.Mvc;

namespace TekConf.Web.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}