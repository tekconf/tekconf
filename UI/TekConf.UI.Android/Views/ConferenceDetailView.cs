using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using TekConf.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using System.Collections.Generic;
using System;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;
	

	[Activity(Label = "Detail", Icon="@drawable/icon")]
	public class ConferenceDetailView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.ConferenceDetailView);

			var set = this.CreateBindingSet<ConferenceDetailView, ConferenceDetailViewModel>();
			set.Apply();

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.ConferenceDetailActionItems,menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferenceDetailViewModel;
			if (vm != null) 
			{
				switch (item.ToString ()) 
				{
					case "Sessions":
					vm.ShowSessionsCommand.Execute(vm.Conference.slug);
					break;
					case "Favorite":

					break;
				case "Refresh":
					vm.Refresh (vm.Conference.slug);
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