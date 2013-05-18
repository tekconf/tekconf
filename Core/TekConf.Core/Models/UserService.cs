using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Newtonsoft.Json;

namespace TekConf.Core.Models
{
	public class UserService
	{
		private readonly Action<string> _success;
		private readonly Action<Exception> _error;
		private string _userId;
		private string _endpointUri;

		private UserService(Action<string> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		public static void GetIsOauthUserRegisteredAsync(string userId, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetIsOauthUserRegistered(userId, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError));
		}

		private static void DoAsyncGetIsOauthUserRegistered(string userId, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			var search = new UserService(getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError);
			search.StartGetIsOauthUserRegistered(userId);
		}

		private void StartGetIsOauthUserRegistered(string providerId)
		{
			try
			{
				string providerName = "";
				string userName = "";
				if (providerId.ToLower().Contains("twitter"))
				{
					providerName = "twitter";
					userName = providerId.ToLower().Replace("twitter:", "");
				}

				var uri = string.Format("http://www.tekconf.com/account/IsOAuthUserRegistered?providerName={0}&userId={1}", providerName, userName);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Method = "GET";
				request.Accept = "application/json";

				request.BeginGetResponse(ReadGetIsOauthUserRegisteredCallback, request);
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void ReadGetIsOauthUserRegisteredCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetIsOauthUserRegisteredNotificationResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandleGetIsOauthUserRegisteredNotificationResponse(string response)
		{
			var message = JsonConvert.DeserializeObject<UserRegistration>(response);

			if (message != null && message.username != null)
				_success(message.username);
			else	
				_success("");

		}

		private class UserRegistration
		{
			public string username { get; set; }
		}

	}
}