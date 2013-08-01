using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace TekConf.Core.ViewModels
{
	public class SettingsViewModel : MvxViewModel
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

		public SettingsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, 
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

		public void Init(string fake)
		{
			RaisePropertyChanged(() => IsAuthenticated);
			RaisePropertyChanged(() => ConferenceUpdated);
			RaisePropertyChanged(() => SchedulesUpdated);

		}

		private string _userProviderId;
		public async void IsOauthUserRegistered(string userId)
		{
			_userProviderId = userId;
			if (!_networkConnection.IsNetworkConnected())
			{
				InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
			}
			else
			{
				var userName = await _remoteDataService.GetIsOauthUserRegistered(userId);

				GetIsOauthUserRegisteredSuccess(userName);
			}
		}

		public bool IsAuthenticated
		{
			get { return _authentication.IsAuthenticated; }
		}

		public bool IsOptedInToNotifications
		{
			get
			{
				return _localNotificationsRepository.IsOptedInToNotifications;
			}
			set
			{
				_localNotificationsRepository.IsOptedInToNotifications = value;
				if (value)
					_pushSharpClient.Register();
				else
					_pushSharpClient.Unregister();

				RaisePropertyChanged(() => IsOptedInToNotifications);
			}
		}

		public string SchedulesUpdated
		{
			get
			{
				string json;
				if (_fileStore.TryReadTextFile("scheduleLastUpdated.json", out json))
				{
					var lastUpdated = JsonConvert.DeserializeObject<DataLastUpdated>(json);

					return lastUpdated.LastUpdated.ToString();
				}
				return "Unknown";
			}
		}

		public string ConferenceUpdated
		{
			get
			{
				string json;
				if (_fileStore.TryReadTextFile("conferencesLastUpdated.json", out json))
				{
					var lastUpdated = JsonConvert.DeserializeObject<DataLastUpdated>(json);

					return lastUpdated.LastUpdated.ToString();
				}
				return "Unknown";
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
				RaisePropertyChanged(() => IsAuthenticated);
			}
		}

		private void GetIsOauthUserRegisteredSuccess(string userName)
		{
			if (!string.IsNullOrWhiteSpace(userName))
			{
				_messenger.Publish(new AuthenticationMessage(this, userName));
				//UserName = userName;
				//ShowViewModel<ConferencesListViewModel>();
			}
			else
			{
				ShowOAuthRegisterCommand.Execute(null);
			}
		}

		private void GetIsOauthUserRegisteredError(Exception exception)
		{
			_messenger.Publish(new SettingsIsOauthUserRegisteredExceptionMessage(this, exception));
		}

		public ICommand ShowOAuthRegisterCommand
		{
			get
			{
				return new MvxCommand(() =>
					ShowViewModel<OAuthRegisterViewModel>(new { providerId = _userProviderId })
					);
			}
		}

		public ICommand ShowTekConfLoginCommand
		{
			get
			{
				return new MvxCommand(() =>
					ShowViewModel<TekConfLoginViewModel>()
					);
			}
		}
	}

	public class DataLastUpdated
	{
		public DateTime LastUpdated { get; set; }
	}
}