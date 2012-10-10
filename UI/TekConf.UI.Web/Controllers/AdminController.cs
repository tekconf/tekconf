using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using System.IO;

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
        public void CreateConferenceAsync(CreateConference conference, HttpPostedFileBase file)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment(2);

            var url = "/img/conferences/" + conference.name.GenerateSlug() + Path.GetExtension(file.FileName); ;
            var filename = Server.MapPath(url);
            conference.imageUrl = url;

            ThreadPool.QueueUserWorkItem(o =>
            {
                file.SaveAs(filename);
                AsyncManager.OutstandingOperations.Decrement();
            }, null);

            repository.CreateConference(conference, c =>
            {
                AsyncManager.Parameters["conference"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult CreateConferenceCompleted(FullConferenceDto conference)
        {
            return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });
        }

        public void AddSessionAsync(string conferenceSlug)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();
            repository.GetFullConference(conferenceSlug, conference =>
            {
                AsyncManager.Parameters["conference"] = conference;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult AddSessionCompleted(FullConferenceDto conference)
        {
            var session = new AddSession() { conferenceSlug = conference.slug, start = conference.start, end = conference.end };
            session.start = conference.start;
            session.end = conference.end;

            return View(session);
        }

        [HttpPost]
        public void AddSessionToConferenceAsync(AddSession session)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();

            repository.AddSessionToConference(session, c =>
            {
                AsyncManager.Parameters["conference"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult AddSessionToConferenceCompleted(FullConferenceDto conference)
        {
            return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });
        }
    }
}
