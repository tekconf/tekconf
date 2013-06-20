using Cirrious.MvvmCross.Test.Core;
using Moq;
using TekConf.Core.Interfaces;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace Cirrious.MvvmCross.Test
{
	public class TestBase : MvxIoCSupportingTest
	{
		protected override void AdditionalSetup()
		{
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var pushSharp = new Mock<IPushSharpClient>();

			Ioc.RegisterSingleton(typeof(IAuthentication), analytics.Object);
			//Mvx.RegisterType<IAuthentication, Authentication>();
			Ioc.RegisterSingleton(typeof(IAuthentication), authentication.Object);
			Ioc.RegisterType<ICacheService, CacheService>();
			Ioc.RegisterType<ILocalNotificationsRepository, LocalNotificationsRepository>();
			Ioc.RegisterType<ILocalConferencesRepository, LocalConferencesRepository>();
			Ioc.RegisterType<ILocalScheduleRepository, LocalScheduleRepository>();
			Ioc.RegisterType<ILocalSessionRepository, LocalSessionRepository>();

			Ioc.RegisterSingleton(typeof(IPushSharpClient), pushSharp.Object);

			base.AdditionalSetup();
		}
	}

}
