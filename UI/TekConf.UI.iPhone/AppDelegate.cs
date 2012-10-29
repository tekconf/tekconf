using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;

namespace TekConf.UI.iPhone
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
        public SlideoutNavigationController Menu { get; private set; }



		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var font = UIFont.FromName("OpenSans-Light", 14f);
			var headerFont = UIFont.FromName("OpenSans", 16f);
			
			UITextAttributes attributes = new UITextAttributes() { Font = headerFont } ;
			UINavigationBar.Appearance.SetTitleTextAttributes(attributes);
			UILabel.Appearance.Font = font;
		
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			Menu = new SlideoutNavigationController();
			Menu.TopView = new ConferencesListViewController();
			Menu.MenuView = new SideListController();
			
			window.RootViewController = Menu;
			window.MakeKeyAndVisible ();
			
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

			var font = UIFont.FromName("OpenSans-Light", 16f);
			
			Root.Add(new Section() {
				new StyledStringElement("Conferences", () => { NavigationController.PushViewController(new ConferencesListViewController(), true); }) { Font = font },

				new StyledStringElement("CodeMash 2013", () => { NavigationController.PushViewController(new ConferenceDetailTabBarController("codemash-2013"), true); }) { Font = font },
				new StyledStringElement("Build 2013", () => { NavigationController.PushViewController(new ConferenceDetailTabBarController("build-2013"), true); }) { Font = font },
				new StyledStringElement("Settings", () => { NavigationController.PushViewController(new ConferenceDetailTabBarController("codemash-2013"), true); }) { Font = font },
				new StyledStringElement("Login", () => { NavigationController.PushViewController(new ConferenceDetailTabBarController("codemash-2013"), true); }) { Font = font },
				
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

