using System;
using System.Collections.Generic;

namespace RemoteData.Shared
{
  public class Conference
  {
    public Guid _Id { get; set; }
    public string Description { get; set; }
    public string FacebookUrl { get; set; }
    public string Slug { get; set; }
    public string HomepageUrl { get; set; }
    public string LanyrdUrl { get; set; }
    public string Location { get; set; }
    public string MeetupUrl { get; set; }
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string TwitterHashTag { get; set; }
    public string TwitterName { get; set; }
    public List<Session> Sessions { get; set; }

  }
}
