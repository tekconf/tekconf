namespace ConferencesIO.UI.Api.UrlResolvers.v1
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
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/speakers/" + _speakerSlug;
    }
  }
}