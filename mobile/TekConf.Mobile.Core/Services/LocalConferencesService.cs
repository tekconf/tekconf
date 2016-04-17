using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fusillade;
using Polly;
using Tekconf.DTO;
using Plugin.Connectivity;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core.Services
{
	public class LocalConferencesService : ILocalConferencesService
	{
		private SQLiteAsyncConnection _connection;

		public LocalConferencesService(IMvxSqliteConnectionFactory sqliteConnectionFactory)
		{
			var databaseName = "tekconf.db3";
			_connection = sqliteConnectionFactory.GetAsyncConnection(databaseName);

		}

		public async Task Init()
		{
			await _connection.CreateTablesAsync<ConferenceModel, SessionModel>();
		}

		public async Task<List<ConferenceModel>> GetConferences()
		{
			return await _connection.Table<ConferenceModel>().ToListAsync();
		}

		public async Task<int> Save(List<ConferenceModel> models)
		{
			await _connection.DeleteAllAsync<ConferenceModel>();

			return await _connection.InsertAllAsync(models);
		}
	}
	
}