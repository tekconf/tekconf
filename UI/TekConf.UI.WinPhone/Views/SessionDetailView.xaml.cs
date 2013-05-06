using System;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;

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
	}
}