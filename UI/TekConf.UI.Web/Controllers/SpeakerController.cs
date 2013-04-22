using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.RemoteData.v1;

	public class SpeakerController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;

		public SpeakerController(IRemoteDataRepository remoteDataRepository)
		{
			_remoteDataRepository = remoteDataRepository;
		}

		[CompressFilter]
		public async Task<ActionResult> Index(string conferenceSlug, string sessionSlug)
		{
			var sessionSpeakers = await _remoteDataRepository.GetSessionSpeakers(conferenceSlug, sessionSlug);

			return View(sessionSpeakers);
		}



		[CompressFilter]
		public async Task<ActionResult> Detail(string conferenceSlug, string sessionSlug, string speakerSlug)
		{
			var speakerTask = _remoteDataRepository.GetSpeaker(conferenceSlug, speakerSlug);
			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
					userName = System.Web.HttpContext.Current.User.Identity.Name;
			}
			var conferenceTask = _remoteDataRepository.GetFullConference(conferenceSlug, userName);

			await Task.WhenAll(speakerTask, conferenceTask);

			if (speakerTask.Result == null || conferenceTask.Result == null)
			{
				return RedirectToAction("NotFound", "Error");
			}

			var conference = conferenceTask.Result;
			var speaker = speakerTask.Result;

			var conferenceDto = new FullConferenceDto()
			{
				description = conference.description,
				end = conference.end,
				imageUrl = conference.imageUrl,
				location = conference.location,
				name = conference.name,
				start = conference.start
			};

			var sessions = from s in conference.sessions
										 from sp in s.speakers
										 where sp.slug == speaker.slug
										 select new SessionsDto()
										 {
											 conferenceSlug = conference.slug,
											 description = s.description,
											 difficulty = s.difficulty,
											 end = s.end,
											 links = s.links,
											 prerequisites = s.prerequisites,
											 room = s.room,
											 sessionType = s.sessionType,
											 slug = s.slug,
											 start = s.start,
											 subjects = s.subjects,
											 tags = s.tags,
											 title = s.title,
											 twitterHashTag = s.twitterHashTag,
										 };

			var viewModel = new SpeakerDetailViewModel()
			{
				Conference = conferenceDto,
				Speaker = speaker,
				Sessions = sessions.ToList(),
			};

			return View(viewModel);
		}



	}
}
