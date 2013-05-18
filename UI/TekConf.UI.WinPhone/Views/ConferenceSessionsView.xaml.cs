using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Shell;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.WinPhone.Bootstrap;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceSessionsView : MvxPhonePage
	{
		public ConferenceSessionsView()
		{
			InitializeComponent();
			var authentication = new Authentication();
			if (!authentication.IsAuthenticated)
				this.SessionsPivot.SelectedIndex = 1;
		}

		private void SessionTitle_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var title = (sender as TextBlock);
			title.MaxWidth = this.ActualWidth;
		}

		private void Session_OnTap(object sender, GestureEventArgs e)
		{
			var button = (sender as Button);
			var session = button.DataContext as FullSessionDto;
			var vm = this.DataContext as ConferenceSessionsViewModel;
			vm.ShowSessionDetailCommand.Execute(new SessionDetailViewModel.Navigation() { ConferenceSlug = vm.Conference.slug, SessionSlug = session.slug });
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as ConferenceSessionsViewModel;
			if (vm != null) vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as ConferenceSessionsViewModel;
			if (vm != null && vm.Conference != null)
			{
				vm.Refresh(vm.Conference.slug);
			}
			
		}
	}
}