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
		private MvxSubscriptionToken _token;

		public ConferenceSessionsView()
		{
			InitializeComponent();
			var authentication = new Authentication();
			if (!authentication.IsAuthenticated)
				SessionsPivot.SelectedIndex = 1;

			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));

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