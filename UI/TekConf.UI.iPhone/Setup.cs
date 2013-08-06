using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Plugins.Sqlite;
using MonoTouch.UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using TekConf.Core.Interfaces;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace TekConf.UI.iPhone
{
	public class iPhoneNetworkConnection : INetworkConnection
	{
		public bool IsNetworkConnected()
		{
			return true; //TODO
		}

		public string NetworkDownMessage
		{
			get
			{
				return "";
			}
		}
	}

	public class iPhoneMessageBox : IMessageBox
	{
		public void Show(string message)
		{
			var uiAlertView = new UIAlertView();
			uiAlertView.Message = message;
			uiAlertView.Show();
		}
	}

	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
			var connection = factory.Create("tekconf.db");

			Mvx.RegisterSingleton(typeof(IAuthentication), new Authentication(connection));
			Mvx.RegisterSingleton<ISQLiteConnection>(connection);
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;
			Mvx.RegisterType<IAnalytics, iPhoneAnalytics>();
			Mvx.RegisterType<ICacheService, CacheService>();
			Mvx.RegisterType<ILocalNotificationsRepository, LocalNotificationsRepository>();
			Mvx.RegisterType<ILocalConferencesRepository, LocalConferencesRepository>();
			Mvx.RegisterType<IRestService, RestService>();
			Mvx.RegisterType<IPushSharpClient, PushSharpClient>();
			Mvx.RegisterType<INetworkConnection, iPhoneNetworkConnection>();
			Mvx.RegisterType<IMessageBox, iPhoneMessageBox>();
			return new Core.App();
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}
	}

	public class Authentication : IAuthentication
	{
		public Authentication(ISQLiteConnection connection)
		{
			
		}
		public bool IsAuthenticated { get; private set; }
		public string OAuthProvider { get; private set; }
		public string UserName { get; set; }
	}

	public class iPhoneAnalytics : IAnalytics
	{
		public void SendView(string view)
		{
			//TODO
		}
	}
	public class PushSharpClient : IPushSharpClient
	{
		public void Unregister()
		{
			//TODO
		}

		public void Register()
		{
			//TODO
		}
	}
}