using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            var bootstrapper = new Bootstrapper();
            bootstrapper.BootstrapAutomapper();
        }

        
    }

    public class Bootstrapper
    {
        public void BootstrapAutomapper()
        {
            Mapper.CreateMap<FullConferenceDto, CreateConference>();
            Mapper.CreateMap<AddressDto, Address>();

            Mapper.CreateMap<FullSessionDto, AddSession>();
        }
    }
}