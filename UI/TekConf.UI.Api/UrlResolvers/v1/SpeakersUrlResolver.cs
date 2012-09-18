namespace TekConf.UI.Api.UrlResolvers.v1
{
  public class SpeakersUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SpeakersUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveUrl(string speakerSlug)
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers/" + speakerSlug);
    }
  }
}