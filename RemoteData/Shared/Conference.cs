using System;

namespace RemoteData.Shared
{
  public class Conference
  {
    public Guid _Id { get; set; }
    public string Description { get; set; }
    public DateTime End { get; set; }
    public string FacebookUrl { get; set; }
    public string Slug { get; set; }
  }
}
