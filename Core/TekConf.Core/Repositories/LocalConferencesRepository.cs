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

		public LocalConferencesRepository(ISQLiteConnectionFactory factory)
		{
			_connection = factory.Create("conferences.db");
			_connection.CreateTable<ConferenceEntity>();
			_connection.CreateTable<SessionEntity>();
		}

		public void Save(IEnumerable<ConferenceEntity> conferences)
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
				_connection.Update(conference);
			}

		}

		public void AddSession(SessionEntity session)
		{
			if (session != null && session.ConferenceId != default (int))
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
			var conference = _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
			return _connection.Table<SessionEntity>().Where(x => x.ConferenceId == conference.Id).FirstOrDefault(x => x.Slug == sessionSlug);
		}

		public ConferenceEntity Get(string conferenceSlug)
		{
			return _connection.Table<ConferenceEntity>().FirstOrDefault(x => x.Slug == conferenceSlug);
		}

		public IEnumerable<ConferenceEntity> List()
		{
			return _connection.Table<ConferenceEntity>();
		}

	}
}