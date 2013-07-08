using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;
using TekConf.Core.Interfaces;
using TekConf.Core.Repositories;
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
			//MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;
			Mvx.RegisterType<IAnalytics, WinPhoneAnalytics>();
			//Mvx.RegisterType<IAuthentication, Authentication>();
			Mvx.RegisterSingleton(typeof(IAuthentication), new Authentication());
			Mvx.RegisterType<ICacheService, CacheService>();
			Mvx.RegisterType<ILocalNotificationsRepository, LocalNotificationsRepository>();
			Mvx.RegisterType<ILocalConferencesRepository, LocalConferencesRepository>();
			Mvx.RegisterType<ILocalScheduleRepository, LocalScheduleRepository>();
			//Mvx.RegisterType<ILocalSessionRepository, LocalSessionRepository>();

			Mvx.RegisterType<IPushSharpClient, PushSharpClient>();
			return new Core.App();
		}
	}

}