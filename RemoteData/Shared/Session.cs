using System;
using System.Collections.Generic;

namespace RemoteData.Shared
{
  public class Session
  {
    public string Slug { get; set; }
    public string ConferenceSlug { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public string End { get; set; }
    public string Room { get; set; }
    public string Difficulty { get; set; }
    public string Description { get; set; }
    public string TwitterHashTag { get; set; }
    public string SessionType { get; set; }
    public List<string> Links { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Subjects { get; set; }
    public List<Speaker> Speakers { get; set; }
  }
}