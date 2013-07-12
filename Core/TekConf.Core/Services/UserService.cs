using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using TekConf.Core.Messages;
using TekConf.Core.ViewModels;

namespace TekConf.Core.Models
{
	public class UserService
	{
		private readonly IMvxMessenger _messenger;
		private readonly Action<string> _success;
		private readonly Action<bool, string> _loginSuccess;

		private readonly Action<Exception> _error;

		private UserService(IMvxMessenger messenger, Action<string> success, Action<Exception> error)
		{
			_messenger = messenger;
			_success = success;
			_error = error;
		}

		private UserService(IMvxMessenger messenger, Action<bool, string> success, Action<Exception> error)
		{
			_messenger = messenger;
			_loginSuccess = success;
			_error = error;
		}

		public static void GetAuthenticationAsync(string userName, string password, IMvxMessenger messenger, Action<bool, string> getAuthenticationSuccess, Action<Exception> getAuthenticationError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetAuthentication(userName, password, messenger, getAuthenticationSuccess, getAuthenticationError));
		}

		public static void GetIsOauthUserRegisteredAsync(string userId, IMvxMessenger messenger, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetIsOauthUserRegistered(userId, messenger, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError));
		}

		public static void CreateOauthUserAsync(string userId, string userName, IMvxMessenger messenger, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncCreateOauthUser(userId, userName, messenger, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError));
		}

		private static void DoAsyncGetAuthentication(string userName, string password, IMvxMessenger messenger, Action<bool, string> getAuthenticationSuccess, Action<Exception> getAuthenticationError)
		{
			var search = new UserService(messenger, getAuthenticationSuccess, getAuthenticationError);
			search.StartGetAuthentication(userName, password);
		}

		private static void DoAsyncGetIsOauthUserRegistered(string userName, IMvxMessenger messenger, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			var search = new UserService(messenger, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError);
			search.StartGetIsOauthUserRegistered(userName);
		}

		private static void DoAsyncCreateOauthUser(string userId, string userName, IMvxMessenger messenger, Action<string> getIsOauthUserRegisteredSuccess, Action<Exception> getIsOauthUserRegisteredError)
		{
			var search = new UserService(messenger, getIsOauthUserRegisteredSuccess, getIsOauthUserRegisteredError);
			search.StartCreateOauthUser(userId, userName);
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
				else if (providerId.ToLower().Contains("facebook"))
				{
					providerName = "facebook";
					userName = providerId.ToLower().Replace("facebook:", "");
				}
				else if (providerId.ToLower().Contains("google"))
				{
					providerName = "google";
					userName = providerId.ToLower().Replace("google:", "");
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

		private void StartCreateOauthUser(string providerId, string userName)
		{
			try
			{
				string providerName = "";
				string userId = "";
				if (providerId.ToLower().Contains("twitter"))
				{
					providerName = "twitter";
					userId = providerId.ToLower().Replace("twitter:", "");
				}
				else if (providerId.ToLower().Contains("facebook"))
				{
					providerName = "facebook";
					userName = providerId.ToLower().Replace("facebook:", "");
				}
				else if (providerId.ToLower().Contains("google"))
				{
					providerName = "google";
					userName = providerId.ToLower().Replace("google:", "");
				}

				var uri = string.Format(App.WebRootUri + "account/CreateOauthUser?providerName={0}&userId={1}&userName={2}", providerName, userId, userName);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Method = "POST";
				//request.Accept = "application/json";

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
				var uri = string.Format(App.WebRootUri + "account/MobileLogin?UserName={0}&Password={1}", userName, password);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Method = "POST";
				//request.ContentType = "application/x-www-form-urlencoded";
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
			var result = JsonConvert.DeserializeObject<MobileLoginResult>(response);
			if (result.IsLoggedIn)
			{
				_messenger.Publish(new AuthenticationMessage(this, result.UserName));
				_loginSuccess(true, result.UserName);
			}
			else
			{
				_error(new Exception("Login Failed"));
			}
		}

		private class UserRegistration
		{
			public string username { get; set; }
		}

		public class MobileLoginResult
		{
			public string UserName { get; set; }
			public bool IsLoggedIn { get; set; }
		}
	}
}