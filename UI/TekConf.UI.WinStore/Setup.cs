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
	using System;

	public class Setup : MvxStoreSetup
	{
		public Setup(Frame rootFrame)
			: base(rootFrame)
		{
		}
		protected override IMvxTrace CreateDebugTrace() { return new MyDebugTrace(); }
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

	public class MyDebugTrace : IMvxTrace
	{
		public void Trace(MvxTraceLevel level, string tag, Func<string> message)
		{
			Debug.WriteLine(tag + ":" + level + ":" + message());
		}

		public void Trace(MvxTraceLevel level, string tag, string message)
		{
			Debug.WriteLine(tag + ":" + level + ":" + message);
		}

		public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
		{
			try
			{
				Debug.WriteLine(string.Format(tag + ":" + level + ":" + message, args));
			}
			catch (FormatException)
			{
				Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
			}
		}
	}
}