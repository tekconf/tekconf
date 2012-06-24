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
  public class ConferencesService : MongoRestServiceBase<ConferencesRequest>
  {
    public override object OnGet(ConferencesRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        List<ConferenceEntity> conferences = null;

        try
        {
          conferences = this.Database.GetCollection<ConferenceEntity>("conferences")
            .AsQueryable()
            .ToList();
        }
        catch (Exception ex)
        {
          var fd = ex.Message;
          throw;
        }


        var dtos = Mapper.Map<List<ConferenceEntity>, List<ConferencesDto>>(conferences);
        return dtos.ToList();
      }
      else
      {
        var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
                          .AsQueryable()
                          .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }

        var dto = Mapper.Map<ConferenceEntity, ConferenceDto>(conference);

        return dto;
      }
    }
  }
}