
namespace TekConf.UI.Api.UrlResolvers.v1
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
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/speakers");
    }
  }
}