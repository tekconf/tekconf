using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Repositories;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.WinPhone.Bootstrap;
using Cirrious.CrossCore;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceSessionsView
	{
		private MvxSubscriptionToken _conferenceSessionExceptionMessageToken;

		public ConferenceSessionsView()
		{
			InitializeComponent();

			var authentication = new Authentication();
			if (!authentication.IsAuthenticated)
				SessionsPivot.SelectedIndex = 1;

			var messenger = Mvx.Resolve<IMvxMessenger>();

			_conferenceSessionExceptionMessageToken = messenger.Subscribe<ConferenceSessionsExceptionMessage>(message =>
								Dispatcher.BeginInvoke(() =>
								{
									if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
									{
										const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
										MessageBox.Show(errorMessage);

										//ConferenceSessionsFavoritesExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsFavoritesExceptionMessage.Visibility = Visibility.Visible;

										//ConferenceSessionsRoomExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsRoomExceptionMessage.Visibility = Visibility.Visible;

										//ConferenceSessionsSpeakerExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsSpeakerExceptionMessage.Visibility = Visibility.Visible;

										//ConferenceSessionsTagExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsTagExceptionMessage.Visibility = Visibility.Visible;

										//ConferenceSessionsTimeExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsTimeExceptionMessage.Visibility = Visibility.Visible;

										//ConferenceSessionsTitleExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
										//ConferenceSessionsTitleExceptionMessage.Visibility = Visibility.Visible;
									}
								}));


			//ConferenceSessionsFavoritesExceptionMessage.Text = "";
			//ConferenceSessionsFavoritesExceptionMessage.Visibility = Visibility.Collapsed;

			//ConferenceSessionsRoomExceptionMessage.Text = "";
			//ConferenceSessionsRoomExceptionMessage.Visibility = Visibility.Collapsed;

			//ConferenceSessionsSpeakerExceptionMessage.Text = "";
			//ConferenceSessionsSpeakerExceptionMessage.Visibility = Visibility.Collapsed;

			//ConferenceSessionsTagExceptionMessage.Text = "";
			//ConferenceSessionsTagExceptionMessage.Visibility = Visibility.Collapsed;

			//ConferenceSessionsTimeExceptionMessage.Text = "";
			//ConferenceSessionsTimeExceptionMessage.Visibility = Visibility.Collapsed;

			//ConferenceSessionsTitleExceptionMessage.Text = "";
			//ConferenceSessionsTitleExceptionMessage.Visibility = Visibility.Collapsed;

		}

		private void SessionTitle_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var title = (sender as TextBlock);
			if (title != null) 
				title.MaxWidth = ActualWidth;
		}

		private void Session_OnTap(object sender, GestureEventArgs e)
		{
			var button = (sender as Button);
			if (button != null)
			{
				var session = button.DataContext as ConferenceSessionListDto;
				var vm = DataContext as ConferenceSessionsViewModel;
				if (vm != null && session != null)
					vm.ShowSessionDetailCommand.Execute(new SessionDetailViewModel.Navigation { ConferenceSlug = vm.Conference.slug, SessionSlug = session.slug });
			}
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceSessionsViewModel;
			if (vm != null) 
				vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			ConferenceSessionsFavoritesExceptionMessage.Text = "";
			ConferenceSessionsFavoritesExceptionMessage.Visibility = Visibility.Collapsed;

			ConferenceSessionsRoomExceptionMessage.Text = "";
			ConferenceSessionsRoomExceptionMessage.Visibility = Visibility.Collapsed;

			ConferenceSessionsSpeakerExceptionMessage.Text = "";
			ConferenceSessionsSpeakerExceptionMessage.Visibility = Visibility.Collapsed;

			ConferenceSessionsTagExceptionMessage.Text = "";
			ConferenceSessionsTagExceptionMessage.Visibility = Visibility.Collapsed;

			ConferenceSessionsTimeExceptionMessage.Text = "";
			ConferenceSessionsTimeExceptionMessage.Visibility = Visibility.Collapsed;

			ConferenceSessionsTitleExceptionMessage.Text = "";
			ConferenceSessionsTitleExceptionMessage.Visibility = Visibility.Collapsed;

			var vm = DataContext as ConferenceSessionsViewModel;
			if (vm != null && vm.Conference != null)
			{
				vm.Refresh(vm.Conference.slug);
			}
			
		}

		private void Search_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceSessionsViewModel;
			if (vm != null)
				vm.ShowSearchCommand.Execute(null);
		}
	}
}