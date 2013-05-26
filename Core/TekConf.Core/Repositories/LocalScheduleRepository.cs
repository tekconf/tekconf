using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class LocalScheduleRepository : ILocalScheduleRepository
	{
		private readonly IMvxFileStore _fileStore;
		private const string _schedulesPath = "schedules.json";

		public LocalScheduleRepository(IMvxFileStore fileStore)
		{
			_fileStore = fileStore;
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

		private void SaveSchedulesToFileStore(IEnumerable<FullConferenceDto> scheduledConferences)
		{
			if (_fileStore.Exists(_conferencesListViewSchedulesPath))
			{
				_fileStore.DeleteFile(_conferencesListViewSchedulesPath);
			}
			if (!_fileStore.Exists(_conferencesListViewSchedulesPath))
			{
				var filteredSchedules = scheduledConferences.Select(x => new ConferencesListViewDto(x, _fileStore)).ToList();
				var json = JsonConvert.SerializeObject(filteredSchedules);
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
				var schedulesLastUpdated = new DataLastUpdated {LastUpdated = DateTime.Now};
				var json = JsonConvert.SerializeObject(schedulesLastUpdated);
				_fileStore.WriteFile(schedulesLastUpdatedPath, json);
			}
		}

		public FullSessionDto NextScheduledSession
		{
			get
			{
				var scheduledConferences = GetConferencesList();
				if (scheduledConferences != null)
				{
					var conference = scheduledConferences.FirstOrDefault(x => x.start > DateTime.Now);
					if (conference != null)
					{
						return new FullSessionDto() {title = conference.name};

					}
					//var session = sessions.Where(x => x.isAddedToSchedule == true).Where(x => x.start >= DateTime.Now).OrderBy(x => x.start).FirstOrDefault();
				}
				

				return new FullSessionDto() { title = "None"};
			}
		}
	}
}