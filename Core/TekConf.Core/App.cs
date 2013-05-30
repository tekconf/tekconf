using Cirrious.CrossCore.IoC;

namespace TekConf.Core
{
	public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
	{
		public static string ApiRootUri = "http://api.tekconf.com/v1/";
		public static string WebRootUri = "http://www.tekconf.com/";

		//public static string ApiRootUri = "http://localhost:25825/v1/";
		//public static string WebRootUri = "http://localhost:2118/";

		public override void Initialize()
		{
			CreatableTypes()
					.EndingWith("Service")
					.AsInterfaces()
					.RegisterAsLazySingleton();

			RegisterAppStart<ViewModels.ConferencesListViewModel>();
		}
	}
}