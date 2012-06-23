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
  public class ConferencesService : RestServiceBase<ConferencesRequest>
  {
    public override object OnGet(ConferencesRequest request)
    {
      var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var _database = _server.GetDatabase("app4727263");

      if (request.conferenceSlug == default(string))
      {
        var conferences = _database.GetCollection<Conference>("conferences").AsQueryable().ToList();
        var dtos = Mapper.Map<List<Conference>, List<ConferencesDto>>(conferences);
        //var dtos = new List<ConferencesDto>() { new ConferencesDto() { }, new ConferencesDto() { } };
        return dtos.ToList();
      }
      else
      {
        var conference = _database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);
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