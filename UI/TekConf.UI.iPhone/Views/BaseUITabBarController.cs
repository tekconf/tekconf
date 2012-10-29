using System;
using MonoTouch.UIKit;

namespace TekConf.UI.iPhone
{
	public class BaseUITabBarController : UITabBarController
	{
	
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
	}
	
}
