using Android.App;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.UI.Android;

namespace KittenView.Droid
{
	[Activity(Label = "TekConf", MainLauncher = true, Icon = "@drawable/icon")]
	public class SplashScreen : MvxSplashScreenActivity
	{
		public SplashScreen()
			: base(Resource.Layout.SplashScreen)
		{
		}
	}
}