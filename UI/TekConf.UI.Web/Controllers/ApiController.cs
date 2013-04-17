using System.Web.Mvc;
using TekConf.UI.Web.App_Start;

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
