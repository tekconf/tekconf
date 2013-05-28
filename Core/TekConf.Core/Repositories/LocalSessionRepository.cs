using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public interface ILocalSessionRepository
	{
		//FullSessionDto NextScheduledSession { get; }
		//void SaveSchedules(IEnumerable<FullConferenceDto> scheduledConferences);
		SessionDetailDto GetSession(string conferenceSlug, string sessionSlug);
		void SaveSession(string conferenceSlug, FullSessionDto fullSession);
	}

	public class LocalSessionRepository : ILocalSessionRepository
	{
		private readonly IMvxFileStore _fileStore;
		//private const string sessionsPath = "schedules.json";

		public LocalSessionRepository(IMvxFileStore fileStore)
		{
			_fileStore = fileStore;
		}

		//public void SaveSchedules(IEnumerable<FullConferenceDto> scheduledConferences)
		//{
		//	SaveSchedulesToFileStore(scheduledConferences);

		//	SaveSchedulesLastUpdated();
		//}

		//private const string _conferencesListViewSchedulesPath = "conferencesListViewSchedules.json";

		public SessionDetailDto GetSession(string conferenceSlug, string sessionSlug)
		{
			var path = conferenceSlug + "-" + sessionSlug + ".json";

			if (_fileStore.Exists(path))
			{
				string json;

				if (_fileStore.TryReadTextFile(path, out json))
				{
					var sessionDto = JsonConvert.DeserializeObject<SessionDetailDto>(json);
					return sessionDto;
				}
			}
	
			return null;
		}

		public void SaveSession(string conferenceSlug, FullSessionDto fullSession)
		{
			var path = conferenceSlug + "-" + fullSession.slug + ".json";

			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}
			if (!_fileStore.Exists(path))
			{
				var sessionDetail = new SessionDetailDto(fullSession);
				string json = JsonConvert.SerializeObject(sessionDetail);
				_fileStore.WriteFile(path, json);
			}
		}

		//private static FullConferenceDto GetFullConference(IMvxFileStore fileStore, string slug)
		//{
		//	var conferenceJsonPath = slug + ".json";

		//	if (fileStore.Exists(conferenceJsonPath))
		//	{
		//		string json;
		//		if (fileStore.TryReadTextFile(conferenceJsonPath, out json))
		//		{
		//			var fullConference = JsonConvert.DeserializeObject<FullConferenceDto>(json);
		//			return fullConference;
		//		}
		//	}

		//	return null;
		//}

		//private void SaveSchedulesToFileStore(IEnumerable<FullConferenceDto> scheduledConferences)
		//{
		//	if (_fileStore.Exists(_conferencesListViewSchedulesPath))
		//	{
		//		_fileStore.DeleteFile(_conferencesListViewSchedulesPath);
		//	}
		//	if (!_fileStore.Exists(_conferencesListViewSchedulesPath))
		//	{
		//		var filteredSchedules = scheduledConferences.Select(x => new ConferencesListViewDto(x, _fileStore)).ToList();
		//		var json = JsonConvert.SerializeObject(filteredSchedules);
		//		_fileStore.WriteFile(_conferencesListViewSchedulesPath, json);
		//	}
		//}

		//private void SaveSchedulesLastUpdated()
		//{
		//	const string schedulesLastUpdatedPath = "scheduleLastUpdated.json";
		//	if (_fileStore.Exists(schedulesLastUpdatedPath))
		//	{
		//		_fileStore.DeleteFile(schedulesLastUpdatedPath);
		//	}

		//	if (!_fileStore.Exists(schedulesLastUpdatedPath))
		//	{
		//		var schedulesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
		//		var json = JsonConvert.SerializeObject(schedulesLastUpdated);
		//		_fileStore.WriteFile(schedulesLastUpdatedPath, json);
		//	}
		//}

		//public FullSessionDto NextScheduledSession
		//{
		//	get
		//	{
		//		var scheduledConferences = GetConferencesList();
		//		if (scheduledConferences != null)
		//		{
		//			var conference = scheduledConferences.FirstOrDefault(x => x.start > DateTime.Now);
		//			if (conference != null)
		//			{
		//				return new FullSessionDto() { title = conference.name };

		//			}
		//			//var session = sessions.Where(x => x.isAddedToSchedule == true).Where(x => x.start >= DateTime.Now).OrderBy(x => x.start).FirstOrDefault();
		//		}


		//		return new FullSessionDto() { title = "None" };
		//	}
		//}
	}
}