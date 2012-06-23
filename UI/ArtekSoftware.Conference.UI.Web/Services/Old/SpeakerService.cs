using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
{
    public class SpeakerService : RestServiceBase<Speaker>
    {
      public override object OnGet(Speaker request)
      {
        var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
        var _database = _server.GetDatabase("app4727263");
        var conference = _database.GetCollection<Conference>("conferences")
                                .AsQueryable()
                                .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }
        var speakers = new List<Speaker>();

        if (request.sessionSlug == default(string))
        {
          if (conference.sessions != null)
          {
            speakers = conference.sessions.SelectMany(x => x.speakers).ToList();
          }
        }
        else
        {
          var session = conference.sessions.SingleOrDefault(s => s.slug == request.sessionSlug);
          if (session == null)
          {
            throw new HttpError(HttpStatusCode.NotFound, "Session not found.");
          }
          speakers = session.speakers;
        }

        return speakers;
      }

      // create
      public override object OnPost(Speaker request)
      {
        var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
        var _database = _server.GetDatabase("app4727263");

        var conference =
            _database.GetCollection<Conference>("conferences")
                .AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference != null)
        {
          var session = conference.sessions.SingleOrDefault(s => s.slug == request.sessionSlug);


          if (session != null)
          {
            if (session.speakers != null && session.speakers.Any(x => x.slug == request.slug))
            {
              // speaker already exists
              throw new HttpError(HttpStatusCode.Conflict, "Speaker Already Exists");
            }
            else
            {
              if (session.speakers == null)
              {
                session.speakers = new List<Speaker>();
              }
              session.speakers.Add(request);
              
              _database.GetCollection<Conference>("conferences").Save(conference);
              var result = new HttpResult() {StatusCode = HttpStatusCode.Created};
              return result;
            }
          }
          else
          {
            throw new HttpError(HttpStatusCode.NotFound, "Session not found.");
          }
        }
        else
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
          
        }
      }



    }


}