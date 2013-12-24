using System.Configuration;
using System.Web.Mvc;
using Common.Logging;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Mvc;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using TekConf.Common.Entities;
using TekConf.Common.Entities.Messages;
using TekConf.Common.Entities.Repositories;
using TekConf.RemoteData.v1;
using TekConf.UI.Api;
using TekConf.UI.Api.Services.v1;
using TekConf.Web.Controllers;
using TinyMessenger;

namespace TekConf.Web
{
	public class AppHost : AppHostBase
	{
		public AppHost()
			: base("TekConf", typeof(ConferencesController).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

			IEntityConfiguration entityConfiguration = new EntityConfiguration();

			container.Register<IEntityConfiguration>(entityConfiguration);
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			container.Register<IRemoteDataRepository>(c => new RemoteDataRepository(baseUrl));

			container.Register<IConferenceRepository>(new ConferenceRepository(entityConfiguration));
			container.Register<IAspNetUsersRepository>(new AspNetUsersRepository(entityConfiguration));
			container.Register<IScheduleRepository>(new ScheduleRepository(entityConfiguration));
			container.Register<IRepository<ScheduleEntity>>(new ScheduleRepository(entityConfiguration));
			container.Register<IRepository<UserEntity>>(new UserRepository(entityConfiguration));
			container.Register<IRepository<GeoLocationEntity>>(new GeoLocationRepository(entityConfiguration));
			container.Register<IRepository<PresentationEntity>>(new GenericRepository<PresentationEntity>(entityConfiguration));
			container.Register<IRepository<ConferenceEntity>>(new ConferenceRepository(entityConfiguration));

			container.Register<IRepository<SubscriptionEntity>>(new GenericRepository<SubscriptionEntity>(entityConfiguration));
			container.Register<IRepository<SessionRoomChangedMessage>>(new GenericRepository<SessionRoomChangedMessage>(entityConfiguration));
			container.Register<IRepository<ConferenceLocationChangedMessage>>(new GenericRepository<ConferenceLocationChangedMessage>(entityConfiguration));
			container.Register<IRepository<ConferenceEndDateChangedMessage>>(new GenericRepository<ConferenceEndDateChangedMessage>(entityConfiguration));
			container.Register<IRepository<ConferencePublishedMessage>>(new GenericRepository<ConferencePublishedMessage>(entityConfiguration));
			container.Register<IRepository<ConferenceUpdatedMessage>>(new GenericRepository<ConferenceUpdatedMessage>(entityConfiguration));
			container.Register<IRepository<ConferenceCreatedMessage>>(new GenericRepository<ConferenceCreatedMessage>(entityConfiguration));
			container.Register<IRepository<ConferenceStartDateChangedMessage>>(new GenericRepository<ConferenceStartDateChangedMessage>(entityConfiguration));
			container.Register<IRepository<SessionAddedMessage>>(new GenericRepository<SessionAddedMessage>(entityConfiguration));
			container.Register<IRepository<SessionRemovedMessage>>(new GenericRepository<SessionRemovedMessage>(entityConfiguration));
			container.Register<IRepository<SessionStartDateChangedMessage>>(new GenericRepository<SessionStartDateChangedMessage>(entityConfiguration));
			container.Register<IRepository<SessionEndDateChangedMessage>>(new GenericRepository<SessionEndDateChangedMessage>(entityConfiguration));
			container.Register<IRepository<SpeakerAddedMessage>>(new GenericRepository<SpeakerAddedMessage>(entityConfiguration));
			container.Register<IRepository<SpeakerRemovedMessage>>(new GenericRepository<SpeakerRemovedMessage>(entityConfiguration));
			container.Register<IRepository<ScheduleCreatedMessage>>(new GenericRepository<ScheduleCreatedMessage>(entityConfiguration));
			container.Register<IRepository<SessionAddedToScheduleMessage>>(new GenericRepository<SessionAddedToScheduleMessage>(entityConfiguration));
			container.Register<IEmailSender>(new EmailSender(container.Resolve<IEntityConfiguration>()));
			container.Register<ICacheClient>(new MemoryCacheClient());
			//container.Register<AuthenticationIdentityManager>(c => new AuthenticationIdentityManager(new IdentityStore())).ReusedWithin(ReuseScope.Request);
				
			var hub = new TinyMessengerHub();
			container.Register<ITinyMessengerHub>(hub);
			container.Register<IConferencesService>(new ConferencesService(container.Resolve<ITinyMessengerHub>(), 
																																			container.Resolve<IConferenceRepository>(), 
																																			container.Resolve<IRepository<GeoLocationEntity>>(),
																																			container.Resolve<IRepository<ScheduleEntity>>(), 
																																			container.Resolve<IEntityConfiguration>()));

			var subscriptions = new HubSubscriptions(hub,
				container.Resolve<IRepository<SessionRoomChangedMessage>>(),
				container.Resolve<IRepository<ConferenceLocationChangedMessage>>(),
				container.Resolve<IRepository<ConferenceEndDateChangedMessage>>(),
				container.Resolve<IRepository<ConferencePublishedMessage>>(),
				container.Resolve<IRepository<ConferenceUpdatedMessage>>(),
				container.Resolve<IRepository<ConferenceStartDateChangedMessage>>(),
				container.Resolve<IRepository<SessionAddedMessage>>(),
				container.Resolve<IRepository<SessionRemovedMessage>>(),
				container.Resolve<IRepository<SpeakerAddedMessage>>(),
				container.Resolve<IRepository<SpeakerRemovedMessage>>(),
				container.Resolve<IRepository<ConferenceCreatedMessage>>(),
				container.Resolve<IRepository<ScheduleCreatedMessage>>(),
				container.Resolve<IRepository<SessionAddedToScheduleMessage>>(),
				container.Resolve<IRepository<SessionStartDateChangedMessage>>(),
				container.Resolve<IRepository<SessionEndDateChangedMessage>>(),
				container.Resolve<IRepository<SubscriptionEntity>>(),
				container.Resolve<IRepository<UserEntity>>(),
				container.Resolve<IRepository<ScheduleEntity>>(),
				container.Resolve<IEmailSender>(),
				container.Resolve<IEntityConfiguration>()

				);

			container.Register<ISessionFactory>(c =>
				new SessionFactory(c.Resolve<ICacheClient>()));

			container.Register(LogManager.GetLogger(typeof(AppHost)));

			var bootstrapper = new Bootstrapper(container);
			bootstrapper.BootstrapAutomapper();
			bootstrapper.BootstrapMongoDb();

			ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
		}
	}
}