using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtekSoftware.Conference.RemoteData.Dtos;
using AutoMapper;
using ServiceStack.Text;

namespace ArtekSoftware.Conference.UI.Web
{
  public class Bootstrapper
  {
    public void BootstrapAutomapper()
    {
      Mapper.CreateMap<ConferenceEntity, ConferencesDto>()
        //.ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
          .ForMember(dest => dest.url, opt => opt.Ignore())

        ;

      Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
        //.ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
        //.ForMember(dest => dest.sessionsUrl, opt => opt.ResolveUsing<ConferencesSessionsResolver>())
        //.ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<ConferencesSpeakersResolver>())
          .ForMember(dest => dest.url, opt => opt.Ignore())
          .ForMember(dest => dest.sessionsUrl, opt => opt.Ignore())
          .ForMember(dest => dest.speakersUrl, opt => opt.Ignore())
        ;

      Mapper.CreateMap<SessionEntity, SessionsDto>()
        //.ForMember(dest => dest.url, opt => opt.ResolveUsing<SessionsUrlResolver>().ConstructedBy(() => new SessionsUrlResolver(string.Empty)))
          .ForMember(dest => dest.url, opt => opt.Ignore())
        ;
      //          Mapper.CreateMap<SessionEntity, SessionsDto>()
      //.ForMember(dest => dest.url, opt => opt.ResolveUsing<Bootstrapper.SessionsUrlResolver>().ConstructedBy(() => new SessionsUrlResolver(request.conferenceSlug)))
      //;
      Mapper.CreateMap<SessionEntity, SessionDto>()
        //.ForMember(dest => dest.url, opt => opt.ResolveUsing<SessionsUrlResolver>())
        //.ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<SessionsSpeakersUrlResolver>())
          .ForMember(dest => dest.url, opt => opt.Ignore())
          .ForMember(dest => dest.speakersUrl, opt => opt.Ignore())

        ;

      //Mapper.CreateMap<SessionEntity, SessionDto>()
      //    .ForMember(dest => dest.url, opt => opt.ResolveUsing<Bootstrapper.SessionsUrlResolver>().ConstructedBy(() => new SessionsUrlResolver(request.conferenceSlug)))
      //    .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<Bootstrapper.SessionsSpeakersUrlResolver>().ConstructedBy(() => new SessionsSpeakersUrlResolver(request.conferenceSlug)))
      //    .ForMember(dest => dest.speakers, opt => opt.ResolveUsing<Bootstrapper.SessionSpeakersResolver>().ConstructedBy(() => new SessionSpeakersResolver(request.conferenceSlug, request.sessionSlug)))
      //    ;

      Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore())
        //.ForMember(dest => dest.url, opt => opt.ResolveUsing<SpeakersUrlResolver>().ConstructedBy(() => new SpeakersUrlResolver(string.Empty, string.Empty)))
        ;

      Mapper.CreateMap<SpeakerEntity, SpeakerDto>()
        ;

      Mapper.CreateMap<ScheduleEntity, ScheduleDto>()
        //.ForMember(dest => dest.sessions, opt => opt.ResolveUsing<ScheduleSessionsResolver>())
      ;
    }




  }


  public class SpeakerUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;
    private readonly string _speakerUrl;

    public SpeakerUrlResolver(string conferenceSlug, string sessionSlug, string speakerUrl)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
      _speakerUrl = speakerUrl;
    }

    public string ResolveUrl()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers/" + _speakerUrl;
    }
  }

  public class SessionsSpeakersUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;

    public SessionsSpeakersUrlResolver(string conferenceSlug)
    {
      _conferenceSlug = conferenceSlug;
    }

    public string ResolveCore(SessionEntity source)
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + source.slug + "/speakers";
    }
  }

  public class SessionsUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionsUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveCore(SessionsDto source)
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug;
    }
  }

  public class SessionUrlResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _sessionSlug;

    public SessionUrlResolver(string conferenceSlug, string sessionSlug)
    {
      _conferenceSlug = conferenceSlug;
      _sessionSlug = sessionSlug;
    }

    public string ResolveCore(SessionDto source)
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug;
    }
  }

  public class ConferencesSessionsResolver : BaseUrlResolver
  {
    public string ResolveCore(ConferenceEntity source)
    {
      return RootUrl + "/api/conferences/" + source.slug + "/sessions";
    }
  }

  public class ConferencesSpeakersResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _speakerSlug;

    public ConferencesSpeakersResolver(string conferenceSlug, string speakerSlug)
    {
      _conferenceSlug = conferenceSlug;
      _speakerSlug = speakerSlug;
    }

    public string ResolveCore()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/speakers/" + _speakerSlug;
    }
  }

  public class ConferencesSpeakerResolver : BaseUrlResolver
  {
    private readonly string _conferenceSlug;
    private readonly string _speakerSlug;

    public ConferencesSpeakerResolver(string conferenceSlug, string speakerSlug)
    {
      _conferenceSlug = conferenceSlug;
      _speakerSlug = speakerSlug;
    }

    public string ResolveCore()
    {
      return RootUrl + "/api/conferences/" + _conferenceSlug + "/speakers/" + _speakerSlug;
    }
  }

  public class ConferencesUrlResolver : BaseUrlResolver
  {
    public string ResolveCore(ConferencesDto source)
    {
      return RootUrl + "/api/conferences/" + source.slug;
    }
  }

  public class ConferenceUrlResolver : BaseUrlResolver
  {
    public string ResolveCore(ConferenceDto source)
    {
      return RootUrl + "/api/conferences/" + source.slug;
    }
  }

  public class BaseUrlResolver
  {
    public string RootUrl
    {
      get
      {
        //TODO - go until /api
        var url = HttpContext.Current.Request.Url;
        var rootUrl = url.GetLeftPart(UriPartial.Authority) + "/ArtekSoftware.Conference.UI.Web";
        return rootUrl;
      }
    }
  }

  public class ScheduleSessionsResolver : BaseUrlResolver
  {
    public List<string> ResolveCore(ScheduleEntity source)
    {
      var sessionUrls = new List<string>();
      foreach (var session in source.SessionUrls)
      {
        sessionUrls.Add(RootUrl + "/" + session);
      }
      return sessionUrls;
    }
  }
}