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
	public interface ILocalScheduleRepository
	{
		FullSessionDto NextScheduledSession { get; }
		void SaveSchedules(IEnumerable<FullConferenceDto> scheduledConferences);
		IEnumerable<ConferencesListViewDto> GetConferencesList();

	}

	public class ConferencesListViewDto
	{
		public string name { get; set; }
		public string DateRange { get; set; }
		public string slug { get; set; }
		public string FormattedAddress { get; set; }
		public string imageUrl { get; set; }
	}

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

		private const string _conferencesListViewPath = "conferencesListView.json";

		public IEnumerable<ConferencesListViewDto> GetConferencesList()
		{
			IEnumerable<ConferencesListViewDto> conferencesListViewDtos = null;
			if (_fileStore.Exists(_conferencesListViewPath))
			{
				string json;
				if (_fileStore.TryReadTextFile(_conferencesListViewPath, out json))
				{
					conferencesListViewDtos = JsonConvert.DeserializeObject<List<ConferencesListViewDto>>(json);
				}
			}

			return conferencesListViewDtos;
		}

		private void SaveSchedulesToFileStore(IEnumerable<FullConferenceDto> scheduledConferences)
		{
			if (_fileStore.Exists(_conferencesListViewPath))
			{
				_fileStore.DeleteFile(_conferencesListViewPath);
			}
			if (!_fileStore.Exists(_conferencesListViewPath))
			{
				var filteredSchedules = scheduledConferences.Select(x => new ConferencesListViewDto { slug = x.slug, name = x.name, DateRange = x.DateRange, FormattedAddress = x.FormattedAddress, imageUrl = x.imageUrl }).ToList();
				var json = JsonConvert.SerializeObject(filteredSchedules);
				_fileStore.WriteFile(_conferencesListViewPath, json);
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

				if (_fileStore.Exists(_schedulesPath))
				{
					string json;

					var ss = _fileStore.NativePath(_schedulesPath);
					if (_fileStore.TryReadTextFile(_schedulesPath, out json))
					{
						var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(json);
						var sessions = conferences.SelectMany(x => x.sessions);
						var session = sessions.Where(x => x.isAddedToSchedule == true).Where(x => x.start >= DateTime.Now).OrderBy(x => x.start).FirstOrDefault();
					}
				}

				return null;
			}
		}
	}
}