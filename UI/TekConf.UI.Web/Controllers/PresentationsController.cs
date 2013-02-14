using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
    public class PresentationsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

				[HttpPost]
				public async Task<ActionResult> Add(AddPresentationViewModel addPresentationViewModel)
				{
					return View("History");
				}

				public ActionResult Add()
				{
					return View();
				}
    }
}
