using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;

namespace ConferencesIO.UI.Web.Controllers
{
    public class ConferencesController : AsyncController
    {
        public void IndexAsync()
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            
            repository.GetConferences(conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult IndexCompleted(List<ConferencesDto> conferences)
        {
            return View(conferences);
        }

        public void DetailAsync(string conferenceSlug)
        {
            if (conferenceSlug == default(string))
            {
                conferenceSlug = "ThatConference-2012"; //TODO
            }
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            repository.GetFullConference(conferenceSlug, conference =>
            {
                AsyncManager.Parameters["conference"] = conference;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult DetailCompleted(FullConferenceDto conference)
        {
            return View(conference);
        }
    }
}
