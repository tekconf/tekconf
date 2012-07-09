namespace ConferencesIO.UI.Api
{
  public class ConferenceSpeakersUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public ConferenceSpeakersUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/speakers";
    }
  }
}