using System;
using MonoTouch.UIKit;

namespace TekConf.UI.iPhone
{
	public class ConferenceDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab, sessionsTab, speakersTab;
		
		public ConferenceDetailTabBarController ()
		{
			aboutTab = new ConferenceDetailAboutViewController();
			aboutTab.Title = "About";

			sessionsTab = new ConferenceDetailViewController();
			sessionsTab.Title = "Sessions";
			
			speakersTab = new ConferenceDetailSpeakersViewController();
			speakersTab.Title = "Speakers";
			
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
				aboutTab, sessionsTab, speakersTab
			};
			
			ViewControllers = tabs;
			
			SelectedViewController = aboutTab; // normally you would default to the left-most tab (ie. tab1)
		}
	}
}
