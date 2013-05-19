using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesSearchView : MvxPhonePage
	{
		public ConferencesSearchView()
		{
			InitializeComponent();
		}

		private void Search_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = this.DataContext as ConferencesSearchViewModel;
			if (vm != null)
				vm.SearchCommand.Execute(null);
		}
	}
}