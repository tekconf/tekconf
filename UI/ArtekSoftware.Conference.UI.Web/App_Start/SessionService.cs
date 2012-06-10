using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionService : RestServiceBase<Session>
  {
    public override object OnGet(Session request)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");
      var conference = _database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);

      if (conference == null)
      {
        throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
      }

      if (request.slug == default(string))
      {
        var sessions = conference.sessions.ToList();
        return sessions;
      }
      else
      {
        var session = conference.sessions.SingleOrDefault(s => s.slug == request.slug);
        if (session == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }
        return session;
      }
    }

    // create
    public override object OnPost(Session session)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");

      var conference =
          _database.GetCollection<Conference>("conferences")
              .AsQueryable()
              .SingleOrDefault(c => c.slug == session.conferenceSlug);

      if (conference != null)
      {
        if (conference.sessions != null && conference.sessions.Any(x => x.slug == session.slug))
        {
          // session already exists
          throw new HttpError(HttpStatusCode.Conflict, "Session Already Exists");
        }
        else
        {
          if (conference.sessions == null)
          {
            conference.sessions = new List<Session>();
          }
          conference.sessions.Add(session);
          _database.GetCollection<Conference>("conferences").Save(conference);
          var result = new HttpResult() { StatusCode = HttpStatusCode.Created };
          return result;
        }
      }
      else
      {
        throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
      }

    }

    //update
    public override object OnPut(Session session)
    {
      //var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      //var _database = _server.GetDatabase("app4727263");

      //var result = _database.GetCollection<Conference>("conferences").Save(session);
      //if (result.UpdatedExisting)
      //{
      //    return new HttpResult() { StatusCode = HttpStatusCode.OK };
      //}
      //else
      //{
      //    return new HttpResult() { StatusCode = HttpStatusCode.Created };
      //}
      return new HttpResult() { StatusCode = HttpStatusCode.MethodNotAllowed };

    }

    public override object OnDelete(Session session)
    {
      throw new NotImplementedException();

      return null;
    }
  }
}