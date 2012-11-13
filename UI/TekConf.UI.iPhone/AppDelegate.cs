using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;
using FA=FlurryAnalytics;

namespace TekConf.UI.iPhone
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
        public SlideoutNavigationController Menu { get; private set; }

		const string account = "UA-20184526-3";

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			GoogleAnalytics.GANTracker.SharedTracker.StartTracker(account, 10, null);

			FA.FlurryAnalytics.StartSession("J57HPDDQQ8J8MVGKBKF7");
			FA.FlurryAnalytics.SetSessionReportsOnPause(true);

			var font = UIFont.FromName("OpenSans-Light", 14f);
			var headerFont = UIFont.FromName("OpenSans", 16f);
			var detailFont = UIFont.FromName ("OpenSans", 12f);

			UITextAttributes buttonAttributes = new UITextAttributes() { Font = detailFont };
			
			UIBarButtonItem.Appearance.SetTitleTextAttributes(buttonAttributes, UIControlState.Normal);
			UITextAttributes attributes = new UITextAttributes() { Font = headerFont };
			UINavigationBar.Appearance.SetTitleTextAttributes(attributes);
			UILabel.Appearance.Font = font;
		
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			Menu = new SlideoutNavigationController();
			Console.WriteLine("AD1");
			Menu.TopView = new ConferencesDialogViewController();
			//Menu.TopView = new ConferencesListViewController();
			Console.WriteLine("AD2");
			
			Menu.MenuView = new SideListController();
			
			window.RootViewController = Menu;
			window.MakeKeyAndVisible ();
			NSError error;

			var success = GoogleAnalytics.GANTracker.SharedTracker.TrackEvent("App events","App Finished Launching", "", 1, out error);
			return true;
		}
	}

	public class SideListController : DialogViewController
	{
		public SideListController() 
			: base(UITableViewStyle.Plain, new RootElement(""))
		{
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var font = UIFont.FromName("OpenSans", 14f);
			
			Root.Add(new Section() {
				new StyledStringElement("Conferences", () => { NavigationController.PushViewController(new ConferencesDialogViewController(), true); }) { Font = font },

				new StyledStringElement("CodeMash 2013", () => { NavigationItems.ConferenceSlug = "codemash-2013"; NavigationController.PushViewController(new ConferenceDetailTabBarController(), true); }) { Font = font },
				new StyledStringElement("Build 2013", () => { NavigationItems.ConferenceSlug = "build-2012"; NavigationController.PushViewController(new ConferenceDetailTabBarController(), true); }) { Font = font },
				new StyledStringElement("Settings", () => { NavigationController.PushViewController(new SettingsViewController(), true); }) { Font = font },
				new StyledStringElement("Login", () => { NavigationController.PushViewController(new LoginViewController(), true); }) { Font = font },
				
				//new StyledStringElement("Stuff", () => { NavigationController.PushViewController(new StuffViewController(), true); }),
				//new StyledStringElement("Full Screen", () => { NavigationController.PushViewController(new FullscreenViewController(), true); })
			});

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f, 
				                                                                alpha:1f);

			}
		}
	}
}

