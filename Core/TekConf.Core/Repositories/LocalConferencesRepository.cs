using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Newtonsoft.Json;
using TekConf.Core.Entities;
using System.Linq;
using TekConf.Core.ViewModels;

namespace TekConf.Core.Repositories
{
	using System.Threading.Tasks;

	public class LocalConferencesRepository : ILocalConferencesRepository
	{
		private readonly ISQLiteConnection _connection;
		private readonly IMvxFileStore _fileStore;

		public LocalConferencesRepository(ISQLiteConnection connection, IMvxFileStore fileStore)
		{
			_connection = connection;
			_fileStore = fileStore;
			_connection.CreateTable<ConferenceEntity>();
			_connection.CreateTable<SessionEntity>();
			_connection.CreateTable<SpeakerEntity>();
		}

		public int Save(string conferenceSlug, SessionEntity session)
		{
			int sessionId;
			var conference = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
			var conferenceId = conference.Id;
			var sessionEntity = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == conferenceId).FirstOrDefault(x => x.Slug == session.Slug);

			if (sessionEntity != null)
			{
				session.Id = sessionEntity.Id;
				sessionId = _connection.Update(session);
			}
			else
			{
				sessionId = _connection.Insert(session);
			}

			return sessionId;
		}

		public void Save(IList<ConferenceEntity> conferences)
		{
			if (conferences != null)
			{
				foreach (var conference in conferences)
				{
					Save(conference);
				}
			}
		}

		public int Save(ConferenceEntity conference)
		{
			int conferenceId = 0;
			var conferenceEntity = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conference.Slug);
			if (conferenceEntity == null)
			{
				_connection.Insert(conference);
				var entity = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conference.Slug);
				if (entity != null)
				{
					conferenceId = entity.Id;
				}
			}
			else
			{
				//_connection.Delete(conferenceEntity);
				//var conferenceId = _connection.Insert(conference);
				_connection.Update(conference);
				var entity = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conference.Slug);
				if (entity != null)
				{
					conferenceId = entity.Id;
				}
			}

			UpdateConferences();
			UpdateFavorites();
			return conferenceId;
		}

		private void UpdateConferences()
		{
			const string path = "conferencesLastUpdated.json";
			var data = new DataLastUpdated() { LastUpdated = DateTime.Now };
			var json = JsonConvert.SerializeObject(data);
			_fileStore.WriteFile(path, json);
		}

		private void UpdateFavorites()
		{
			const string path = "scheduleLastUpdated.json";
			var data = new DataLastUpdated() { LastUpdated = DateTime.Now };
			var json = JsonConvert.SerializeObject(data);
			_fileStore.WriteFile(path, json);
		}

		public int Delete(ConferenceEntity conference)
		{
			return _connection.Delete(conference);
		}

		public int AddSpeaker(SpeakerEntity speaker)
		{
			int speakerId = 0;
			if (speaker != null && speaker.SessionId != default(int))
			{
				var sessionEntity = _connection.Table<SpeakerEntity>().Where(x => x.SessionId == speaker.SessionId).FirstOrDefault(x => x.Slug == speaker.Slug);
				if (sessionEntity == null)
				{
					_connection.Insert(speaker);
					sessionEntity = _connection.Table<SpeakerEntity>().Where(x => x.SessionId == speaker.SessionId).FirstOrDefault(x => x.Slug == speaker.Slug);
					speakerId = sessionEntity.Id;
				}
				else
				{
					speaker.Id = sessionEntity.Id;
					_connection.Update(speaker);
					sessionEntity = _connection.Table<SpeakerEntity>().Where(x => x.SessionId == speaker.SessionId).FirstOrDefault(x => x.Slug == speaker.Slug);
					speakerId = sessionEntity.Id;
				}
			}

			return speakerId;
		}

		public Task<IList<SpeakerEntity>> GetSpeakersAsync(int sessionId)
		{
			var taskSource = new TaskCompletionSource<IList<SpeakerEntity>>();
			taskSource.SetResult(GetSpeakers(sessionId));
			return taskSource.Task;
		}

		private IList<SpeakerEntity> GetSpeakers(int sessionId)
		{
			var speakers = _connection.Table<SpeakerEntity>().Where(x => x.SessionId == sessionId).ToList();

			return speakers;
		}

		public int AddSession(SessionEntity session)
		{
			int sessionId = 0;
			if (session != null && session.ConferenceId != default(int))
			{
				var sessionEntity = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == session.ConferenceId).FirstOrDefault(x => x.Slug == session.Slug);
				if (sessionEntity == null)
				{
					_connection.Insert(session);
					sessionEntity = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == session.ConferenceId).FirstOrDefault(x => x.Slug == session.Slug);
					sessionId = sessionEntity.Id;
				}
				else
				{
					session.Id = sessionEntity.Id;
					_connection.Update(session);
					sessionEntity = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == session.ConferenceId).FirstOrDefault(x => x.Slug == session.Slug);
					sessionId = sessionEntity.Id;
				}
			}

			return sessionId;
		}

		public SessionEntity Get(string conferenceSlug, string sessionSlug)
		{
			SessionEntity returnValue = null;
			var conference = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
			returnValue = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == conference.Id).FirstOrDefault(x => x.Slug == sessionSlug);
			return returnValue;
		}

		public ConferenceEntity Get(string conferenceSlug)
		{
			ConferenceEntity conference = null;
			conference = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
			return conference;
		}

		private IList<ConferenceEntity> GetFavorites()
		{
			var conferences = _connection.Table<ConferenceEntity>().Where(x => x.IsAddedToSchedule).ToList();

			return conferences;
		}

		public Task<IList<ConferenceEntity>> ListFavoritesAsync()
		{
			var taskSource = new TaskCompletionSource<IList<ConferenceEntity>>();
			taskSource.SetResult(GetFavorites());
			return taskSource.Task;
		}

		private IList<SessionEntity> ListFavoriteSessions(string conferenceSlug)
		{
			var conference = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
			var conferenceId = conference.Id;
			var sessions = _connection.Table<SessionEntity>().Where(x => x.ConferenceId == conferenceId).Where(x => x.IsAddedToSchedule).ToList();

			return sessions;
		}
		public Task<IList<SessionEntity>> ListFavoriteSessionsAsync(string conferenceSlug)
		{
			var taskSource = new TaskCompletionSource<IList<SessionEntity>>();
			taskSource.SetResult(ListFavoriteSessions(conferenceSlug));
			return taskSource.Task;
		}

		private IList<ConferenceEntity> List()
		{
			var filterDate = DateTime.Now.Date.AddDays(-1);
			var conferences = _connection.Table<ConferenceEntity>().Where(x => x.End >= filterDate).OrderBy(x => x.Start).ToList();
			return conferences;
		}

		public Task<IList<ConferenceEntity>> ListAsync()
		{
			var taskSource = new TaskCompletionSource<IList<ConferenceEntity>>();
			taskSource.SetResult(List());
			return taskSource.Task;
		}
	}
}