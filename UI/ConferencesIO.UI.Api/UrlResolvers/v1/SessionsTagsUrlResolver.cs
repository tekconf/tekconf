namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class SessionsTagsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsTagsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/tags");
    }
  }
}