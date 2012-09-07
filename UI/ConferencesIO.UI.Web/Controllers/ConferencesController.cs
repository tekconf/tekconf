using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
