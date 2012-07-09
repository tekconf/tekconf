using System.Linq;
using System.Net;
using AutoMapper;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.UI.Api.Services.Requests;
using FluentMongo.Linq;
using ServiceStack.Common.Web;

namespace ConferencesIO.UI.Api
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