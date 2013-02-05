using System;

namespace TekConf.UI.Api.UrlResolvers.v1
{
  public class SessionSpeakersUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionSpeakersUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl()
    {
      var rootUri = new Uri(RootUrl);
      var uri = new Uri(rootUri, "/v1/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers");
      return uri.ToString();
    }
  }
}