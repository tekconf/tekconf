using System;
using System.ComponentModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Cirrious.MvvmCross.Plugins.Messenger;
using Microsoft.WindowsAzure.MobileServices;
using TekConf.Core.Models;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;
using Cirrious.CrossCore;
using TekConf.UI.WinPhone.Classes;

namespace TekConf.UI.WinPhone.Views
{
	public partial class SettingsView
	{
		private MvxSubscriptionToken _exceptionMessageToken;
		private MvxSubscriptionToken _authenticationMessageToken;
		private MvxSubscriptionToken _settingsExceptionToken;
		private IAuthentication _authentication;

		public SettingsView()
		{
			InitializeComponent();

			var messenger = Mvx.Resolve<IMvxMessenger>();
			_authentication = Mvx.Resolve<IAuthentication>();

			_exceptionMessageToken = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));
			_authenticationMessageToken = messenger.Subscribe<AuthenticationMessage>(OnAuthenticateMessage);

			_settingsExceptionToken = messenger.Subscribe<SettingsIsOauthUserRegisteredExceptionMessage>(message =>
					Dispatcher.BeginInvoke(() =>
					{
						if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
						{
							const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
							MessageBox.Show(errorMessage);
							//SettingsExceptionMessage.Text = errorMessage;
							//SettingsExceptionMessage.Visibility = Visibility.Visible;
						}
					}));

			
			SetLoggedInState();

			Loaded += (sender, args) =>
			{
				var xmlDocument = XDocument.Load("WMAppManifest.xml");
				if (xmlDocument.Root != null)
				{
					var xElement = xmlDocument.Root.Element("App");
					if (xElement != null)
					{
						var xAttribute = xElement.Attribute("Version");
						if (xAttribute != null)
						{
							string version = xAttribute.Value;
							this.AppVersion.Text = version;
						}
					}
				}
			};
		}

		private void OnAuthenticateMessage(AuthenticationMessage message)
		{
			App.UserName = message.UserName;
		}

		private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "UserName")
			{
				var vm = sender as SettingsViewModel;
				if (vm != null) 
					App.UserName = vm.UserName;
			}
		}

		private async void LoginWithTwitter_OnClick(object sender, RoutedEventArgs e)
		{
			if (App.MobileService.CurrentUser != null)
			{
				var result = MessageBox.Show(string.Format("You are logged in as {0}. Are you sure you want to login again?", App.UserName), "Logout?", MessageBoxButton.OKCancel);
				if (result == MessageBoxResult.OK)
				{
					Logout_OnClick(sender, e);
				}
			}

			var vm = DataContext as SettingsViewModel;
			if (vm != null) 
				vm.PropertyChanged += VmOnPropertyChanged;

			await AuthenticateWithTwitter();
		}

		private async void LoginWithFacebook_OnClick(object sender, RoutedEventArgs e)
		{
			if (App.MobileService.CurrentUser != null)
			{
				var result = MessageBox.Show(string.Format("You are logged in as {0}. Are you sure you want to login again?", App.UserName), "Logout?", MessageBoxButton.OKCancel);
				if (result == MessageBoxResult.OK)
				{
					Logout_OnClick(sender, e);
				}
			}

			await AuthenticateWithFacebook();
		}

		private async void LoginWithGoogle_OnClick(object sender, RoutedEventArgs e)
		{
			if (App.MobileService.CurrentUser != null)
			{
				var result = MessageBox.Show(string.Format("You are logged in as {0}. Are you sure you want to login again?", App.UserName), "Logout?", MessageBoxButton.OKCancel);
				if (result == MessageBoxResult.OK)
				{
					Logout_OnClick(sender, e);
				}
			}

			await AuthenticateWithGoogle();
		}

		private MobileServiceUser _user;
		private async Task AuthenticateWithTwitter()
		{
			while (_user == null)
			{
				try
				{
					_user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
				}
				catch
				{
					//MessageBox.Show(ex.Message);
				}
			}

			if (_user != null)
			{
				LiveTileAgent.StartPeriodicAgent();
				var vm = DataContext as SettingsViewModel;
				if (vm != null) 
					vm.IsOauthUserRegistered(_user.UserId);
			}
			SetLoggedInState();
		}

		private async Task AuthenticateWithFacebook()
		{
			while (_user == null)
			{
				try
				{
					_user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
				}
				catch (InvalidOperationException)
				{
					//MessageBox.Show(ex.Message);
				}
			}

			if (_user != null)
			{
				LiveTileAgent.StartPeriodicAgent();
				var vm = DataContext as SettingsViewModel;
				if (vm != null)
					vm.IsOauthUserRegistered(_user.UserId);
			}
			SetLoggedInState();

		}

		private async Task AuthenticateWithGoogle()
		{
			while (_user == null)
			{
				try
				{
					_user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Google);
				}
				catch (InvalidOperationException)
				{
					//MessageBox.Show(ex.Message);
				}
			}

			if (_user != null)
			{
				LiveTileAgent.StartPeriodicAgent();
				var vm = DataContext as SettingsViewModel;
				if (vm != null)
					vm.IsOauthUserRegistered(_user.UserId);
			}

			SetLoggedInState();

		}

		private void SetLoggedInState()
		{
			var isLoggedIn = _authentication.IsAuthenticated;

			LoginInWithFacebookButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			LoginInWithGoogleButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			LoginInWithTekConfButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			LoginInWithTwitterButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			RegisterButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			LogoutButton.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
			LoginName.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
			LoginName.Text = isLoggedIn ? "Logged in as " + App.UserName : "";

			if (isLoggedIn)
			{
				var vm = DataContext as SettingsViewModel;
				if (vm != null && !vm.IsOptedInToNotifications)
				{
					var result = MessageBox.Show("Enable Push Notifications?", "Push Notifications", MessageBoxButton.OKCancel);
					if (result == MessageBoxResult.OK)
					{
						vm.IsOptedInToNotifications = true;
					}
					else if (result == MessageBoxResult.Cancel)
					{
						vm.IsOptedInToNotifications = false;
					}
				}
				else
				{
					if (vm != null) 
						vm.IsOptedInToNotifications = true;
				}
			}
			else
			{
				var vm = DataContext as SettingsViewModel;
				if (vm != null) 
					vm.IsOptedInToNotifications = false;
			}
		}

		private void Logout_OnClick(object sender, RoutedEventArgs e)
		{
			App.MobileService.Logout();
			App.UserName = "";
			_user = null;
			_authentication.UserName = "";
			
			SetLoggedInState();
		}

		private void Register_OnClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Not yet implemented");
		}

		private void LoginWithTekConf_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = this.DataContext as SettingsViewModel;
			if (vm != null)
				vm.ShowTekConfLoginCommand.Execute(null);
		}
	}
}