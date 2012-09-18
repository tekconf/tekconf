using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferencesIO.LocalData.iOS;
using System.Diagnostics;
using System.IO;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;
using Catnap.Migration;
using Catnap.Database;
using Catnap.Logging;

namespace ConferencesIO.LocalData.Shared
{

//	public class LocalDatabase : ILocalDatabase{
//
//		public ISessionFactory CreateDatabase ()
//		{
//			var monoSqliteAdapter = new SqliteAdapter(typeof(Mono.Data.Sqlite.SqliteConnection));
//			return BootstrapEmbeddedDb(new CreateSchema_Sqlite(), monoSqliteAdapter, null);
//		}
//
//		private static ISessionFactory BootstrapEmbeddedDb(IDatabaseMigration createSchema, IDbAdapter dbAdapter, Action<string> createDatabaseFunc)
//		{
//			Log.Level = LogLevel.Off;
//			const string dbFileName = "conferences.db3";
//			var connectionString = "Data source=" + Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), dbFileName);
//			
//			File.Delete(dbFileName);
//			if (createDatabaseFunc != null)
//			{
//				createDatabaseFunc(connectionString);
//			}
//
//			//var monoSqliteAdapter = new SqliteAdapter(typeof(Mono.Data.Sqlite.SqliteConnection));
//			var sessionFactory = Fluently.Configure
//				.ConnectionString(connectionString)
//					.DatabaseAdapter(dbAdapter)
//					.Domain(d =>
//					        {
//							d.IdConvention(x => "Id")
//							.Access(Access.Property)
//								.Generator(Generator.GuidComb);
//						
//						d.ListParentIdColumnNameConvention(x => x.ParentType.Name + "_id");
//						d.BelongsToColumnNameConvention(x => x.PropertyName + "_id");
//						
//						d.Entity<ConferenceEntity>(c => {
//							c.Property(p => p.description);
//							//c.Property(p => p.end);
//							c.Property(p => p.facebookUrl);
//							c.Property(p => p.homepageUrl);
//							c.Property(p => p.lanyrdUrl);
//							c.Property(p => p.location);
//							c.Property(p => p.meetupUrl);
//							c.Property(p => p.name);
//							c.Property(p => p.slug);
//							//c.Property(p => p.start);
//							c.Property(p => p.twitterHashTag);
//							c.Property(p => p.twitterName);
//							//c.List(l => l.Sessions).Lazy(false);
//						});
//
//						d.Entity<SessionEntity>(s => {
//							s.Property(p => p.description);
//							s.Property(p => p.difficulty);
//							s.Property(p => p.end);
//							s.Property(p => p.room);
//							s.Property(p => p.sessionType);
//							s.Property(p => p.slug);
//							s.Property(p => p.start);
//							s.Property(p => p.title);
//							s.Property(p => p.twitterHashTag);
//
//						});
//
//						d.Entity<SessionSpeakerEntity>(s => {
//							s.Property(p => p.SessionId);
//							s.Property(p => p.SpeakerId);
//						});
//
//						d.Entity<SpeakerEntity>(s => {
//							s.Property(p => p.blogUrl);
//							s.Property(p => p.description);
//							s.Property(p => p.emailAddress);
//							s.Property(p => p.facebookUrl);
//							s.Property(p => p.firstName);
//							s.Property(p => p.lastName);
//							s.Property(p => p.linkedInUrl);
//							s.Property(p => p.phoneNumber);
//							s.Property(p => p.slug);
//							s.Property(p => p.twitterName);
//						});
//					})
//					.Build();
//
//			UnitOfWork.Initialize(sessionFactory);
//			using (var s = sessionFactory.Create())
//			{
//				s.Open();
//				new DatabaseMigratorUtility(s).Migrate(createSchema);
//			}
//			return sessionFactory;
//		}
//
//		public void SaveSessions (IEnumerable<SessionEntity> sessions)
//		{
//			throw new NotImplementedException ();
//		}
//
//		public void SaveConference (ConferenceEntity conference, IEnumerable<SessionEntity> sessions, IEnumerable<SpeakerEntity> speakers)
//		{
//			throw new NotImplementedException ();
//		}
//	
//	}
}
