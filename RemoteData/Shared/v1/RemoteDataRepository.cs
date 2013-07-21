using System;
using System.Collections.Generic;
using ServiceStack.ServiceClient.Web;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.Common.ServiceClient.Web;

namespace TekConf.RemoteData.v1
{
	using System.Threading.Tasks;

	using ServiceStack.ServiceHost;

	public class RemoteDataRepository : IRemoteDataRepository
	{
		private readonly string _baseUrl;
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
					_restClient = new JsonServiceClient(_baseUrl) { Timeout = new TimeSpan(0, 0, 0, 120, 0) };
				}

				return _restClient;
			}
		}

		public async Task<ScheduleDto> GetSchedule(string conferenceSlug, string userName)
		{
			var schedule = new Schedule() { conferenceSlug = conferenceSlug, userName = userName };

			var response = await ServiceClient.GetAsync(schedule);
		
			return response;
		}

		public async Task<List<FullConferenceDto>> GetSchedules(string userName)
		{
			var schedules = new Schedules()
			{
				userName = userName
			};

			var response = await ServiceClient.GetAsync(schedules);

			return response;
		}

		public async Task<List<PresentationDto>> GetPresentations(string userName)
		{
			var presentation = new Presentations()
			{
				speakerSlug = userName
			};

			var response = await ServiceClient.GetAsync(presentation);

			return response;
		}

		public async Task<PresentationDto> GetPresentation(string slug, string userName)
		{
			var presentation = new Presentation()
			{
				slug = slug,
				speakerSlug = userName
			};

			var response = await ServiceClient.GetAsync(presentation);

			return response;
		}

		public async Task<SubscriptionDto> GetSubscription(string emailAddress)
		{
			var subscription = new Subscription()
			{
				emailAddress = emailAddress
			};

			var response = await ServiceClient.GetAsync(subscription);
			return response;
		}

		public async Task<SubscriptionDto> AddSubscription(string emailAddress)
		{
			var subscription = new CreateSubscription()
			{
				EmailAddress = emailAddress
			};

			var response = await ServiceClient.PostAsync(subscription);
			return response;
		}

		public async Task<ScheduleDto> AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName)
		{
			var schedule = new AddSessionToSchedule()
			{
				userName = userName,
				conferenceSlug = conferenceSlug,
				sessionSlug = sessionSlug,
			};

			var response = await ServiceClient.PostAsync(schedule);
			return response;
		}

		public async Task<ScheduleDto> RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName)
		{
			var schedule = new RemoveSessionFromSchedule()
			{
				userName = userName,
				conferenceSlug = conferenceSlug,
				sessionSlug = sessionSlug,
			};

			var response = await ServiceClient.DeleteAsync(schedule);
			return response;
		}

		public async Task<IList<FullConferenceDto>> GetConferencesAsync(string userName, string sortBy = "end", 
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
				distance = distance,
				userName = userName
			};

			var response = await ServiceClient.GetAsync(conferences);

			return response;
		}

		public async Task<int> GetConferencesCount(bool? showPastConferences = false, string search = null)
		{
			if (!showPastConferences.HasValue)
			{
				showPastConferences = false;
			}

			var conferences = new ConferencesCount() { showPastConferences = showPastConferences, searchTerm = search };

			var response = await ServiceClient.GetAsync(conferences);
			return response;
		}

		public async Task<IList<FullConferenceDto>> GetFeaturedConferences()
		{
			var featured = new Conferences() { showOnlyFeatured = true };

			var response = await ServiceClient.GetAsync(featured);

			return response;
		}

		public async Task<IList<FullConferenceDto>> GetConferencesWithOpenCalls()
		{
			var openCalls = new Conferences() { showOnlyFeatured = true, showOnlyWithOpenCalls = true };

			var response = await ServiceClient.GetAsync(openCalls);
			
			return response;
		}

		public async Task<FullConferenceDto> GetConference(string slug)
		{
			var response = await ServiceClient.GetAsync(new Conference() { conferenceSlug = slug });
			return response;
		}

		public async Task<FullConferenceDto> GetFullConference(string slug, string userName)
		{
			var response = await ServiceClient.GetAsync(new Conference() { conferenceSlug = slug, userName = userName });
			return response;
		}

		public async Task<List<FullSpeakerDto>> GetFeaturedSpeakers()
		{
			var response = await ServiceClient.GetAsync(new FeaturedSpeakers());
			return response;
		}

		public async Task<IList<SpeakersDto>> GetSessionSpeakers(string conferenceSlug, string sessionSlug)
		{
			var response = await ServiceClient.GetAsync(new SessionSpeakers() { conferenceSlug = conferenceSlug, sessionSlug = sessionSlug });
			return response;
		}

		public async Task<IList<FullSpeakerDto>> GetSpeakers(string conferenceSlug)
		{
			var response = await ServiceClient.GetAsync(new Speakers() { conferenceSlug = conferenceSlug });
			return response;
		}

		public async Task<FullSpeakerDto> GetSpeaker(string conferenceSlug, string speakerSlug)
		{
			var response = await ServiceClient.GetAsync(new Speaker() { conferenceSlug = conferenceSlug, speakerSlug = speakerSlug });

			return response;
		}

		public async Task<List<SessionsDto>> GetSessionsAsync(string conferenceSlug)
		{
			var request = new Sessions() { conferenceSlug = conferenceSlug };

			var result = await ServiceClient.GetAsync<List<SessionsDto>>(request);

			return result;
		}
		
		public async Task<SessionDto> GetSession(string conferenceSlug, string slug)
		{
			var response = await ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug });
			return response;
		}

		public async Task<PresentationDto> CreatePresentation(CreatePresentation presentation, string userName, string password)
		{
			presentation.Slug = presentation.Title.GenerateSlug();
			presentation.UserName = userName;

			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PostAsync(presentation);
			return response;
		}

		public async Task<PresentationDto> CreatePresentationHistory(CreatePresentationHistory history, string userName, string password)
		{
			history.UserName = userName;

			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PutAsync(history);
			return response;
		}

		public async Task<FullConferenceDto> CreateConference(CreateConference conference, string userName, string password)
		{
			conference.slug = conference.name.GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PostAsync(conference);
			return response;
		}

		public async Task<FullConferenceDto> EditConference(CreateConference conference, string userName, string password)
		{
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PutAsync(conference);
			return response;
		}

		public async Task<UserDto> GetUser(string userName)
		{
			var response = await ServiceClient.GetAsync(new User() { userName = userName });
			return response;
		}

		public async Task<SessionDto> AddSessionToConference(AddSession session, string userName, string password)
		{
			session.slug = session.title.GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PostAsync(session);
			return response;
		}

		public async Task<SessionDto> EditSessionInConference(AddSession session, string userName, string password)
		{
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PutAsync(session);
			return response;
		}

		public async Task<FullSpeakerDto> AddSpeakerToSession(CreateSpeaker speaker, string userName, string password)
		{
			speaker.slug = (speaker.firstName.ToLower() + " " + speaker.lastName.ToLower()).GenerateSlug();
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PostAsync(speaker);
			return response;
		}

		public async Task<FullSpeakerDto> EditSpeaker(CreateSpeaker speaker, string userName, string password)
		{
			ServiceClient.SetCredentials(userName, password);
			var response = await ServiceClient.PutAsync(speaker);
			return response;
		}


	}
}
