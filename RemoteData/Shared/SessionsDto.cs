using System.Collections.Generic;

namespace ArtekSoftware.Conference.RemoteData.Dtos
{
  public class SessionsDto
  {
    public string Slug { get; set; }
    public string ConferenceSlug { get; set; }
    public string Title { get; set; }
    public object Start { get; set; }
    public object End { get; set; }
    public string Room { get; set; }
    public string Difficulty { get; set; }
    public string Description { get; set; }
    public string TwitterHashTag { get; set; }
    public string SessionType { get; set; }
    public string Url { get; set; }
    public string LinksUrl { get; set; }
    public string TagsUrl { get; set; }
    public string SubjectsUrl { get; set; }
    public string SpeakersUrl { get; set; }

    public List<string> Links { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Subjects { get; set; }
    public List<SpeakersDto> Speakers { get; set; }
  }
}