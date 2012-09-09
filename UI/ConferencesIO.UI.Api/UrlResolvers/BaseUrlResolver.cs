using System;
using System.Web;

namespace ConferencesIO.UI.Api
{
  public class BaseUrlResolver
  {
    public string CombineUrl(string partial)
    {
      var rootUri = new Uri(this.RootUrl);
      var uri = new Uri(rootUri, partial);
      return uri.ToString();
    }

    public string RootUrl
    {
      get
      {
        var rootUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;

        return rootUrl.Replace(":80", "");
      }
    }
  }
}