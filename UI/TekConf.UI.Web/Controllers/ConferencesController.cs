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
        public void IndexAsync(string sortBy)
        {
            var repository = new RemoteDataRepository();

            AsyncManager.OutstandingOperations.Increment();
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
            else if (sortBy == "callForSpeakersOpeningDate")
            {
                //sorted = conferences.OrderBy(c => c.start).ToList();
                sorted = conferences.OrderBy(c => c.start).ToList();              
            }
            else if (sortBy == "callForSpeakersClosingDate")
            {
                //sorted = conferences.OrderBy(c => c.start).ToList();
                sorted = conferences.OrderBy(c => c.start).ToList();
            }
            else if (sortBy == "callForSpeakersDate")
            {
                //sorted = conferences.OrderBy(c => c.start).ToList();
                sorted = conferences.OrderBy(c => c.start).ToList();             
            }
            else
            {
                sorted = conferences.OrderBy(c => c.start).ToList();
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
