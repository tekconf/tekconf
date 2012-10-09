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
        public void IndexAsync(string sortBy, bool? showPastConferences)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();
            ViewData["showPastConferences"] = showPastConferences;
            ViewData["sortBy"] = sortBy;

            repository.GetConferences(conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult IndexCompleted(List<FullConferenceDto> conferences)
        {
            List<FullConferenceDto> sorted = null;
            string sortBy = string.Empty;

            if (ViewData["sortBy"] != null)
            {
                sortBy = ViewData["sortBy"].ToString();
            }

            if (sortBy == "startDate")
            {
                sorted = conferences.OrderBy(c => c.start).ToList();                
            }
            else if (sortBy == "name")
            {
                sorted = conferences.OrderBy(c => c.name).ToList();
            }
            else if (sortBy == "callForSpeakersOpeningDate")
            {
                sorted = conferences.OrderBy(c => c.callForSpeakersOpens).ToList();              
            }
            else if (sortBy == "callForSpeakersClosingDate")
            {
                sorted = conferences.OrderBy(c => c.callForSpeakersCloses).ToList();
            }
            else if (sortBy == "registrationOpens")
            {
                sorted = conferences.OrderBy(c => c.registrationOpens).ToList();             
            }
            else
            {
                sorted = conferences.OrderBy(c => c.start).ToList();
            }

            var showPastConferences = ViewData["showPastConferences"];
            if (showPastConferences == null || !(bool)showPastConferences)
            {
                sorted = sorted.Where(c => c.end > DateTime.Now).ToList();
            }
            return View(sorted);
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
