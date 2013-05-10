using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class PushService
	{
		private readonly bool _isRefreshing;
		private readonly Action<bool> _success;
		private readonly Action<Exception> _error;
		private string _userName;
		private string _endpointUri;

		private PushService(bool isRefreshing, Action<bool> success, Action<Exception> error)
		{
			_isRefreshing = isRefreshing;
			_success = success;
			_error = error;
		}

		public static void PostWindowsPhonePushNotificationAsync(string userName, string endpointUri, bool isRefreshing, Action<bool> postWindowsPhonePushNotificationSuccess, Action<Exception> postWindowsPhonePushNotificationError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncPostWindowsPhonePushNotification(userName, endpointUri, isRefreshing, postWindowsPhonePushNotificationSuccess, postWindowsPhonePushNotificationError));
		}

		private static void DoAsyncPostWindowsPhonePushNotification(string userName, string endpointUri, bool isRefreshing, Action<bool> postWindowsPhonePushNotificationSuccess, Action<Exception> postWindowsPhonePushNotificationError)
		{
			var search = new PushService(isRefreshing, postWindowsPhonePushNotificationSuccess, postWindowsPhonePushNotificationError);
			search.StartPostWindowsPhonePushNotification(userName, endpointUri);
		}

		private void StartPostWindowsPhonePushNotification(string userName, string endpointUri)
		{
			try
			{
				_userName = userName;
				_endpointUri = endpointUri;

				var uri = string.Format("http://api.tekconf.com/v1/push/{0}/wp?endpointUri={1}", userName, endpointUri);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Method = "POST";
				request.Accept = "application/json";

				request.BeginGetResponse(ReadPostWindowsPhonePushNotificationCallback, request);
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void ReadPostWindowsPhonePushNotificationCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandlePostWindowsPhonePushNotificationResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandlePostWindowsPhonePushNotificationResponse(string response)
		{
			_success(true);
		}


	}
}