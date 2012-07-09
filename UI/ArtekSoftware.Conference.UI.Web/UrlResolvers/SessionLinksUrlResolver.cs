namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionLinksUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionLinksUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/links";
    }
  }
}