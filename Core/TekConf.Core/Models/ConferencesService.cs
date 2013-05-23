using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferencesService
	{
		private const string ConferencesUrl = "http://api.tekconf.com/v1/conferences?format=json";

		private readonly string _searchTerm;
		private readonly IMvxFileStore _fileStore;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;
		private readonly Action<IEnumerable<FullConferenceDto>> _success;
		private readonly Action<Exception> _error;

		private ConferencesService(string searchTerm, IMvxFileStore fileStore, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_searchTerm = searchTerm;
			_fileStore = fileStore;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_success = success;
			_error = error;
		}

		public static void GetConferencesAsync(string searchTerm, IMvxFileStore fileStore, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetAllSearch(searchTerm, fileStore, isRefreshing, cache, success, error));
		}

		private static void DoAsyncGetAllSearch(string searchTerm, IMvxFileStore fileStore, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var service = new ConferencesService(searchTerm, fileStore, isRefreshing, cache, success, error);
			service.StartGetConferences();
		}

		private void StartGetConferences()
		{
			try
			{
				const string path = "conferences.json";

				//var json = _cache.Get<string, string>("conferences.json");
				//string json = null; 
				List<FullConferenceDto> cachedConferences = null;
				//if (!string.IsNullOrWhiteSpace(json) && !_isRefreshing)
				//{
				//	cachedConferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(json);
				//}

				if (cachedConferences != null)
				{
					var results = SearchConferences(cachedConferences);
					_success(results);
				}
				else if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();
						_cache.Add("conferences.json", response, new TimeSpan(0, 8, 0));

						var results = SearchConferences(conferences);
						_success(results);
					}
				}
				else
				{
					// perform the search
					const string uri = ConferencesUrl;
					var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
					request.Accept = "application/json";
					request.BeginGetResponse(ReadCallback, request);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private IEnumerable<FullConferenceDto> SearchConferences(List<FullConferenceDto> conferences)
		{
			List<FullConferenceDto> filteredConferences;
			if (!string.IsNullOrWhiteSpace(_searchTerm))
			{
				var searchTerm = _searchTerm.ToLower();

				filteredConferences = conferences.Where(
					c => c.name.ToLower().Contains(searchTerm)
					|| c.slug.ToLower().Contains(searchTerm)
					|| (c.description.IsNotNull() && c.description.ToLower().Contains(searchTerm))
					|| (c.address.IsNotNull() && c.address.City.IsNotNull() && c.address.City.ToLower().Contains(searchTerm))
					|| (c.address.IsNotNull() && c.address.Country.IsNotNull() && c.address.Country.ToLower().Contains(searchTerm))
					|| c.sessions.Any(s => (s.description.IsNotNull() && s.description.ToLower().Contains(searchTerm)))
					|| c.sessions.Any(s => (s.title.IsNotNull() && s.title.ToLower().Contains(searchTerm)))
					|| c.sessions.Any(s => s.speakers.Any(sp => (sp.lastName.IsNotNull() && sp.lastName.ToLower().Contains(searchTerm))))
					|| c.sessions.Any(s => s.speakers.Any(sp => (sp.twitterName.IsNotNull() && sp.twitterName.ToLower().Contains(searchTerm))))
					).ToList();
			}
			else
			{
				filteredConferences = conferences;
			}

			return filteredConferences.OrderBy(x => x.start).ToList();
		}

		private void ReadCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandleResponse(string response)
		{
			const string conferencesLastUpdatedPath = "conferencesLastUpdated.json";
			if (_fileStore.Exists(conferencesLastUpdatedPath))
			{
				_fileStore.DeleteFile(conferencesLastUpdatedPath);
			}

			if (!_fileStore.Exists(conferencesLastUpdatedPath))
			{
				var conferencesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
				var json = JsonConvert.SerializeObject(conferencesLastUpdated);
				_fileStore.WriteFile(conferencesLastUpdatedPath, json);
			}

			const string path = "conferences.json";
			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}

			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}

			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();
			_cache.Add("conferences.json", response, new TimeSpan(0, 8, 0));

			var results = SearchConferences(conferences);

			_success(results);
		}

	}

	public static class Extensions
	{
		public static bool IsNotNull(this object value)
		{
			return value != null;
		}

		public static bool IsNull(this object value)
		{
			return value == null;
		}

		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
	}
}
