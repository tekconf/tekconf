using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class PresentationsController : Controller
	{
		private RemoteDataRepositoryAsync _repository;
		public PresentationsController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

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
			var presentation = new CreatePresentation()
				{
					UserName = System.Web.HttpContext.Current.User.Identity.Name,
					SpeakerSlug = System.Web.HttpContext.Current.User.Identity.Name,
					Description = addPresentationViewModel.Description,
					Tags = addPresentationViewModel.Tags,
					Title = addPresentationViewModel.Title
				};
			var presentationTask = _repository.CreatePresentation(presentation);

			await Task.WhenAll(presentationTask);

			return RedirectToAction("AddHistory", new { presentationSlug = presentation.Slug });
		}


		public ActionResult AddHistory(string slug)
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> AddHistory(AddPresentationHistoryViewModel addPresentationHistoryViewModel)
		{
			return RedirectToRoute("PresentationDetail", new { slug = "test" });
		}



	}
}
