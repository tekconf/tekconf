using System;
using System.Windows;
using System.Windows.Controls;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesListView : MvxPhonePage
	{
		public ConferencesListView()
		{
			InitializeComponent();
			this.Loaded += Page_Loaded;
		}

		async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			await Authenticate();
		}

		private MobileServiceUser user;
		private async System.Threading.Tasks.Task Authenticate()
		{
			while (user == null)
			{
				try
				{
					user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
				}
				catch (InvalidOperationException)
				{
					const string message = "You must log in. Login Required";
					MessageBox.Show(message);
				}
			}

			//var table = App.MobileService.GetTable("Users");
			var table = App.MobileService.GetTable<Users>();

			var u = new Users
			{
				UserId = user.UserId,
				UserName = ""
			};

			await table.InsertAsync(u);

		}


		private void Conference_OnSelected(object sender, SelectionChangedEventArgs e)
		{
			var vm = this.DataContext as ConferencesListViewModel;
			var conference = ((sender as ListBox).SelectedItem) as FullConferenceDto;
			vm.ShowDetailCommand.Execute(conference.slug);
		}

		private void ConferenceName_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var textBlock = (sender as TextBlock);
			textBlock.MaxWidth = this.ActualWidth - 20;
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var image = (sender as Image);
			image.Width = this.ActualWidth - 20;
			image.Height = 180 * (image.Width / 260);
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}