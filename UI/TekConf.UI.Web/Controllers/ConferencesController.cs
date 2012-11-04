using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
    public class ConferencesController : Controller
    {
        private RemoteDataRepositoryAsync _repository;

        public ConferencesController()
        {
            _repository = new RemoteDataRepositoryAsync();
        }

        [CompressFilter]
        public async Task<ActionResult> Index(string sortBy, bool? showPastConferences, string search)
        {
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
                return RedirectToAction("NotFound", "Error");
            }

            return View(conferenceTask.Result);
        }
    }
}