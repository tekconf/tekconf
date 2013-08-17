using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;

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
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConferencesListView);

			_bindableProgress = new BindableProgress(this);

			var set = this.CreateBindingSet<ConferencesListView, ConferencesListViewModel>();
			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsLoadingConferences);
			set.Apply();
		}
	}
}