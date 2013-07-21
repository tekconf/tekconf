using System.Web.Mvc;

namespace TekConf.UI.SL.Web.Controllers
{
  [HandleError]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewData["Message"] = "Welcome to ASP.NET MVC!";

      return View();
    }

    public ActionResult About()
    {
      return View();
    }
  }
}
