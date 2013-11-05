using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Elmah;
using TekConf.Azure;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.Web.ViewModels;

namespace TekConf.Web.Controllers
{
	[Authorize]
	public class PresentationsController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;

		public PresentationsController(IRemoteDataRepository remoteDataRepository)
		{
			_remoteDataRepository = remoteDataRepository;
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

			var presentation = await _remoteDataRepository.GetPresentation(slug, userName);

			if (presentation == null)
			{
				ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(new Exception("Presentation " + slug + " not found")));
				return RedirectToAction("NotFound", "Error");
			}

			return View(presentation);
		}

		public ActionResult Add()
		{
			var difficulties = new List<string>() { "Beginner", "Intermediate", "Expert" };
			var difficultiesList = new SelectList(difficulties.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.DifficultiesList = difficultiesList;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Add(AddPresentationViewModel addPresentationViewModel, HttpPostedFileBase file)
		{
			var presentation = new CreatePresentation()
			{
				UserName = this.ControllerContext.HttpContext.User.Identity.Name,
				SpeakerSlug = this.ControllerContext.HttpContext.User.Identity.Name,
				Description = addPresentationViewModel.Description,
				Title = addPresentationViewModel.Title,
				Difficulty = addPresentationViewModel.Difficulty,
				Length = addPresentationViewModel.Length,
				Videos = addPresentationViewModel.Videos.Where(x => !string.IsNullOrWhiteSpace(x)).ToList()
			};

			if (file != null)
			{
				IImageSaver imageSaver = null;

#if DEBUG
				//TODO, Move this to configuration
				imageSaver = new FileSystemImageSaver();
#else
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				imageSaver = new AzureImageSaver(configuration);
#endif

				presentation.Slug = presentation.Title.GenerateSlug();
				presentation.imageUrl = imageSaver.SaveImage(presentation.Slug + Path.GetExtension(file.FileName), file);
			}

			if (Request.Form["tags"] != null)
				presentation.Tags = Request.Form["tags"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			if (Request.Form["subjects"] != null)
				presentation.Subjects = Request.Form["subjects"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


			var response = await _remoteDataRepository.CreatePresentation(presentation, presentation.UserName, "password");

			return RedirectToAction("AddHistory", new { speakerSlug = presentation.SpeakerSlug, presentationSlug = presentation.Slug });
		}


		public ActionResult AddHistory(string speakerSlug, string presentationSlug)
		{
			var model = new AddPresentationHistoryViewModel() { SpeakerSlug = speakerSlug, PresentationSlug = presentationSlug };
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
			
			var dto = await _remoteDataRepository.CreatePresentationHistory(history, history.UserName, "password");

			return RedirectToRoute("PresentationDetail", new { slug = dto.Slug });
		}



	}
}
