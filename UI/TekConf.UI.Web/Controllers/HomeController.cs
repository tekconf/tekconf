using System.Web.Mvc;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
  public class HomeController : Controller
  {
      [CompressFilter]
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

  }
}
