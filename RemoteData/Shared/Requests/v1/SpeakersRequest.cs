namespace ConferencesIO.Requests.v1
{
  public class SpeakersRequest
  {
    public string conferenceSlug { get; set; }
    public string speakerSlug { get; set; }
  }
}