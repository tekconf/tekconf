using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferenceService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxReachability _reachability;
		private readonly bool _isRefreshing;
		private readonly Action<FullConferenceDto> _success;
		private readonly Action<Exception> _error;
		private string _slug;

		private ConferenceService(IMvxFileStore fileStore, IMvxReachability reachability,  bool isRefreshing, Action<FullConferenceDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_reachability = reachability;
			_isRefreshing = isRefreshing;
			_success = success;
			_error = error;
		}

		public static void GetConferenceAsync(IMvxFileStore fileStore, IMvxReachability reachability, string slug, bool isRefreshing, Action<FullConferenceDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetConference(fileStore, reachability, slug, isRefreshing, success, error));
		}

		private static void DoAsyncGetConference(IMvxFileStore fileStore, IMvxReachability reachability, string slug, bool isRefreshing, Action<FullConferenceDto> success, Action<Exception> error)
		{
			var search = new ConferenceService(fileStore, reachability, isRefreshing, success, error);
			search.StartGetConferenceSearch(slug);
		}

		private void StartGetConferenceSearch(string slug)
		{
			_slug = slug;

			try
			{
				var path = _slug + ".json";

				if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var conference = JsonConvert.DeserializeObject<FullConferenceDto>(response);
						_success(conference);
					}
				}
				else
				{
					//if (_reachability.IsHostReachable("http://api.tekconf.com"))
					//{
						var uri = string.Format("http://api.tekconf.com/v1/conferences/{0}?format=json", slug);
						var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
						request.Accept = "application/json";
						request.BeginGetResponse(ReadGetConferenceCallback, request);
					//}
					//else
					//{
						//_error(new Exception("Please connect to the network"));
					//}
				}


			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		

		private void ReadGetConferenceCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetConferenceResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}


		private void HandleGetConferenceResponse(string response)
		{
			var path = _slug + ".json";
			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}
			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}
			var conferences = JsonConvert.DeserializeObject<FullConferenceDto>(response);
			_success(conferences);
		}



	}
}