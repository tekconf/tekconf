namespace ConferencesIO.UI.Web
{
  public class SessionsSubjectsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsSubjectsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/subjects";
    }
  }
}