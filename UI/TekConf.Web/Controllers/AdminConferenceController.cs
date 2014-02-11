using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TekConf.Azure;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.Web.App_Start;
using TekConf.Web.Models;

namespace TekConf.Web.Controllers
{
	using System;

	//[Authorize]
	public class AdminConferenceController : AsyncController
	{
		private readonly IRemoteDataRepository _remoteDataRepository;
	    private readonly UserManager<ApplicationUser> _userManager;

	    public AdminConferenceController(IRemoteDataRepository remoteDataRepository, UserManager<ApplicationUser> userManager)
		{
		    _remoteDataRepository = remoteDataRepository;
		    _userManager = userManager;
		}

	    #region Add Conference

		[HttpGet]
		[CompressFilter]
		public ActionResult CreateConference()
		{
		    var conference = new CreateConference();
			return View(conference);
		}

		[HttpPost]
        public async Task<ActionResult> CreateConference(CreateConference conference, HttpPostedFileBase rectangularImage, HttpPostedFileBase squareImage)
		{
			if (Request.Form["tags"] != null)
				conference.tags = Request.Form["tags"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			if (Request.Form["sessionTypes"] != null)
				conference.sessionTypes = Request.Form["sessionTypes"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			if (Request.Form["subjects"] != null)
				conference.subjects = Request.Form["subjects"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			if (Request.Form["rooms"] != null)
				conference.rooms = Request.Form["rooms"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			string url = string.Empty;

			if (rectangularImage != null)
			{
				IImageSaver imageSaver = null;

#if DEBUG
				//TODO, Move this to configuration
				imageSaver = new FileSystemImageSaver();
#else
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				imageSaver = new AzureImageSaver(configuration);
#endif

				url = imageSaver.SaveImage(conference.name.GenerateSlug() + Path.GetExtension(rectangularImage.FileName), rectangularImage);
				conference.imageUrl = url;
			}

            if (squareImage != null)
            {
                IImageSaver imageSaver = null;

#if DEBUG
                //TODO, Move this to configuration
                imageSaver = new FileSystemImageSaver();
#else
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				imageSaver = new AzureImageSaver(configuration);
#endif

                url = imageSaver.SaveImage(conference.name.GenerateSlug() + "-square" + Path.GetExtension(squareImage.FileName), squareImage);
                conference.imageUrlSquare = url;
            }

			var fullConferenceDto = await _remoteDataRepository.CreateConference(conference, "user", "password");
		    var userId = User.Identity.GetUserId();
		    await _userManager.AddToRoleAsync(userId, "ConferenceCreator");

			return RedirectToAction("Detail", "Conferences", new { conferenceSlug = fullConferenceDto.slug });

		}


		#endregion

		#region Edit Conference

		[HttpGet]
		public async Task<ActionResult> EditConference(string conferenceSlug)
		{
			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
				userName = System.Web.HttpContext.Current.User.Identity.Name;
			}

			var conference = await _remoteDataRepository.GetFullConference(conferenceSlug, userName);
			var createConference = Mapper.Map<CreateConference>(conference);
			return View(createConference);
		}

		[HttpPost]
        public async Task<ActionResult> EditConf(CreateConference conference, HttpPostedFileBase rectangularImage, HttpPostedFileBase squareImage)
		{
			if (Request.Form["tags"] != null)
				conference.tags = Request.Form["tags"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			if (Request.Form["subjects"] != null)
				conference.subjects = Request.Form["subjects"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			if (Request.Form["sessionTypes"] != null)
				conference.sessionTypes = Request.Form["sessionTypes"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			if (Request.Form["rooms"] != null)
				conference.rooms = Request.Form["rooms"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			if (rectangularImage != null)
			{
				AsyncManager.OutstandingOperations.Increment(1);
			}

			if (rectangularImage != null)
			{
				//TODO : Make async
				IImageSaverConfiguration configuration = new ImageSaverConfiguration();
				var imageName = conference.name.GenerateSlug() + Path.GetExtension(rectangularImage.FileName);
				conference.imageUrl = configuration.ImageUrl + imageName;

				ThreadPool.QueueUserWorkItem(o =>
								{
									IImageSaver imageSaver = new AzureImageSaver(configuration);
									imageSaver.SaveImage(imageName, rectangularImage);
									AsyncManager.OutstandingOperations.Decrement();
								}, null);
			}

            if (squareImage != null)
            {
                //TODO : Make async
                IImageSaverConfiguration configuration = new ImageSaverConfiguration();
                var imageName = conference.name.GenerateSlug() + "-square" + Path.GetExtension(squareImage.FileName);
                conference.imageUrlSquare = configuration.ImageUrl + imageName;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    IImageSaver imageSaver = new AzureImageSaver(configuration);
                    imageSaver.SaveImage(imageName, squareImage);
                    AsyncManager.OutstandingOperations.Decrement();
                }, null);
            }

			var c = await _remoteDataRepository.EditConference(conference, "user", "password");
			return RedirectToAction("Detail", "Conferences", new { conferenceSlug = c.slug });

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


		#endregion

		public async Task<ActionResult> EditConferencesIndex(string sortBy, bool? showPastConferences, string search)
		{
			var conferences = await _remoteDataRepository.GetConferencesAsync(sortBy: sortBy, showPastConferences: showPastConferences, search: search);

			return View(conferences.OrderBy(c => c.name).ToList());

		}

	}
}