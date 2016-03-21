using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;

namespace TekConf.Mobile.iOS
{
	public partial class SettingsView : MvxViewController
	{
		public SettingsView() : base("SettingsView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();
			//set.Bind(Label).To(vm => vm.Hello);
			//set.Bind(TextField).To(vm => vm.Hello);
			set.Apply();
		}
	}
}


