using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.Web.Controllers
{
    public class SessionController : AsyncController
    {
        public string BaseUrl()
        {
            return ConfigurationManager.AppSettings["baseUrl"];
        }

        public void IndexAsync(string conferenceSlug)
        {
            var remoteData = new RemoteDataRepository(BaseUrl());
            AsyncManager.OutstandingOperations.Increment();
            remoteData.GetSessions(conferenceSlug, sessions =>
            {
                AsyncManager.Parameters["sessions"] = sessions;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult IndexCompleted(List<SessionsDto> sessions)
        {
            return View(sessions);
        }

        public void DetailAsync(string conferenceSlug, string sessionSlug)
        {
            var remoteData = new RemoteDataRepository(BaseUrl());
            AsyncManager.OutstandingOperations.Increment();
            remoteData.GetSession(conferenceSlug, sessionSlug, session =>
            {
                session.conferenceSlug = conferenceSlug;
                AsyncManager.Parameters["session"] = session;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult DetailCompleted(SessionDto session)
        {
            var gravatarImage = new GravatarImage();
            foreach (var speaker in session.speakers)
            {
                speaker.profileImageUrl = gravatarImage.GetURL("robgibbens@gmail.com", 65, "pg");
            }
            return View(session);
        }

    }
}
