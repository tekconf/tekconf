using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;
using TekConf.UI.WinPhone.Bootstrap;

namespace TekConf.UI.WinPhone
{
	public class Setup : MvxPhoneSetup
	{
		public Setup(PhoneApplicationFrame rootFrame)
			: base(rootFrame)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;
			Mvx.RegisterType<IAnalytics, WinPhoneAnalytics>();
			Mvx.RegisterType<IAuthentication, Authentication>();
			return new Core.App();
		}
	}
}