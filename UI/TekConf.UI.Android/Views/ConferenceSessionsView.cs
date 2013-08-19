using TekConf.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;

	[Activity(Label = "Sessions", Icon="@drawable/icon")]
	public class ConferenceSessionsView : MvxActivity
	{
		private BindableProgress _bindableProgress;


		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConferenceSessionsView);

			_bindableProgress = new BindableProgress(this);

			var set = this.CreateBindingSet<ConferenceSessionsView, ConferenceSessionsViewModel>();
			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsLoadingConference);
			set.Apply();
		}
	}
}