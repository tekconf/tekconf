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
  public class OldConferenceService : MongoRestServiceBase<Conference>
  {
    public override object OnGet(Conference request)
    {
      if (request.slug == default(string))
      {
        var conferences = this.Database.GetCollection<Conference>("conferences").AsQueryable().ToList();
        var dtos = new List<ConferencesDto>() { new ConferencesDto() { }, new ConferencesDto() { } };
        return dtos;
      }
      else
      {
        var conference = this.Database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.slug);
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
      var isExisting =
          this.Database.GetCollection<Conference>("conferences").AsQueryable().Any(c => c.slug == conference.slug);

      if (isExisting)
      {
        throw new HttpError(HttpStatusCode.Conflict, "Conference Already Exists");
      }

      this.Database.GetCollection<Conference>("conferences").Insert(conference);

      var result = new HttpResult() { StatusCode = HttpStatusCode.Created };
      return result;

    }

    //update
    public override object OnPut(Conference conference)
    {
      var result = this.Database.GetCollection<Conference>("conferences").Save(conference);
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
