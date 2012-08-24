using System;
using NUnit.Framework;
using ConferencesIO.LocalData.Shared;
using System.IO;
using Catnap;
using ConferencesIO.LocalData.iOS;
using System.Collections.Generic;
using System.Linq;

namespace ConferencesIO.UI.iOS.Tests.Int
{
	[TestFixture]
	public class LocalDatabaseFixtures
	{
		private string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "conferences.db3");
		
		public LocalDatabaseFixtures ()
		{ 
		}

//		[Test]
//		public void Should_create_database()
//		{
//			CreateDatabase ();
//		}

		[Test]
		public void should_insert_conference ()
		{
			CreateDatabase ();

			InsertSession ();
			ListSessions (true);

		}

//		[Test]
//		public void Should_list_conferences()
//		{
//			CreateDatabase ();
//			ListConferences(false);
//		}

		void CreateDatabase ()
		{
			if (File.Exists (dbPath)) {
				File.Delete (dbPath);
			}
			Assert.False (File.Exists (dbPath), "db not deleted");

			var db = new LocalDatabaseBootstrapper ();
			db.Initialize ();

			Assert.True (File.Exists (dbPath), "db does not exist");
		}

		void InsertSession ()
		{
			if (!UnitOfWork.IsUnitOfWorkStarted ()) {
				using (var uow = UnitOfWork.Start()) {
					var repo = new LocalSessionsRepository ();
					var session = new SessionEntity()
					{
						title = "Test",
					};
					repo.Save(session);
				}
			}


			//sessions = repo.GetForSpeaker (speaker.SpeakerURI);
			//}
//			using (var uow = UnitOfWork.Start())
//			{
//				var conference = new ConferenceEntity()
//				{
//					description = "description",
//					//end = DateTime.Now,
//					facebookUrl = "facebookUrl",
//					name = "name",
//					//Sessions = new List<SessionEntity>(),
//				};
//				uow.Session.SaveOrUpdate(conference);
//			}		
		}

		void ListSessions (bool shouldCheckCount)
		{
//			var db = new LocalDatabaseBootstrapper ();
//			db.Initialize();

			IList<ConferenceEntity> conferences = null;

			if (!UnitOfWork.IsUnitOfWorkStarted ()) {
				using (var uow = UnitOfWork.Start()) {
					var repo = new LocalSessionsRepository ();
					var criteria = Criteria.For<SessionEntity> ();
					var sessions = repo.Find (criteria);;
				}
			} else {
				var repo = new LocalSessionsRepository ();
				var criteria = Criteria.For<SessionEntity> ();
				var sessions = repo.Find (criteria);
			}
			
			Assert.IsNotNull (conferences);
			if (shouldCheckCount) {
				Assert.IsTrue (conferences.Count > 0);
			}
		}
	}
}

