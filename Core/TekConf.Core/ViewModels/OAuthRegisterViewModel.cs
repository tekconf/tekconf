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
		private readonly IMessageBox _messageBox;
		private readonly INetworkConnection _networkConnection;

		public OAuthRegisterViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, 
			IAuthentication authentication, IMvxMessenger messenger, IMvxFileStore fileStore, 
			ILocalNotificationsRepository localNotificationsRepository, IPushSharpClient pushSharpClient,
			IMessageBox messageBox, INetworkConnection networkConnection)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			_fileStore = fileStore;
			_localNotificationsRepository = localNotificationsRepository;
			_pushSharpClient = pushSharpClient;
			_messageBox = messageBox;
			_networkConnection = networkConnection;
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
			if (!_networkConnection.IsNetworkConnected())
			{
				InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
			}
			else
			{
				var userName = await _remoteDataService.CreateOauthUser(_userProviderId, UserName);
				if (!string.IsNullOrWhiteSpace(userName))
				{
					_messenger.Publish(new AuthenticationMessage(this, userName));
					ShowViewModel<ConferencesListViewModel>();
				}
			}
		}

	}
}