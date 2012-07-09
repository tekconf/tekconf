namespace ConferencesIO.UI.Api
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
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug;
    }
  }
}