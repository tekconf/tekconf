using System.Web.Mvc;
using TekConf.Web.App_Start;

namespace TekConf.Web.Controllers
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
