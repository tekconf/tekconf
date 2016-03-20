using AutoMapper;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
		IMapper _mapper;
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ConferencesViewModel>();

			var config = new MapperConfiguration(cfg => cfg.CreateMap<ConferenceModel, ConferenceListViewModel>());
			_mapper = config.CreateMapper();

			Mvx.RegisterSingleton<IMapper>(_mapper);
        }
    }
}
