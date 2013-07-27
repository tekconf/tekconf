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
		private readonly IMessageBox _messageBox;
		private readonly INetworkConnection _networkConnection;

		public TekConfLoginViewModel(IRemoteDataService remoteDataService, IAuthentication authentication, IMessageBox messageBox, 
			INetworkConnection networkConnection)
		{
			_remoteDataService = remoteDataService;
			_authentication = authentication;
			_messageBox = messageBox;
			_networkConnection = networkConnection;
		}

		public void Init()
		{
			
		}

		public async void Login()
		{
			IsLoggingIn = true;
			if (!_networkConnection.IsNetworkConnected())
			{
				InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
			}
			else
			{
				var result = await _remoteDataService.LoginWithTekConf(UserName, Password);
				LoginSuccess(result.IsLoggedIn, result.UserName);
			}
		}

		public ICommand ShowConferencesListCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ConferencesListViewModel>());
			}
		}

		private void LoginSuccess(bool isLoggedIn, string userName)
		{
			IsLoggingIn = false;
			UserName = userName;
			_authentication.UserName = userName;
			ShowConferencesListCommand.Execute(null);
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