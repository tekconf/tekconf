using System.Configuration;
using MongoDB.Driver;

namespace UberImporter
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
            //        _remoteServer = MongoServer.Create("mongodb://tekconf:tekconf1234@alex.mongohq.com:10035/?safe=true");
            //    }

            //    if (_remoteDatabase == null)
            //    {
            //        _remoteDatabase = _remoteServer.GetDatabase("tekconf");
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
                    _localDatabase = _localServer.GetDatabase("tekconf");

                }
                return _localDatabase;
            }
        }
    }
}
