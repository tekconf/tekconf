using System;
using System.Collections.Generic;
using ServiceStack.ServiceClient.Web;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.Common.ServiceClient.Web;

namespace TekConf.RemoteData.v1
{
	public class RemoteDataRepository
	{
		private string _baseUrl;
		public RemoteDataRepository(string baseUrl)
		{
			_baseUrl = baseUrl;
		}
		private JsonServiceClient _restClient;
		private JsonServiceClient ServiceClient
		{
			get
			{
				if (_restClient == null)
				{
					_restClient = new JsonServiceClient(_baseUrl);
					_restClient.Timeout = new TimeSpan(0, 0, 0, 120, 0);
				}

				return _restClient;
			}
		}

		public void Register(string authenticationMethod, string userName, string identifier, string password, string email)
		{
			Registration request = new Registration()
			{
				Email = email,
				UserName = userName,
				Password = password
			};

			var response = ServiceClient.Post<RegistrationResponse>("/register", request);
		}

		public void GetSchedule(string conferenceSlug, string authenticationMethod, string authenticationToken, string userName, string password, Action<ScheduleDto> callback)
		{
			var schedule = new Schedule() { authenticationMethod = authenticationMethod, authenticationToken = authenticationToken, conferenceSlug = conferenceSlug };

			ServiceClient.SetCredentials(userName, password);
			ServiceClient.GetAsync(schedule, callback, (r, ex) =>
														 {
															 var x = ex;
															 callback(null);
														 });
		}

		public void GetSchedules(string authenticationMethod, string authenticationToken, string userName, string password, Action<List<SchedulesDto>> callback)
		{
			var schedules = new Schedules()
			{
				authenticationMethod = authenticationMethod,
				authenticationToken = authenticationToken
			};

			ServiceClient.SetCredentials(userName, password);
			ServiceClient.GetAsync(schedules, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName, string password, Action<ScheduleDto> callback)
		{
			var schedule = new AddSessionToSchedule()
			{
				conferenceSlug = conferenceSlug,
				sessionSlug = sessionSlug,
			};

			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PostAsync(schedule, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void GetConferences(Action<IList<ConferencesDto>> callback, string sortBy = "end", bool? showPastConferences = false, string search = null)
		{
			if (sortBy == null)
			{
				sortBy = "end";
			}

			if (!showPastConferences.HasValue)
			{
				showPastConferences = false;
			}

			var conferences = new Conferences() { sortBy = sortBy, showPastConferences = showPastConferences, search = search, showOnlyFeatured = false };
			ServiceClient.GetAsync(conferences, callback, (r, ex) => { callback(null); });
		}

		public void GetFeaturedConferences(Action<IList<ConferencesDto>> callback)
		{
			var featured = new Conferences() { showOnlyFeatured = true };
			ServiceClient.GetAsync(featured, callback, (r, ex) => { callback(null); });
		}

		public void GetConferencesWithOpenCalls(Action<IList<ConferencesDto>> callback)
		{
			var openCalls = new Conferences() { showOnlyFeatured = true, showOnlyWithOpenCalls = true };
			ServiceClient.GetAsync(openCalls, callback, (r, ex) => { callback(null); });
		}

		public void GetConference(string slug, Action<FullConferenceDto> callback)
		{
			ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) => { callback(null); });
		}

		public void GetFullConference(string slug, Action<FullConferenceDto> callback)
		{
			ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) =>
																																											 {
																																												 callback(null);
																																											 });
		}

		public void GetFeaturedSpeakers(Action<List<FullSpeakerDto>> callback)
		{
			ServiceClient.GetAsync(new FeaturedSpeakers(), callback, (r, ex) => { callback(null); });
		}

		public void GetSessionSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback)
		{
			ServiceClient.GetAsync(new SessionSpeakers() { conferenceSlug = conferenceSlug, sessionSlug = sessionSlug }, callback, (r, ex) => { callback(null); });
		}

		public void GetSpeakers(string conferenceSlug, Action<IList<FullSpeakerDto>> callback)
		{
			ServiceClient.GetAsync(new Speakers() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { callback(null); });
		}

		public void GetSpeaker(string conferenceSlug, string speakerSlug, Action<FullSpeakerDto> callback)
		{
			ServiceClient.GetAsync(new Speaker() { conferenceSlug = conferenceSlug, speakerSlug = speakerSlug }, callback, (r, ex) =>
																									{
																										callback(null);
																									});
		}

		public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
		{
			ServiceClient.GetAsync(new Sessions() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { callback(null); });
		}

		public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
		{
			ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug }, callback, (r, ex) =>
																			{
																				callback(null);
																			});
		}

		public void CreateConference(CreateConference conference, string userName, string password, Action<FullConferenceDto> callback)
		{
			conference.slug = conference.name.GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PostAsync(conference, callback, (r, ex) => { callback(null); });
		}

		public void EditConference(CreateConference conference, string userName, string password, Action<FullConferenceDto> callback)
		{
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PutAsync(conference, callback, (r, ex) => { callback(null); });
		}


		public void GetUser(string userName, Action<UserDto> callback)
		{
			ServiceClient.GetAsync(new User() { userName = userName }, callback, (r, ex) => { callback(null); });
		}

		public void AddSessionToConference(AddSession session, string userName, string password, Action<SessionDto> callback)
		{
			session.slug = session.title.GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PostAsync(session, callback, (r, ex) => { callback(null); });
		}

		public void EditSessionInConference(AddSession session, string userName, string password, Action<SessionDto> callback)
		{
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PutAsync(session, callback, (r, ex) => { callback(null); });
		}

		public void AddSpeakerToSession(CreateSpeaker speaker, string userName, string password, Action<FullSpeakerDto> callback)
		{
			speaker.slug = (speaker.firstName.ToLower() + " " + speaker.lastName.ToLower()).GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PostAsync(speaker, callback, (r, ex) =>
																										 {
																											 callback(null);
																										 });
		}

		public void EditSpeaker(CreateSpeaker speaker, string userName, string password, Action<FullSpeakerDto> callback)
		{
			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PutAsync(speaker, callback, (r, ex) =>
															{
																callback(null);
															});
		}

	}
}
