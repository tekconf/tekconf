using System;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;

namespace TekConf.UI.iPhone
{
	public class BaseUITabBarController : UITabBarController
	{
	
		protected UIAlertView UnreachableAlert()
		{
			return new UIAlertView("Unreachable", "Can not access TekConf.com. Check internet connection.", null, "OK", null);
		}

		protected bool IsReachable()
		{
			return Reachability.IsHostReachable("api.tekconf.com");
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

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f, 
				                                                                alpha:1f);
			}
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
		
		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
	}
	
}
