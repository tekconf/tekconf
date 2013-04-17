using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class AdminSpeakerController : Controller
	{
		private readonly IRemoteDataRepositoryAsync _remoteDataRepositoryAsync;

		public AdminSpeakerController(IRemoteDataRepositoryAsync remoteDataRepositoryAsync)
		{
			_remoteDataRepositoryAsync = remoteDataRepositoryAsync;
		}

		[HttpGet]
		public ActionResult CreateSpeaker()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateSpeaker(CreateSpeaker speaker, HttpPostedFileBase file)
		{
			string url = string.Empty;

			if (file != null)
			{
				url = "/img/speakers/" + (speaker.firstName + " " + speaker.lastName).GenerateSlug() + Path.GetExtension(file.FileName);
				speaker.profileImageUrl = url;
			}

			var imageTask = SaveSpeakerImage(url, file);

			if (file != null)
			{
				var filename = Server.MapPath(url);

				ThreadPool.QueueUserWorkItem(o =>
																				 {
																					 file.SaveAs(filename);
																					 AsyncManager.OutstandingOperations.Decrement();
																				 }, null);
			}

			var addSpeakerTask = _remoteDataRepositoryAsync.AddSpeakerToSession(speaker);

			await Task.WhenAll(addSpeakerTask, imageTask);

			return RedirectToAction("Detail", "Session", new { conferenceSlug = speaker.conferenceSlug, sessionSlug = speaker.sessionSlug });
		}

		public Task SaveSpeakerImage(string url, HttpPostedFileBase file)
		{
			return Task.Factory.StartNew(() =>
			{
				if (file != null)
				{
					var filename = Server.MapPath(url);
					file.SaveAs(filename);
				}
			});
		}

		[HttpGet]
		public async Task<ActionResult> EditSpeaker(string conferenceSlug, string speakerSlug)
		{
			var speakerTask = _remoteDataRepositoryAsync.GetSpeaker(conferenceSlug, speakerSlug);
			await speakerTask;

			var createSpeaker = Mapper.Map<CreateSpeaker>(speakerTask.Result);
			return View(createSpeaker);
		}

		[HttpPost]
		public async Task<ActionResult> EditSpeakerInConference(CreateSpeaker speaker, HttpPostedFileBase file)
		{
			string url = string.Empty;

			if (file != null)
			{
				url = "/img/speakers/" + (speaker.firstName + " " + speaker.lastName).GenerateSlug() + Path.GetExtension(file.FileName); ;
				speaker.profileImageUrl = url;
			}

			var imageTask = SaveSpeakerImage(url, file);
			var speakerTask = _remoteDataRepositoryAsync.EditSpeaker(speaker);

			await Task.WhenAll(imageTask, speakerTask);

			return RedirectToRoute("SessionSpeakerDetail", new { conferenceSlug = speaker.conferenceSlug, speakerSlug = speaker.slug });

		}

	}
}