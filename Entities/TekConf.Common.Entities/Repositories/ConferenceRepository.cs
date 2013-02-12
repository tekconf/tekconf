using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using TinyMessenger;

namespace TekConf.UI.Api
{

		public class GeoConferenceEntity : ConferenceEntity
		{
				public GeoConferenceEntity(ITinyMessengerHub hub, IRepository<ConferenceEntity> repository)
						: base(hub, repository)
				{

				}

				public double Distance { get; set; }
		}

		public class ConferenceRepository : IRepository<ConferenceEntity>
		{
				private readonly IConfiguration _configuration;

				public ConferenceRepository(IConfiguration configuration)
				{
						_configuration = configuration;
						CreateIndexes();
				}

				private void CreateIndexes()
				{
						var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");

						//collection.EnsureIndex(new string[] { "name" });
				}
				public void Save(ConferenceEntity entity)
				{
						var collection = MongoCollection();
						collection.Save(entity);
				}

				public IQueryable<ConferenceEntity> AsQueryable()
				{
						var collection = MongoCollection();
						return collection.AsQueryable();
				}

				public void Remove(Guid id)
				{
						var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");
						collection.Remove(Query.EQ("_id", id));
				}

				public List<ConferenceEntity> GeoSearch(double latitude, double longitude, double rangeInMiles)
				{
						List<ConferenceEntity> conferences = null;
						var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");

						var earthRadius = 3959.0; // miles

						var options = GeoNearOptions
								.SetMaxDistance(rangeInMiles / earthRadius /* to radians */)
								.SetSpherical(true);

						var results = collection.GeoNear(
								null,
								longitude,// note the order
								latitude,  // [lng, lat]
								200,
								options
						);

						if (results != null)
						{
								conferences = results.Hits.Select(x => x.Document).ToList();
								foreach (var conference in conferences)
								{
										var firstOrDefault = results.Hits.FirstOrDefault(x => x.Document.slug == conference.slug);
										if (firstOrDefault != null)
												conference.distance = firstOrDefault.Distance * earthRadius;
								}
						}



						return conferences;
				}
				private MongoCollection<ConferenceEntity> MongoCollection()
				{
						var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");
						return collection;
				}

				private MongoServer _localServer;
				private MongoDatabase _localDatabase;
				private MongoDatabase LocalDatabase
				{
						get
						{
								if (_localServer == null)
								{
										var mongoServer = _configuration.MongoServer;
										_localServer = MongoServer.Create(mongoServer);
								}

								if (_localDatabase == null)
								{
										_localDatabase = _localServer.GetDatabase("tekconf");

								}
								return _localDatabase;
						}
				}
		}
}