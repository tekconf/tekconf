using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;
using Android.OS;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views;

namespace TekConf.UI.Android
{
	[Activity(Label = "Settings")]
	public class SettingsView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SettingsView);

			var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();
			set.Apply();

			ActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(r:129,g:153,b:77)));
			ActionBar.SetDisplayShowHomeEnabled(false);
		}
	}
}

