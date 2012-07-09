namespace ConferencesIO.UI.Api
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
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/subjects";
    }
  }
}