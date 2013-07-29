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

		public TekConfLoginViewModel(IRemoteDataService remoteDataService, IAuthentication authentication, IMessageBox messageBox, INetworkConnection networkConnection)
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

		public bool IsLoggingIn { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}