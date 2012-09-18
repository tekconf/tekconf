using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace ArtekSoftware.Conference.Mobile.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UINavigationController navigationController;
		UISplitViewController splitViewController;
		UIWindow window;


		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// load the appropriate UI, depending on whether the app is running on an iPhone or iPad
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				var controller = new RootViewController ();
				navigationController = new UINavigationController (controller);
				window.RootViewController = navigationController;
			} else {
				var masterViewController = new RootViewController ();
				var masterNavigationController = new UINavigationController (masterViewController);
				var detailViewController = new DetailViewController ();
				var detailNavigationController = new UINavigationController (detailViewController);
				
				splitViewController = new UISplitViewController ();
				splitViewController.WeakDelegate = detailViewController;
				splitViewController.ViewControllers = new UIViewController[] {
					masterNavigationController,
					detailNavigationController
				};
				
				window.RootViewController = splitViewController;
			}

			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

