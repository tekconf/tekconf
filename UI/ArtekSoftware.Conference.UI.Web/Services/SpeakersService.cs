using System;
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
  public class SpeakersService : MongoRestServiceBase<SpeakersRequest>
  {
    public override object OnGet(SpeakersRequest request)
    {
      if (request.conferenceSlug == default(string))
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

      if (conference.sessions == null)
      {
        return new HttpError() { StatusCode = HttpStatusCode.NotFound };
      }
      var sessions = conference.sessions;

      var speakers = conference.sessions.SelectMany(s => s.speakers).ToList();

      if (request.speakerSlug == default(string))
      {
        List<SpeakersDto> speakersDtos = Mapper.Map<List<Speaker>, List<SpeakersDto>>(speakers);

        return speakersDtos.ToList();
      }
      else
      {
        var speaker = speakers.FirstOrDefault(s => s.slug == request.speakerSlug);

        var dto = Mapper.Map<Speaker, SpeakerDto>(speaker);

        return dto;
      }


    }
  }
}