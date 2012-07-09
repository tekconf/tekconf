using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConferencesIO.RemoteData.Dtos;
using AutoMapper;

namespace ConferencesIO.UI.Web
{
  public class Bootstrapper
  {
    public void BootstrapAutomapper()
    {
      Mapper.CreateMap<ConferenceEntity, ConferencesDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore());

      Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore())
          .ForMember(dest => dest.sessionsUrl, opt => opt.Ignore())
          .ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

      Mapper.CreateMap<SessionEntity, SessionsDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore());

      Mapper.CreateMap<SessionEntity, SessionDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore())
          .ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

      Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
          .ForMember(dest => dest.url, opt => opt.Ignore());

      Mapper.CreateMap<SpeakerEntity, SpeakerDto>();

      Mapper.CreateMap<ScheduleEntity, ScheduleDto>();
    }

  }


  public class BaseUrlResolver
  {
    public string RootUrl
    {
      get
      {
        var url = HttpContext.Current.Request.Url;
        var stringSeparators = new[] { "/api" };
        var uriParts = url.OriginalString.Split(stringSeparators, StringSplitOptions.None);
        var rootUrl = uriParts.First();

        return rootUrl.Replace(":80", "");
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