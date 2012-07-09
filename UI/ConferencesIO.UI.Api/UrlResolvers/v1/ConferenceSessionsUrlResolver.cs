namespace ConferencesIO.UI.Api.UrlResolvers.v1
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
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/sessions";
    }
  }
}