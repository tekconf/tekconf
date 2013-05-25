using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;

namespace TekConf.UI.WinPhone.Views
{
	public partial class TekConfLoginView : MvxPhonePage
	{
		public TekConfLoginView()
		{
			InitializeComponent();
		}

		private void Login_OnClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Not yet implemented");
			return;

			var vm = this.DataContext as TekConfLoginViewModel;
			if (vm != null)
				vm.Login();
		}
	}
}