
namespace TekConf.UI.Api.UrlResolvers.v1
{
  public class ConferenceUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public ConferenceUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl()
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug);
    }
  }


}