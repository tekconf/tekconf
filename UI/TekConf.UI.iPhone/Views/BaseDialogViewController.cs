using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Dialog;

namespace TekConf.UI.iPhone
{
	public class BaseDialogViewController : DialogViewController
	{
		public BaseDialogViewController (UITableViewStyle style, RootElement root, bool pushing) : base(style, root, pushing)
		{
			
		}

		private string _baseUrl = "http://api.tekconf.com";
		private RemoteDataRepository _client;
		protected RemoteDataRepository Repository
		{
			get
			{
				if (this._client == null)
				{
					this._client = new RemoteDataRepository(_baseUrl);
				}
				
				return this._client;
			}
		}

		protected void TrackAnalyticsEvent(string eventName)
		{
			FlurryAnalytics.FlurryAnalytics.LogEvent(eventName);
			NSError error;
			var success = GoogleAnalytics.GANTracker.SharedTracker.TrackPageView(eventName, out error);
		}

		protected UIAlertView UnreachableAlert()
		{
			return new UIAlertView("Unreachable", "Can not access TekConf.com. Check internet connection.", null, "OK", null);
		}
		
		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			
			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f,
				                                                                alpha:1f);
				
			}
			
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f,
				                                                                alpha:1f);
				
			}
			
		}

		protected bool IsReachable()
		{
			return Reachability.IsHostReachable("api.tekconf.com");
		}
	}
	
}
