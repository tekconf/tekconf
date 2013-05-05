using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Platform;
using Windows.UI.Xaml.Controls;
using TekConf.Core.Interfaces;

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
			Mvx.RegisterType<IAnalytics, WinStoreAnalytics>();
			return new TekConf.Core.App();
		}
	}
}