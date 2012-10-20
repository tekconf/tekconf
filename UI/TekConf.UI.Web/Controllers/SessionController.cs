using System.Collections.Generic;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.Web.Controllers
{
    public class SessionController : AsyncController
    {
        public void IndexAsync(string conferenceSlug)
        {
            var remoteData = new RemoteDataRepository();
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
            var remoteData = new RemoteDataRepository();
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
            if (session == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(session);
        }

    }
}
