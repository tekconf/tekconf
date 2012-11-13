using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{

	public class ConferenceDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab, sessionsTab, speakersTab;


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetTabs();
		}

		public void SetTabs ()
		{
			aboutTab = new ConferenceDetailAboutViewController();
			aboutTab.Title = "About";
			
			sessionsTab = new ConferenceDetailViewController();
			sessionsTab.Title = "Sessions";
			sessionsTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_061_keynote");
			
			speakersTab = new ConferenceDetailSpeakersViewController();
			speakersTab.Title = "Speakers";
			speakersTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_042_group");
			
			var tabs = new UIViewController[] {
				aboutTab, sessionsTab, speakersTab
			};
			
			ViewControllers = tabs;
			
			SelectedViewController = aboutTab;
		}
	}
}
