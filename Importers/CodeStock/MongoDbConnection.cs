using System.Configuration;
using MongoDB.Driver;

namespace CodeStock
{
    public class MongoDbConnection
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
            //        _remoteServer = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
            //    }

            //    if (_remoteDatabase == null)
            //    {
            //        _remoteDatabase = _remoteServer.GetDatabase("app4727263");
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
                    var mongoServer = ConfigurationManager.AppSettings["MongoServer"];
                    _localServer = MongoServer.Create(mongoServer);
                }

                if (_localDatabase == null)
                {
                    _localDatabase = _localServer.GetDatabase("conferences");

                }
                return _localDatabase;
            }
        }
    }
}
