using Android.App;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.UI.Android;

namespace TekConf.UI.Android
{
	[Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Splash", NoHistory = true)]
	public class SplashScreen : MvxSplashScreenActivity
	{
		public SplashScreen() : base(Resource.Layout.SplashScreen)
		{
		}
	}
}