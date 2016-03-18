using System;
using System.Diagnostics;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using Microsoft.WindowsAzure.MobileServices;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Messages;

namespace TekConf.UI.Android
{
	public class Setup : MvxAndroidSetup
	{
		public static Context CurrentActivityContext { get; set; }
		public static MobileServiceClient MobileService = new MobileServiceClient(
			"https://tekconfauth.azure-mobile.net/",
			"NeMPYjchPdsFKlUqDdyAJYZtdrOPiJ11"
			);

		private static string _userName;

		public static string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				var messenger = Mvx.Resolve<IMvxMessenger>();
				if (_userName != value && messenger != null)
				{
					var obj = new object();
					messenger.Publish(new UserNameChangedMessage(obj) { UserName = value });
				}
				_userName = value;
			}
		}

		public Setup(Context applicationContext)
			: base(applicationContext)
		{
			CurrentActivityContext = applicationContext;
		}

		protected override IMvxTrace CreateDebugTrace() { return new MyDebugTrace(); }

		protected override IMvxApplication CreateApp()
		{
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;

			var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
			var connection = factory.Create("tekconf.db");

			Mvx.RegisterSingleton(typeof(IAuthentication), new Authentication(connection));
			Mvx.RegisterSingleton<ISQLiteConnection>(connection);
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;
			Mvx.RegisterType<IAnalytics, DroidAnalytics>();
			Mvx.RegisterType<ICacheService, CacheService>();
			Mvx.RegisterType<ILocalNotificationsRepository, LocalNotificationsRepository>();
			Mvx.RegisterType<ILocalConferencesRepository, LocalConferencesRepository>();
			Mvx.RegisterType<IRestService, RestService>();
			Mvx.RegisterType<IPushSharpClient, PushSharpClient>();

			Mvx.RegisterType<INetworkConnection, DroidNetworkConnection>();
			Mvx.RegisterType<IMessageBox, DroidMessageBox>();

			return new TekConf.Core.App();
		}
	}
}