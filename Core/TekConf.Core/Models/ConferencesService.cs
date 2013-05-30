using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferencesService
	{
		private string ConferencesUrl = App.ApiRootUri + "conferences?format=json";

		private readonly string _searchTerm;
		private readonly IMvxFileStore _fileStore;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;
		private readonly Action<IEnumerable<ConferencesListViewDto>> _success;
		private readonly Action<Exception> _error;

		private ConferencesService(string searchTerm, IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, bool isRefreshing, ICacheService cache, Action<IEnumerable<ConferencesListViewDto>> success, Action<Exception> error)
		{
			_searchTerm = searchTerm;
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_success = success;
			_error = error;
		}

		public static void GetConferencesAsync(string searchTerm, IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, bool isRefreshing, ICacheService cache, Action<IEnumerable<ConferencesListViewDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetAllSearch(searchTerm, fileStore, localConferencesRepository, isRefreshing, cache, success, error));
		}

		private static void DoAsyncGetAllSearch(string searchTerm, IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, bool isRefreshing, ICacheService cache, Action<IEnumerable<ConferencesListViewDto>> success, Action<Exception> error)
		{
			var service = new ConferencesService(searchTerm, fileStore, localConferencesRepository, isRefreshing, cache, success, error);
			service.StartGetConferences();
		}

		private void StartGetConferences()
		{
			try
			{
				var conferences = _localConferencesRepository.GetConferencesListView();
				
				if (conferences != null && !_isRefreshing)
				{
					//var results = SearchConferences(conferences);
					_success(conferences);
				}
				else
				{
					GetConferencesFromWeb();
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void GetConferencesFromWeb()
		{
			string uri = ConferencesUrl;
			var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";
			request.BeginGetResponse(ReadCallback, request);
		}

		//private IEnumerable<FullConferenceDto> SearchConferences(List<FullConferenceDto> conferences)
		//{
		//	List<FullConferenceDto> filteredConferences;
		//	if (!string.IsNullOrWhiteSpace(_searchTerm))
		//	{
		//		var searchTerm = _searchTerm.ToLower();

		//		filteredConferences = conferences.Where(
		//			c => c.name.ToLower().Contains(searchTerm)
		//			|| c.slug.ToLower().Contains(searchTerm)
		//			|| (c.description.IsNotNull() && c.description.ToLower().Contains(searchTerm))
		//			|| (c.address.IsNotNull() && c.address.City.IsNotNull() && c.address.City.ToLower().Contains(searchTerm))
		//			|| (c.address.IsNotNull() && c.address.Country.IsNotNull() && c.address.Country.ToLower().Contains(searchTerm))
		//			|| c.sessions.Any(s => (s.description.IsNotNull() && s.description.ToLower().Contains(searchTerm)))
		//			|| c.sessions.Any(s => (s.title.IsNotNull() && s.title.ToLower().Contains(searchTerm)))
		//			|| c.sessions.Any(s => s.speakers.Any(sp => (sp.lastName.IsNotNull() && sp.lastName.ToLower().Contains(searchTerm))))
		//			|| c.sessions.Any(s => s.speakers.Any(sp => (sp.twitterName.IsNotNull() && sp.twitterName.ToLower().Contains(searchTerm))))
		//			).ToList();
		//	}
		//	else
		//	{
		//		filteredConferences = conferences;
		//	}

		//	return filteredConferences.OrderBy(x => x.start).ToList();
		//}

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
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();

			_localConferencesRepository.SaveConferences(conferences);
			var results = _localConferencesRepository.GetConferencesListView();

			//TODO : var results = SearchConferences(conferences);

			_success(results);
		}

	}
}
