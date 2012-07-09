using System;
using System.Web;

namespace ConferencesIO.UI.Api
{
  public class BaseUrlResolver
  {
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