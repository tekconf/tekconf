using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.Common.Entities;

	using Configuration = System.Configuration.Configuration;

	[Authorize]
	public class AdminSessionController : AsyncController
	{
		private RemoteDataRepositoryAsync _repository;
		private readonly IConferenceRepository _conferenceRepository;

		public AdminSessionController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
			var configuration = new EntityConfiguration();
			_conferenceRepository = new ConferenceRepository(configuration);
		}

		#region Add Session

		public void AddSessionAsync(string conferenceSlug)
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);
			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
				userName = System.Web.HttpContext.Current.User.Identity.Name;
			}

			AsyncManager.OutstandingOperations.Increment();
			repository.GetFullConference(conferenceSlug, userName, conference =>
												{
													AsyncManager.Parameters["conference"] = conference;
													AsyncManager.OutstandingOperations.Decrement();
												});
		}

		public ActionResult AddSessionCompleted(FullConferenceDto conference)
		{
			var session = new AddSession() { conferenceSlug = conference.slug, start = conference.start, end = conference.end, defaultTalkLength = conference.defaultTalkLength };

			var sessionTypes = (conference.sessionTypes ?? new List<string>()).OrderBy(x => x).ToList();
			var sessionTypesList = new SelectList(sessionTypes.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.SessionTypesList = sessionTypesList;

			var difficulties = new List<string>() { "Beginner", "Intermediate", "Expert" };
			var difficultiesList = new SelectList(difficulties.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.DifficultiesList = difficultiesList;

			var rooms = (conference.rooms ?? new List<string>()).OrderBy(x => x).ToList();
			var roomsList = new SelectList(rooms.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.RoomsList = roomsList;

			

			session.start = conference.start;
			session.end = conference.end;

			return View(session);
		}

		[HttpPost]
		public void AddSessionToConferenceAsync(AddSession session)
		{
			if (Request.Form["hidden-tags"] != null)
				session.tags = Request.Form["hidden-tags"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			if (Request.Form["hidden-subjects"] != null)
				session.subjects = Request.Form["hidden-subjects"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);

			AsyncManager.OutstandingOperations.Increment();

			repository.AddSessionToConference(session, "user", "password", c =>
																										 {
																											 AsyncManager.Parameters["session"] = c;
																											 AsyncManager.OutstandingOperations.Decrement();
																										 });
		}

		public ActionResult AddSessionToConferenceCompleted(SessionDto session)
		{
			return RedirectToRoute("AdminAddSpeaker", new { conferenceSlug = session.conferenceSlug, sessionSlug = session.slug });
		}

		#endregion

		#region Edit Session

		public void EditSessionAsync(string conferenceSlug, string sessionSlug)
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			var repository = new RemoteDataRepository(baseUrl);
			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
				userName = System.Web.HttpContext.Current.User.Identity.Name;
			}

			AsyncManager.OutstandingOperations.Increment();
			repository.GetFullConference(conferenceSlug, userName, conference =>
																											 {
																												 AsyncManager.Parameters["sessionSlug"] = sessionSlug;
																												 AsyncManager.Parameters["conference"] = conference;
																												 AsyncManager.OutstandingOperations.Decrement();
																											 });
		}

		public ActionResult EditSessionCompleted(string sessionSlug, FullConferenceDto conference)
		{
			var session = conference.sessions.FirstOrDefault(s => s.slug == sessionSlug);

			var addSession = Mapper.Map<AddSession>(session);

			var sessionTypes = (conference.sessionTypes ?? new List<string>()).OrderBy(x => x).ToList();
			var sessionTypesList = new SelectList(sessionTypes.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.SessionTypesList = sessionTypesList;

			var difficulties = new List<string>() { "Beginner", "Intermediate", "Expert" };
			var difficultiesList = new SelectList(difficulties.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.DifficultiesList = difficultiesList;

			var rooms = (conference.rooms ?? new List<string>()).OrderBy(x => x).ToList();
			var roomsList = new SelectList(rooms.Select(x => new KeyValuePair<string, string>(x, x)), "Key", "Value");
			ViewBag.RoomsList = roomsList;

			return View(addSession);
		}

		[HttpPost]
		public async Task<ActionResult> EditSessionInConference(AddSession request)
		{
			if (Request.Form["hidden-tags"] != null)
				request.tags = Request.Form["hidden-tags"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			if (Request.Form["hidden-subjects"] != null)
				request.subjects = Request.Form["hidden-subjects"].Trim().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			SessionEntity sessionEntity = null;
			ConferenceEntity conferenceEntity = null;
			var saveSessionTask = Task.Factory.StartNew(() =>
			{
				var session = Mapper.Map<AddSession, SessionEntity>(request);
				sessionEntity = _conferenceRepository.SaveSession(request.conferenceSlug, request.slug, session);
				conferenceEntity = _conferenceRepository.AsQueryable().Single(x => x.slug == request.conferenceSlug);
			});
			
			await saveSessionTask;

			var sessionDto = Mapper.Map<SessionEntity, SessionDto>(sessionEntity);
			sessionDto.conferenceSlug = request.conferenceSlug;
			sessionDto.conferenceName = conferenceEntity.name;

			return RedirectToRoute("SessionDetail", new { conferenceSlug = sessionDto.conferenceSlug, sessionSlug = sessionDto.slug });

		}

		#endregion
	}
}