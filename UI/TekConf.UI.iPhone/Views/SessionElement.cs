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

	public class SessionElement : Element, IElementSizing
	{
		private SessionsDto _session;

		public SessionElement (SessionsDto session) : base(string.Empty)
		{
			_session = session;
		}

		public SessionElement (string title, string room) : base(string.Empty)
		{
			_session = new SessionsDto() { title = title, room = room };	
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

			cell.TextLabel.Text = _session.title;
			cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			cell.TextLabel.Lines = 0;
			cell.TextLabel.SizeToFit();
			cell.SizeToFit();

			if (string.IsNullOrEmpty(_session.room))
			{
				cell.DetailTextLabel.Text = "No room set";
			}
			else
			{
				cell.DetailTextLabel.Text = _session.room;
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
			var titleSize = tableView.StringSize(_session.title, BaseUIViewController.TitleFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
			SizeF descriptionSize = new SizeF(0,0);
			if (!string.IsNullOrWhiteSpace(_session.room))
			{
				descriptionSize = tableView.StringSize(_session.room, BaseUIViewController.DescriptionFont, new SizeF(237.0f, 1000.0f), UILineBreakMode.WordWrap);
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

				var sessionDetailTabBarViewController = new SessionDetailTabBarController (_session.slug, _session.title);
				
				dvc.NavigationController.PushViewController (
					sessionDetailTabBarViewController,
					true
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

			if (_session.title.ToLower().Contains(text))
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
