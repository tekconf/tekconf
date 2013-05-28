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
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private const string _schedulesPath = "schedules.json";

		public LocalScheduleRepository(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository)
		{
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;
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

		public void SaveScheduleDetail(ScheduleDto schedule)
		{
			if (schedule != null)
			{
				ConferenceFavoritesDto favoriteConference = null;
				var path = schedule.conferenceSlug + "-favorites.json";
				if (_fileStore.Exists(path))
				{
					string json;
					if (_fileStore.TryReadTextFile(path, out json))
					{
						favoriteConference = JsonConvert.DeserializeObject<ConferenceFavoritesDto>(json);
					}
				}
				if (favoriteConference == null)
					favoriteConference = new ConferenceFavoritesDto() { slug = schedule.conferenceSlug };

				var favorites = schedule.sessions.Where(x => x.isAddedToSchedule == true).ToList();
				foreach (var favorite in favorites)
				{
					if (favoriteConference.sessions.All(x => x.slug != favorite.slug))
					{
						favoriteConference.sessions.Add(new ConferenceFavoriteSessionDto()
						{
							room = favorite.room,
							slug = favorite.slug,
							startDescription = favorite.startDescription,
							start = favorite.start,
							title = favorite.title
						});
					}
				}

				if (_fileStore.Exists(path))
				{
					_fileStore.DeleteFile(path);
				}

				string serializedFavorites = JsonConvert.SerializeObject(favoriteConference);
				_fileStore.WriteFile(path, serializedFavorites);
			}
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
				var schedulesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
				var json = JsonConvert.SerializeObject(schedulesLastUpdated);
				_fileStore.WriteFile(schedulesLastUpdatedPath, json);
			}
		}

		public ConferenceFavoriteSessionDto NextScheduledSession
		{
			get
			{
				var scheduledConferences = GetConferencesList();
				if (scheduledConferences != null)
				{
					var conference = scheduledConferences.FirstOrDefault(x => x.start > DateTime.Now);
					if (conference != null)
					{
						var path = conference.slug + "-favorites.json";
						if (_fileStore.Exists(path))
						{
							string json;
							if (_fileStore.TryReadTextFile(path, out json))
							{
								var favoriteConference = JsonConvert.DeserializeObject<ConferenceFavoritesDto>(json);
								if (favoriteConference != null)
								{
									var session = favoriteConference.sessions.FirstOrDefault(x => x.start >= DateTime.Now.AddMinutes(-30));
									if (session != null)
										return session;
								}
							}
						}
					}
				}


				return null;
			}
		}
	}
}