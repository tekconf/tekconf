using System.Threading;
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


        #region Add Conference

        [HttpGet]
        public ActionResult CreateConference()
        {
            return View();
        }

        [HttpPost]
        public void CreateConferenceAsync(CreateConference conference, HttpPostedFileBase file)
        {
            var repository = new RemoteDataRepository();

            if (file != null)
            {
                AsyncManager.OutstandingOperations.Increment(2);                
            }
            else
            {
                AsyncManager.OutstandingOperations.Increment(1);                
            }

            if (file != null)
            {

                var url = "/img/conferences/" + conference.name.GenerateSlug() + Path.GetExtension(file.FileName); ;
                var filename = Server.MapPath(url);
                conference.imageUrl = url;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    file.SaveAs(filename);
                    AsyncManager.OutstandingOperations.Decrement();
                }, null);
            }

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

        #endregion


        #region Add Session

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
                AsyncManager.Parameters["session"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult AddSessionToConferenceCompleted(SessionDto session)
        {
            return RedirectToRoute("AdminAddSpeaker", new { conferenceSlug = session.conferenceSlug, sessionSlug = session.slug });
        }

        #endregion


        #region Add Speaker

        [HttpGet]
        public ActionResult CreateSpeaker()
        {
            return View();
        }

        [HttpPost]
        public void CreateSpeakerAsync(CreateSpeaker speaker, HttpPostedFileBase file)
        {
            var repository = new RemoteDataRepository();

            if (file != null)
            {
                AsyncManager.OutstandingOperations.Increment(2);
            }
            else
            {
                AsyncManager.OutstandingOperations.Increment(1);
            }

            if (file != null)
            {
                var url = "/img/speakers/" + (speaker.firstName + " " + speaker.lastName).GenerateSlug() + Path.GetExtension(file.FileName); ;
                var filename = Server.MapPath(url);
                speaker.profileImageUrl = url;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    file.SaveAs(filename);
                    AsyncManager.OutstandingOperations.Decrement();
                }, null);
            }

            repository.AddSpeakerToSession(speaker, s =>
            {
                AsyncManager.Parameters["speaker"] = s;
                AsyncManager.Parameters["conferenceSlug"] = speaker.conferenceSlug;
                AsyncManager.Parameters["sessionSlug"] = speaker.sessionSlug;

                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult CreateSpeakerCompleted(FullSpeakerDto speaker, string conferenceSlug, string sessionSlug)
        {
            return RedirectToAction("Detail", "Session", new { conferenceSlug = conferenceSlug, sessionSlug =  sessionSlug});
        }

        #endregion

    }
}
