//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using MongoDB.Driver;
//using MongoDB.Driver.Linq;
//using ServiceStack.Common.Web;

//namespace ArtekSoftware.Conference.UI.Web
//{
//  public class SessionService : MongoRestServiceBase<SessionEntities>
//  {
//    public override object OnGet(SessionEntities request)
//    {
//      var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
//                        .AsQueryable()
//                        .SingleOrDefault(c => c.slug == request.conferenceSlug);

//      if (conference == null)
//      {
//        throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
//      }

//      if (request.slug == default(string))
//      {
//        var sessions = conference.sessions.ToList();
//        return sessions;
//      }
//      else
//      {
//        var session = conference.sessions.SingleOrDefault(s => s.slug == request.slug);
//        if (session == null)
//        {
//          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
//        }
//        return session;
//      }
//    }

//    // create
//    public override object OnPost(SessionEntities sessionEntities)
//    {
//      var conference =
//          this.Database.GetCollection<ConferenceEntity>("conferences")
//              .AsQueryable()
//              .SingleOrDefault(c => c.slug == sessionEntities.conferenceSlug);

//      if (conference != null)
//      {
//        if (conference.sessions != null && conference.sessions.Any(x => x.slug == sessionEntities.slug))
//        {
//          // session already exists
//          throw new HttpError(HttpStatusCode.Conflict, "Session Already Exists");
//        }
//        else
//        {
//          if (conference.sessions == null)
//          {
//            conference.sessions = new List<SessionEntities>();
//          }
//          conference.sessions.Add(sessionEntities);
//          this.Database.GetCollection<ConferenceEntity>("conferences").Save(conference);
//          var result = new HttpResult() { StatusCode = HttpStatusCode.Created };
//          return result;
//        }
//      }
//      else
//      {
//        throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
//      }

//    }

//    //update
//    public override object OnPut(SessionEntities sessionEntities)
//    {
//      //var result = this.Database.GetCollection<ConferenceEntity>("conferences").Save(SessionEntities);
//      //if (result.UpdatedExisting)
//      //{
//      //    return new HttpResult() { StatusCode = HttpStatusCode.OK };
//      //}
//      //else
//      //{
//      //    return new HttpResult() { StatusCode = HttpStatusCode.Created };
//      //}
//      return new HttpResult() { StatusCode = HttpStatusCode.MethodNotAllowed };

//    }

//    public override object OnDelete(SessionEntities sessionEntities)
//    {
//      throw new NotImplementedException();

//      return null;
//    }
//  }
//}