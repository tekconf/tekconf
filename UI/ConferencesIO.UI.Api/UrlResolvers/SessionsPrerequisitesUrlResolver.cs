namespace ConferencesIO.UI.Web
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
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/prerequisites";
    }
  }
}