using System;
using System.Windows;
using System.Windows.Controls;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.ViewModels;
using TekConf.UI.WinPhone.Bootstrap;
using Cirrious.CrossCore;

namespace TekConf.UI.WinPhone.Views
{
	public partial class SessionDetailView
	{
		private MvxSubscriptionToken _sessionDetailExceptionToken;

		public SessionDetailView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
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
			var authentication = new Authentication();
			if (authentication.IsAuthenticated)
			{
				throw new NotImplementedException();
			}

			MessageBox.Show("You must be logged in to favorite a session");
		}
	}
}