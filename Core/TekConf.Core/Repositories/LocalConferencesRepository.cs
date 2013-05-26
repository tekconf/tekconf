using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;
using System.Linq;

namespace TekConf.Core.Repositories
{
	public class LocalConferencesRepository : ILocalConferencesRepository
	{
		private readonly IMvxFileStore _fileStore;
		private const string _conferencesListViewPath = "conferencesListView.json";

		public LocalConferencesRepository(IMvxFileStore fileStore)
		{
			_fileStore = fileStore;
		}

		#region Save
		public void SaveConferences(IEnumerable<FullConferenceDto> conferences)
		{
			var conferencesList = conferences.ToList();
			conferences = null;
			SaveConferencesToFileStore(conferencesList);
			SaveConferencesLastUpdated();

		}

		public void SaveConference(FullConferenceDto conference)
		{
			SaveIndividualConference(conference);
		}

		private void SaveConferencesToFileStore(IList<FullConferenceDto> conferences)
		{
			if (conferences != null)
			{
				if (_fileStore.Exists(_conferencesListViewPath))
				{
					_fileStore.DeleteFile(_conferencesListViewPath);
				}

				if (!_fileStore.Exists(_conferencesListViewPath))
				{
					var filteredConferences = conferences.Select(x => new ConferencesListViewDto(x, _fileStore)).ToList();

					var json = JsonConvert.SerializeObject(filteredConferences);
					_fileStore.WriteFile(_conferencesListViewPath, json);
				}

				SaveIndividualConferences(conferences);
			}
		}

		private void SaveIndividualConferences(IEnumerable<FullConferenceDto> conferences)
		{
			foreach (var conference in conferences)
			{
				SaveIndividualConference(conference);
			}
		}

		private void SaveIndividualConference(FullConferenceDto conference)
		{
			if (conference != null)
			{
				var conferenceJsonPath = conference.slug + ".json";
				if (_fileStore.Exists(conferenceJsonPath))
				{
					_fileStore.DeleteFile(conferenceJsonPath);
				}

				if (!_fileStore.Exists(conferenceJsonPath))
				{
					var json = JsonConvert.SerializeObject(conference);
					_fileStore.WriteFile(conferenceJsonPath, json);
				}
			}
		}

		private void SaveConferencesLastUpdated()
		{
			const string conferencesLastUpdatedPath = "conferencesLastUpdated.json";
			if (_fileStore.Exists(conferencesLastUpdatedPath))
			{
				_fileStore.DeleteFile(conferencesLastUpdatedPath);
			}

			if (!_fileStore.Exists(conferencesLastUpdatedPath))
			{
				var conferencesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
				var json = JsonConvert.SerializeObject(conferencesLastUpdated);
				_fileStore.WriteFile(conferencesLastUpdatedPath, json);
			}
		}
		#endregion


		#region Get

		public IEnumerable<ConferencesListViewDto> GetConferencesListView()
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

		public ConferenceDetailViewDto GetConferenceDetail(string slug)
		{
			var fullConference = GetFullConference(slug);

			if (fullConference != null)
			{
				var conferenceDetailViewDto = new ConferenceDetailViewDto(fullConference);
				return conferenceDetailViewDto;
			}

			return null;
		}

		private FullConferenceDto GetFullConference(string slug)
		{
			var conferenceJsonPath = slug + ".json";

			if (_fileStore.Exists(conferenceJsonPath))
			{
				string json;
				if (_fileStore.TryReadTextFile(conferenceJsonPath, out json))
				{
					var fullConference = JsonConvert.DeserializeObject<FullConferenceDto>(json);
					return fullConference;
				}
			}

			return null;
		}

		#endregion

	}
}