using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;
using Foundation;

namespace TekConf.Mobile.iOS
{
	public partial class ConferencesView : MvxTableViewController<ConferencesViewModel>
	{
		public ConferencesView() : base("ConferencesView", null)
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

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel>();
			set.Bind(source).To(vm => vm.Conferences);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowConference);
			set.Apply();

			TableView.Source = source;
			TableView.ReloadData();

			ViewModel.LoadCommand.Execute(null);

			TableView.ReloadData();
		}
	}


}


