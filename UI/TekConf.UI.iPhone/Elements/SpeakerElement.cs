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
	public class SpeakerElement : Element, IImageUpdated, IElementSizing
	{
		private FullSpeakerDto _speaker;
		private UIImage _defaultImage;
		private UITableViewCell _cell;
		
		public SpeakerElement (FullSpeakerDto speaker, UIImage defaultImage) : base(string.Empty)
		{
			_speaker = speaker;
			_defaultImage = defaultImage;
		}
		
		static NSString MyKey = new NSString ("SessionCell");
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
			
			_cell.TextLabel.Text = _speaker.fullName;
			_cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			_cell.TextLabel.Lines = 0;
			_cell.TextLabel.SizeToFit();
			_cell.SizeToFit();
			
			if (!string.IsNullOrEmpty(_speaker.twitterName))
			{
				_cell.DetailTextLabel.Text = _speaker.twitterName;
			}

			if (!string.IsNullOrWhiteSpace (_speaker.profileImageUrl) && !_speaker.profileImageUrl.Contains("DefaultUser.png")) {
				var logo = ImageLoader.DefaultRequestImage (new Uri ("http://www.tekconf.com" + _speaker.profileImageUrl), this);
				if (logo == null) {
					_cell.ImageView.Image = _defaultImage;
				} else {
					_cell.ImageView.Image = logo;
				}
			}
			else
			{
				_cell.ImageView.Image = _defaultImage;
			}
			
			return _cell; 
		}

		public void UpdatedImage (Uri uri)
		{
			_cell.ImageView.Image = ImageLoader.DefaultRequestImage (uri, this);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}
		
		#region IElementSizing implementation
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			var titleSize = tableView.StringSize(_speaker.fullName, BaseUIViewController.TitleFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
			SizeF descriptionSize = new SizeF(0,0);
			if (!string.IsNullOrWhiteSpace(_speaker.twitterName))
			{
				descriptionSize = tableView.StringSize(_speaker.twitterName, BaseUIViewController.DescriptionFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
			}
			var sizeTotal = new SizeF(237.0f, titleSize.Height + descriptionSize.Height + 20);
			var cellSize = sizeTotal;
			
			return cellSize.Height + 10; 
		}
		
#endregion
		
		public override string Summary ()
		{
			return base.Summary ();
		}
		
		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			//var selectedSession = tableView [path.Row];  
			
			if (UserInterfaceIdiomIsPhone) {
				
				var speakerDetailTabBarViewController = new SpeakerDetailTabBarController (_speaker);
				
				dvc.NavigationController.PushViewController (
					speakerDetailTabBarViewController,
					true
					);
			} else {
				// Navigation logic may go here -- for example, create and push another view controller.
			}
		}
		
		public override bool Matches (string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower();
			}
			
			if (_speaker.fullName.ToLower().Contains(text))
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
