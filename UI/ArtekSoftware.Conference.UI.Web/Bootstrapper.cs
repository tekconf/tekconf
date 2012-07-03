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
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
        //.ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferencesDateResolver>())
        //.ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferencesDateResolver>())
        ;

      Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
        .ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferencesDateResolver>())
        .ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferencesDateResolver>())
        .ForMember(dest => dest.sessionsUrl, opt => opt.ResolveUsing<ConferencesSessionsResolver>())
        .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<ConferencesSpeakersResolver>())
        ;




      Mapper.CreateMap<SessionEntity, SessionsDto>()
        .ForMember(dest => dest.Url, opt => opt.ResolveUsing<SessionsUrlResolver>())
        //.ForMember(dest => dest.Start, opt => opt.ResolveUsing<SessionsDateResolver>())
        //.ForMember(dest => dest.End, opt => opt.ResolveUsing<SessionsDateResolver>())
        ;

      Mapper.CreateMap<SessionEntity, SessionDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<SessionsUrlResolver>())
        //.ForMember(dest => dest.start, opt => opt.ResolveUsing<SessionsDateResolver>())
        //.ForMember(dest => dest.end, opt => opt.ResolveUsing<SessionsDateResolver>())
        .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<SessionsSpeakersUrlResolver>())

        ;



      Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<SpeakersUrlResolver>())

        ;

      Mapper.CreateMap<SpeakerEntity, SpeakerDto>()
        ;


      Mapper.CreateMap<ScheduleEntity, ScheduleDto>()
        .ForMember(dest => dest.sessions, opt => opt.ResolveUsing<ScheduleSessionsResolver>())
      ;
    }

    public abstract class UrlResolver<TSource, TDestination> : ValueResolver<TSource, TDestination>
    {
      public string RootUrl
      {
        get
        {
          var url = HttpContext.Current.Request.Url;
          var rootUrl= url.GetLeftPart(UriPartial.Authority);
          return rootUrl;
        }
      }
    }
    public class SpeakersUrlResolver : UrlResolver<SpeakerEntity, string>
    {
      protected override string ResolveCore(SpeakerEntity source)
      {
        //TODO : Needs conference slug
        //TODO : Needs session slug
        return RootUrl + "/api/conferences/sessions/speakers/" + source.slug;
      }
    }


    public class SessionsSpeakersUrlResolver : UrlResolver<SessionEntity, string>
    {
      protected override string ResolveCore(SessionEntity source)
      {
        //TODO : Needs conference slug
        return RootUrl + "/api/conferences/sessions/" + source.slug + "/speakers";
      }
    }

    public class SessionsUrlResolver : UrlResolver<SessionEntity, string>
    {
      protected override string ResolveCore(SessionEntity source)
      {
        //TODO : Needs conference slug
        return RootUrl + "/api/conferences/sessions/" + source.slug;
      }
    }

    public class SessionsDateResolver : ValueResolver<SessionEntity, DateTime>
    {
      protected override DateTime ResolveCore(SessionEntity source)
      {
          //ServiceStack.Text.JsonSerializer<DateTime> serializer = new JsonSerializer<DateTime>();
          //var date = serializer.DeserializeFromString(source.start);
        return DateTime.Now; //TODO: Don't do this
        // return (DateTime)source.start;
      }
    }

    public class ConferencesSessionsResolver : UrlResolver<ConferenceEntity, string>
    {
      protected override string ResolveCore(ConferenceEntity source)
      {
        return RootUrl + "/api/conferences/" + source.slug + "/sessions";
      }
    }

    public class ConferencesSpeakersResolver : UrlResolver<ConferenceEntity, string>
    {
      protected override string ResolveCore(ConferenceEntity source)
      {
        return RootUrl + "/api/conferences/" + source.slug + "/speakers";
      }
    }

    public class ConferencesUrlResolver : UrlResolver<ConferenceEntity, string>
    {
      protected override string ResolveCore(ConferenceEntity source)
      {
        return RootUrl + "/api/conferences/" + source.slug;
      }
    }

    public class ConferencesDateResolver : ValueResolver<ConferenceEntity, DateTime>
    {
      protected override DateTime ResolveCore(ConferenceEntity source)
      {
        return DateTime.Now; //TODO: Don't do this
        // return (DateTime)source.start;
      }
    }



    public class ScheduleSessionsResolver : UrlResolver<ScheduleEntity, List<string>>
    {
      protected override List<string> ResolveCore(ScheduleEntity source)
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
}