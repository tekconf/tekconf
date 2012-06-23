using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
{
  public class OldConferenceService : RestServiceBase<Conference>
  {
    public override object OnGet(Conference request)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");

      if (request.slug == default(string))
      {
        var conferences = _database.GetCollection<Conference>("conferences").AsQueryable().ToList();
        var dtos = new List<ConferencesDto>() { new ConferencesDto() { }, new ConferencesDto() { } };
        return dtos;
      }
      else
      {
        var conference = _database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.slug);
        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }
        return conference;
      }
    }

    // create
    public override object OnPost(Conference conference)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");

      var isExisting =
          _database.GetCollection<Conference>("conferences").AsQueryable().Any(c => c.slug == conference.slug);

      if (isExisting)
      {
        throw new HttpError(HttpStatusCode.Conflict, "Conference Already Exists");
      }

      _database.GetCollection<Conference>("conferences").Insert(conference);

      var result = new HttpResult() { StatusCode = HttpStatusCode.Created };
      return result;

    }

    //update
    public override object OnPut(Conference conference)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");

      var result = _database.GetCollection<Conference>("conferences").Save(conference);
      if (result.UpdatedExisting)
      {
        return new HttpResult() { StatusCode = HttpStatusCode.OK };
      }
      else
      {
        return new HttpResult() { StatusCode = HttpStatusCode.Created };
      }

    }

    public override object OnDelete(Conference conference)
    {
      throw new NotImplementedException();

      return null;
    }
  }

}
