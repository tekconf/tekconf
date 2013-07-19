using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using TekConf.Core.ViewModels;

namespace TekConf.UI.iPhone.Views
{
	[Register("ConferencesListView")]
	public class ConferencesListView : MvxViewController
	{
		public override void ViewDidLoad()
		{
			View = new UIView() { BackgroundColor = UIColor.White };
			base.ViewDidLoad();

			var label = new UISwitch(new RectangleF(10, 10, 300, 40));
			Add(label);
			var textField = new UITextField(new RectangleF(10, 50, 300, 40));
			Add(textField);

			var set = this.CreateBindingSet<ConferencesListView, ConferencesListViewModel>();
			set.Bind(label).To(vm => vm.IsAuthenticated);
			//set.Bind(textField).To(vm => vm.Hello);
			set.Apply();
		}
	}
}