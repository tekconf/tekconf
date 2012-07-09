namespace ConferencesIO.UI.Api
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
      return RootUrl + "/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers/" + _speakerUrl;
    }
  }

}