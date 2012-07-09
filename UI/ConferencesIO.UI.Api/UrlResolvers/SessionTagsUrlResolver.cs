namespace ConferencesIO.UI.Api
{
  public class SessionTagsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionTagsUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/tags";
    }
  }
}