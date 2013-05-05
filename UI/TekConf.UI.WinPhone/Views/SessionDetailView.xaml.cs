using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;

namespace TekConf.UI.WinPhone.Views
{
	public partial class SessionDetailView : MvxPhonePage
	{
		public SessionDetailView()
		{
			InitializeComponent();
			Loaded += (sender, args) =>
			{
				var vm = DataContext as SessionDetailViewModel;

				if (vm != null && vm.Session != null)
					GoogleAnalytics.EasyTracker.GetTracker().SendView("SessionDetailView-" + vm.ConferenceSlug + "-" + vm.Session.slug);
			};

		}
	}
}