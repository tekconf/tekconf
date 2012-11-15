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
	public class SpeakerElement : Element, IElementSizing
	{
		private FullSpeakerDto _speaker;
		
		public SpeakerElement (FullSpeakerDto speaker) : base(string.Empty)
		{
			_speaker = speaker;
		}
		
		static NSString MyKey = new NSString ("SessionCell");
		protected override NSString CellKey {
			get {
				return MyKey;
			}
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (CellKey) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, CellKey); 
			
			cell.TextLabel.Font = BaseUIViewController.TitleFont;
			cell.DetailTextLabel.Font = BaseUIViewController.DescriptionFont;
			
			cell.TextLabel.Text = _speaker.fullName;
			cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			cell.TextLabel.Lines = 0;
			cell.TextLabel.SizeToFit();
			cell.SizeToFit();
			
			if (!string.IsNullOrEmpty(_speaker.twitterName))
			{
				cell.DetailTextLabel.Text = _speaker.twitterName;
			}
			
			return cell; 
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
