using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;

namespace TekConf.Core.Models
{
	public class PushService
	{
		private readonly Action<bool> _success;
		private readonly Action<Exception> _error;

		private PushService(Action<bool> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		public static void PostWindowsPhonePushNotificationAsync(string userName, string endpointUri, Action<bool> postWindowsPhonePushNotificationSuccess, Action<Exception> postWindowsPhonePushNotificationError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncPostWindowsPhonePushNotification(userName, endpointUri, postWindowsPhonePushNotificationSuccess, postWindowsPhonePushNotificationError));
		}

		private static void DoAsyncPostWindowsPhonePushNotification(string userName, string endpointUri, Action<bool> postWindowsPhonePushNotificationSuccess, Action<Exception> postWindowsPhonePushNotificationError)
		{
			var search = new PushService(postWindowsPhonePushNotificationSuccess, postWindowsPhonePushNotificationError);
			search.StartPostWindowsPhonePushNotification(userName, endpointUri);
		}

		private void StartPostWindowsPhonePushNotification(string userName, string endpointUri)
		{
			try
			{
				var uri = string.Format(App.ApiRootUri + "push/{0}/wp?endpointUri={1}", userName, endpointUri);
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
					streamReader1.ReadToEnd();
					HandlePostWindowsPhonePushNotificationResponse();
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandlePostWindowsPhonePushNotificationResponse()
		{
			_success(true);
		}


	}
}