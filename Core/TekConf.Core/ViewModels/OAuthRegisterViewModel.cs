using System;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace TekConf.Core.ViewModels
{
	public class OAuthRegisterViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;
		private readonly IMvxFileStore _fileStore;
		private readonly ILocalNotificationsRepository _localNotificationsRepository;
		private readonly IPushSharpClient _pushSharpClient;

		public OAuthRegisterViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger, IMvxFileStore fileStore, ILocalNotificationsRepository localNotificationsRepository, IPushSharpClient pushSharpClient)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			_fileStore = fileStore;
			_localNotificationsRepository = localNotificationsRepository;
			_pushSharpClient = pushSharpClient;
		}

		public void Init(string providerId)
		{
			_userProviderId = providerId;
		}

		public bool IsRegistering { get; set; }
		public string UserName { get; set; }

		private string _userProviderId;
		public async void CreateOAuthUser()
		{
			var userName = await _remoteDataService.CreateOauthUser(_userProviderId, UserName);
			GetCreateOAuthUserSuccess(userName);
		}

		private void GetCreateOAuthUserSuccess(string userName)
		{
			if (!string.IsNullOrWhiteSpace(userName))
			{
				_messenger.Publish(new AuthenticationMessage(this, this.UserName));
				//UserName = userName;
				ShowViewModel<ConferencesListViewModel>();
			}
			else
			{
				//ShowOAuthRegisterCommand.Execute(null);
			}
		}

		private void GetCreateOAuthUserError(Exception exception)
		{
			//IsLoadingConferences = false;
			_messenger.Publish(new CreateOAuthUserExceptionMessage(this, exception));
		}
	}
}