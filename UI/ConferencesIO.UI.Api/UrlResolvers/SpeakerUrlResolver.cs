namespace ConferencesIO.UI.Web
{
  public class SpeakerUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;
    private readonly string _speakerUrl;

    public SpeakerUrlResolver(string conferenceSlug, string sessionSlug, string speakerUrl)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
      _speakerUrl = speakerUrl;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers/" + _speakerUrl;
    }
  }

}