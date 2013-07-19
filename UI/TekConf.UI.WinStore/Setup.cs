using System.Diagnostics;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Platform;
using Windows.UI.Xaml.Controls;
using TekConf.Core.Interfaces;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

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
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;

			var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
			var connection = factory.Create("tekconf.db");


			Mvx.RegisterSingleton<ISQLiteConnection>(connection);
			
			Mvx.RegisterType<IRemoteDataService, RemoteDataService>();
			Mvx.RegisterType<ILocalConferencesRepository, LocalConferencesRepository>();
			Mvx.RegisterType<IAnalytics, WinStoreAnalytics>();
			Mvx.RegisterSingleton(typeof(IAuthentication), new Authentication(connection));
			Mvx.RegisterType<ICacheService, CacheService>();
			Mvx.RegisterType<ILocalNotificationsRepository, LocalNotificationsRepository>();
			Mvx.RegisterType<IRestService, RestService>();
			Mvx.RegisterType<IPushSharpClient, PushSharpClient>();

			//IRemoteDataService remoteDataService,
			//															ILocalConferencesRepository localConferencesRepository,
			//															IAnalytics analytics,
			//															IAuthentication authentication,
			//															IMvxFileStore fileStore,
			//															IMvxMessenger messenger



			return new TekConf.Core.App();
		}
	}
}