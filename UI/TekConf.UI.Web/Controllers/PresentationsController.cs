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

		public ActionResult Detail(string slug)
		{
			return View();
		}

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Add(AddPresentationViewModel addPresentationViewModel)
		{
			return View("AddHistory");
		}


		public ActionResult AddHistory(string slug)
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> AddHistory(AddPresentationHistoryViewModel addPresentationHistoryViewModel)
		{
			return RedirectToRoute("PresentationDetail", new { slug = "test"});
		}



	}
}
