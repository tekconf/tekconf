using System.Threading.Tasks;
using System.Web.Mvc;

namespace TekConf.UI.Web.Controllers
{
	public class ProfileController : Controller
	{
		public async Task<ActionResult> Index()
		{
			return View();
		}
	}
}