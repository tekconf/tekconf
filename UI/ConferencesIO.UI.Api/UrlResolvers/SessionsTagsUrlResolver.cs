namespace ConferencesIO.UI.Api
{
  public class SessionsTagsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsTagsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/tags";
    }
  }
}