using System;
using NUnit.Framework;
using ConferencesIO.LocalData.Shared;
using System.IO;
using Catnap;
using ConferencesIO.LocalData.iOS;
using System.Collections.Generic;

namespace ConferencesIO.UI.iOS.Tests.Int
{
	[TestFixture]
	public class LocalDatabaseFixtures
	{
		private string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "conferences.db");
		
		public LocalDatabaseFixtures ()
		{

		}


		[Test]
		public void Should_create_database()
		{
			CreateDatabase ();
		}

		[Test]
		public void should_insert_conference()
		{
			CreateDatabase ();
			InsertConference();
			ListConferences(true);
		}

		[Test]
		public void Should_list_conferences()
		{
			CreateDatabase ();
			ListConferences(false);
		}

		void CreateDatabase ()
		{
			if (File.Exists(dbPath))
			{
				File.Delete(dbPath);
			}
			Assert.False (File.Exists(dbPath), "db not deleted");

			ILocalDatabase db = new LocalDatabase ();
			try {
				db.CreateDatabase ();
			}
			catch (Exception e) {
				var message = e.Message;
				var stackTrace = e.StackTrace;
				var x = "";
			}
			Assert.True (File.Exists (dbPath), "db does not exist");
		}

		void InsertConference()
		{
			using (var uow = UnitOfWork.Start())
			{
				var conference = new ConferenceEntity()
				{
					description = "description",
					end = DateTime.Now,
					facebookUrl = "facebookUrl",
					name = "name",
					Sessions = new List<SessionEntity>(),
				};
				uow.Session.SaveOrUpdate(conference);
			}
		}

		void ListConferences(bool shouldCheckCount)
		{
			IList<ConferenceEntity> conferences = null;
			using (var uow = UnitOfWork.Start())
			{
				conferences = UnitOfWork.Current.Session.List<ConferenceEntity>();
				
			}
			
			Assert.IsNotNull(conferences);
			if (shouldCheckCount)
			{
				Assert.IsTrue(conferences.Count > 0);
			}
		}
	}
}

