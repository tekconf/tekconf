using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;

namespace TekConf.Mobile.iOS
{
	public class ConferencesTableSource : MvxTableViewSource
	{
		public ConferencesTableSource(UITableView tableView) : base(tableView)
		{
			//tableView.RegisterClassForCellReuse(typeof(ConferenceCell), new NSString(ConferenceCell.Key));
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 221f;
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
	
			var cell = tableView.DequeueReusableCell(ConferenceCell.Key, indexPath);
			cell.ContentView.Frame = cell.Bounds;
			cell.ContentView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin 
												| UIViewAutoresizing.FlexibleWidth 
												| UIViewAutoresizing.FlexibleRightMargin 
												| UIViewAutoresizing.FlexibleTopMargin 
												| UIViewAutoresizing.FlexibleHeight 
												| UIViewAutoresizing.FlexibleBottomMargin;
			return cell;
		}
	}
}
