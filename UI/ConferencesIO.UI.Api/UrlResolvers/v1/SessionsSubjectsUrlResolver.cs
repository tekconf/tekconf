namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class SessionsSubjectsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsSubjectsUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveUrl(string sessionSlug)
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/sessions/" + sessionSlug + "/subjects");
    }
  }
}