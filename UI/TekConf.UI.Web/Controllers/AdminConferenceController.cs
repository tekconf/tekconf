using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TekConf.Azure;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
	public class AdminConferenceController : AsyncController
	{
		private RemoteDataRepositoryAsync _repository;
		public AdminConferenceController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		#region Add Conference

		[HttpGet]
		[CompressFilter]
		public ActionResult CreateConference()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateConference(CreateConference conference, HttpPostedFileBase file)
		{
			string url = string.Empty;

			if (file != null)
			{
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				IImageSaver imageSaver = new AzureImageSaver(configuration);
				url = imageSaver.SaveImage(conference.name.GenerateSlug() + Path.GetExtension(file.FileName), file);
				conference.imageUrl = url;
			}

			var conferenceTask = _repository.CreateConference(conference);

			await Task.WhenAll(conferenceTask);

			return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });

		}


		#endregion

		#region Edit Conference

		[HttpGet]
		public void EditConferenceAsync(string conferenceSlug)
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);

			AsyncManager.OutstandingOperations.Increment();
			repository.GetFullConference(conferenceSlug, conference =>
			{
				AsyncManager.Parameters["conference"] = conference;
				AsyncManager.OutstandingOperations.Decrement();
			});
		}

		public ActionResult EditConferenceCompleted(FullConferenceDto conference)
		{
			var createConference = Mapper.Map<CreateConference>(conference);
			return View(createConference);
		}

		[HttpPost]
		public void EditConfAsync(CreateConference conference, HttpPostedFileBase file)
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);

			if (file != null)
			{
				AsyncManager.OutstandingOperations.Increment(2);
			}
			else
			{
				AsyncManager.OutstandingOperations.Increment(1);
			}

			if (file != null)
			{
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				var imageName = conference.name.GenerateSlug() + Path.GetExtension(file.FileName);
				conference.imageUrl = configuration.ImageUrl + imageName;

				ThreadPool.QueueUserWorkItem(o =>
								{
									IImageSaver imageSaver = new AzureImageSaver(configuration);
									imageSaver.SaveImage(imageName, file);
									AsyncManager.OutstandingOperations.Decrement();
								}, null);
			}

			repository.EditConference(conference, "user", "password", c =>
			{
				AsyncManager.Parameters["conference"] = c;
				AsyncManager.OutstandingOperations.Decrement();
			});

		}

		public string FullyQualifiedApplicationPath
		{
			get
			{
				//Return variable declaration
				var appPath = string.Empty;

				//Getting the current context of HTTP request

				//Checking the current context content
				//Formatting the fully qualified website url/name
				appPath = string.Format("{0}://{1}{2}{3}",
																HttpContext.Request.Url.Scheme,
																HttpContext.Request.Url.Host,
																HttpContext.Request.Url.Port == 80
																		? string.Empty
																		: ":" + HttpContext.Request.Url.Port,
																HttpContext.Request.ApplicationPath);

				if (!appPath.EndsWith("/"))
					appPath += "/";

				return appPath;
			}
		}

		public ActionResult EditConfCompleted(FullConferenceDto conference)
		{
			return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });
		}

		#endregion

		public void EditConferencesIndexAsync(string sortBy, bool? showPastConferences, string search)
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);

			AsyncManager.OutstandingOperations.Increment();

			repository.GetConferences(sortBy: sortBy, showPastConferences: showPastConferences, search: search, callback: conferences =>
			{
				AsyncManager.Parameters["conferences"] = conferences;
				AsyncManager.OutstandingOperations.Decrement();
			});
		}

		public ActionResult EditConferencesIndexCompleted(List<ConferencesDto> conferences)
		{
			return View(conferences.OrderBy(c => c.name).ToList());
		}
	}
}