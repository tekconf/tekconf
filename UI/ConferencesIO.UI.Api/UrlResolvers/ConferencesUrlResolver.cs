namespace ConferencesIO.UI.Api
{
  public class ConferencesUrlResolver : BaseUrlResolver
  {
    public string ResolveUrl(string conferenceSlug)
    {
      return RootUrl + "/api/conferences/" + conferenceSlug;
    }
  }
}