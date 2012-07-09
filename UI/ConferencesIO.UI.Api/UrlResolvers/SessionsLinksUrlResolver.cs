namespace ConferencesIO.UI.Web
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
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/links";
    }
  }
}