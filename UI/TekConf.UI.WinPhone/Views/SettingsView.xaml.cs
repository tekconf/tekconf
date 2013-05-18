using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.WindowsAzure.MobileServices;
using TekConf.Core.ViewModels;

namespace TekConf.UI.WinPhone.Views
{
	public partial class SettingsView : MvxPhonePage
	{
		public SettingsView()
		{
			InitializeComponent();
			SetLoggedInState();
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
			var vm = this.DataContext as SettingsViewModel;
			if (vm != null) 
				vm.PropertyChanged += VmOnPropertyChanged;
			await AuthenticateWithTwitter();
		}

		private async void LoginWithFacebook_OnClick(object sender, RoutedEventArgs e)
		{
			await AuthenticateWithFacebook();
		}

		private async void LoginWithGoogle_OnClick(object sender, RoutedEventArgs e)
		{
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
				catch (InvalidOperationException ex)
				{
					//MessageBox.Show(ex.Message);
				}
			}

			if (_user != null)
			{
				var vm = this.DataContext as SettingsViewModel;
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
				catch (InvalidOperationException ex)
				{
					//MessageBox.Show(ex.Message);
				}
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
				catch (InvalidOperationException ex)
				{
					//MessageBox.Show(ex.Message);
				}
			}
			SetLoggedInState();

		}


		private void SetLoggedInState()
		{
			var isLoggedIn = App.MobileService.CurrentUser != null;

			this.LoginInWithFacebookButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			this.LoginInWithGoogleButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			this.LoginInWithTekConfButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			this.LoginInWithTwitterButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			this.RegisterButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
			this.LogoutButton.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
			this.LoginName.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
			//this.LoginName.Text = isLoggedIn ? "Logged in as " + App.MobileService.CurrentUser.UserId : "";
			
		}


		private void Logout_OnClick(object sender, RoutedEventArgs e)
		{
			App.MobileService.Logout();
			SetLoggedInState();
		}

		private void Register_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void LoginWithTekConf_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
			SetLoggedInState();
		}
	}
}