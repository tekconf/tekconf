using System;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.WebBrowser;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using TekConf.Core.Messages;
using TekConf.Core.ViewModels;
using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceDetailView
	{
		private MvxSubscriptionToken _conferenceDetailExceptionMessageToken;
		private MvxSubscriptionToken _favoriteRefreshMessageToken;

		public ConferenceDetailView()
		{
			InitializeComponent();
			LocationMap.Loaded += (sender, args) =>
			{
				Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "9078e309-8113-4ea7-9061-75d1a392743c";
				Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "Th8jcERPJgUlwBJDdtHs6w";
			};

			var messenger = Mvx.Resolve<IMvxMessenger>();


			_favoriteRefreshMessageToken = messenger.Subscribe<RefreshConferenceFavoriteIconMessage>(message => Dispatcher.BeginInvoke(RefreshFavoriteIcon));

			_conferenceDetailExceptionMessageToken = messenger.Subscribe<ConferenceDetailExceptionMessage>(message =>
						Dispatcher.BeginInvoke(() =>
						{
							if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
							{
								const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
								MessageBox.Show(errorMessage);
							}
						}));

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			RefreshFavoriteIcon();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null && vm.Conference.latitude != default(double) && vm.Conference.longitude != default(double))
			{

				LocationMap.Center = new GeoCoordinate(vm.Conference.latitude, vm.Conference.longitude);


				MapLayer layer0 = new MapLayer();

				var conferenceLocationPin = new Pushpin
				{
					GeoCoordinate = new GeoCoordinate(vm.Conference.latitude, vm.Conference.longitude),
					Content = vm.Conference.FormattedAddress
				};
				var overlay0 = new MapOverlay
				{
					Content = conferenceLocationPin,
					GeoCoordinate = new GeoCoordinate(vm.Conference.latitude, vm.Conference.longitude)
				};
				layer0.Add(overlay0);
				LocationMap.Layers.Add(layer0);
			}

		}

		private void RefreshFavoriteIcon()
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
			{
				string imageUrl = "/img/appbar.heart.png";
				if (vm.Conference != null)
				{
					if (vm.Conference.isAddedToSchedule == true)
					{
						imageUrl = "/img/appbar.heart.cross.png";
					}
				}

				((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri(imageUrl, UriKind.Relative);
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

		private void ConferenceLocationTapped(object sender, GestureEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null && vm.Conference.latitude != default(double) && vm.Conference.longitude != default(double))
			{

				LocationMap.Center = new GeoCoordinate(vm.Conference.latitude, vm.Conference.longitude);
			}
		}

		private void ConnectItemTap(object sender, GestureEventArgs e)
		{
			var button = sender as Button;
			
			if (button != null)
			{
				var connectItem = button.DataContext as ConnectItem;
				if (connectItem != null)
				{
					string text = connectItem.Value;
					ShowConnectItemUrl(text);
				}
			}
			
		}

		private void ShowConnectItemUrl(string text)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null)
			{
				string url = "";

				if (text.StartsWith("http"))
				{
					url = text;
				}
				else if (text.StartsWith("@"))
				{
					url = string.Format("http://mobile.twitter.com/{0}", text.Replace("@", ""));
				}
				else if (text.StartsWith("#"))
				{
					url = string.Format("http://mobile.twitter.com/search?q={0}", HttpUtility.UrlEncode(text));
				}

				var webBrowser = Mvx.Resolve<IMvxWebBrowserTask>();
				webBrowser.ShowWebPage(url);
			}
		}
	}
}