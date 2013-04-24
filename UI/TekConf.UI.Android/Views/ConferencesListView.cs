using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace TekConf.UI.Android.Views
{
    [Activity(Label = "View for ConferencesListViewModel")]
    public class ConferencesListView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ConferencesListView);
        }
    }
}