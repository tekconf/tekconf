namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class SessionUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug;
    }
  }
}