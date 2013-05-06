using System;
using System.Threading.Tasks;
using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.WindowsAzure.MobileServices;

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

			////var table = App.MobileService.GetTable("Users");
			//var table = App.MobileService.GetTable<Users>();

			//var u = new Users
			//{
			//	UserId = _user.UserId,
			//	UserName = ""
			//};

			//await table.InsertAsync(u);

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

			var x = _user;
			////var table = App.MobileService.GetTable("Users");
			//var table = App.MobileService.GetTable<Users>();

			//var u = new Users
			//{
			//	UserId = _user.UserId,
			//	UserName = ""
			//};

			//await table.InsertAsync(u);

		}


	}
}