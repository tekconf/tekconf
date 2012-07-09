namespace ConferencesIO.UI.Api
{
  public class ConferencesSpeakersResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _speakerSlug;

    public ConferencesSpeakersResolver(string conferenceSlug, string speakerSlug)
    {
      _conferenceSlug = conferenceSlug;
      _speakerSlug = speakerSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/conferences/" + _conferenceSlug + "/speakers/" + _speakerSlug;
    }
  }
}