using System;
using System.Collections.Generic;
using System.Web;
using ArtekSoftware.Conference.RemoteData.Dtos;
using AutoMapper;

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
        //.ForMember(dest => dest.sessionsUrl, opt => opt.ResolveUsing<ConferenceSessionsUrlResolver>())
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
        //.ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<SessionSpeakersUrlResolver>())
          .ForMember(dest => dest.url, opt => opt.Ignore())
          .ForMember(dest => dest.speakersUrl, opt => opt.Ignore())

        ;

      //Mapper.CreateMap<SessionEntity, SessionDto>()
      //    .ForMember(dest => dest.url, opt => opt.ResolveUsing<Bootstrapper.SessionsUrlResolver>().ConstructedBy(() => new SessionsUrlResolver(request.conferenceSlug)))
      //    .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<Bootstrapper.SessionSpeakersUrlResolver>().ConstructedBy(() => new SessionSpeakersUrlResolver(request.conferenceSlug)))
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