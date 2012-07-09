using MongoDB.Driver;
using ServiceStack.ServiceInterface;

namespace ConferencesIO.UI.Api.Services
{
  public class MongoRestServiceBase<T> : RestServiceBase<T>
  {
    private MongoServer _server;
    private MongoDatabase _database;

    public MongoDatabase Database
    {
      get
      {
        if (_server == null)
        {
          _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
        }

        if (_database == null)
        {
          _database = _server.GetDatabase("app4727263");
        }
        return _database;
      }
    }
  }
}