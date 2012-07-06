namespace ConferencesIO.Requests.v1
{
  public class SessionSpeakersRequest
  {
    public string conferenceSlug { get; set; }
    public string sessionSlug { get; set; }
    public string speakerSlug { get; set; }
  }
}