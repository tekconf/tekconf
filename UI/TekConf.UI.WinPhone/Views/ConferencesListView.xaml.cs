using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesListView : MvxPhonePage
	{
		private MvxSubscriptionToken _token;

		public ConferencesListView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));
		}

		private void Conference_OnSelected(object sender, GestureEventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			var stackPanel = sender as StackPanel;
			if (stackPanel == null) 
				return;
			var conference = (stackPanel.DataContext) as FullConferenceDto;
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

		private void Conferences_OnDoubleTap(object sender, GestureEventArgs e)
		{

		}
	}
}