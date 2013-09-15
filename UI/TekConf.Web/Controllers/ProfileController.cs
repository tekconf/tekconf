using System.Threading.Tasks;
using System.Web.Mvc;

namespace TekConf.Web.Controllers
{
	public class ProfileController : Controller
	{
		public async Task<ActionResult> Index()
		{
			return View();
		}
	}
}