using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace TekConf.UI.Android.Views
{
	using System.Threading;

	using Cirrious.MvvmCross.Binding.Droid.Views;

	using global::Android.Views;

	[Activity(Label = "Conferences")]
	public class ConferencesListView : MvxActivity
	{
		private BindableProgress _bindableProgress;

		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConferencesListView);

			_bindableProgress = new BindableProgress(this);

			var set = this.CreateBindingSet<ConferencesListView, ConferencesListViewModel>();
			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsLoadingConferences);
			set.Apply();

			ActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(r:129,g:153,b:77)));
			ActionBar.SetDisplayShowHomeEnabled(false);

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.ConferencesListActionItems,menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferencesListViewModel;
			if (vm != null) 
			{
				switch (item.ToString ()) 
				{
					case "Search":
					//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
					break;

					case "Refresh":
					vm.Refresh ();
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