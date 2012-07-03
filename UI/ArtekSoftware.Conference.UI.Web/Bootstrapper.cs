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
              ;

            Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
              .ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
              .ForMember(dest => dest.sessionsUrl, opt => opt.ResolveUsing<ConferencesSessionsResolver>())
              .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<ConferencesSpeakersResolver>())
              ;


            //Mapper.CreateMap<SessionEntity, SessionsDto>()
            //  .ForMember(dest => dest.Url, opt => opt.ResolveUsing<SessionsUrlResolver>().ConstructedBy(() => new Bootstrapper.SessionsUrlResolver(string.Empty)))
            //  ;

            //Mapper.CreateMap<SessionEntity, SessionDto>()
            //  .ForMember(dest => dest.url, opt => opt.ResolveUsing<SessionsUrlResolver>())
            //  //.ForMember(dest => dest.start, opt => opt.ResolveUsing<SessionsDateResolver>())
            //  //.ForMember(dest => dest.end, opt => opt.ResolveUsing<SessionsDateResolver>())
            //  .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<SessionsSpeakersUrlResolver>())
            //  ;

            //Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
            //  .ForMember(dest => dest.url, opt => opt.ResolveUsing<SpeakersUrlResolver>().ConstructedBy(() => new Bootstrapper.SpeakersUrlResolver(string.Empty, string.Empty)))
            //  ;

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
                    //TODO - go until /api
                    var url = HttpContext.Current.Request.Url;
                    var rootUrl = url.GetLeftPart(UriPartial.Authority) + "/ArtekSoftware.Conference.UI.Web";
                    return rootUrl;
                }
            }
        }

        public class SpeakersUrlResolver : UrlResolver<SpeakerEntity, string>
        {
            private readonly string _conferenceSlug;
            private readonly string _sessionSlug;

            public SpeakersUrlResolver(string conferenceSlug, string sessionSlug)
            {
                _conferenceSlug = conferenceSlug;
                _sessionSlug = sessionSlug;
            }

            protected override string ResolveCore(SpeakerEntity source)
            {
                return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + _sessionSlug + "/speakers/" + source.slug;
            }
        }


        public class SessionsSpeakersUrlResolver : UrlResolver<SessionEntity, string>
        {
            private readonly string _conferenceSlug;

            public SessionsSpeakersUrlResolver(string conferenceSlug)
            {
                _conferenceSlug = conferenceSlug;
            }

            protected override string ResolveCore(SessionEntity source)
            {
                return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + source.slug + "/speakers";
            }
        }

        public class SessionsUrlResolver : UrlResolver<SessionEntity, string>
        {
            private readonly string _conferenceSlug;

            public SessionsUrlResolver(string conferenceSlug)
            {
                _conferenceSlug = conferenceSlug;
            }

            protected override string ResolveCore(SessionEntity source)
            {
                return RootUrl + "/api/conferences/" + _conferenceSlug + "/sessions/" + source.slug;
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