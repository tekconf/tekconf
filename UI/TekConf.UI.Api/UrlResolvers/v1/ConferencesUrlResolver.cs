
namespace TekConf.UI.Api.UrlResolvers.v1
{
  public class ConferencesUrlResolver : BaseUrlResolver
  {
    public string ResolveUrl(string conferenceSlug)
    {
      return CombineUrl("/v1/conferences/" + conferenceSlug);
    }
  }
}