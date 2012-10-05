using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web.Controllers
{
    public class AdminController : AsyncController
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateConference()
        {
            return View();
        }

        [HttpPost]
        public void CreateConferenceAsync(CreateConference conference)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();

            repository.CreateConference(conference, c =>
            {
                AsyncManager.Parameters["conference"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult CreateConferenceCompleted(FullConferenceDto conference)
        {
            return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug }); //TODO 
        }

    }
}
