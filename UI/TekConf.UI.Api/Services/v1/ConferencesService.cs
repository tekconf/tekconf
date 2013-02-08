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
				var search = GetSearchTermSearch(searchTerm);
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
				var searchTermSearch = GetSearchTermSearch(searchTerm);

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
				var searchTermExpression = GetSearchTermSearch(searchTerm);
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
			//Prerun();

			if (request.showOnlyFeatured)
				return GetFeaturedConferences(request);

			return GetAllConferences(request);
		}

		//private void Prerun()
		//{
		//		try
		//		{
		//				var confs = _conferenceRepository.AsQueryable()
		//						.ToList();

		//				var toDelete = confs.Where(x => x.slug.ToLower().StartsWith("temp")).ToList();

		//				foreach (var conf in toDelete)
		//				{
		//						_conferenceRepository.Remove(conf._id);
		//				}

		//		}
		//		catch (Exception ex)
		//		{
		//				throw;
		//		}

		//}

		private object GetAllConferences(Conferences request)
		{
			string searchCacheKey = string.IsNullOrWhiteSpace(request.search) ? string.Empty : request.search.Trim();
			string sortByCacheKey = request.sortBy ?? string.Empty;
			string openCallsCacheKey = request.showOnlyWithOpenCalls.ToString() ?? string.Empty;
			string showPastConferencesCacheKey = request.showPastConferences.ToString() ?? string.Empty;
			string showOnlyOnSaleCacheKey = request.showOnlyOnSale.ToString() ?? string.Empty;

			var cacheKey = "GetAllConferences-" + searchCacheKey + "-" + sortByCacheKey + "-" + showPastConferencesCacheKey + "-" + showOnlyOnSaleCacheKey + "-" + openCallsCacheKey;
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);

			var result = base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
				var orderByFunc = GetOrderByFunc(request.sortBy);
				var search = GetSearchTermSearch(request.search);
				var showPastConferences = GetShowPastConferences(request.showPastConferences);
				var showOnlyOpenCalls = GetShowOnlyOpenCalls(request.showOnlyWithOpenCalls);
				var showOnlyOnSale = GetShowOnlyOnSale(request.showOnlyOnSale);

				var query = _conferenceRepository
					.AsQueryable();

				if (search != null)
				{
					query = query.Where(search);
				}

				if (showPastConferences != null)
				{
					query = query.Where(showPastConferences);
				}

				if (showOnlyOpenCalls != null)
				{
					query = query.Where(showOnlyOpenCalls);
				}

				if (showOnlyOnSale != null)
				{
					query = query.Where(showOnlyOnSale);
				}

				query = query.Where(c => c.isLive);

				List<ConferencesDto> conferencesDtos = null;
				List<ConferenceEntity> conferences = null;
				try
				{
					if (request.sortBy == "dateAdded")
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
				}
				catch (Exception ex)
				{
					var e = ex.Message;
					throw;
				}

				return conferencesDtos.ToList();
			});

			return result;
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


		private Expression<Func<ConferenceEntity, bool>> GetSearchTermSearch(string search)
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