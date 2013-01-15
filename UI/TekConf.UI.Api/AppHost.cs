using MongoDB.Driver;
using ServiceStack.Configuration;
using TekConf.UI.Api.Services.v1;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using TinyMessenger;

namespace TekConf.UI.Api
{
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


			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;


			//Enable Authentication
			ConfigureAuth(container);
			IConfiguration configuration = new Configuration();

			container.Register<IConfiguration>(configuration);
			container.Register<IRepository<ConferenceEntity>>(new ConferenceRepository(configuration));

			container.Register<IRepository<SessionRoomChangedMessage>>(new SessionRoomChangedRepository(configuration));
			container.Register<IRepository<ConferenceLocationChangedMessage>>(new ConferenceLocationChangedRepository(configuration));
			container.Register<IRepository<ConferenceEndDateChangedMessage>>(new ConferenceEndDateChangedRepository(configuration));
			container.Register<IRepository<ConferencePublishedMessage>>(new ConferencePublishedRepository(configuration));
			container.Register<IRepository<ConferenceUpdatedMessage>>(new ConferenceUpdatedRepository(configuration));
			container.Register<IRepository<ConferenceCreatedMessage>>(new ConferenceCreatedRepository(configuration));
			container.Register<IRepository<ConferenceStartDateChangedMessage>>(new ConferenceStartDateChangedRepository(configuration));
			container.Register<IRepository<SessionAddedMessage>>(new SessionAddedRepository(configuration));
			container.Register<IRepository<SessionRemovedMessage>>(new SessionRemovedRepository(configuration));
			container.Register<IRepository<SpeakerAddedMessage>>(new SpeakerAddedRepository(configuration));
			container.Register<IRepository<SpeakerRemovedMessage>>(new SpeakerRemovedRepository(configuration));
		
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
								container.Resolve<IRepository<ConferenceCreatedMessage>>()

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
					Server = new MongoServerAddress("tekconfdb.cloudapp.net"), //TODO
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