using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;

namespace TekConf.Mobile.iOS
{
	public partial class ConferencesView : MvxViewController
	{
		public ConferencesView() : base("ConferencesView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel>();
			//set.Bind(Label).To(vm => vm.Hello);
			//set.Bind(TextField).To(vm => vm.Hello);
			set.Apply();
		}
	}
}


