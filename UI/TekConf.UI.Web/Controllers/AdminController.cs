using System.Web.Mvc;

namespace TekConf.UI.Web.Controllers
{
    public class AdminController : Controller
    {
        private RemoteDataRepositoryAsync _repository;
        public AdminController()
        {
            _repository = new RemoteDataRepositoryAsync();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}