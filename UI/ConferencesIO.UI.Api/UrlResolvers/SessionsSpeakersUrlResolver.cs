namespace ConferencesIO.UI.Api
{
  public class SessionsSpeakersUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsSpeakersUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/speakers";
    }
  }
}