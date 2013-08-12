using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace TekConf.UI.WinStore.Views
{
	using Windows.UI.Core;
	using Windows.UI.Xaml;
	using Cirrious.CrossCore;
	using Cirrious.MvvmCross.Plugins.Messenger;
	using Cirrious.MvvmCross.WindowsStore.Views;
	using TekConf.Core.Messages;
	using TekConf.Core.Repositories;
	class ThreadUtility
	{
		// Warning: "Because this call is not awaited, execution of the current method 
		// continues before the call is completed. Consider applying the 'await' 
		// operator to the result of the call."
		// But that's what we want here --- just to schedule it and carry on.
#pragma warning disable 4014
		public static void runOnUiThread(Windows.UI.Core.DispatchedHandler del)
		{
			CoreWindow window = CoreWindow.GetForCurrentThread();
			window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, del);
		}
#pragma warning restore 4014
	}
	/// <summary>
	/// A page that displays a grouped collection of items.
	/// </summary>
	public sealed partial class ConferencesListView : MvxStorePage
	{
		private MvxSubscriptionToken _conferencesListAllExceptionToken;
		private MvxSubscriptionToken _conferencesListFavoritesExceptionToken;
		public ConferencesListView()
		{
			this.InitializeComponent();

			this.conferencesLargeGridView.SelectionMode = ListViewSelectionMode.Multiple;
			this.conferencesSmallListView.SelectionMode = ListViewSelectionMode.Multiple;

			var messenger = Mvx.Resolve<IMvxMessenger>();

			_conferencesListAllExceptionToken = messenger.Subscribe<ConferencesListAllExceptionMessage>(message =>

				ThreadUtility.runOnUiThread(
							delegate()
							{
								if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
								{
									const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
									var messageDialog = new Windows.UI.Popups.MessageDialog(errorMessage);
									messageDialog.ShowAsync();
								}
							}
						)

					);

			_conferencesListFavoritesExceptionToken = messenger.Subscribe<ConferencesListFavoritesExceptionMessage>(message =>

				ThreadUtility.runOnUiThread(
							delegate()
							{
								if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
								{
									const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
									var messageDialog = new Windows.UI.Popups.MessageDialog(errorMessage);
									messageDialog.ShowAsync();
									//ConferencesFavoritesExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
									//ConferencesFavoritesExceptionMessage.Visibility = Visibility.Visible;
								}
							}
						)
				);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//TODO
			//var vm = this.Favorites.DataContext as ConferencesListViewModel;
			//if (vm != null && vm.Favorites != null)
			//{
			//	var x = vm.Favorites.Count;
			//}
			base.OnNavigatedTo(e);
		}

		private void Conference_OnClick(object sender, ItemClickEventArgs e)
		{
			var vm = this.DataContext as ConferencesListViewModel;
			var conference = e.ClickedItem as ConferencesListViewDto;
			if (vm != null && conference != null)
			{
				vm.ShowDetailCommand.Execute(conference.slug);
			}
		}

		//private void Conference_OnSelected(object sender, GestureEventArgs e)
		//{
		//	var vm = DataContext as ConferencesListViewModel;
		//	var stackPanel = sender as StackPanel;
		//	if (stackPanel == null)
		//		return;
		//	var conference = (stackPanel.DataContext) as ConferencesListViewDto;
		//	if (vm != null && conference != null)
		//		vm.ShowDetailCommand.Execute(conference.slug);
		//}

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

		private void Search_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
				vm.ShowSearchCommand.Execute(null);
		}

		private void GoBack(object sender, RoutedEventArgs e)
		{
			//TODO : 
		}

		private void OnRefresh(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
			{
				vm.Refresh();
			}
		}

		private void OnSettings(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null) 
				vm.ShowSettingsCommand.Execute(null);
		}
	}
}
