using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace TekConf.Mobile.iOS
{
	[MvxFromStoryboard("ConferencesStoryboard")]
    public partial class ConferencesViewController : MvxTableViewController<ConferencesViewModel>
    {
        public ConferencesViewController (IntPtr handle) : base (handle)
        {
			Title = "Conferences";
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 221;
			TableView.TranslatesAutoresizingMaskIntoConstraints = false;
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new ConferencesTableSource(TableView);

			var set = this.CreateBindingSet<ConferencesViewController, ConferencesViewModel>();
			set.Bind(source).To(vm => vm.Conferences);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowConference);
			set.Apply();

			TableView.Source = source;

			this.ViewModel.LoadCommand.Execute(null);

			TableView.ReloadData();

		}
    }
}