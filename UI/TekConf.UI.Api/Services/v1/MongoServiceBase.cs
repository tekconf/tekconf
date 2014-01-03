using System.Configuration;
using MongoDB.Driver;
using ServiceStack.ServiceInterface;
using TekConf.Common.Entities;

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
			//    if (_remoteServer.IsNull())
			//    {
			//        _remoteServer = MongoServer.Create("mongodb://tekconf:tekconf1234@alex.mongohq.com:10035/?safe=true");
			//    }

			//    if (_remoteDatabase.IsNull())
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
				if (_localServer.IsNull())
				{
					var mongoServer = ConfigurationManager.AppSettings["MongoServer"];
					_localServer = MongoServer.Create(mongoServer);
				}

				if (_localDatabase.IsNull())
				{
					_localDatabase = _localServer.GetDatabase("tekconf");

				}
				return _localDatabase;
			}
		}
	}
}