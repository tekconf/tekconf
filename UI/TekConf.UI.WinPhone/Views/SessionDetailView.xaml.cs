using System;
using System.Windows;
using System.Windows.Controls;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Messages;
using TekConf.Core.ViewModels;
using TekConf.UI.WinPhone.Bootstrap;
using Cirrious.CrossCore;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace TekConf.UI.WinPhone.Views
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	public partial class SessionDetailView
	{
		private MvxSubscriptionToken _sessionDetailExceptionToken;
		private MvxSubscriptionToken _favoriteRefreshMessageToken;

		public SessionDetailView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();

			_favoriteRefreshMessageToken = messenger.Subscribe<RefreshSessionFavoriteIconMessage>(message => Dispatcher.BeginInvoke(RefreshFavoriteIcon));

			_sessionDetailExceptionToken = messenger.Subscribe<SessionDetailExceptionMessage>(message =>
					Dispatcher.BeginInvoke(() =>
					{
						if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
						{
							const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
							MessageBox.Show(errorMessage);
							//SessionDetailExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
							//SessionDetailExceptionMessage.Visibility = Visibility.Visible;

							//SessionDetailSpeakersExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
							//SessionDetailSpeakersExceptionMessage.Visibility = Visibility.Visible;
						}
					}));

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			RefreshFavoriteIcon();
		}

		private void RefreshFavoriteIcon()
		{
			string imageUrl = "/img/appbar.heart.png";
			var viewModel = DataContext as SessionDetailViewModel;
			if (viewModel != null && viewModel.Session != null)
			{
				if (viewModel.Session.isAddedToSchedule == true)
				{
					imageUrl = "/img/appbar.heart.cross.png";
				}
			}

			((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IconUri = new Uri(imageUrl, UriKind.Relative);
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as SessionDetailViewModel;
			if (vm != null)
				vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as SessionDetailViewModel;
			if (vm != null && vm.Session != null)
			{
				var navigation = new SessionDetailViewModel.Navigation
				{
					ConferenceSlug = vm.ConferenceSlug,
					SessionSlug = vm.Session.slug
				};

				vm.Refresh(navigation);
			}
		}

		private void SpeakerFullName_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var title = (sender as TextBlock);
			if (title != null)
				title.MaxWidth = ActualWidth;
		}

		private void AddFavorite_OnClick(object sender, EventArgs e)
		{
			var authentication = new Authentication(Mvx.Resolve<ISQLiteConnection>());
			if (authentication.IsAuthenticated)
			{
				var vm = this.DataContext as SessionDetailViewModel;
				if (vm != null)
					vm.AddFavoriteCommand.Execute(null);
			}
			else
			{
				MessageBox.Show("You must be logged in to favorite a session");				
			}

		}
	}
}