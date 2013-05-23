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
		private MvxSubscriptionToken _token;

		public SessionDetailView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));

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