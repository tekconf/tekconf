using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public partial class ConferenceDetailSpeakersViewController : BaseUIViewController
	{
		private string conferenceSlug = "codemash-2013";

		public ConferenceDetailSpeakersViewController () : base ("ConferenceDetailSpeakersViewController", null)
		{

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
			
			var loading = new UIAlertView (" Downloading Speakers", "Please wait...", null, null, null);
			
			loading.Show ();
			
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
			indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
			indicator.StartAnimating (); 
			loading.AddSubview (indicator);
			
			Repository.GetSpeakers (conferenceSlug, speakers => 
			{ 
				InvokeOnMainThread (() => 
				{ 
					speakersTableView.Source = new SpeakersTableViewSource (this, speakers); 
					speakersTableView.ReloadData (); 
					loading.DismissWithClickedButtonIndex (0, true); 
				});
			});
			
			if (!UserInterfaceIdiomIsPhone) {
				this.speakersTableView.SelectRow (
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


		private class SpeakersTableViewSource : UITableViewSource
		{ 
			private readonly IList<FullSpeakerDto> _speakers;
			private const string SpeakerCell = "SpeakerCell";
			private ConferenceDetailSpeakersViewController _rootViewController;
			private SpeakerDetailTabBarController _speakerDetailViewController;
			
			public SpeakersTableViewSource (ConferenceDetailSpeakersViewController controller, IList<FullSpeakerDto> speakers)
			{ 
				_rootViewController = controller;
				_speakers = speakers; 
			}
			
			public override int RowsInSection (UITableView tableView, int section)
			{ 
				return _speakers.Count; 
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{ 
				return 60; 
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{ 
				var cell = tableView.DequeueReusableCell (SpeakerCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, SpeakerCell); 
				var speaker = _speakers [indexPath.Row]; 
				cell.TextLabel.Text = speaker.fullName; 
				//cell.DetailTextLabel.Text = speaker.start.ToLocalTime ().ToString (); 
				return cell; 
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{ 
				FullSpeakerDto selectedSpeaker = _speakers [indexPath.Row]; 
				//new UIAlertView ("View Speaker", selectedSpeaker.title, null, "Ok", null).Show (); 
				
				if (UserInterfaceIdiomIsPhone) {
					_speakerDetailViewController = new SpeakerDetailTabBarController (selectedSpeaker);

					//_rootViewController.SelectedSpeakerSlug = selectedSpeaker.slug;
					//_rootViewController.PerformSegue (MoveToMapSegueName, _rootViewController);
					// Pass the selected object to the new view controller.
					_rootViewController.NavigationController.PushViewController (
											_speakerDetailViewController,
											true
					);
				} else {
					// Navigation logic may go here -- for example, create and push another view controller.
				}
			} 
		}
		
		private class DataSource : UITableViewSource
		{
			public DataSource ()
			{
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
					controller.speakersTableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
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
//				if (UserInterfaceIdiomIsPhone) {
//					FullSpeakerDto selectedSpeaker = _speakers [indexPath.Row]; 
//					
//					var speakerTabBarController = new SpeakerDetailTabBarController (selectedSpeaker);
//
//				} else {
//					// Navigation logic may go here -- for example, create and push another view controller.
//				}
			}
		}
		

	}
}

