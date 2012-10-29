using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public partial class ConferenceDetailViewController : BaseUIViewController
	{
		private FullConferenceDto _conference;

		public ConferenceDetailViewController (FullConferenceDto conference) : base ("ConferenceDetailViewController", null)
		{
			_conference = conference;
			Title = _conference.name;
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var loading = new UIAlertView (" Downloading Sessions", "Please wait...", null, null, null);
			
			loading.Show ();
			
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
			indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
			indicator.StartAnimating (); 
			loading.AddSubview (indicator);

//			if (_conference == null) {
//				Repository.GetSessions (conferenceSlug, sessions => 
//				{ 
//					InvokeOnMainThread (() => 
//					{ 
//						sessionsTableView.Source = new SessionsTableViewSource (this, sessions); 
//						sessionsTableView.ReloadData (); 
//						loading.DismissWithClickedButtonIndex (0, true); 
//					});
//				});
//			} else {
				sessionsTableView.Source = new SessionsTableViewSource (this, _conference.sessions); 
				sessionsTableView.ReloadData (); 
				loading.DismissWithClickedButtonIndex (0, true); 
			//}

			if (!UserInterfaceIdiomIsPhone)
			{
				this.sessionsTableView.SelectRow (
					NSIndexPath.FromRowSection (0, 0),
					false,
					UITableViewScrollPosition.Middle
					);
			}
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	
	
		
		private class SessionsTableViewSource : UITableViewSource
		{ 
			private readonly IList<FullSessionDto> _sessions;
			private const string SessionCell = "SessionCell";
			private ConferenceDetailViewController _rootViewController;
			
			public SessionsTableViewSource (ConferenceDetailViewController controller, IList<FullSessionDto> sessions)
			{ 
				_rootViewController = controller;
				_sessions = sessions; 
			}
			
			public override int RowsInSection (UITableView tableView, int section)
			{ 
				return _sessions.Count; 
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{ 
				return 60; 
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{ 
				var cell = tableView.DequeueReusableCell (SessionCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, SessionCell); 
				var session = _sessions [indexPath.Row]; 

				var font = UIFont.FromName("OpenSans", 12f);
				cell.TextLabel.Font = font;
				cell.DetailTextLabel.Font = font;
				cell.TextLabel.Text = session.title; 
				cell.DetailTextLabel.Text = session.start.ToLocalTime ().ToString (); 
				return cell; 
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{ 
				var selectedSession = _sessions [indexPath.Row]; 
				//new UIAlertView ("View Session", selectedSession.title, null, "Ok", null).Show (); 
				
				if (UserInterfaceIdiomIsPhone) {
					//_sessionDetailViewController = new SessionDetailViewController (selectedSession.slug);
					//_rootViewController.SelectedSessionSlug = selectedSession.slug;
					//_rootViewController.PerformSegue (MoveToMapSegueName, _rootViewController);
					// Pass the selected object to the new view controller.
					//_rootViewController.NavigationController.PushViewController (
					//						_sessionDetailViewController,
					//						true
					//					);
				} else {
					// Navigation logic may go here -- for example, create and push another view controller.
				}
			} 
		}
		
		private class DataSource : UITableViewSource
		{
			//RootViewController controller;
			
			public DataSource ()
			{
				//this.controller = controller;
			}
			
			// Customize the number of sections in the table view.
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return 1;
			}
			
			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				string cellIdentifier = "Cell";
				var cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					}
				}
				var font = UIFont.FromName("OpenSans", 12f);
				cell.TextLabel.Font = font;
				cell.DetailTextLabel.Font = font;
				// Configure the cell.
				//cell.TextLabel.Text = NSBundle.MainBundle.LocalizedString (
				//	"Detail",
				//	"Detail"
				//);
				return cell;
			}
			
			/*
			// Override to support conditional editing of the table view.
			public override bool CanEditRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}
			*/
			
			/*
			// Override to support editing the table view.
			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					// Delete the row from the data source.
					controller.sessionsTableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}
			*/
			
			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/
			
			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (UserInterfaceIdiomIsPhone) {
					//					var DetailViewController = new SessionDetailViewController ();
					//					// Pass the selected object to the new view controller.
					//					controller.NavigationController.PushViewController (
					//						DetailViewController,
					//						true
					//						);
				} else {
					// Navigation logic may go here -- for example, create and push another view controller.
				}
			}
		}
		

	
	}
}

