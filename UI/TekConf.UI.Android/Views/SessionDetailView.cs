using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;

	[Activity(Label = "Session Detail", Icon="@drawable/icon")]
	public class SessionDetailView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SessionDetailView);

			var set = this.CreateBindingSet<SessionDetailView, SessionDetailViewModel>();
			set.Apply();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.SessionDetailActionItems, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as SessionDetailViewModel;
			if (vm != null) 
			{
				switch (item.ToString ()) 
				{
					case "Favorite":
					//TODO
					break;
				case "Refresh":
					var navigation = new SessionDetailViewModel.Navigation () 
					{
						ConferenceSlug = vm.ConferenceSlug,
						SessionSlug = vm.Session.slug
					};
					vm.Refresh (navigation);
					break;
					case "Settings":
					vm.ShowSettingsCommand.Execute (null);
					break;
				}
			}

			return false;
		}
	}
}