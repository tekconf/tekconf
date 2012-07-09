namespace ConferencesIO.UI.Web
{
  public class ConferencesUrlResolver : BaseUrlResolver
  {
    public string ResolveUrl(string conferenceSlug)
    {
      return RootUrl + "/api/conferences/" + conferenceSlug;
    }
  }
}