using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class SettingsViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;

		public SettingsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
		}

		public void Init(string fake)
		{
		}

		public void IsOauthUserRegistered(string userId)
		{
			_remoteDataService.GetIsOauthUserRegistered(userId, GetIsOauthUserRegisteredSuccess, GetIsOauthUserRegisteredError);
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
		private void GetIsOauthUserRegisteredSuccess(string userName)
		{
			_messenger.Publish(new AuthenticationMessage(this, userName));
			this.UserName = userName;
			//InvokeOnMainThread(() => DisplayAllConferences(enumerable));
		}

		private void GetIsOauthUserRegisteredError(Exception exception)
		{
			// for now we just hide the error...
			//IsLoadingConferences = false;
		}
	}
}