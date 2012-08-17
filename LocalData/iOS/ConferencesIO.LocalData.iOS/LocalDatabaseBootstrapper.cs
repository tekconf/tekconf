using System;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;
using ConferencesIO.LocalData.Shared;

namespace ConferencesIO.LocalData.iOS
{
	public class LocalDatabaseBootstrapper
	{
		public void Initialize()
		{
			InitializeCatnap();
		}

		private void InitializeCatnap ()
		{
			ILocalDatabase db = new LocalDatabase();
			db.CreateDatabase();
		}
	}
}

