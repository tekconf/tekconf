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

		public void GetSchedule(string conferenceSlug, string userName, Action<ScheduleDto> callback)
		{
			var schedule = new Schedule() { conferenceSlug = conferenceSlug, userName = userName };

			ServiceClient.GetAsync(schedule, callback, (r, ex) =>
														 {
															 var x = ex;
															 callback(null);
														 });
		}

		public void GetSchedules(string userName, Action<List<FullConferenceDto>> callback)
		{
			var schedules = new Schedules()
			{
				userName = userName
			};

			ServiceClient.GetAsync(schedules, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void GetPresentations(string userName, Action<List<PresentationDto>> callback)
		{
			var presentation = new Presentations()
			{
				speakerSlug = userName
			};

			ServiceClient.GetAsync(presentation, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void GetPresentation(string slug, string userName, Action<PresentationDto> callback)
		{
			var presentation = new Presentation()
			{
				slug = slug,
				speakerSlug = userName
			};

			ServiceClient.GetAsync(presentation, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName, Action<ScheduleDto> callback)
		{
			var schedule = new AddSessionToSchedule()
			{
				userName = userName,
				conferenceSlug = conferenceSlug,
				sessionSlug = sessionSlug,
			};

			ServiceClient.PostAsync(schedule, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName, Action<ScheduleDto> callback)
		{
			var schedule = new RemoveSessionFromSchedule()
			{
				userName = userName,
				conferenceSlug = conferenceSlug,
				sessionSlug = sessionSlug,
			};

			ServiceClient.DeleteAsync(schedule, callback, (r, ex) =>
			{
				var x = ex;
				callback(null);
			});
		}

		public void GetConferences(Action<IList<FullConferenceDto>> callback, string sortBy = "end", 
			bool? showPastConferences = false, bool? showOnlyOpenCalls = false, bool? showOnlyOnSale = false, 
			string search = null, string city = null, string state = null, string country = null, double? latitude = null, 
			double? longitude = null, double? distance = null)
		{
			if (sortBy == null)
			{
				sortBy = "end";
			}

			if (!showPastConferences.HasValue)
			{
				showPastConferences = false;
			}

			if (!showOnlyOpenCalls.HasValue)
			{
				showOnlyOpenCalls = false;
			}

			if (!showOnlyOnSale.HasValue)
			{
				showOnlyOnSale = false;
			}

			var conferences = new Conferences() { 
				sortBy = sortBy, 
				showPastConferences = showPastConferences, 
				showOnlyWithOpenCalls = showOnlyOpenCalls, 
				showOnlyOnSale = showOnlyOnSale, 
				search = search, 
				showOnlyFeatured = false,
				city = city,
				state = state,
				country = country,
				latitude = latitude,
				longitude = longitude,
				distance = distance
			};
			ServiceClient.GetAsync(conferences, callback, (r, ex) => { callback(null); });
		}

		public void GetConferencesCount(Action<int> callback, bool? showPastConferences = false, string search = null)
		{
			if (!showPastConferences.HasValue)
			{
				showPastConferences = false;
			}

			var conferences = new ConferencesCount() { showPastConferences = showPastConferences, searchTerm = search };
			ServiceClient.GetAsync(conferences, callback, (r, ex) =>
				{
					ServiceClient.GetAsync(conferences, callback, (r2, ex2) =>
					{
						callback(0);
					});
				});
		}

		public void GetFeaturedConferences(Action<IList<FullConferenceDto>> callback)
		{
			var featured = new Conferences() { showOnlyFeatured = true };
			ServiceClient.GetAsync(featured, callback, (r, ex) => { callback(null); });
		}

		public void GetConferencesWithOpenCalls(Action<IList<FullConferenceDto>> callback)
		{
			var openCalls = new Conferences() { showOnlyFeatured = true, showOnlyWithOpenCalls = true };
			ServiceClient.GetAsync(openCalls, callback, (r, ex) => { callback(null); });
		}

		public void GetConference(string slug, Action<FullConferenceDto> callback)
		{
			ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) => { callback(null); });
		}

		public void GetFullConference(string slug, string userName, Action<FullConferenceDto> callback)
		{
			ServiceClient.GetAsync(new Conference() { conferenceSlug = slug, userName = userName }, callback, (r, ex) =>
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

		public void CreatePresentation(CreatePresentation presentation, string userName, string password, Action<PresentationDto> callback)
		{
			presentation.Slug = presentation.Title.GenerateSlug();
			presentation.UserName = userName;

			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PostAsync(presentation, callback, (r, ex) => { callback(null); });
		}

		public void CreatePresentationHistory(CreatePresentationHistory history, string userName, string password, Action<PresentationDto> callback)
		{
			history.UserName = userName;

			ServiceClient.SetCredentials(userName, password);
			ServiceClient.PutAsync(history, callback, (r, ex) => { callback(null); });
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
