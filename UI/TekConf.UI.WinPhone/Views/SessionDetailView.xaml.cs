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
			if (title != null) 
				title.MaxWidth = this.ActualWidth;
		}

		private void AddFavorite_OnClick(object sender, EventArgs e)
		{
			var authentication = new Authentication();
			if (authentication.IsAuthenticated)
			{
				throw new NotImplementedException();
			}
			else
			{
				MessageBox.Show("You must be logged in to favorite a session");
			}
		}
	}
}