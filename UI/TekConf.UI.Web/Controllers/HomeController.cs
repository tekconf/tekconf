using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Web.App_Start;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
    public class HomeController : AsyncController
    {
        public void IndexAsync()
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment(2);

            repository.GetConferences(conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });

            repository.GetFeaturedSpeakers(speakers =>
            {
                AsyncManager.Parameters["featuredSpeakers"] = speakers;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        [CompressFilter]
        public ActionResult IndexCompleted(List<ConferencesDto> conferences, List<SpeakersDto> featuredSpeakers)
        {
            var filteredConferences = conferences.Where(c => c.start >= DateTime.Now.AddDays(-2)).OrderByDescending(c => c.start).Take(4).ToList();

            var vm = new HomePageViewModel()
                         {
                             FeaturedConferences = filteredConferences,
                             FeaturedSpeakers = featuredSpeakers
                         };
            return View(vm);
        }

    }
}
