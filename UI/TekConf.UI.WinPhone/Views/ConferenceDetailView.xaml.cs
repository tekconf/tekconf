using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Microsoft.Phone.Shell;
using TekConf.Core.ViewModels;
using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceDetailView
	{
		private MvxSubscriptionToken _token;
		public ConferenceDetailView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{

			var vm = DataContext as INotifyPropertyChanged;
			if (vm != null)
			{
				vm.PropertyChanged += (o, args) =>
				{
					if (args.PropertyName == "Conference")
					{
						string imageUrl = "/img/appbar.heart.png";
						var viewModel = DataContext as ConferenceDetailViewModel;
						if (viewModel != null && viewModel.Conference != null)
						{
							if (viewModel.Conference.isAddedToSchedule == true)
							{
								imageUrl = "/img/appbar.heart.cross.png";
							}
						}

						((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri(imageUrl, UriKind.Relative);
					}
				};
			}
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var image = (sender as Image);
			if (image != null)
			{
				image.Width = ActualWidth - 20;
				image.Height = 180 * (image.Width / 260);
			}
		}

		private void BrowseSessions_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;

			if (vm != null) 
				vm.ShowSessionsCommand.Execute(vm.Conference.slug);
		}

		private void AddFavorite_OnClick(object sender, EventArgs e)
		{
			var authentication = Mvx.Resolve<IAuthentication>();

			if (authentication.IsAuthenticated)
			{
				var vm = DataContext as ConferenceDetailViewModel;

				if (vm != null)
					vm.AddFavoriteCommand.Execute(vm.Conference.slug);
			}
			else
			{
				MessageBox.Show("You must be logged in to favorite a conference");
			}
		}

		//private void ShowWebBrowser(string uri)
		//{
		//	if (!string.IsNullOrWhiteSpace(uri))
		//	{
		//		var webBrowserTask = new WebBrowserTask { Uri = new Uri(uri) };
		//		webBrowserTask.Show();
		//	}
		//}

		//private void FacebookNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.facebookUrl);
		//}

		//private void HomepageNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.homepageUrl);
		//}

		//private void LanyrdNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.lanyrdUrl);
		//}

		//private void MeetupNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.meetupUrl);
		//}

		//private void GooglePlusNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.googlePlusUrl);
		//}

		//private void VimeoNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.vimeoUrl);
		//}

		//private void YouTubeNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.youtubeUrl);
		//}

		//private void GitHubNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.githubUrl);
		//}

		//private void LinkedInNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//		ShowWebBrowser(vm.Conference.linkedInUrl);
		//}

		//private void TwitterHashTagNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	throw new NotImplementedException();
		//}

		//private void TwitterNameNavigate_OnClick(object sender, RoutedEventArgs e)
		//{
		//	throw new NotImplementedException();
		//}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null) vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null)
			{
				vm.Refresh(vm.Conference.slug);
			}
		}
	}
}