using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionSpeakersService : MongoRestServiceBase<SessionSpeakersRequest>
  {
    public override object OnGet(SessionSpeakersRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      var conference = this.Database.GetCollection<Conference>("conferences")
        .AsQueryable()
        .SingleOrDefault(c => c.slug == request.conferenceSlug);

      if (conference == null)
      {
        throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
      }

      var session = conference.sessions.SingleOrDefault(s => s.slug == request.sessionSlug);

      if (session == null)
      {
        throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
      }

      if (request.speakerSlug == default(string))
      {
        var speakers = Mapper.Map<List<Speaker>, List<SpeakersDto>>(session.speakers);
        return speakers.ToList();
      }
      else
      {
        var speaker = session.speakers.FirstOrDefault(s => s.slug == request.speakerSlug);

        var dto = Mapper.Map<Speaker, SpeakerDto>(speaker);

        return dto;
      }


    }
  }
}