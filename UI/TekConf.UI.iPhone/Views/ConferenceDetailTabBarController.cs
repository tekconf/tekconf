using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public class ConferenceDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab, sessionsTab, speakersTab;
		public ConferenceDetailTabBarController (string conferenceSlug)
		{
			Repository.GetConference (conferenceSlug, conference => 
			{ 
				InvokeOnMainThread (() => 
				{ 
					if (conference != null)
					{
					
						SetTabs(conference);
					}
				});
			});
		}

//		public ConferenceDetailTabBarController (ConferencesDto conference)
//		{
//			SetTabs(conference);
//		}

		public void SetTabs (FullConferenceDto conference)
		{
			if (conference != null) {
				this.Title = conference.name;
			}
			aboutTab = new ConferenceDetailAboutViewController(conference);
			aboutTab.Title = "About";
			
			sessionsTab = new ConferenceDetailViewController(conference);
			sessionsTab.Title = "Sessions";
			sessionsTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_061_keynote");
			
			speakersTab = new ConferenceDetailSpeakersViewController(conference);
			speakersTab.Title = "Speakers";
			speakersTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_042_group");
			
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
