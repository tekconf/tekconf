namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class SessionsPrerequisitesUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsPrerequisitesUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/prerequisites";
    }
  }
}