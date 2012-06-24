using System.Collections.Generic;

namespace ArtekSoftware.Conference.RemoteData.Dtos
{
  public class ScheduleDto
  {
    public string userSlug { get; set; }
    public string conferenceSlug { get; set; }
    public List<string> sessions { get; set; }
  }
}