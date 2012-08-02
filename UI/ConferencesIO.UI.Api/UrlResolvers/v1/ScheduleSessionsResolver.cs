using System.Collections.Generic;

namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class ScheduleSessionsResolver : BaseUrlResolver
  {
    public List<string> ResolveCore(ScheduleEntity source)
    {
      var sessionUrls = new List<string>();
      foreach (var session in source.SessionUrls)
      {
        sessionUrls.Add(CombineUrl("/" + session));
      }
      return sessionUrls;
    }
  }
}