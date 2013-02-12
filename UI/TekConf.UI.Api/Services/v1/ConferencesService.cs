using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TinyMessenger;

namespace TekConf.UI.Api.Services.v1
{

	public class ConferencesService : MongoServiceBase
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<ConferenceEntity> _conferenceRepository;
		private readonly IRepository<GeoLocationEntity> _geolocationRepository;
		private readonly IConfiguration _configuration;
		public ICacheClient CacheClient { get; set; }

		public ConferencesService(ITinyMessengerHub hub, IRepository<ConferenceEntity> conferenceRepository, IRepository<GeoLocationEntity> geolocationRepository, IConfiguration configuration)
		{
			_hub = hub;
			_conferenceRepository = conferenceRepository;
			_geolocationRepository = geolocationRepository;
			_configuration = configuration;
		}

		public object Get(ConferencesCount request)
		{
			//TODO : Add search back in
			var showPastConferences = GetShowPastConferences(request.showPastConferences);
			var query = _conferenceRepository.AsQueryable()
								 .Where(c => c.isLive);

			if (!string.IsNullOrWhiteSpace(request.searchTerm))
			{
				var searchTerm = request.searchTerm.ToLower();
				var search = GetSearchTermSearchForMongo(searchTerm);
				if (search != null)
				{
					query = query.Where(search);
				}
			}

			if (showPastConferences != null)
			{
				query = query.Where(showPastConferences);
			}

			var count = query
								 .Count();

			return count;
		}

		public object Get(Search request)
		{
			List<SearchResultDto> searchResults = null;

			if (request.latitude.HasValue && request.longitude.HasValue)
			{
				if (!request.distance.HasValue)
					request.distance = 100.0;

				searchResults = SearchByLatLong(request.latitude.Value, request.longitude.Value, request.distance.Value, request.searchTerm, request.showPastConferences);
			}
			else if (!string.IsNullOrWhiteSpace(request.city) && !string.IsNullOrWhiteSpace(request.state))
			{
				if (!request.distance.HasValue)
					request.distance = 100.0;

				if (string.IsNullOrWhiteSpace(request.country))
					request.country = "US";

				var city = _geolocationRepository.AsQueryable()
																				 .Where(g => Regex.IsMatch(g.name, request.city, RegexOptions.IgnoreCase))
																				 .Where(g => Regex.IsMatch(g.fipscode, request.state, RegexOptions.IgnoreCase))
																				 .ToList()
																				 .FirstOrDefault(x => x.name.ToLower() == request.city.ToLower());

				if (city != null)
					searchResults = SearchByLatLong(city.latitude, city.longitude, request.distance.Value, request.searchTerm, request.showPastConferences);

			}
			else
			{
				var searchTerm = request.searchTerm.ToLower();
				var searchTermSearch = GetSearchTermSearchForMongo(searchTerm);

				var showPastConferences = GetShowPastConferences(request.showPastConferences);

				var query = _conferenceRepository.AsQueryable()
									 .Where(searchTermSearch)
									 .Where(c => c.isLive);

				if (showPastConferences != null)
				{
					query = query.Where(showPastConferences);
				}

				searchResults = query
										.Select(c => new SearchResultDto() { label = c.name, value = c.slug })
									 .ToList()
									 .OrderBy(s => s.label)
									 .ToList();


			}

			return searchResults;
		}

		private List<SearchResultDto> SearchByLatLong(double latitude, double longitude, double distance, string searchTerm, bool? showPastConferences)
		{
			var conferences = (_conferenceRepository as ConferenceRepository).GeoSearch(latitude,
																																				longitude,
																																				distance);


			var query = conferences.AsQueryable()
				 .Where(c => c.isLive);

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				searchTerm = searchTerm.ToLower();
				var searchTermExpression = GetSearchTermSearchForMongo(searchTerm);
				if (searchTermExpression != null)
				{
					query = query.Where(searchTermExpression);
				}
			}
			var showPastConferencesExpression = GetShowPastConferences(showPastConferences);
			if (showPastConferencesExpression != null)
			{
				query = query.Where(showPastConferencesExpression);
			}

			var searchResults = query
									.Select(c => new SearchResultDto() { label = c.name, value = c.slug })
								 .ToList()
								 .OrderBy(s => s.label)
								 .ToList();

			return searchResults;
		}

		public object Get(Conferences request)
		{
			if (request.showOnlyFeatured)
				return GetFeaturedConferences(request);

			return GetAllConferences(request);
		}

		private object GetAllConferences(Conferences request)
		{
			var cacheKey = GetCacheKey(request);

			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			List<ConferencesDto> conferencesDtos = null;

			var result = base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
				{
					if (request.latitude.HasValue && request.longitude.HasValue)
					{
						if (!request.distance.HasValue)
						{
							request.distance = 100.0;
						}

						var conferences = (_conferenceRepository as ConferenceRepository).GeoSearch(request.latitude.Value,
																																				request.longitude.Value,
																																				request.distance.Value);
						var searchExpression = GetSearchTermSearchForInMemory(request.search);
						conferencesDtos = BuildConferencesSearch(conferences.AsQueryable(), searchExpression, request.sortBy, request.search, request.showPastConferences, request.showOnlyWithOpenCalls, request.showOnlyOnSale);
					}
					else if (!string.IsNullOrWhiteSpace(request.city) && !string.IsNullOrWhiteSpace(request.state))
					{
						if (!request.distance.HasValue)
							request.distance = 100.0;

						if (string.IsNullOrWhiteSpace(request.country))
							request.country = "US";

						var city = _geolocationRepository.AsQueryable()
																						 .Where(g => Regex.IsMatch(g.name, request.city, RegexOptions.IgnoreCase))
																						 .Where(g => Regex.IsMatch(g.fipscode, request.state, RegexOptions.IgnoreCase))
																						 .ToList()
																						 .FirstOrDefault(x => x.name.ToLower() == request.city.ToLower());

						if (city != null)
						{
							var conferences = (_conferenceRepository as ConferenceRepository).GeoSearch(city.latitude,
																														city.longitude,
																														request.distance.Value);
							var searchExpression = GetSearchTermSearchForInMemory(request.search);

							conferencesDtos = BuildConferencesSearch(conferences.AsQueryable(), searchExpression, request.sortBy, request.search, request.showPastConferences, request.showOnlyWithOpenCalls, request.showOnlyOnSale);
						}

					}
					else
					{
						var searchExpression = GetSearchTermSearchForMongo(request.search);

						conferencesDtos = BuildConferencesSearch(_conferenceRepository.AsQueryable(), searchExpression, request.sortBy, request.search, request.showPastConferences, request.showOnlyWithOpenCalls, request.showOnlyOnSale);
					}


					return conferencesDtos;
				});

			return result;
		}


		private List<ConferencesDto> BuildConferencesSearch(IQueryable<ConferenceEntity> query, Expression<Func<ConferenceEntity, bool>> searchExpression, string sortBy, string searchTerm, bool? showPastConferences, bool? showOnlyWithOpenCalls, bool? showOnlyOnSale)
		{
			var orderByFunc = GetOrderByFunc(sortBy);
			var showPastConferencesExpression = GetShowPastConferences(showPastConferences);
			var showOnlyOpenCallsExpression = GetShowOnlyOpenCalls(showOnlyWithOpenCalls);
			var showOnlyOnSaleExpression = GetShowOnlyOnSale(showOnlyOnSale);

			//var query = _conferenceRepository
			//	.AsQueryable();

			if (searchExpression != null)
			{
				query = query.Where(searchExpression);
			}

			if (showPastConferencesExpression != null)
			{
				query = query.Where(showPastConferencesExpression);
			}

			if (showOnlyOpenCallsExpression != null)
			{
				query = query.Where(showOnlyOpenCallsExpression);
			}

			if (showOnlyOnSaleExpression != null)
			{
				query = query.Where(showOnlyOnSaleExpression);
			}

			query = query.Where(c => c.isLive);

			List<ConferencesDto> conferencesDtos = null;
			List<ConferenceEntity> conferences = null;

			if (sortBy == "dateAdded")
			{
				query = query.OrderByDescending(orderByFunc).ThenBy(c => c.start).AsQueryable();
			}
			else
			{
				query = query.OrderBy(orderByFunc).ThenBy(c => c.start).AsQueryable();
			}

			conferences = query
				.ToList();

			conferencesDtos = Mapper.Map<List<ConferencesDto>>(conferences);
			return conferencesDtos;
		}

		private static string GetCacheKey(Conferences request)
		{
			string searchCacheKey = string.IsNullOrWhiteSpace(request.search) ? string.Empty : request.search.Trim();
			string sortByCacheKey = string.IsNullOrWhiteSpace(request.sortBy) ? string.Empty : request.sortBy.Trim();
			string openCallsCacheKey = request.showOnlyWithOpenCalls.HasValue ? request.showOnlyWithOpenCalls.Value.ToString() : string.Empty;
			string showPastConferencesCacheKey = request.showPastConferences.HasValue ? request.showPastConferences.Value.ToString() : string.Empty;
			string showOnlyOnSaleCacheKey = request.showOnlyOnSale.HasValue ? request.showOnlyOnSale.Value.ToString() : string.Empty;
			string cityCacheKey = string.IsNullOrWhiteSpace(request.city) ? string.Empty : request.city.Trim();
			string stateCacheKey = string.IsNullOrWhiteSpace(request.state) ? string.Empty : request.state.Trim();
			string countryCacheKey = string.IsNullOrWhiteSpace(request.country) ? string.Empty : request.country.Trim();
			string latitudeCacheKey = request.latitude.HasValue ? request.latitude.Value.ToString() : string.Empty;
			string longitudeCacheKey = request.longitude.HasValue ? request.longitude.Value.ToString() : string.Empty;
			string distanceCacheKey = request.distance.HasValue ? request.distance.Value.ToString() : string.Empty;


			var cacheKey = "GetAllConferences-" + 
										searchCacheKey + "-" + 
										sortByCacheKey + "-" + 
										showPastConferencesCacheKey + "-" +
										 showOnlyOnSaleCacheKey + "-" + 
										 openCallsCacheKey + "-" +
										 cityCacheKey + "-" +
										 stateCacheKey + "-" +
										 countryCacheKey + "-" +
										 latitudeCacheKey + "-" +
										 longitudeCacheKey + "-" +
										 distanceCacheKey
										 ;
			return cacheKey;
		}

		private object GetFeaturedConferences(Conferences request)
		{
			var cacheKey = "GetFeaturedConferences";
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
				List<ConferenceEntity> conferences;
				try
				{
					conferences = _conferenceRepository
							.AsQueryable()
							.Where(c => c.end >= DateTime.Now.AddDays(-7))
							.Where(c => c.isLive)
							.OrderBy(c => c.start)
							.ToList()
							.Where(c => !string.IsNullOrWhiteSpace(c.description))
							.Take(4)
							.ToList();
				}
				catch (Exception ex)
				{
					var s = ex.Message;
					throw;
				}


				var conferencesDtos = Mapper.Map<List<FullConferenceDto>>(conferences);

				return conferencesDtos.ToList();
			});
		}


		private Expression<Func<ConferenceEntity, bool>> GetShowPastConferences(bool? showPastConferences)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			//Only show current conferences
			if (showPastConferences == null || !(bool)showPastConferences)
			{
				searchBy = c => c.end >= DateTime.Now.AddDays(-3);
			}

			return searchBy;
		}

		private Expression<Func<ConferenceEntity, bool>> GetShowOnlyOpenCalls(bool? showOnlyOpenCalls)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (showOnlyOpenCalls.HasValue && showOnlyOpenCalls.Value)
			{
				searchBy = c => c.callForSpeakersOpens <= DateTime.Now && c.callForSpeakersCloses >= DateTime.Now;
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

		private Expression<Func<ConferenceEntity, bool>> GetSearchTermSearchForInMemory(string search)
		{
			Expression<Func<ConferenceEntity, bool>> searchBy = null;

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.Trim().ToLower();
				//var regex = new Regex(search, RegexOptions.IgnoreCase);

				searchBy = c => c.name.ToLower().Contains(search)
						|| (c.slug == null || c.slug.ToLower().Contains(search))
						|| (c.description == null || c.description.ToLower().Contains(search))
						|| (c.address == null || c.address.City == null || c.address.City.ToLower().Contains(search))
						|| (c.address == null || c.address.Country == null || c.address.Country.ToLower().Contains(search))
						|| c.sessions.Any(s => (s.description == null || s.description.ToLower().Contains(search)))
						|| c.sessions.Any(s => (s.title == null || s.title.ToLower().Contains(search)))
						|| c.sessions.Any(s => s.speakers.Any(sp => (sp.lastName == null || sp.lastName.ToLower().Contains(search))))
						|| c.sessions.Any(s => s.speakers.Any(sp => (sp.twitterName == null || sp.twitterName.ToLower().Contains(search))))
						;
			
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
	}

	public class SessionResult
	{
		public DateKey DateKey { get; set; }
		public SessionEntity Session { get; set; }
	}

	public class DateKey
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
	}

}