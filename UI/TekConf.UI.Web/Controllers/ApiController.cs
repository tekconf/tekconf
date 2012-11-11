using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.App_Start;
using TekConf.UI.Web.Models;

namespace TekConf.UI.Web.Controllers
{
    public class ApiController : Controller
    {
        [CompressFilter]
        public async Task<ActionResult> Index()
        {
            var documentationTask = GetDocumentation();
            
            await documentationTask;

            return View(documentationTask.Result);
        }

        private Task<List<ApiDocumentation>> GetDocumentation()
        {
            return Task.Run(() =>
            {
                var t = new TaskCompletionSource<List<ApiDocumentation>>();

                var documentation = new List<ApiDocumentation>()
                    {
                      new ApiDocumentation()
                        {
                          Group = "Conferences",
                          Resource = "/v1/conferences",
                          Description = "Returns a list of all upcoming conferences",
                          ExampleRequestPayload = "",
                          ExampleResponsePayload = @"[
                                    {
                                      name: ""CodeMash"",
                                      start: ""/Date(1326171600000)/"",
                                      end: ""/Date(1326430800000)/"",
                                      location: ""Sandusky, OH, USA"",
                                      url: ""http://api.tekconf.com/v1/conferences/CodeMash-2012"",
                                      slug: ""CodeMash-2012""
                                    },
                                    {
                                      name: ""ThatConference"",
                                      start: ""/Date(1344830400000)/"",
                                      end: ""/Date(1345003200000)/"",
                                      location: ""Kalahari Resort, Wisconsin Dells, WI"",
                                      url: ""http://api.tekconf.com/v1/conferences/ThatConference-2012"",
                                      slug: ""ThatConference-2012""
                                    }
                                    ]",
                          ExampleUri = "http://api.tekconf.com/v1/conferences",
                          HttpMethod = "GET",
                          Uri = "http://api.tekconf.com/v1/conferences"
              
                        }
                    };
                t.TrySetResult(documentation);

                return t.Task;
            });
        }

        [CompressFilter]
        public ActionResult Detail()
        {
            throw new NotImplementedException();
        }
    }
}
