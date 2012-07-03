using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ArtekSoftware.Conference.UI.Web.Services.Requests;
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

      var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
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
      var speakersList = new List<SpeakerEntity>();

      //TODO : Linq this
      foreach(var session in conference.sessions)
      {
        if (session.speakers != null)
        {
          foreach (var speakerEntity in session.speakers)
          {
            if (!speakersList.Any(s => s.slug == speakerEntity.slug))
            {
              speakersList.Add(speakerEntity);
            }
          }
        }
      }
      var speakers = speakersList.ToList();
     // var speakers = conference.sessions.SelectMany(s => s.speakers).ToList();

      if (request.speakerSlug == default(string))
      {

        List<SpeakersDto> speakersDtos = Mapper.Map<List<SpeakerEntity>, List<SpeakersDto>>(speakers);

        return speakersDtos.ToList();
      }
      else
      {
        var speaker = speakers.FirstOrDefault(s => s.slug == request.speakerSlug);


        var dto = Mapper.Map<SpeakerEntity, SpeakerDto>(speaker);

        return dto;
      }


    }
    }
}