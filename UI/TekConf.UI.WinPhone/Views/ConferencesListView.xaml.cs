using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using TekConf.Core.Repositories;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesListView : MvxPhonePage
	{
		private MvxSubscriptionToken _conferencesListAllExceptionToken;
		private MvxSubscriptionToken _conferencesListFavoritesExceptionToken;

		public ConferencesListView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();

			_conferencesListAllExceptionToken = messenger.Subscribe<ConferencesListAllExceptionMessage>(message =>
					Dispatcher.BeginInvoke(() =>
					{
						if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
						{
							const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
							MessageBox.Show(errorMessage);
							//ConferencesExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
							//ConferencesExceptionMessage.Visibility = Visibility.Visible;
						}
					}));

			_conferencesListFavoritesExceptionToken = messenger.Subscribe<ConferencesListFavoritesExceptionMessage>(message =>
		Dispatcher.BeginInvoke(() =>
		{
			if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
			{
				const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
				MessageBox.Show(errorMessage);
				//ConferencesFavoritesExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
				//ConferencesFavoritesExceptionMessage.Visibility = Visibility.Visible;
			}
		}));
		}

		private void Conference_OnSelected(object sender, GestureEventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			var stackPanel = sender as StackPanel;
			if (stackPanel == null)
				return;
			var conference = (stackPanel.DataContext) as ConferencesListViewDto;
			if (vm != null && conference != null)
				vm.ShowDetailCommand.Execute(conference.slug);
		}

		private void ConferenceName_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var textBlock = (sender as TextBlock);
			if (textBlock != null)
				textBlock.MaxWidth = ActualWidth - 20;
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

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null) vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
			{
				vm.Refresh();
			}
		}

		private void Search_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
				vm.ShowSearchCommand.Execute(null);
		}


	}
}