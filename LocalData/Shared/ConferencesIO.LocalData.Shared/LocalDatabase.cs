using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferencesIO.LocalData.iOS;
using SQLite;
using System.Diagnostics;
using System.IO;

namespace ConferencesIO.LocalData.Shared
{
	public class LocalDatabase : SQLiteConnection, ILocalDatabase
	{
		public LocalDatabase (string path) : base(path)
		{
			//var connectionString = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "conferences.db");

			//var _db = new Database (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "conferences.db"));
			//var conn = new SQLiteAsyncConnection(connectionString);
		}

		public bool CreateDatabase ()
		{
			CreateTable<ConferenceEntity>();
			CreateTable<SessionEntity>();

			return true;
		}

		public void SaveSessions (IEnumerable<SessionEntity> sessions)
		{
			InsertAll(sessions);
		}

		public void SaveConference(ConferenceEntity conference)
		{
			Insert(conference);
		}
	}
}
