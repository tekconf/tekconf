using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Platform;
using Windows.UI.Xaml.Controls;

namespace TekConf.UI.WinStore
{
	public class Setup : MvxStoreSetup
	{
		public Setup(Frame rootFrame)
			: base(rootFrame)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new TekConf.Core.App();
		}
	}
}