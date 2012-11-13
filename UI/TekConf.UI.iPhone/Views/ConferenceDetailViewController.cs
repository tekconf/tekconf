using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using System.Linq;

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


			sessionsTableView.Source = new SessionsTableViewSource (this, _conference.sessions); 
			sessionsTableView.ReloadData (); 
			loading.DismissWithClickedButtonIndex (0, true); 
			//}

			if (!UserInterfaceIdiomIsPhone) {
				this.sessionsTableView.SelectRow (
					NSIndexPath.FromRowSection (0, 0),
					false,
					UITableViewScrollPosition.Middle
				);
			}

			if (_conference != null)
			{
				TrackAnalyticsEvent("ConferenceDetailViewController-" + _conference.slug);
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
			private SessionDetailTabBarController _sessionDetailTabBarViewController;
			private List<DateTime> _sessionStartTimes;

			public SessionsTableViewSource (ConferenceDetailViewController controller, IList<FullSessionDto> sessions)
			{ 
				_rootViewController = controller;
				_sessions = sessions; 
			
				_sessionStartTimes = _sessions.Select (x => x.start).Distinct ().ToList ();
			}


//			public override string[] SectionIndexTitles (UITableView tableView)
//			{
//				return _sessionStartTimes.Select (x => x.ToString ("dddd h:mm tt")).ToArray ();
//			}

//			public override UIView GetViewForHeader (UITableView tableView, int section)
//			{
//				var label = new UILabel();
//				label.Text = "Here";
//
//				return label;
//			}

			public override int NumberOfSections (UITableView tableView)
			{
				return _sessionStartTimes.Count ();
			}

			public override string TitleForHeader (UITableView tableView, int section)
			{
				return _sessionStartTimes [section].ToString ("dddd h:mm tt");
			}

			public override int SectionFor (UITableView tableView, string title, int atIndex)
			{
				return atIndex;
			}

			public override int RowsInSection (UITableView tableView, int section)
			{ 
				return _sessions.ToList ().FindAll (x => x.start == _sessionStartTimes [section]).Count;
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{ 
				return 60; 
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{ 
				var startTimeSection = _sessionStartTimes [indexPath.Section];
				var filteredSessions = _sessions.Where (s => s.start == startTimeSection).OrderBy (o => o.title).ToArray ();

				var cell = tableView.DequeueReusableCell (SessionCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, SessionCell); 
				var session = filteredSessions [indexPath.Row]; 

				var font = UIFont.FromName ("OpenSans", 12f);
				cell.TextLabel.Font = font;
				cell.TextLabel.Text = session.title;
				cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
				cell.TextLabel.Lines = 0;
				cell.TextLabel.SizeToFit();
				cell.SizeToFit();
				//cell.DetailTextLabel.Font = font;

				//cell.DetailTextLabel.Text = session.startDescription;
				return cell; 
			}


			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{ 
				var startTimeSection = _sessionStartTimes [indexPath.Section];
				var filteredSessions = _sessions.Where (s => s.start == startTimeSection).OrderBy (o => o.title).ToArray ();
				
				//var cell = tableView.DequeueReusableCell (SessionCell) ?? new UITableViewCell (UITableViewCellStyle.Subtitle, SessionCell); 
				var selectedSession = filteredSessions [indexPath.Row]; 

				//var selectedSession = _sessions [indexPath.Row]; 
				
				if (UserInterfaceIdiomIsPhone) {
					_sessionDetailTabBarViewController = new SessionDetailTabBarController (selectedSession);
					
					_rootViewController.NavigationController.PushViewController (
						_sessionDetailTabBarViewController,
						true
					);
				} else {
					// Navigation logic may go here -- for example, create and push another view controller.
				}
			} 
		}

	}
}

