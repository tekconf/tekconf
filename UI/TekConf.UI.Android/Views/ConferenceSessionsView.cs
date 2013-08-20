using TekConf.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;

	[Activity(Label = "Sessions")]
	public class ConferenceSessionsView : MvxActivity
	{
		private BindableProgress _bindableProgress;


		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConferenceSessionsView);

			_bindableProgress = new BindableProgress(this);

			var set = this.CreateBindingSet<ConferenceSessionsView, ConferenceSessionsViewModel>();
			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsLoadingConference);
			set.Apply();

			ActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(r:129,g:153,b:77)));
			ActionBar.SetDisplayShowHomeEnabled(false);

		}
	}
}