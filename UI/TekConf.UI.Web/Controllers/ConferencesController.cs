using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.Web.Controllers
{
    public class ConferencesController : AsyncController
    {
        public void IndexAsync()
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();
            
            repository.GetConferences(conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult IndexCompleted(List<FullConferenceDto> conferences)
        {
            return View(conferences);
        }

        public void DetailAsync(string conferenceSlug)
        {
            var repository = new RemoteDataRepository();

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
