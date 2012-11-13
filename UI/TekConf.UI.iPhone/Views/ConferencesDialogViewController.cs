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

	public class ConferencesDialogViewController : BaseDialogViewController
	{

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
			if (this.IsReachable())
			{
				var loading = new UIAlertView (" Downloading Conferences", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);

				Repository.GetConferences (sortBy: "", showPastConferences: false, search: "", callback: conferences => 
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
			else
			{
				UnreachableAlert().Show();
			}

			TrackAnalyticsEvent("ConferencesDialogViewController");
		}
	
	


		private class ConferencesTableViewSource : UITableViewSource, IImageUpdated
		{ 
			private UIImage _defaultImage;
			private readonly IList<ConferencesDto> _conferences;
			private const string ConferenceCell = "ConferenceCell";
			private ConferencesDialogViewController _rootViewController;
			private ConferenceDetailTabBarController _conferenceDetailViewController;
			
			public ConferencesTableViewSource (ConferencesDialogViewController controller, IList<ConferencesDto> conferences)
			{ 

				_rootViewController = controller;
				_conferences = conferences;
				_defaultImage = UIImage.FromBundle(@"images/DefaultConference.png");
			}
			
			public override int RowsInSection (UITableView tableView, int section)
			{ 
				if (_conferences == null)
				{
					return 0;
				}
				else
				{
					return _conferences.Count; 
				}
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
						cell.ImageView.Image = _defaultImage;
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
				
				if (UserInterfaceIdiomIsPhone) {
					_conferenceDetailViewController = new ConferenceDetailTabBarController (selectedConference.slug);
					_rootViewController.NavigationController.PushViewController (
						_conferenceDetailViewController,
						false
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
			
			public DataSource ()
			{
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return 1;
			}

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

				return cell;
			}

			protected static bool UserInterfaceIdiomIsPhone {
				get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
			}


		}

	}
	
}
