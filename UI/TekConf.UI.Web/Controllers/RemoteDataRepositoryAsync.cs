using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web.Controllers
{
	public class RemoteDataRepositoryAsync : IRemoteDataRepositoryAsync
	{
		private readonly string _baseUrl;
		private RemoteDataRepository _repository;

		public RemoteDataRepositoryAsync(string baseUrl)
		{
			_baseUrl = baseUrl;
			_repository = new RemoteDataRepository(_baseUrl);
		}

		public Task<List<PresentationDto>>  GetPresentations(string userName)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<List<PresentationDto>>();

				_repository.GetPresentations(userName, callback: c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<PresentationDto> GetPresentation(string slug, string userName)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<PresentationDto>();

				_repository.GetPresentation(slug, userName,
														callback: c => t.TrySetResult(c));

				return t.Task;
			});
		}
		public Task<IList<FullConferenceDto>> GetConferences(
														string sortBy = null,
														bool? showPastConferences = false,
														bool? showOnlyOpenCalls = false,
														bool? showOnlyOnSale = false,
														string search = null,
														string city = null, string state = null, string country = null,
														double? latitude = null, double? longitude = null, double? distance = null)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<FullConferenceDto>>();

				_repository.GetConferences(sortBy: sortBy,
														showPastConferences: showPastConferences,
														showOnlyOpenCalls: showOnlyOpenCalls,
														showOnlyOnSale: showOnlyOnSale,
														search: search,
														city: city,
														state: state,
														country: country,
														latitude: latitude,
														longitude: longitude,
														distance: distance,
														callback: c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<int> GetConferencesCount(bool? showPastConferences, string search)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<int>();

				_repository.GetConferencesCount(showPastConferences: showPastConferences,
														search: search,
														callback: c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<ScheduleDto> GetSchedule(string conferenceSlug, string userName)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<ScheduleDto>();

				_repository.GetSchedule(conferenceSlug, userName, c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<List<FullConferenceDto>> GetSchedules(string userName)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<List<FullConferenceDto>>();

				_repository.GetSchedules(userName, c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<FullConferenceDto> GetFullConference(string conferenceSlug, string userName)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<FullConferenceDto>();

				_repository.GetFullConference(conferenceSlug, userName, c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<IList<FullConferenceDto>> GetFeaturedConferences()
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<FullConferenceDto>>();

				_repository.GetFeaturedConferences(c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<IList<FullConferenceDto>> GetConferencesWithOpenCalls()
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<FullConferenceDto>>();

				_repository.GetConferencesWithOpenCalls(c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<IList<FullSpeakerDto>> GetFeaturedSpeakers()
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<FullSpeakerDto>>();

				_repository.GetFeaturedSpeakers(s => t.TrySetResult(s));

				return t.Task;

			});
		}

		public Task<IList<SessionsDto>> GetSessions(string conferenceSlug)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<SessionsDto>>();

				_repository.GetSessions(conferenceSlug, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<SessionDto> GetSessionDetail(string conferenceSlug, string sessionSlug)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<SessionDto>();

				_repository.GetSession(conferenceSlug, sessionSlug, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<FullSpeakerDto> GetSpeaker(string conferenceSlug, string speakerSlug)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<FullSpeakerDto>();

				_repository.GetSpeaker(conferenceSlug, speakerSlug, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<IList<SpeakersDto>> GetSessionSpeakers(string conferenceSlug, string sessionSlug)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<IList<SpeakersDto>>();

				_repository.GetSessionSpeakers(conferenceSlug, sessionSlug, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<PresentationDto> CreatePresentation(CreatePresentation presentation)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<PresentationDto>();

				_repository.CreatePresentation(presentation, presentation.UserName, "password", c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<PresentationDto> CreatePresentationHistory(CreatePresentationHistory history)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<PresentationDto>();

				_repository.CreatePresentationHistory(history, history.UserName, "password", c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<FullConferenceDto> CreateConference(CreateConference conference)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<FullConferenceDto>();

				_repository.CreateConference(conference, "user", "password", c => t.TrySetResult(c));

				return t.Task;
			});
		}

		public Task<ScheduleDto> AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName, string password)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<ScheduleDto>();

				_repository.AddSessionToSchedule(conferenceSlug, sessionSlug, userName, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<ScheduleDto> RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName, string password)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<ScheduleDto>();

				_repository.RemoveSessionFromSchedule(conferenceSlug, sessionSlug, userName, s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<FullSpeakerDto> AddSpeakerToSession(CreateSpeaker speaker)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<FullSpeakerDto>();

				_repository.AddSpeakerToSession(speaker, "user", "password", s => t.TrySetResult(s));

				return t.Task;
			});
		}

		public Task<FullSpeakerDto> EditSpeaker(CreateSpeaker speaker)
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<FullSpeakerDto>();

				_repository.EditSpeaker(speaker, "user", "password", s => t.TrySetResult(s));

				return t.Task;
			});
		}



	}
}