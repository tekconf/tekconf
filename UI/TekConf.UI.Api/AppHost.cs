using System.Configuration;
using MongoDB.Driver;
using ServiceStack.Configuration;
using TekConf.Common.Entities;
using TekConf.Common.Entities.Messages;
using TekConf.UI.Api.Services.v1;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using TinyMessenger;

namespace TekConf.UI.Api
{
	using ServiceStack.Api.Swagger;
	using ServiceStack.ServiceInterface.Cors;

	using TekConf.Common.Entities.Repositories;

	public class CustomUserSession : AuthUserSession
	{
		public string CustomProperty { get; set; }
	}

	public class AppHost : AppHostBase
	{
		public AppHost()
			: base("TekConf", typeof(ConferencesService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			//SetConfig(new EndpointHostConfig
			//{
			//	MetadataPageBodyHtml = "<script>window.location = 'swagger-ui/index.html';</script>",
			//});

			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
			Plugins.Add(new SwaggerFeature());
			//Enable Authentication
			ConfigureAuth(container);
			IEntityConfiguration entityConfiguration = new EntityConfiguration();

			container.Register<IEntityConfiguration>(entityConfiguration);

			container.Register<IConferenceRepository>(new ConferenceRepository(entityConfiguration));
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
			var hub = new TinyMessengerHub();
			container.Register<ITinyMessengerHub>(hub);

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
			var bootstrapper = new Bootstrapper();
			bootstrapper.BootstrapAutomapper(container);
			bootstrapper.BootstrapMongoDb(container);
		}

		//// Uncomment to enable ServiceStack Authentication and CustomUserSession
		private void ConfigureAuth(Funq.Container container)
		{
			var appSettings = new AppSettings();

			//Default route: /auth/{provider}
			Plugins.Add(new AuthFeature(() => new CustomUserSession(),
					new IAuthProvider[] {
										new CredentialsAuthProvider(), 
										//new FacebookAuthProvider(appSettings), 
										//new TwitterAuthProvider(appSettings), 
										new BasicAuthProvider(appSettings), 
								}));

			//Default route: /register
			Plugins.Add(new RegistrationFeature());

			//Requires ConnectionString configured in Web.Config
			//var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
			//container.Register<IDbConnectionFactory>(c =>
			//		new OrmLiteConnectionFactory(connectionString, SqlServerOrmLiteDialectProvider.Instance));

			var serverSettings = new MongoServerSettings()
				{
					Server = new MongoServerAddress(ConfigurationManager.AppSettings["mongoServerAddress"]), //TODO

					//Server = new MongoServerAddress("tekconfdb.cloudapp.net"), //TODO
				};
			var server = new MongoServer(serverSettings);
			var databaseSettings = new MongoDatabaseSettings(server, "tekconf");
			var mongoDatabase = new MongoDatabase(server, databaseSettings);
			container.Register<IUserAuthRepository>(c =>
					new MongoDBAuthRepository(mongoDatabase, createMissingCollections: true));

			var authRepo = (MongoDBAuthRepository)container.Resolve<IUserAuthRepository>();
			authRepo.CreateMissingCollections();
		}


		public static void Start()
		{
			new AppHost().Init();
		}
	}
}