using MvvmCross.Platform.IoC;
using TekConf.Mobile.Core.ViewModels;

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

            RegisterAppStart<ConferencesViewModel>();
        }
    }
}
