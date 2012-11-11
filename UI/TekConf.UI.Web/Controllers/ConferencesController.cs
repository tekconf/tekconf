using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
    public class ConferencesController : AsyncController
    {
        public void IndexAsync(string sortBy, bool? showPastConferences, string search)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();

            repository.GetConferences(sortBy: sortBy, showPastConferences: showPastConferences, search:search, callback:conferences =>
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
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

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
            if (conference == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(conference);
        }
    
    }
}
