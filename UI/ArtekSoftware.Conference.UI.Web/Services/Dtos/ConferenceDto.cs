using System;

namespace ArtekSoftware.Conference.UI.Web
{
  public class ConferenceDto
  {
    public string name { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string location { get; set; }
    public string url { get; set; }
    public string slug { get; set; }
  }
}