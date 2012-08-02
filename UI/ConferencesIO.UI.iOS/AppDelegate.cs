using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ConferencesIO.RemoteData.v1;
using ServiceStack.Text;
using System.IO;

namespace ConferencesIO.UI.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		private RemoteDataRepository _client;
		private string _baseUrl = "http://conferencesioapi.azurewebsites.net/v1/";

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

		public override bool FinishedLaunching (UIApplication application, NSDictionary launcOptions)
		{
			//CustomizeAppearance();
			GetLatestFullConference();
			// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
			return true;
		}

		void GetLatestFullConference ()
		{
			_client = new RemoteDataRepository (_baseUrl);
			_client.GetFullConference ("CodeMash-2012", conference => 
			{ 

				var x = conference;

				var json = JsonSerializer.SerializeToString(conference);
				string path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string filePath = Path.Combine(path, "fullConference.json");
				File.WriteAllText(filePath, json);
			});
		}

		private void CustomizeAppearance()
		{
			UIImage gradientImage44 = UIImage.FromBundle(@"images/appview/top-bar");
			UIEdgeInsets capInsets = new UIEdgeInsets(0,0,0,0);
			gradientImage44.CreateResizableImage(capInsets);
			UINavigationBar.Appearance.SetBackgroundImage(gradientImage44, UIBarMetrics.Default);

			UITextAttributes titleAttributes = new UITextAttributes() { 
				TextColor = UIColor.FromRGBA(red:0.204f, green:0.212f, blue:0.239f, alpha:1f), 
				TextShadowColor = UIColor.FromRGBA(red:1f, green:1f, blue:1f, alpha:0.8f), 
				TextShadowOffset = new UIOffset(horizontal:0, vertical:1),
				Font = UIFont.FromName("Helvetica-Neue", size:0.0f),
			};

			UINavigationBar.Appearance.SetTitleTextAttributes(titleAttributes);
		}
	}
}

