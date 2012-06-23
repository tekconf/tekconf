using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ArtekSoftware.Conference.RemoteData.Dtos;
using AutoMapper;

namespace ArtekSoftware.Conference.UI.Web
{
  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      routes.IgnoreRoute("api/{*pathInfo}");
      routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" }); //Prevent exceptions for favicon

      //routes.MapHttpRoute(
      //    name: "DefaultApi",
      //    routeTemplate: "api/{controller}/{id}",
      //    defaults: new { id = RouteParameter.Optional }
      //);

      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }

    protected void Application_Start()
    {
      BootstrapAutomapper();

      AreaRegistration.RegisterAllAreas();

      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);

      BundleTable.Bundles.RegisterTemplateBundles();
    }

    private void BootstrapAutomapper()
    {
      Mapper.CreateMap<ConferenceEntity, ConferencesDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
        .ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferencesDateResolver>())
        .ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferencesDateResolver>())
        ;

      Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<ConferencesUrlResolver>())
        .ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferencesDateResolver>())
        .ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferencesDateResolver>())
        ;

      Mapper.CreateMap<SessionEntities, SessionsDto>()
        .ForMember(dest => dest.Url, opt => opt.ResolveUsing<SessionsUrlResolver>())
        .ForMember(dest => dest.Start, opt => opt.ResolveUsing<SessionsDateResolver>())
        .ForMember(dest => dest.End, opt => opt.ResolveUsing<SessionsDateResolver>())
        ;

      Mapper.CreateMap<SessionEntities, SessionDto>()
        .ForMember(dest => dest.url, opt => opt.ResolveUsing<SessionsUrlResolver>())
        .ForMember(dest => dest.start, opt => opt.ResolveUsing<SessionsDateResolver>())
        .ForMember(dest => dest.end, opt => opt.ResolveUsing<SessionsDateResolver>())
        ;

      Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
        ;

      Mapper.CreateMap<SpeakerEntity, SpeakerDto>()
        ;
    }

    protected void Application_BeginRequest(object src, EventArgs e)
    {
        if (Request.IsLocal)
            ServiceStack.MiniProfiler.Profiler.Start();
    }

    protected void Application_EndRequest(object src, EventArgs e)
    {
        ServiceStack.MiniProfiler.Profiler.Stop();
    }
  }

  public class ConferencesUrlResolver : ValueResolver<ConferenceEntity, string>
  {
    protected override string ResolveCore(ConferenceEntity source)
    {
      //TODO : Make relative
      return "http://localhost:6327/api/conferences/" + source.slug;
    }
  }

  public class ConferencesDateResolver : ValueResolver<ConferenceEntity, DateTime>
  {
    protected override DateTime ResolveCore(ConferenceEntity source)
    {
      return DateTime.Now; //TODO: DOn't do this
     // return (DateTime)source.start;
    }
  }

  public class SessionsUrlResolver : ValueResolver<SessionEntities, string>
  {
    protected override string ResolveCore(SessionEntities source)
    {
      //TODO : Make relative
      return "http://localhost:6327/api/conferences/sessions/" + source.slug;
    }
  }

  public class SessionsDateResolver : ValueResolver<SessionEntities, DateTime>
  {
    protected override DateTime ResolveCore(SessionEntities source)
    {
      return DateTime.Now; //TODO: DOn't do this
      // return (DateTime)source.start;
    }
  }
}