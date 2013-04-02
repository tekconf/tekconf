using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Elmah;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class PresentationsController : Controller
	{
		private readonly IRemoteDataRepositoryAsync _repository;

		public PresentationsController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}
		public PresentationsController(IRemoteDataRepositoryAsync repository)
		{
			_repository = repository;
			//var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			//_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> Detail(string slug)
		{
			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
				userName = System.Web.HttpContext.Current.User.Identity.Name;
			}

			var presentationTask = _repository.GetPresentation(slug, userName);

			await presentationTask;

			if (presentationTask.Result == null)
			{
				Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(new Exception("Presentation " + slug + " not found")));
				return RedirectToAction("NotFound", "Error");
			}

			return View(presentationTask.Result);
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
					UserName = this.ControllerContext.HttpContext.User.Identity.Name,
					SpeakerSlug = this.ControllerContext.HttpContext.User.Identity.Name,
					Description = addPresentationViewModel.Description,
					Tags = addPresentationViewModel.Tags,
					Title = addPresentationViewModel.Title
				};
			var presentationTask = _repository.CreatePresentation(presentation);

			await Task.WhenAll(presentationTask);

			return RedirectToAction("AddHistory", new { speakerSlug = presentation.SpeakerSlug, presentationSlug = presentation.Slug });
		}


		public ActionResult AddHistory(string speakerSlug, string presentationSlug)
		{
			var model = new AddPresentationHistoryViewModel() {SpeakerSlug = speakerSlug, PresentationSlug = presentationSlug};
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> AddHistory(AddPresentationHistoryViewModel addPresentationHistoryViewModel)
		{

			var history = new CreatePresentationHistory()
			{
				UserName = this.ControllerContext.HttpContext.User.Identity.Name,
				SpeakerSlug = addPresentationHistoryViewModel.SpeakerSlug,
				PresentationSlug = addPresentationHistoryViewModel.PresentationSlug,
				ConferenceName = addPresentationHistoryViewModel.ConferenceName,
				Links = addPresentationHistoryViewModel.Links,
				Notes = addPresentationHistoryViewModel.Notes
			};
			var presentationTask = _repository.CreatePresentationHistory(history);

			await Task.WhenAll(presentationTask);

			var dto = presentationTask.Result;

			return RedirectToRoute("PresentationDetail", new { slug = dto.Slug });
		}



	}
}
