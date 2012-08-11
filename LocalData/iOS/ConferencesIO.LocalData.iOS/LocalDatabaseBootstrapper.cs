using System;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;

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
			var monoSqliteAdapter = new SqliteAdapter(typeof(Mono.Data.Sqlite.SqliteConnection));


			var sessionFactory = Fluently.Configure
				.ConnectionString("Data source=MyDatabase.sqlite")
					.DatabaseAdapter(monoSqliteAdapter)
					.Domain(d =>
					        {
						d.IdConvention(x => x.EntityType.Name + "Id")
							.Access(Access.Property)
								.Generator(Generator.GuidComb);
						d.ListParentIdColumnNameConvention(x => x.ParentType.Name + "_id");
						d.BelongsToColumnNameConvention(x => x.PropertyName + "_id");
						
						d.Entity<Person>(e => {
							e.Property(x => x.FirstName);
							e.Property(x => x.LastName);
							e.Property(x => x.Active);
							e.Property(x => x.MemberSince);
						});
						d.Entity<Forum>(e => {
							e.List(x => x.Posts).ParentIdColumn("PostedTo_id");
							e.Property(x => x.Name);
							e.Property(x => x.TimeOfDayLastUpdated);
						});
						d.Entity<Post>(e => {
							e.Property(x => x.Title);
							e.Property(x => x.Body);
							e.Property(x => x.DatePosted);
							e.BelongsTo(x => x.Poster).Column("Poster_id");
						});
					})
					.Build();
			
			UnitOfWork.Initialize(sessionFactory);
		}
	}
}

