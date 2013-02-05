using System.Configuration;
using MongoDB.Driver;
using ServiceStack.ServiceInterface;

namespace TekConf.UI.Api.Services
{
	public class MongoServiceBase : Service
	{
		//private MongoServer _remoteServer;
		//private MongoDatabase _remoteDatabase;
		private MongoServer _localServer;
		private MongoDatabase _localDatabase;

		public MongoDatabase RemoteDatabase
		{
			//get
			//{
			//    if (_remoteServer == null)
			//    {
			//        _remoteServer = MongoServer.Create("mongodb://tekconf:tekconf1234@alex.mongohq.com:10035/?safe=true");
			//    }

			//    if (_remoteDatabase == null)
			//    {
			//        _remoteDatabase = _remoteServer.GetDatabase("conferences");
			//    }
			//    return _remoteDatabase;
			//}
			get { return this.LocalDatabase; }
		}

		public MongoDatabase LocalDatabase
		{
			get
			{
				if (_localServer == null)
				{
					var mongoServer = ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString;
					_localServer = MongoServer.Create(mongoServer);
				}

				if (_localDatabase == null)
				{
					_localDatabase = _localServer.GetDatabase("tekconf");

				}
				return _localDatabase;
			}
		}
	}
}