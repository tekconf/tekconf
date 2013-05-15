using System;
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
		}

		private async void LoginWithTwitter_OnClick(object sender, RoutedEventArgs e)
		{
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

			////var table = App.MobileService.GetTable("Users");
			//var table = App.MobileService.GetTable<Users>();

			//var u = new Users
			//{
			//	UserId = _user.UserId,
			//	UserName = ""
			//};

			//await table.InsertAsync(u);

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

		}


		private void Logout_OnClick(object sender, RoutedEventArgs e)
		{
			App.MobileService.Logout();
		}

		private void Register_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void LoginWithTekConf_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}