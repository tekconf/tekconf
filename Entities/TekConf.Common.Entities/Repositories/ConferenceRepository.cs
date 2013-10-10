using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Win32;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using TekConf.Common.Entities.Repositories;

namespace TekConf.Common.Entities
{
	public class ConferenceRepository : IConferenceRepository
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public ConferenceRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
			CreateIndexes();
		}

		public int GetConferenceCount(string searchTerm, bool? showPastConferences)
		{
			var shouldShowPastConferences = GetShowPastConferences(showPastConferences);
			var query = this.AsQueryable()
													 .Where(c => c.isLive);

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				searchTerm = searchTerm.ToLower();
				var search = GetSearchTermSearchForMongo(searchTerm);
				if (search.IsNotNull())
				{
					query = query.Where(search);
				}
			}

			if (shouldShowPastConferences.IsNotNull())
			{
				query = query.Where(shouldShowPastConferences);
			}

			var count = query.Count();

			return count;

		}

		public IEnumerable<SpeakerEntity> GetFeaturedSpeakers()
		{
			var featuredSpeakers = this.AsQueryable()
																			.Where(c => c.isLive)
																			.ToList()
																			.Where(c => c.sessions != null)
																			.SelectMany(c => c.sessions)
																			.Where(s => s.speakers != null)
																			.SelectMany(s => s.speakers)
																			.Where(s => s.isFeatured)
																			.Where(s => !string.IsNullOrWhiteSpace(s.description))
																			.Distinct()
																			.Take(3);

			return featuredSpeakers;
		}

		public IEnumerable<ConferenceEntity> GetFeaturedConferences()
		{
			var conferences = this
									.AsQueryable()
									.Where(c => c.end >= DateTime.Now.Date)
									.Where(c => c.isLive)
									.OrderBy(c => c.start)
									.ToList()
									.Where(c => !string.IsNullOrWhiteSpace(c.description))
									.Take(4)
									.ToList();

			return conferences;
		}

		public IEnumerable<ConferenceEntity> GetNewestConferences()
		{
			var conferences = this
									.AsQueryable()
									.Where(c => c.end >= DateTime.Now.AddDays(-1))
									.Where(c => c.isLive)
									.OrderByDescending(c => c.dateAdded)
									.ToList()
									.Where(c => !string.IsNullOrWhiteSpace(c.description))
									.Take(3)
									.ToList();

			return conferences;
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

			const double earthRadius = 3959.0; // miles

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

			var onlineConferences = collection.AsQueryable().Where(x => x.isOnline == true);
			var hs = new HashSet<ConferenceEntity>(onlineConferences, new ConferenceComparer());
			if (conferences != null)
				hs.UnionWith(conferences);
			
			var merged = hs.ToList();
			return merged;
		}

		public IEnumerable<ConferenceEntity> GetConferences(string search, string sortBy, bool? showPastConferences, bool? showOnlyWithOpenCalls, bool? showOnlyOnSale, bool showOnlyFeatured, double? longitude, double? latitude, double? distance, string city, string state, string country)
		{
			IEnumerable<ConferenceEntity> conferenceEntities = null;

			if (latitude.HasValue && longitude.HasValue)
			{
				if (!distance.HasValue)
				{
					distance = 100.0;
				}

				var conferences = this.GeoSearch(latitude.Value,
																														longitude.Value,
																														distance.Value);
				var searchExpression = GetSearchTermSearchForInMemory(search);
				conferenceEntities = BuildConferencesSearch(conferences.AsQueryable(), searchExpression, sortBy, search, showPastConferences, showOnlyWithOpenCalls, showOnlyOnSale);
			}
			else if (!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(state))
			{
				if (!distance.HasValue)
					distance = 100.0;

				if (string.IsNullOrWhiteSpace(country))
					country = "US";

				IEntityConfiguration entityConfiguration = new EntityConfiguration();
				IGeoLocationRepository geoLocationRepository = new GeoLocationRepository(entityConfiguration);
				var cityEntity = geoLocationRepository.AsQueryable()
																				.Where(g => Regex.IsMatch(g.name, city, RegexOptions.IgnoreCase))
																				.Where(g => Regex.IsMatch(g.fipscode, state, RegexOptions.IgnoreCase))
																				.ToList()
																				.FirstOrDefault(x => x.name.ToLower() == city.ToLower());

				if (cityEntity.IsNotNull())
				{
					var conferences = this.GeoSearch(cityEntity.latitude,
																											cityEntity.longitude,
																											distance.Value);
					var searchExpression = GetSearchTermSearchForInMemory(search);

					conferenceEntities = BuildConferencesSearch(conferences.AsQueryable(), searchExpression, sortBy, search, showPastConferences, showOnlyWithOpenCalls, showOnlyOnSale);
				}

			}
			else
			{
				var searchExpression = GetSearchTermSearchForMongo(search);

				conferenceEntities = BuildConferencesSearch(this.AsQueryable(), searchExpression, sortBy, search, showPastConferences, showOnlyWithOpenCalls, showOnlyOnSale);
			}

			return conferenceEntities;
		}

		public SessionEntity SaveSession(string conferenceSlug, string originalSessionSlug, SessionEntity session)
		{
			var conference = this.AsQueryable().FirstOrDefault(c => c.slug.ToLower() == conferenceSlug.ToLower());
			SessionEntity returnSession = null;
			string slug = "";
			if (conference != null)
			{
				var existingSession = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == originalSessionSlug.ToLower());
				if (existingSession != null)
				{
					existingSession.TrimAllProperties();
					Mapper.Map(session, existingSession);
					existingSession.slug = existingSession.title.GenerateSlug();
					slug = existingSession.slug;
				}
				else
				{
					conference.AddSession(session);
					session.slug = session.title.GenerateSlug();
					slug = session.slug;
				}
				conference.Save();
				returnSession = conference.sessions.SingleOrDefault(s => s.slug == slug);
			}

			return returnSession;
		}

		private Expression<Func<ConferenceEntity, bool>> GetShowPastConferences(bool? showPastConferences)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			//Only show current conferences
			if (showPastConferences.IsNull() || !(bool)showPastConferences)
			{
				searchBy = c => c.end >= DateTime.Now.AddDays(-3);
			}

			return searchBy;
		}

		private Expression<Func<ConferenceEntity, bool>> GetSearchTermSearchForMongo(string search)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.Trim();
				var regex = new Regex(search, RegexOptions.IgnoreCase);

				searchBy = c => Regex.IsMatch(c.name, search, RegexOptions.IgnoreCase)
								|| Regex.IsMatch(c.slug, search, RegexOptions.IgnoreCase)
								|| Regex.IsMatch(c.description, search, RegexOptions.IgnoreCase)
								|| Regex.IsMatch(c.address.City, search, RegexOptions.IgnoreCase)
								|| Regex.IsMatch(c.address.Country, search, RegexOptions.IgnoreCase)
								|| c.sessions.Any(s => Regex.IsMatch(s.description, search, RegexOptions.IgnoreCase))
								|| c.sessions.Any(s => Regex.IsMatch(s.title, search, RegexOptions.IgnoreCase))
								|| c.sessions.Any(s => s.speakers.Any(sp => Regex.IsMatch(sp.lastName, search, RegexOptions.IgnoreCase)))
								|| c.sessions.Any(s => s.speakers.Any(sp => Regex.IsMatch(sp.twitterName, search, RegexOptions.IgnoreCase)))
								;
			}

			return searchBy;
		}

		private Expression<Func<ConferenceEntity, bool>> GetSearchTermSearchForInMemory(string search)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.Trim().ToLower();
				//var regex = new Regex(search, RegexOptions.IgnoreCase);

				searchBy = c => c.name.ToLower().Contains(search)
								|| (c.slug.IsNull() || c.slug.ToLower().Contains(search))
								|| (c.description.IsNull() || c.description.ToLower().Contains(search))
								|| (c.address.IsNull() || c.address.City.IsNull() || c.address.City.ToLower().Contains(search))
								|| (c.address.IsNull() || c.address.Country.IsNull() || c.address.Country.ToLower().Contains(search))
								|| c.sessions.Any(s => (s.description.IsNull() || s.description.ToLower().Contains(search)))
								|| c.sessions.Any(s => (s.title.IsNull() || s.title.ToLower().Contains(search)))
								|| c.sessions.Any(s => s.speakers.Any(sp => (sp.lastName.IsNull() || sp.lastName.ToLower().Contains(search))))
								|| c.sessions.Any(s => s.speakers.Any(sp => (sp.twitterName.IsNull() || sp.twitterName.ToLower().Contains(search))))
								;

			}

			return searchBy;
		}

		private IEnumerable<ConferenceEntity> BuildConferencesSearch(IQueryable<ConferenceEntity> query, Expression<Func<ConferenceEntity, bool>> searchExpression, string sortBy, string searchTerm, bool? showPastConferences, bool? showOnlyWithOpenCalls, bool? showOnlyOnSale)
		{
			var orderByFunc = GetOrderByFunc(sortBy);
			var showPastConferencesExpression = GetShowPastConferences(showPastConferences);
			var showOnlyOpenCallsExpression = GetShowOnlyOpenCalls(showOnlyWithOpenCalls);
			var showOnlyOnSaleExpression = GetShowOnlyOnSale(showOnlyOnSale);

			//var query = _conferenceRepository
			//	.AsQueryable();

			if (searchExpression.IsNotNull())
			{
				query = query.Where(searchExpression);
			}

			if (showPastConferencesExpression.IsNotNull())
			{
				query = query.Where(showPastConferencesExpression);
			}

			if (showOnlyOpenCallsExpression.IsNotNull())
			{
				query = query.Where(showOnlyOpenCallsExpression);
			}

			if (showOnlyOnSaleExpression.IsNotNull())
			{
				query = query.Where(showOnlyOnSaleExpression);
			}

			query = query.Where(c => c.isLive);

			List<ConferenceEntity> conferences = null;

			if (sortBy == "dateAdded")
			{
				query = query.OrderByDescending(orderByFunc).ThenBy(c => c.start).AsQueryable();
			}
			else
			{
				query = query.OrderBy(orderByFunc).ThenBy(c => c.start).AsQueryable();
			}

			conferences = query.ToList();

			return conferences;
		}

		private Func<ConferenceEntity, object> GetOrderByFunc(string sortBy)
		{
			Func<ConferenceEntity, object> orderByFunc = null;

			if (sortBy == "startDate")
			{
				orderByFunc = c => c.start;
			}
			else if (sortBy == "name")
			{
				orderByFunc = c => c.name;
			}
			else if (sortBy == "callForSpeakersOpeningDate")
			{
				orderByFunc = c => c.callForSpeakersOpens;
			}
			else if (sortBy == "callForSpeakersClosingDate")
			{
				orderByFunc = c => c.callForSpeakersCloses;
			}
			else if (sortBy == "registrationOpens")
			{
				orderByFunc = c => c.registrationOpens;
			}
			else if (sortBy == "dateAdded")
			{
				orderByFunc = c => c.datePublished;
			}
			else
			{
				orderByFunc = c => c.end;
			}

			return orderByFunc;
		}

		private Expression<Func<ConferenceEntity, bool>> GetShowOnlyOpenCalls(bool? showOnlyOpenCalls)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (showOnlyOpenCalls.HasValue && showOnlyOpenCalls.Value)
			{
				searchBy = c => c.callForSpeakersOpens <= DateTime.Now.AddMonths(1) && c.callForSpeakersCloses >= DateTime.Now;
			}

			return searchBy;
		}

		private Expression<Func<ConferenceEntity, bool>> GetShowOnlyOnSale(bool? showOnlyOnSale)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (showOnlyOnSale.HasValue && showOnlyOnSale.Value)
			{
				searchBy = c => c.registrationOpens <= DateTime.Now && c.registrationCloses >= DateTime.Now;
			}

			return searchBy;
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
					var mongoServer = this._entityConfiguration.MongoServer;
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

	class ConferenceComparer : IEqualityComparer<ConferenceEntity>
	{
		public bool Equals(ConferenceEntity c1, ConferenceEntity c2)
		{
			return c1.slug == c2.slug;
		}

		public int GetHashCode(ConferenceEntity c)
		{
			return c.slug.GetHashCode();
		}
	}
}