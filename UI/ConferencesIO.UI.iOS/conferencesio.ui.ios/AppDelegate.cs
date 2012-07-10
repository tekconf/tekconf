using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace conferencesio.ui.ios
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		
		public override UIWindow Window {
			get;
			set;
		}
		
		//
		// This method is invoked when the application is about to move from active to inactive state.
		//
		// OpenGL applications should use this method to pause.
		//
		public override void OnResignActivation (UIApplication application)
		{
		}
		
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		
		// This method is called when the application is about to terminate. Save data, if needed. 
		public override void WillTerminate (UIApplication application)
		{
		}

//		UISplitViewController splitViewController;
//		public static AppDelegate CurrentAppDelegate;
//		private TabBarController _tabBarController;
//
//		public TabBarController TabBar {
//			get { return _tabBarController; }
//			set { _tabBarController = value; }
//		}
		
//		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
//		{	
//			CopyDb ();
//			
//			var bootstrapper = new Bootstrapper ();
//			bootstrapper.Initialize ();				
//			
//			CurrentAppDelegate = this;
//			Window = new UIWindow (UIScreen.MainScreen.Bounds);
//			
//			//Console.WriteLine("AppDelegate.FinishedLaunching - creating new TabBarController and DetailViewController");
//			
//			//this.TabBar = new TabBarController ();
//			//var detailViewController = new DetailViewController ();
//
//			//splitViewController = new UISplitViewController ();
//			//splitViewController.WeakDelegate = detailViewController;
//			//splitViewController.Delegate = new SplitDelegate ();
//			//splitViewController.ViewControllers = new UIViewController[] {
//				//this.TabBar,
//				//detailViewController
//			//};
//			
//			//Console.WriteLine("AppDelegate.FinishedLaunching - setting window.RootViewController");
//			
//			//Window.RootViewController = splitViewController;
//
//			Window.MakeKeyAndVisible ();
//			
//			return true;
//		}

		public void CopyDb ()
		{
			string dbname = "codemash.db3";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, dbname);
 
			string rootPath = Path.Combine (Environment.CurrentDirectory, "DefaultDatabase");
			string rootDbPath = Path.Combine (rootPath, dbname);
 
			var runtimeDbExists = File.Exists (db);
			var defaultDatabaseExists = File.Exists (rootDbPath);
			if (!runtimeDbExists && defaultDatabaseExists) {
				File.Copy (rootDbPath, db);
			
				//TestFlight.PassCheckpoint ("Copied default database");
			}
 
		}
	}
}

