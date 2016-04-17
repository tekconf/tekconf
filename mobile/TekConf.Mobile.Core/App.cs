using AutoMapper;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Tekconf.DTO;
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

			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ConferenceModel, ConferenceListViewModel>();

				cfg.CreateMap<Conference, ConferenceModel>();
				cfg.CreateMap<Session, SessionModel>();
			}
			                                   
			                                    );
			_mapper = config.CreateMapper();

			Mvx.RegisterSingleton<IMapper>(_mapper);
        }
    }
}
