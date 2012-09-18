using System;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;
using TekConf.LocalData.Shared;
using System.IO;
using System;
using Catnap.Database;
using Catnap;
using Catnap.Migration;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TekConf.LocalData.iOS
{
//	public class LocalDatabaseBootstrapper
//	{
//		public void Initialize()
//		{
//			InitializeCatnap();
//		}
//
//		private void InitializeCatnap ()
//		{
//			ILocalDatabase db = new LocalDatabase();
//			db.CreateDatabase();
//		}
//	}

	public class LocalDatabaseBootstrapper
	{
		private ISessionFactory sessionFactory {get;set;}

		public void Initialize ()
		{
			//InitializeCatnap ();
			MapEntities ();
			CreateDatabase ();
		}
		
//		private void InitializeCatnap ()
//		{
//			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
//			
//			string db = Path.Combine (documents, "codemash.db3");
//			//TestFlight.PassCheckpoint ("Initialized CatNap");
//			//Catnap.SessionFactory.Initialize ("Data Source=" + db, new SqliteAdapter (typeof(Mono.Data.Sqlite.SqliteConnection)));
//		}
		
		private void MapEntities ()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, "conferences.db3");
			var connectionString = "Data Source=" + db;

			var monoSqliteAdapter = new SqliteAdapter(typeof(Mono.Data.Sqlite.SqliteConnection));
			var sessionFactory = Fluently.Configure
				.ConnectionString(connectionString)
					.DatabaseAdapter(monoSqliteAdapter)
					.Domain(d =>
					        {
								d.IdConvention(x => "Id")
								.Access(Access.Property)
								.Generator(Generator.GuidComb);
						
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
						
						d.Entity<SessionEntity>(s => {
							s.Property(p => p.description);
							s.Property(p => p.difficulty);
							s.Property(p => p.end);
							s.Property(p => p.room);
							s.Property(p => p.sessionType);
							s.Property(p => p.slug);
							s.Property(p => p.start);
							s.Property(p => p.title);
							s.Property(p => p.twitterHashTag);
							
						});
						
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
					})
					.Build();
			
			UnitOfWork.Initialize(sessionFactory);


//			Log.Level = LogLevel.Off;
//			Domain.Configure
//				(
//					Map.Entity<SessionEntity> ()
//					.Table (SessionEntity.TableName)
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.URI))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.Title))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.Abstract))
//					.Map (new ValuePropertyMap<SessionEntity, DateTime> (x => x.Start))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.Room))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.Difficulty))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerName))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.Technology))
//					.Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerURI)),
//					
//					Map.Entity<SpeakerEntity>()
//					.Table(SpeakerEntity.TableName)
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.SpeakerURI))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Name))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Biography))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.TwitterHandle))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.BlogURL)),
//					
//					Map.Entity<RefreshEntity>()
//					.Table (RefreshEntity.TableName)
//					.Map (new ValuePropertyMap<RefreshEntity, DateTime> (x => x.LastRefreshTime))
//					.Map (new ValuePropertyMap<RefreshEntity, string> (x => x.EntityName)),
//					
//					Map.Entity<ScheduledSessionEntity> ()
//					.Table (ScheduledSessionEntity.TableName)
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.URI))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Title))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Abstract))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, DateTime> (x => x.Start))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Room))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Difficulty))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerName))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Technology))
//					.Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerURI)),
//					
//					Map.Entity<RemoteQueueEntity> ()
//					.Table (RemoteQueueEntity.TableName)
//					.Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.URI))
//					.Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.ConferenceName))
//					.Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.UserName))
//					.Map (new ValuePropertyMap<RemoteQueueEntity, DateTime> (x => x.DateQueuedOn))
//					.Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.AddOrRemove))
//					
//					
//					);

			this.sessionFactory = sessionFactory;
		}
		
		private void CreateDatabase ()
		{
			var monoSqliteAdapter = new SqliteAdapter(typeof(Mono.Data.Sqlite.SqliteConnection));
			var createSchema = new CreateSchema_Sqlite();

			using (var s = this.sessionFactory.Create())
			{
				s.Open();
				new DatabaseMigratorUtility(s).Migrate(createSchema);
			}
		}
		
	}



}

