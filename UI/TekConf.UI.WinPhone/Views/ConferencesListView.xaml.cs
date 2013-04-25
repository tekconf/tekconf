using System.Windows;
using System.Windows.Controls;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesListView : MvxPhonePage
	{
		public ConferencesListView()
		{
			InitializeComponent();
		}

		private void Conference_OnSelected(object sender, SelectionChangedEventArgs e)
		{
			var vm = this.DataContext as ConferencesListViewModel;
			var conference = ((sender as ListBox).SelectedItem) as FullConferenceDto;
			vm.ShowDetailCommand.Execute(conference.slug);
		}

		private void ConferenceName_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var textBlock = (sender as TextBlock);
			textBlock.MaxWidth = this.ActualWidth - 20;
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var image = (sender as Image);
			image.Width = this.ActualWidth - 20;
			image.Height = 180 * (image.Width / 260);
		}
	}
}