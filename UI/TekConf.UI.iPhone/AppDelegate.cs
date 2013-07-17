using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TekConf.UI.iPhone
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		// class-level declarations
		UIWindow _window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the _window, load the UI into it and then make the _window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			// create a new _window instance based on the screen size
			_window = new UIWindow(UIScreen.MainScreen.Bounds);

			var setup = new Setup(this, _window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();
			// If you have defined a view, add it here:
			// _window.AddSubview (navigationController.View);

			// make the _window visible
			_window.MakeKeyAndVisible();

			return true;
		}
	}
}