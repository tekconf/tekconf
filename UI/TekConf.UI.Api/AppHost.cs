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
				SetConfig(new EndpointHostConfig
				{
						MetadataPageBodyHtml = "<script>window.location = 'swagger-ui/index.html';</script>",
				});

			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
			Plugins.Add(new SwaggerFeature());
			//Enable Authentication
			ConfigureAuth(container);
			IConfiguration configuration = new Configuration();

			container.Register<IConfiguration>(configuration);
			container.Register<IRepository<ConferenceEntity>>(new ConferenceRepository(configuration));
			container.Register<IRepository<ScheduleEntity>>(new ScheduleRepository(configuration));
			container.Register<IRepository<UserEntity>>(new UserRepository(configuration));
			container.Register<IRepository<GeoLocationEntity>>(new GeoLocationRepository(configuration));

			container.Register<IRepository<SessionRoomChangedMessage>>(new GenericRepository<SessionRoomChangedMessage>(configuration));
			container.Register<IRepository<ConferenceLocationChangedMessage>>(new GenericRepository<ConferenceLocationChangedMessage>(configuration));
			container.Register<IRepository<ConferenceEndDateChangedMessage>>(new GenericRepository<ConferenceEndDateChangedMessage>(configuration));
			container.Register<IRepository<ConferencePublishedMessage>>(new GenericRepository<ConferencePublishedMessage>(configuration));
			container.Register<IRepository<ConferenceUpdatedMessage>>(new GenericRepository<ConferenceUpdatedMessage>(configuration));
			container.Register<IRepository<ConferenceCreatedMessage>>(new GenericRepository<ConferenceCreatedMessage>(configuration));
			container.Register<IRepository<ConferenceStartDateChangedMessage>>(new GenericRepository<ConferenceStartDateChangedMessage>(configuration));
			container.Register<IRepository<SessionAddedMessage>>(new GenericRepository<SessionAddedMessage>(configuration));
			container.Register<IRepository<SessionRemovedMessage>>(new GenericRepository<SessionRemovedMessage>(configuration));
			container.Register<IRepository<SpeakerAddedMessage>>(new GenericRepository<SpeakerAddedMessage>(configuration));
			container.Register<IRepository<SpeakerRemovedMessage>>(new GenericRepository<SpeakerRemovedMessage>(configuration));
			container.Register<IRepository<ScheduleCreatedMessage>>(new GenericRepository<ScheduleCreatedMessage>(configuration));
			container.Register<IRepository<SessionAddedToScheduleMessage>>(new GenericRepository<SessionAddedToScheduleMessage>(configuration));


			container.Register<IEmailSender>(new EmailSender(container.Resolve<IConfiguration>()));
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
								container.Resolve<IEmailSender>(),
								container.Resolve<IConfiguration>()

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