namespace ConferencesIO.UI.Api
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
      return RootUrl + "/conferences/" + _conferenceSlug;
    }
  }


}