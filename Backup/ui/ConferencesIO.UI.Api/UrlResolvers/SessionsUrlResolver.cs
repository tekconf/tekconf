namespace ConferencesIO.UI.Api
{
  public class SessionsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug;
    }
  }
}