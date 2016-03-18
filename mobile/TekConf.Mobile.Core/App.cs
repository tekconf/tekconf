using MvvmCross.Platform.IoC;

namespace TekConf.Mobile.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.ConferencesViewModel>();
        }
    }
}
