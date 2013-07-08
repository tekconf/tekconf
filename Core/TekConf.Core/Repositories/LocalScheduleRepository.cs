using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Models;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class LocalScheduleRepository : ILocalScheduleRepository
	{
		private readonly IMvxFileStore _fileStore;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private const string _schedulesPath = "schedules.json";

		public LocalScheduleRepository(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository)
		{
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;
		}

		public void SaveSchedules(IEnumerable<ConferencesListViewDto> scheduledConferences)
		{
			SaveSchedulesToFileStore(scheduledConferences);

			SaveSchedulesLastUpdated();
		}
		public void SaveSchedules(IEnumerable<FullConferenceDto> scheduledConferences)
		{
			SaveSchedulesToFileStore(scheduledConferences);

			SaveSchedulesLastUpdated();
		}

		private const string _conferencesListViewSchedulesPath = "conferencesListViewSchedules.json";

		public IEnumerable<ConferencesListViewDto> GetConferencesList()
		{
			IEnumerable<ConferencesListViewDto> conferencesListViewDtos = null;
			if (_fileStore.Exists(_conferencesListViewSchedulesPath))
			{
				string json;
				if (_fileStore.TryReadTextFile(_conferencesListViewSchedulesPath, out json))
				{
					conferencesListViewDtos = JsonConvert.DeserializeObject<List<ConferencesListViewDto>>(json);
				}
			}

			return conferencesListViewDtos;
		}

		public void SaveSchedule(ScheduleDto schedule)
		{
			string path = schedule.conferenceSlug + "-schedule.json";

			if (schedule != null)
			{
				if (_fileStore.Exists(path))
				{
					_fileStore.DeleteFile(path);
				}

				if (schedule.IsNull())
					schedule = new ScheduleDto();

				schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();

				string serializedFavorites = JsonConvert.SerializeObject(schedule);
				_fileStore.WriteFile(path, serializedFavorites);

				IEnumerable<ConferencesListViewDto> conferences = GetConferencesList();
				if (conferences != null)
				{
					var conferenceList = conferences.ToList();
					if (conferenceList.All(x => x.slug != schedule.conferenceSlug))
					{
						var fullConference = _localConferencesRepository.Get(schedule.conferenceSlug);
						conferenceList.Add(new ConferencesListViewDto(fullConference, _fileStore));
						SaveSchedules(conferenceList);
					}
				}

			}

			
		}

		public ScheduleDto GetSchedule(string conferenceSlug)
		{
			string path = conferenceSlug + "-schedule.json";

			if (_fileStore.Exists(path))
			{
				string json;
				if (_fileStore.TryReadTextFile(path, out json))
				{
					var schedule = JsonConvert.DeserializeObject<ScheduleDto>(json);
					schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();
					return schedule;
				}
			}

			return null;
		}

		private void SaveSchedulesToFileStore(IEnumerable<FullConferenceDto> scheduledConferences)
		{
			var filteredSchedules = scheduledConferences.Select(x => new ConferencesListViewDto(x, _fileStore)).ToList();
			SaveSchedulesToFileStore(filteredSchedules);
		}

		private void SaveSchedulesToFileStore(IEnumerable<ConferencesListViewDto> scheduledConferences)
		{
			if (_fileStore.Exists(_conferencesListViewSchedulesPath))
			{
				_fileStore.DeleteFile(_conferencesListViewSchedulesPath);
			}
			if (!_fileStore.Exists(_conferencesListViewSchedulesPath))
			{
				var json = JsonConvert.SerializeObject(scheduledConferences);
				_fileStore.WriteFile(_conferencesListViewSchedulesPath, json);
			}
		}

		private void SaveSchedulesLastUpdated()
		{
			const string schedulesLastUpdatedPath = "scheduleLastUpdated.json";
			if (_fileStore.Exists(schedulesLastUpdatedPath))
			{
				_fileStore.DeleteFile(schedulesLastUpdatedPath);
			}

			if (!_fileStore.Exists(schedulesLastUpdatedPath))
			{
				var schedulesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
				var json = JsonConvert.SerializeObject(schedulesLastUpdated);
				_fileStore.WriteFile(schedulesLastUpdatedPath, json);
			}
		}

		public ConferencesListViewDto NextScheduledConference
		{
			get
			{
				var scheduledConferences = GetConferencesList();
				if (scheduledConferences != null)
				{
					var conference = scheduledConferences.FirstOrDefault(x => x.start > DateTime.Now);
					return conference;
				}

				return null;
			}
		}
		public FullSessionDto NextScheduledSession
		{
			get
			{
				var conference = NextScheduledConference;

				if (conference != null)
				{
					var schedule = GetSchedule(conference.slug);
					if (schedule != null)
					{
						var sessions = schedule.sessions;
						var session = sessions.OrderBy(x => x.start).FirstOrDefault(x => x.start >= DateTime.Now.AddMinutes(-10));
						return session;
					}
				}

				return null;
			}
		}


		public void RemoveFromSchedule(string slug)
		{
			var conferences = GetConferencesList();
			conferences = conferences.Where(x => x.slug != slug).ToList();
			SaveSchedules(conferences);
		}
	}
}