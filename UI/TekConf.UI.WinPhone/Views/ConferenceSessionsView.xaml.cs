using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Views;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.WinPhone.Bootstrap;
using Cirrious.CrossCore;

namespace TekConf.UI.WinPhone.Views
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	using TekConf.Core.Services;

	public partial class ConferenceSessionsView
	{
		private MvxSubscriptionToken _conferenceSessionExceptionMessageToken;
		private MvxSubscriptionToken _sessionAddedToken;

		public ConferenceSessionsView()
		{
			InitializeComponent();

			var authentication = new Authentication(Mvx.Resolve<ISQLiteConnection>());
			if (!authentication.IsAuthenticated)
				SessionsPivot.SelectedIndex = 1;

			var messenger = Mvx.Resolve<IMvxMessenger>();

			_sessionAddedToken = messenger.Subscribe<FavoriteSessionAddedMessage>(message => Dispatcher.BeginInvoke(() => Refresh_OnClick(null, null)));

			_conferenceSessionExceptionMessageToken = messenger.Subscribe<ConferenceSessionsExceptionMessage>(message =>
								Dispatcher.BeginInvoke(() =>
								{
									if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
									{
										const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
										MessageBox.Show(errorMessage);
									}
								}));
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