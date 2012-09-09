﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferencesIO.UI.Web.Models;

namespace ConferencesIO.UI.Web.Controllers
{
  public class ApiController : Controller
  {
    //
    // GET: /Api/

    public ActionResult Index()
    {
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
                          url: ""http://conferencesio.cloudapp.net:81/v1/conferences/CodeMash-2012"",
                          slug: ""CodeMash-2012""
                        },
                        {
                          name: ""ThatConference"",
                          start: ""/Date(1344830400000)/"",
                          end: ""/Date(1345003200000)/"",
                          location: ""Kalahari Resort, Wisconsin Dells, WI"",
                          url: ""http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012"",
                          slug: ""ThatConference-2012""
                        }
                        ]",
              ExampleUri = "http://conferencesio.cloudapp.net:81/v1/conferences",
              HttpMethod = "GET",
              Uri = "http://conferencesio.cloudapp.net:81/v1/conferences"
              
            }
        };
      return View(documentation);
    }

    public ActionResult Detail()
    {
      throw new NotImplementedException();
    }
  }
}
