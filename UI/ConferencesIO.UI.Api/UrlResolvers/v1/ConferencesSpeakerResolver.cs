using System;

namespace ConferencesIO.UI.Api.UrlResolvers.v1
{
  public class ConferencesSpeakerResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _speakerSlug;

    public ConferencesSpeakerResolver(string conferenceSlug, string speakerSlug)
    {
      _conferenceSlug = conferenceSlug;
      _speakerSlug = speakerSlug;
    }

    public string ResolveUrl()
    {
      return CombineUrl("/v1/conferences/" + _conferenceSlug + "/speakers/" + _speakerSlug);
    }
  }
}