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


	public class ConferenceElement : Element, IImageUpdated, IElementSizing
	{
		private ConferencesDto _conference;
		private UIImage _defaultImage;
		private UITableViewCell _cell;

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
			_cell = tv.DequeueReusableCell (CellKey) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, CellKey); 

			_cell.TextLabel.Font = BaseUIViewController.TitleFont;
			_cell.DetailTextLabel.Font = BaseUIViewController.DescriptionFont;

			if (!string.IsNullOrWhiteSpace (_conference.imageUrl) && !_conference.imageUrl.Contains("DefaultConference.png")) {
				var logo = ImageLoader.DefaultRequestImage (new Uri (_conference.imageUrl), this);
				if (logo == null) {

					_cell.ImageView.Image = _defaultImage;
//					_cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
//					_cell.ImageView.ClipsToBounds = true;
//					RectangleF frame = new RectangleF(_cell.ImageView.Frame.X, _cell.ImageView.Frame.Y, 260f, 180f);
//					_cell.ImageView.Frame = frame;
				} else {

					_cell.ImageView.Image = logo;
//					_cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
//					_cell.ImageView.ClipsToBounds = true;
//					RectangleF frame = new RectangleF(_cell.ImageView.Frame.X, _cell.ImageView.Frame.Y, 260f, 180f);
//					_cell.ImageView.Frame = frame;
				}
			}
			else
			{

				_cell.ImageView.Image = _defaultImage;
//				_cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
//				_cell.ImageView.ClipsToBounds = true;
//				RectangleF frame = new RectangleF(_cell.ImageView.Frame.X, _cell.ImageView.Frame.Y, 260f, 180f);
//				_cell.ImageView.Frame = frame;
			}
			
			_cell.TextLabel.Text = _conference.name;
			//_cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			//_cell.TextLabel.Lines = 0;
			//_cell.TextLabel.SizeToFit();
			_cell.SizeToFit();

			_cell.DetailTextLabel.Text = _conference.CalculateConferenceDates (_conference); 
			return _cell;
		}

		public void UpdatedImage (Uri uri)
		{
			_cell.ImageView.Image = ImageLoader.DefaultRequestImage (uri, this);
//			_cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
//			_cell.ImageView.ClipsToBounds = true;
//			RectangleF frame = new RectangleF(_cell.ImageView.Frame.X, _cell.ImageView.Frame.Y, 260f, 180f);
//			_cell.ImageView.Frame = frame;
			//var cell = tableView.DequeueReusableCell (ConferenceCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, ConferenceCell); 
			
			//logoImage.Image = ImageLoader.DefaultRequestImage(uri, this);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}

		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			var titleSize = tableView.StringSize(_conference.name, BaseUIViewController.TitleFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
			SizeF descriptionSize = new SizeF(0,0);

			var startDate = _conference.CalculateConferenceDates (_conference);
			if (!string.IsNullOrWhiteSpace(startDate))
			{
				descriptionSize = tableView.StringSize(startDate, BaseUIViewController.DescriptionFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
			}

			var sizeTotal = new SizeF(237.0f, titleSize.Height + descriptionSize.Height + 20);
			var cellSize = sizeTotal;
			
			return cellSize.Height + 10; 
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
