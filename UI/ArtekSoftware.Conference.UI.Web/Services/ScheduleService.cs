using System.Linq;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.UI.Web.Services.Requests;
using AutoMapper;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;

namespace ConferencesIO.UI.Web
{
  public class ScheduleService : MongoRestServiceBase<ScheduleRequest>
  {
    public override object OnGet(ScheduleRequest request)
    {
      if (request.conferenceSlug == default(string) || request.userSlug == default(string))
      {
        return new HttpError() {StatusCode = HttpStatusCode.BadRequest};
      }

      var schedule = this.Database.GetCollection<ScheduleEntity>("schedules")
        .AsQueryable()
        .Where(s => s.ConferenceSlug == request.conferenceSlug)
        .SingleOrDefault(s => s.UserSlug == request.userSlug);

      var scheduleDto = Mapper.Map<ScheduleEntity, ScheduleDto>(schedule);
      var resolver = new ScheduleUrlResolver(request.conferenceSlug, request.userSlug);
      scheduleDto.url = resolver.ResolveUrl();
      return scheduleDto;
    }
  }
}