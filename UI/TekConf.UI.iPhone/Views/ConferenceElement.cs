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

	public class ConferenceElement : Element, IImageUpdated
	{
		private ConferencesDto _conference;
		private UIImage _defaultImage;
		
		public ConferenceElement (ConferencesDto conference, UIImage defaultImage) : base(string.Empty)
		{
			_conference = conference;
			_defaultImage = defaultImage;
		}
		
		static NSString MyKey = new NSString ("ConferenceCell");
		protected override NSString CellKey {
			get {
				return MyKey;
			}
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (CellKey) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, CellKey); 
			
			var mainFont = UIFont.FromName ("OpenSans", 14f);
			var detailFont = UIFont.FromName ("OpenSans", 12f);
			cell.TextLabel.Font = mainFont;
			cell.DetailTextLabel.Font = detailFont;
			
			if (!string.IsNullOrWhiteSpace (_conference.imageUrl)) {
				var logo = ImageLoader.DefaultRequestImage (new Uri ("http://www.tekconf.com" + _conference.imageUrl), this);
				if (logo == null) {
					cell.ImageView.Image = _defaultImage;
				} else {
					cell.ImageView.Image = logo;
				}
			}
			
			cell.TextLabel.Text = _conference.name; 
			cell.DetailTextLabel.Text = _conference.CalculateConferenceDates (_conference); 
			return cell; 

			//return base.GetCell (tv);
		}

		public void UpdatedImage (Uri uri)
		{
			//var cell = tableView.DequeueReusableCell (ConferenceCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, ConferenceCell); 
			
			//logoImage.Image = ImageLoader.DefaultRequestImage(uri, this);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}
		
		public override string Summary ()
		{
			return base.Summary ();
		}

		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			//var selectedConference = _conferences [indexPath.Row];  
			
			if (UserInterfaceIdiomIsPhone) {
				NavigationItems.ConferenceSlug = _conference.slug;

				var _conferenceDetailViewController = new ConferenceDetailTabBarController ();
				dvc.NavigationController.PushViewController (
					_conferenceDetailViewController,
					false
					);
			} else {
				// Navigation logic may go here -- for example, create and push another view controller.
			}

			//base.Selected (dvc, tableView, path);
		}
		
		public override bool Matches (string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower();
			}

			if (_conference.name.ToLower().Contains(text))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
}
