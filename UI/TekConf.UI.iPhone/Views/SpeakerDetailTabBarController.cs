using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public class SpeakerDetailTabBarController : BaseUITabBarController {
		
		UIViewController aboutTab, sessionsTab;

		private FullSpeakerDto _speaker;
		public SpeakerDetailTabBarController (FullSpeakerDto speaker)
		{
			_speaker = speaker;

			this.Title = _speaker.fullName;

			aboutTab = new SpeakerDetailAboutViewController(_speaker);
			aboutTab.Title = "About";
			aboutTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_003_user");
			
			sessionsTab = new SpeakerDetailSessionsViewController(_speaker);
			sessionsTab.Title = "Sessions";
			sessionsTab.TabBarItem.Image = UIImage.FromBundle("images/glyphicons_061_keynote");
			
			var tabs = new UIViewController[] {
				aboutTab, sessionsTab
			};
			
			ViewControllers = tabs;
			
			SelectedViewController = aboutTab;
		}

	}
}