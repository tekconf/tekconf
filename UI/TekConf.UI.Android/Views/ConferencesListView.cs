using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace TekConf.UI.Android.Views
{
	using System.Threading;

	using Cirrious.MvvmCross.Binding.Droid.Views;

	using global::Android.Views;

	[Activity(Label = "Conferences")]
	public class ConferencesListView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConferencesListView);
		}
 
	}
}