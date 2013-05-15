using System;
using System.Collections.Generic;
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

		public SettingsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
		}

		public void Init(string fake)
		{
		}

		//private bool _oauthUserIsRegistered;
		//public bool OauthUserIsRegistered
		//{
		//	get
		//	{
		//		return _oauthUserIsRegistered;
		//	}
		//	set
		//	{
		//		_oauthUserIsRegistered = value;
		//		RaisePropertyChanged(() => OauthUserIsRegistered);
		//	}
		//}

		public void IsOauthUserRegistered(string userId)
		{
			_remoteDataService.GetIsOauthUserRegistered(userId, GetIsOauthUserRegisteredSuccess, GetIsOauthUserRegisteredError);
		}

		private void GetIsOauthUserRegisteredSuccess(bool isOauthUserRegistered)
		{
			//InvokeOnMainThread(() => DisplayAllConferences(enumerable));
		}

		private void GetIsOauthUserRegisteredError(Exception exception)
		{
			// for now we just hide the error...
			//IsLoadingConferences = false;
		}
	}
}