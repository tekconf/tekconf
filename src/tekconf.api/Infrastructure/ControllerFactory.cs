using TekConf.Api.Infrastructure.Extensions;

namespace TekConf.Api.Infrastructure
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var controller = 
                typeof (ControllerFactory).Assembly.GetType($"TekConf.Api.Features.{controllerName.ToTitleCase()}.UiController");


            return controller;
            
        }
    }
}