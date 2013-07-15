using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Newtonsoft.Json;
using TekConf.Core.Entities;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;
using System.Linq;

namespace TekConf.Core.Repositories
{
	public class LocalConferencesRepository : ILocalConferencesRepository
	{
		private readonly ISQLiteConnection _connection;

		public LocalConferencesRepository(ISQLiteConnection connection)
		{
			_connection = connection;
			_connection.CreateTable<ConferenceEntity>();
			_connection.CreateTable<SessionEntity>();
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

		public void Save(ConferenceEntity conference)
		{
			var entity = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conference.Slug);
			if (entity == null)
			{
				_connection.Insert(conference);
			}
			else
			{
				_connection.Delete(entity);
				_connection.Insert(conference);
			}
		}

		public void AddSession(SessionEntity session)
		{
			if (session != null && session.ConferenceId != default(int))
			{
				var entity = _connection.Table<SessionEntity>().Where(x => x.Id == session.Id).FirstOrDefault(x => x.Slug == session.Slug);
				if (entity == null)
				{
					_connection.Insert(session);
				}
				else
				{
					_connection.Update(session);
				}
			}
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
		public IList<ConferenceEntity> GetFavorites()
		{
			var conferences = _connection.Table<ConferenceEntity>().Where(x => x.IsAddedToSchedule).ToList();

			return conferences;
		}


		public IList<ConferenceEntity> List()
		{
			var filterDate = DateTime.Now.Date.AddDays(-1);
			var conferences = _connection.Table<ConferenceEntity>().Where(x => x.End >= filterDate).OrderBy(x => x.Start).ToList();
			return conferences;
		}
	}
}