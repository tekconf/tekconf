using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class SessionDetailView : MvxPhonePage
	{
		public SessionDetailView()
		{
			InitializeComponent();
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as SessionDetailViewModel;
			if (vm != null) 
				vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as SessionDetailViewModel;
			if (vm != null && vm.Session != null)
			{
				var navigation = new SessionDetailViewModel.Navigation()
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
			title.MaxWidth = this.ActualWidth;
		}

		private void Speaker_OnTap(object sender, GestureEventArgs e)
		{
			//var button = (sender as Button);
			//var speaker = button.DataContext as FullSpeakerDto;
			//var vm = this.DataContext as SessionDetailViewModel;
			//vm.ShowSessionDetailCommand.Execute(new SessionDetailViewModel.Navigation() { ConferenceSlug = vm.Conference.slug, SessionSlug = speaker.slug });
		}
	}
}