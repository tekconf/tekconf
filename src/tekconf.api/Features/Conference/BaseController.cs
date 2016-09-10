using System.Web.Mvc;
using TekConf.Api.Infrastructure;

namespace TekConf.Api.Features.Conference
{
    public class BaseController : Controller
    {
        protected virtual ActionResult Result(object data)
        {
            ActionResult actionResult;

            switch (GetContentType().ToLower())
            {
                case "xml":
                    actionResult = new XmlResult(data);
                    break;
                case "json":
                    actionResult = new JsonResult
                    {
                        Data = data,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    break;
                default:
                    actionResult = View(data);
                    break;
            }

            return actionResult;
        }

        protected virtual string GetContentType()
        {
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {

                if (Request.Headers["Accept"].Contains("text/html"))
                {
                    type = "html";
                } else if (Request.Headers["Accept"].Contains("application/json"))
                {
                    type = "json";
                } else if (Request.Headers["Accept"].Contains("application/xml"))
                {
                    type = "xml";
                }
                
            }

            return type;
        }
    }
}