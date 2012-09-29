using System.Configuration;
using MongoDB.Driver;
using ServiceStack.ServiceInterface;

namespace TekConf.UI.Api.Services
{
    public class MongoRestServiceBase<T> : RestServiceBase<T>
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
            //        _remoteServer = MongoServer.Create("mongodb://admin:mongo12@flame.mongohq.com:27100/app4727263?safe=true");
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
                    _localDatabase = _localServer.GetDatabase("conferences");

                }
                return _localDatabase;
            }
        }


    }
}