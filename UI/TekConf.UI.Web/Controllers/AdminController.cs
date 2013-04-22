using System.Web.Mvc;

namespace TekConf.UI.Web.Controllers
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