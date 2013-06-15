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
		private readonly ILocalSessionRepository _localSessionRepository;
		private const string _conferencesListViewPath = "conferencesListView.json";

		public LocalConferencesRepository(IMvxFileStore fileStore, ILocalSessionRepository localSessionRepository)
		{
			_fileStore = fileStore;
			_localSessionRepository = localSessionRepository;
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

				SaveConferenceDetail(conference);
				SaveConferenceSessions(conference);
			}
		}

		private void SaveConferenceDetail(FullConferenceDto conference)
		{
			if (conference != null)
			{
				var conferenceJsonPath = conference.slug + "-detail.json";
				if (_fileStore.Exists(conferenceJsonPath))
				{
					_fileStore.DeleteFile(conferenceJsonPath);
				}

				if (!_fileStore.Exists(conferenceJsonPath))
				{
					var conferenceDetail = new ConferenceDetailViewDto(conference);
					var json = JsonConvert.SerializeObject(conferenceDetail);
					_fileStore.WriteFile(conferenceJsonPath, json);
				}
			}
		}

		private void SaveConferenceSessions(FullConferenceDto conference)
		{
			if (conference.sessions != null && conference.sessions.Any())
			{
				foreach (var session in conference.sessions)
				{
					_localSessionRepository.SaveSession(conference.slug, session);
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

		public FullConferenceDto GetConference(string conferenceSlug)
		{
			FullConferenceDto conference = null;
			var conferenceJsonPath = conferenceSlug + ".json";
			if (_fileStore != null)
			{
				if (_fileStore.Exists(conferenceJsonPath))
				{
					string json;
					_fileStore.TryReadTextFile(conferenceJsonPath, out json);
					conference = JsonConvert.DeserializeObject<FullConferenceDto>(json);
				}
			}

			return conference;
		}

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
			if (!string.IsNullOrWhiteSpace(slug))
			{
				var conferenceJsonPath = slug + "-detail.json";

				if (_fileStore.Exists(conferenceJsonPath))
				{
					string json;
					if (_fileStore.TryReadTextFile(conferenceJsonPath, out json))
					{
						var conferenceDetail = JsonConvert.DeserializeObject<ConferenceDetailViewDto>(json);
						return conferenceDetail;
					}
				}
			}

			return null;
		}

		//private FullConferenceDto GetFullConference(string slug)
		//{
		//	var conferenceJsonPath = slug + ".json";

		//	if (_fileStore.Exists(conferenceJsonPath))
		//	{
		//		string json;
		//		if (_fileStore.TryReadTextFile(conferenceJsonPath, out json))
		//		{
		//			var fullConference = JsonConvert.DeserializeObject<FullConferenceDto>(json);
		//			return fullConference;
		//		}
		//	}

		//	return null;
		//}

		#endregion

	}
}