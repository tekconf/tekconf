using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using CoreGraphics;

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
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//AddSettingsButton();

			var close = new UIBarButtonItem(UIBarButtonSystemItem.Add);
			NavigationItem.RightBarButtonItem = close;

			var source = new ConferencesTableSource(TableView);

			var set = this.CreateBindingSet<ConferencesViewController, ConferencesViewModel>();
			set.Bind(source).To(vm => vm.Conferences);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailCommand);
			set.Bind(close).For("Clicked").To(vm => vm.ShowSettingsCommand);
			set.Apply();

			TableView.Source = source;

			this.ViewModel.LoadCommand.Execute(null);

			TableView.ReloadData();
		}
		//UIBarButtonItem menuItem;
		//private void AddSettingsButton()
		//{
		//	//var settingsAttributes = new UIStringAttributes()
		//	//{
		//	//	ForegroundColor = UIColor.White,
		//	//	Font = UIFont.FromName("FontAwesome", 16f)
		//	//};

		//	//UIButton menuButton = new UIButton(UIButtonType.Custom);
		//	//var prettyString = new NSMutableAttributedString("\xf013");
		//	//prettyString.SetAttributes(settingsAttributes.Dictionary, new NSRange(0, 1));
		//	//menuButton.SetAttributedTitle(prettyString, UIControlState.Normal);
		//	//menuButton.Frame = new CGRect(0, 0, 24, 24);

		//	UIButton menuButton = new UIButton(UIButtonType.ContactAdd);

		//	menuItem = new UIBarButtonItem(menuButton);

		//	//menuButton.TouchUpInside += (sender, e) =>
		//	//{
		//	//	var storyboard = UIStoryboard.FromName("Main", null);
		//	//	var settingsController = storyboard.InstantiateViewController("SettingsNavigationController") as SettingsNavigationController;

		//	//	this.NavigationController.PresentModalViewController(settingsController, animated: true);

		//	//};

		//	this.NavigationItem.SetRightBarButtonItem(menuItem, true);

		//}
    }
}