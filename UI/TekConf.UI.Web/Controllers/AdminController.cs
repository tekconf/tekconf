using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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

        #region Edit Conference

        [HttpGet]
        public void EditConferenceAsync(string conferenceSlug)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            repository.GetFullConference(conferenceSlug, conference =>
            {
                AsyncManager.Parameters["conference"] = conference;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditConferenceCompleted(FullConferenceDto conference)
        {
            var createConference = Mapper.Map<CreateConference>(conference);
            return View(createConference);
        }

        [HttpPost]
        public void EditConfAsync(CreateConference conference, HttpPostedFileBase file)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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

            repository.EditConference(conference, c =>
            {
                AsyncManager.Parameters["conference"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult EditConfCompleted(FullConferenceDto conference)
        {
            return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });
        }

        #endregion

        #region Add Session

        public void AddSessionAsync(string conferenceSlug)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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

        #region Edit Session

        public void EditSessionAsync(string conferenceSlug, string sessionSlug)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            repository.GetFullConference(conferenceSlug, conference =>
            {
                var session = conference.sessions.FirstOrDefault(s => s.slug == sessionSlug);
                AsyncManager.Parameters["session"] = session;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditSessionCompleted(FullSessionDto session)
        {
            var addSession = Mapper.Map<AddSession>(session);
            
            return View(addSession);
        }

        [HttpPost]
        public void EditSessionInConferenceAsync(AddSession session)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();

            repository.EditSessionInConference(session, c =>
            {
                AsyncManager.Parameters["session"] = c;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditSessionInConferenceCompleted(SessionDto session)
        {
            return RedirectToRoute("SessionDetail", new { conferenceSlug = session.conferenceSlug, sessionSlug = session.slug });
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
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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

        #region Edit Speaker

        [HttpGet]
        public void EditSpeakerAsync(string conferenceSlug, string speakerSlug)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var remoteData = new RemoteDataRepository(baseUrl);
            AsyncManager.OutstandingOperations.Increment();

            remoteData.GetSpeaker(conferenceSlug, speakerSlug, speaker =>
            {
                AsyncManager.Parameters["speaker"] = speaker;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditSpeakerCompleted(FullSpeakerDto speaker)
        {
            var createSpeaker = Mapper.Map<CreateSpeaker>(speaker);
            return View(createSpeaker);
        }

        [HttpPost]
        public void EditSpeakerInConferenceAsync(CreateSpeaker speaker, HttpPostedFileBase file)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

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

            repository.EditSpeaker(speaker, s =>
            {
                AsyncManager.Parameters["speaker"] = s;
                AsyncManager.Parameters["conferenceSlug"] = speaker.conferenceSlug;

                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult EditSpeakerInConferenceCompleted(FullSpeakerDto speaker, string conferenceSlug)
        {
            return RedirectToRoute("SessionSpeakerDetail", new { conferenceSlug = conferenceSlug, speakerSlug = speaker.slug });
        }

        #endregion

        public void EditConferencesIndexAsync(string sortBy, bool? showPastConferences, string search)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();

            repository.GetConferences(sortBy: sortBy, showPastConferences: showPastConferences, search: search, callback: conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditConferencesIndexCompleted(List<FullConferenceDto> conferences)
        {
            return View(conferences.OrderBy(c => c.name).ToList());
        }
    }
}
