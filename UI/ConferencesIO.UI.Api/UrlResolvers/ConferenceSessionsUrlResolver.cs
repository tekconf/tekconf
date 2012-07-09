namespace ConferencesIO.UI.Api
{
  public class ConferenceSessionsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public ConferenceSessionsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions";
    }
  }
}