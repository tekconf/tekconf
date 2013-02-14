using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TekConf.UI.Web.ViewModels;

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
