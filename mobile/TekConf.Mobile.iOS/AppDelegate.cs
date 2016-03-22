using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;
using MvvmCross.iOS.Views.Presenters;

namespace TekConf.Mobile.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : MvxApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var presenter = new MvxModalSupportIosViewPresenter(this, Window);

			var setup = new Setup(this, presenter);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			AdjustDefaultUI ();

			Window.MakeKeyAndVisible();

			return true;
		}

		private void AdjustDefaultUI ()
		{

			//UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(red: 34, green: 91, blue: 149);
			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB (red: 128, green: 153, blue: 77);
			UIBarButtonItem.Appearance.TintColor = UIColor.White;

			UINavigationBar.Appearance.TintColor = UIColor.White;
			var navStyle = new UITextAttributes () {
				TextColor = UIColor.White,
				TextShadowColor = UIColor.Clear,

				Font = UIFont.FromName ("OpenSans-Light", 16f)
			};

			UINavigationBar.Appearance.SetTitleTextAttributes (navStyle);
			UIImageView.AppearanceWhenContainedIn (typeof (UINavigationBar)).TintColor = UIColor.White;
			UIBarButtonItem.Appearance.SetTitleTextAttributes (navStyle, UIControlState.Normal);
		}
	}
}


