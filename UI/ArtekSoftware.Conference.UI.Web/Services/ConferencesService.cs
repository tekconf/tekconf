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
  public class ConferencesService : MongoRestServiceBase<ConferencesRequest>
  {
    public override object OnGet(ConferencesRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        var conferences = this.Database.GetCollection<Conference>("conferences")
                            .AsQueryable()
                            .ToList();

        var dtos = Mapper.Map<List<Conference>, List<ConferencesDto>>(conferences);
        return dtos.ToList();
      }
      else
      {
        var conference = this.Database.GetCollection<Conference>("conferences")
                          .AsQueryable()
                          .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }

        var dto = Mapper.Map<Conference, ConferenceDto>(conference);

        return dto;
      }
    }
  }
}