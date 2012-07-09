namespace ConferencesIO.UI.Api.UrlResolvers.v1
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
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/speakers";
    }
  }
}