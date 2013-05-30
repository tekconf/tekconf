using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;

namespace TekConf.Core.ViewModels
{
	public class TekConfLoginViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAuthentication _authentication;

		public TekConfLoginViewModel(IRemoteDataService remoteDataService, IAuthentication authentication)
		{
			_remoteDataService = remoteDataService;
			_authentication = authentication;
		}

		public void Init()
		{
			
		}

		public void Login()
		{
			IsLoggingIn = true;
			_remoteDataService.LoginWithTekConf(UserName, Password, LoginSuccess, LoginError);
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}

		private void LoginSuccess(bool isLoggedIn, string userName)
		{
			IsLoggingIn = false;
			ShowSettingsCommand.Execute(null);
			UserName = userName;
			_authentication.UserName = userName;
		}

		private void LoginError(Exception exception)
		{
			IsLoggingIn = false;
		}

		private bool _isLoggingIn;
		public bool IsLoggingIn
		{
			get
			{
				return _isLoggingIn;
			}
			set
			{
				_isLoggingIn = value;
				RaisePropertyChanged(() => IsLoggingIn);
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

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
			}
		}
	}
}