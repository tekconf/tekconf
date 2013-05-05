using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceSessionsView : MvxPhonePage
	{
		public ConferenceSessionsView()
		{
			InitializeComponent();
			Loaded += (sender, args) =>
			{
				var vm = DataContext as ConferenceSessionsViewModel;

				if (vm != null && vm.Conference != null) 
					GoogleAnalytics.EasyTracker.GetTracker().SendView("ConferenceSessions-" + vm.Conference.slug);
			};
			
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

	}
}