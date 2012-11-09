using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public class SessionDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab;
		
		private FullSessionDto _session;
		public SessionDetailTabBarController (FullSessionDto session)
		{
			_session = session;
			this.Title = _session.title;
			aboutTab = new SessionDetailAboutViewController(_session);
			aboutTab.Title = "About";
			
//			sessionsTab = new SessionDetailSessionsViewController(_session);
//			sessionsTab.Title = "Sessions";
			
			#region Additional Info
			//			tab1.TabBarItem = new UITabBarItem (UITabBarSystemItem.History, 0); // sets image AND text
			//			tab2.TabBarItem = new UITabBarItem ("Orange", UIImage.FromFile("Images/first.png"), 1);
			//			tab3.TabBarItem = new UITabBarItem ();
			//			tab3.TabBarItem.Image = UIImage.FromFile("Images/second.png");
			//			tab3.TabBarItem.Title = "Rouge"; // this overrides tab3.Title set above
			//			tab3.TabBarItem.BadgeValue = "4";
			//			tab3.TabBarItem.Enabled = false;
#endregion
			
			var tabs = new UIViewController[] {
				aboutTab
			};
			
			ViewControllers = tabs;
			
			SelectedViewController = aboutTab; // normally you would default to the left-most tab (ie. tab1)
		}
		
	}
}