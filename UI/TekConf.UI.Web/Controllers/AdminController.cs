using System.Web.Mvc;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private IRemoteDataRepositoryAsync _remoteDataRepositoryAsync;
		public AdminController(IRemoteDataRepositoryAsync remoteDataRepositoryAsync)
		{
			_remoteDataRepositoryAsync = remoteDataRepositoryAsync;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}