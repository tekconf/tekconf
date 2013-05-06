using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferencesService
	{
		private const string ConferencesUrl = "http://api.tekconf.com/v1/conferences?format=json";

		private readonly IMvxFileStore _fileStore;
		private readonly bool _isRefreshing;
		private readonly Action<IEnumerable<FullConferenceDto>> _success;
		private readonly Action<Exception> _error;

		private ConferencesService(IMvxFileStore fileStore, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_isRefreshing = isRefreshing;
			_success = success;
			_error = error;
		}

		public static void GetConferencesAsync(IMvxFileStore fileStore, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetAllSearch(fileStore, isRefreshing, success, error));
		}

		private static void DoAsyncGetAllSearch(IMvxFileStore fileStore, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ConferencesService(fileStore, isRefreshing, success, error);
			search.StartSearch();
		}

		private void StartSearch()
		{
			try
			{
				const string path = "conferences.json";

				if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
						_success(conferences.OrderBy(x => x.start).ToList());
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
			const string path = "conferences.json";
			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}

			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
			_success(conferences.OrderBy(x => x.start).ToList());
		}

	}

}
