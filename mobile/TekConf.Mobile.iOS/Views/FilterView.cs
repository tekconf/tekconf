using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;

namespace TekConf.Mobile.iOS
{
	public partial class FilterView : MvxViewController
	{
		public FilterView() : base("FilterView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var set = this.CreateBindingSet<FilterView, FilterViewModel>();
			//set.Bind(Label).To(vm => vm.Hello);
			//set.Bind(TextField).To(vm => vm.Hello);
			set.Apply();
		}
	}
}


