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
			await Authenticate();
		}


		private MobileServiceUser _user;
		private async Task Authenticate()
		{
			while (_user == null)
			{
				try
				{
					_user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
				}
				catch (InvalidOperationException ex)
				{
					MessageBox.Show(ex.Message);
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
	}
}