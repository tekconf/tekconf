namespace ConferencesIO.UI.Api
{
  public class SessionsLinksUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsLinksUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/links";
    }
  }
}