using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.Services.v1;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using TinyMessenger;

namespace TekConf.UI.Api
{
    //A customizeable typed UserSession that can be extended with your own properties
    //To access ServiceStack's Session, Cache, etc from MVC Controllers inherit from ControllerBase<CustomUserSession>
    public class CustomUserSession : AuthUserSession
    {
        public string CustomProperty { get; set; }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("TekConf", typeof(ConferencesService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            //Change the default ServiceStack configuration
            //SetConfig(new EndpointHostConfig {
            //    DebugMode = true, //Show StackTraces in responses in development
            //});

            //Enable Authentication
            //ConfigureAuth(container);

            //Register all your dependencies
            //container.Register(new Repository());

            //Register In-Memory Cache provider. 
            //For Distributed Cache Providers Use: PooledRedisClientManager, BasicRedisClientManager or see: https://github.com/ServiceStack/ServiceStack/wiki/Caching
            container.Register<ICacheClient>(new MemoryCacheClient());
            var hub = new TinyMessengerHub();
            container.Register<ITinyMessengerHub>(hub);
            
            var subscriptions = new HubSubscriptions(hub);


            //container.Register<ICacheClient>(new AzureCacheClient());

            container.Register<ISessionFactory>(c =>
                      new SessionFactory(c.Resolve<ICacheClient>()));

        }

        /* Uncomment to enable ServiceStack Authentication and CustomUserSession
        private void ConfigureAuth(Funq.Container container)
        {
            var appSettings = new AppSettings();

            //Default route: /auth/{provider}
            Plugins.Add(new AuthFeature(this, () => new CustomUserSession(),
                new IAuthProvider[] {
                    new CredentialsAuthProvider(appSettings), 
                    new FacebookAuthProvider(appSettings), 
                    new TwitterAuthProvider(appSettings), 
                    new BasicAuthProvider(appSettings), 
                })); 

            //Default route: /register
            Plugins.Add(new RegistrationFeature()); 

            //Requires ConnectionString configured in Web.Config
            var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(connectionString, SqlServerOrmLiteDialectProvider.Instance));

            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
            authRepo.CreateMissingTables();
        }
        */

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}