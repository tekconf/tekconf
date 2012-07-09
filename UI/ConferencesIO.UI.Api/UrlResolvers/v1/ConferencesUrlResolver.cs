namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class ConferencesUrlResolver : BaseUrlResolver
  {
    public string ResolveUrl(string conferenceSlug)
    {
      return RootUrl + "/v1/conferences/" + conferenceSlug;
    }
  }
}