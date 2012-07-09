using System;

namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class SessionPrerequisitesUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionPrerequisitesUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl()
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/prerequisites");
    }
  }
}