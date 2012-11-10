using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Dialog;

namespace TekConf.UI.iPhone
{
	public class ConferencesDialogViewController : DialogViewController
	{
		private string _baseUrl = "http://api.tekconf.com";
		private RemoteDataRepository _client;
		protected RemoteDataRepository Repository
		{
			get
			{
				if (this._client == null)
				{
					this._client = new RemoteDataRepository(_baseUrl);
				}
				
				return this._client;
			}
		}

		public ConferencesDialogViewController () : base(UITableViewStyle.Plain, new RootElement("Conferences"), false)
		{

			if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) {
				// UIRefreshControl iOS6
				RefreshControl = new UIRefreshControl();
				RefreshControl.ValueChanged += (sender, e) => { Refresh(); };
			} else {
				// old style refresh button
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh), false);
				NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { Refresh(); };
			}

			Refresh();
		}

		public override void LoadView ()
		{
			base.LoadView ();

			this.View = this.TableView;
			if (ParentViewController != null && ParentViewController.View != null)
			{
				ParentViewController.View.BackgroundColor = UIColor.Red;
			}
		}

		public void Refresh()
		{
			var loading = new UIAlertView (" Downloading Conferences", "Please wait...", null, null, null);
			
			loading.Show ();
			
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
			indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
			indicator.StartAnimating (); 
			loading.AddSubview (indicator);

			Repository.GetConferences (sortBy: "", showPastConferences: true, search: "", callback: conferences => 
			{ 
				InvokeOnMainThread (() => 
				{ 
					TableView.Source = new ConferencesTableViewSource (this, conferences); 
					TableView.ReloadData (); 
					loading.DismissWithClickedButtonIndex (0, true);

					if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) {
						RefreshControl.EndRefreshing();
					}

				});
			});
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			var frame = this.View.Frame;
			TableView.Frame = frame;

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f,
				                                                                alpha:1f);
				
			}

		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			var frame = this.View.Frame;
			TableView.Frame = frame;

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f,
				                                                                alpha:1f);
				
			}
		}

		private class ConferencesTableViewSource : UITableViewSource, IImageUpdated
		{ 
			private readonly IList<ConferencesDto> _conferences;
			private const string ConferenceCell = "ConferenceCell";
			private ConferencesDialogViewController _rootViewController;
			private ConferenceDetailTabBarController _conferenceDetailViewController;
			
			public ConferencesTableViewSource (ConferencesDialogViewController controller, IList<ConferencesDto> conferences)
			{ 
				_rootViewController = controller;
				_conferences = conferences; 
			}
			
			public override int RowsInSection (UITableView tableView, int section)
			{ 
				return _conferences.Count; 
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{ 
				return 60; 
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{ 
				var cell = tableView.DequeueReusableCell (ConferenceCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, ConferenceCell); 
				var conference = _conferences [indexPath.Row]; 
				
				var mainFont = UIFont.FromName ("OpenSans", 14f);
				var detailFont = UIFont.FromName ("OpenSans", 12f);
				cell.TextLabel.Font = mainFont;
				cell.DetailTextLabel.Font = detailFont;
				
				if (!string.IsNullOrWhiteSpace (conference.imageUrl)) {
					var logo = ImageLoader.DefaultRequestImage (new Uri ("http://www.tekconf.com" + conference.imageUrl), this);
					if (logo == null) {
						//logoImage.Image = DefaultImage;
					} else {
						cell.ImageView.Image = logo;
					}
				}
				
				cell.TextLabel.Text = conference.name; 
				cell.DetailTextLabel.Text = conference.CalculateConferenceDates (conference); 
				return cell; 
			}
			protected static bool UserInterfaceIdiomIsPhone {
				get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{ 
				var selectedConference = _conferences [indexPath.Row]; 
				//new UIAlertView ("View Conference", selectedConference.name, null, "Ok", null).Show (); 
				
				if (UserInterfaceIdiomIsPhone) {
					_conferenceDetailViewController = new ConferenceDetailTabBarController (selectedConference.slug);
					_rootViewController.NavigationController.PushViewController (
						_conferenceDetailViewController,
						true
						);
				} else {
					// Navigation logic may go here -- for example, create and push another view controller.
				}
				
			} 
			
			#region IImageUpdated implementation
			
			public void UpdatedImage (Uri uri)
			{
				//logoImage.Image = ImageLoader.DefaultRequestImage(uri, this);
			}
			
#endregion
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
				var font = UIFont.FromName ("OpenSans", 12f);
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
					controller.conferencesTableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
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
			protected static bool UserInterfaceIdiomIsPhone {
				get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (UserInterfaceIdiomIsPhone) {
					//					var DetailViewController = new ConferenceDetailViewController ();
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
