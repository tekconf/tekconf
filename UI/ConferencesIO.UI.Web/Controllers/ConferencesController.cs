using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferencesIO.RemoteData.Dtos.v1;

namespace ConferencesIO.UI.Web.Controllers
{
    public class ConferencesController : Controller
    {
        //
        // GET: /Conferences/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Detail(string conferenceSlug)
        {
            ConferenceDto dto = new ConferenceDto();
            return View(dto);
        }

    }
}
