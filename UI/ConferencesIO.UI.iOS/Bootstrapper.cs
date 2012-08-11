using System;
using ConferencesIO.LocalData.iOS;

namespace ConferencesIO.UI.iOS
{
	public class Bootstrapper
	{
		public void Initialize ()
		{
			InitializeLocalDatabase();
		}

		void InitializeLocalDatabase ()
		{
			var localDatabaseBootstrapper = new LocalDatabaseBootstrapper();
			localDatabaseBootstrapper.Initialize();
		}
	}
}

