using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;

namespace TekConf.UI.WinPhone.Views
{
	public partial class OAuthRegisterView : MvxPhonePage
	{
		public OAuthRegisterView()
		{
			InitializeComponent();
		}

		private void Register_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = this.DataContext as OAuthRegisterViewModel;
			if (vm != null)
				vm.CreateOAuthUser();
		}
	}
}