using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Elmah;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
    public class ConferencesController : Controller
    {
        private RemoteDataRepositoryAsync _repository;

        public ConferencesController()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            _repository = new RemoteDataRepositoryAsync(baseUrl);
        }

        [CompressFilter]
        public async Task<ActionResult> Index(string sortBy, bool? showPastConferences, string viewAs, string search)
        {
            if (!string.IsNullOrWhiteSpace(viewAs) && viewAs == "table")
            {
                ViewBag.ShowTable = true;
            }
            else
            {
                ViewBag.ShowTable = false;                
            }
            var conferencesTask = _repository.GetConferences(sortBy, showPastConferences, search);

            await conferencesTask;

            return View(conferencesTask.Result.ToList());

        }

        [CompressFilter]
        public async Task<ActionResult> Detail(string conferenceSlug)
        {
            var conferenceTask = _repository.GetFullConference(conferenceSlug);

            await conferenceTask;

            if (conferenceTask.Result == null)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(new Exception("Conference " + conferenceSlug + " not found")));
                return RedirectToAction("NotFound", "Error");
            }

            return View(conferenceTask.Result);
        }
    }
}
