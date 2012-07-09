namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class ScheduleUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _userSlug;

    public ScheduleUrlResolver(string conferenceSlug, string userSlug)
    {
      _conferenceSlug = conferenceSlug;
      _userSlug = userSlug;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/v1/conferences/" + _conferenceSlug + "/schedule/" + _userSlug;
    }
  }
}