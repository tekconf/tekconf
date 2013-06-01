using System;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
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

		private bool _isRegistering;
		public bool IsRegistering
		{
			get
			{
				return _isRegistering;
			}
			set
			{
				_isRegistering = value;
				RaisePropertyChanged(() => IsRegistering);
			}
		}

		private string _userName;
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				_userName = value;
				RaisePropertyChanged(() => UserName);
			}
		}

		private string _userProviderId;
		public void CreateOAuthUser()
		{
			_remoteDataService.CreateOauthUser(_userProviderId, UserName, GetCreateOAuthUserSuccess, GetCreateOAuthUserError);
		}

		private void GetCreateOAuthUserSuccess(string userName)
		{
			if (!string.IsNullOrWhiteSpace(userName))
			{
				_messenger.Publish(new AuthenticationMessage(this, userName));
				UserName = userName;
			}
			else
			{
				//ShowOAuthRegisterCommand.Execute(null);
			}
		}

		private void GetCreateOAuthUserError(Exception exception)
		{
			// for now we just hide the error...
			//IsLoadingConferences = false;
			_messenger.Publish(new ExceptionMessage(this, exception));
		}
	}
}