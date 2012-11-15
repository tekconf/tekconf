using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{

	public class ConferenceDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab, scheduleTab, sessionsTab, speakersTab;


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetTabs();
		}

		public void SetTabs ()
		{
			aboutTab = new ConferenceDetailAboutViewController();
			aboutTab.Title = "About";
			aboutTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_088_address_book");

			sessionsTab = new SessionsListDialogViewController();
			sessionsTab.Title = "Sessions";
			sessionsTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_061_keynote");
			
			speakersTab = new ConferenceDetailSpeakersViewController();
			speakersTab.Title = "Speakers";
			speakersTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_042_group");

			scheduleTab = new ConferenceScheduleViewController();
			scheduleTab.Title = "My Schedule";
			scheduleTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_057_calendar");

			var tabs = new UIViewController[] {
				aboutTab, sessionsTab, speakersTab, scheduleTab
			};
			
			ViewControllers = tabs;
			
			SelectedViewController = aboutTab;
		}
	}
}
