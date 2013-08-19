using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;
using Android.OS;

namespace TekConf.UI.Android
{
	[Activity(Label = "Settings", Icon="@drawable/icon")]
	public class SettingsView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SettingsView);

			var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();
			set.Apply();
		}
	}
}

