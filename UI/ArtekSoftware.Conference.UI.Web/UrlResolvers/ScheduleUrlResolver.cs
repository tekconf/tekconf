namespace ArtekSoftware.Conference.UI.Web
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
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/schedule/" + _userSlug;
    }
  }
}