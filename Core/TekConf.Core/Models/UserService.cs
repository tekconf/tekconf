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
		private readonly Action<bool, string> _loginSuccess;

		private readonly Action<Exception> _error;

		private UserService(Action<string> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		private UserService(Action<bool, string> success, Action<Exception> error)
		{
			_loginSuccess = success;
			_error = error;
		}

		public static void GetAuthenticationAsync(string userName, string password, Action<bool, string> getAuthenticationSuccess, Action<Exception> getAuthenticationError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetAuthentication(userName, password, getAuthenticationSuccess, getAuthenticationError));
		}

		public static void GetIsOauthUserRegisteredAsync(string userId, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetIsOauthUserRegistered(userId, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError));
		}

		private static void DoAsyncGetAuthentication(string userName, string password, Action<bool, string> getAuthenticationSuccess, Action<Exception> getAuthenticationError)
		{
			var search = new UserService(getAuthenticationSuccess, getAuthenticationError);
			search.StartGetAuthentication(userName, password);
		}

		private static void DoAsyncGetIsOauthUserRegistered(string userName, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			var search = new UserService(getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError);
			search.StartGetIsOauthUserRegistered(userName);
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

				var uri = string.Format(App.WebRootUri + "account/IsOAuthUserRegistered?providerName={0}&userId={1}", providerName, userName);
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

		private string _userName;
		
		private void StartGetAuthentication(string userName, string password)
		{
			try
			{
				_userName = userName;
				var uri = string.Format(App.WebRootUri + "account/Login?UserName={0}&Password={1}&RememberMe={2}&returnUrl=", userName, password, true);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				//request.Accept = "application/json";

				request.BeginGetResponse(ReadGetAuthenticationCallback, request);
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

		private void ReadGetAuthenticationCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetIsAuthenticationResponse(resultString);
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

		private void HandleGetIsAuthenticationResponse(string response)
		{
			_loginSuccess(true, _userName);
		}

		private class UserRegistration
		{
			public string username { get; set; }
		}

	}
}