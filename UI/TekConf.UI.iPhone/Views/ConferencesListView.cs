using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using TekConf.Core.ViewModels;

namespace TekConf.UI.iPhone.Views
{
	[Register("ConferencesListView")]
	public class ConferenceDetailView : MvxViewController
	{
		
	}

	[Register("ConferencesListView")]
	public class ConferencesListView : MvxTableViewController
	{
		public ConferencesListView()
		{
			Title = "Conferences";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new MvxStandardTableViewSource(TableView, "TitleText name");
			TableView.Source = source;

			var set = this.CreateBindingSet<ConferencesListView, ConferencesListViewModel>();
			set.Bind(source).To(vm => vm.Conferences);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailCommand);
			set.Apply();

			TableView.ReloadData();
		}
	}
}