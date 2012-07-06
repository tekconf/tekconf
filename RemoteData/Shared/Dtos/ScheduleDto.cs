using System.Collections.Generic;

namespace ConferencesIO.RemoteData.Dtos
{
  public class ScheduleDto
  {
    public string userSlug { get; set; }
    public string conferenceSlug { get; set; }
    public string url { get; set; }
    public List<string> sessions { get; set; }
  }
}