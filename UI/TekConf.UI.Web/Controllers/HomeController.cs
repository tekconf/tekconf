using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Web.App_Start;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        [CompressFilter]
        public async Task<ActionResult> Index()
        {
            var conferencesTask = GetFeaturedConferences();
            var speakersTask = GetFeaturedSpeakers();

            await Task.WhenAll(conferencesTask, speakersTask);

            var featuredSpeakers = speakersTask.Result == null ? new List<FullSpeakerDto>() : speakersTask.Result.ToList();
            var featuredConferences = conferencesTask.Result == null ? new List<FullConferenceDto>() : conferencesTask.Result.ToList();

            var filteredConferences = featuredConferences
                                        .Where(c => c.start >= DateTime.Now.AddDays(-2))
                                        .OrderBy(c => c.start)
                                        .Take(4)
                                        .ToList();

            var vm = new HomePageViewModel()
            {
                FeaturedConferences = filteredConferences,
                FeaturedSpeakers = featuredSpeakers
            };

            return View(vm);
        }



        public Task<IList<FullConferenceDto>> GetFeaturedConferences()
        {
            return Task.Run(() =>
            {
                var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

                var repository = new RemoteDataRepository(baseUrl);

                var t = new TaskCompletionSource<IList<FullConferenceDto>>();

                repository.GetFeaturedConferences(c => t.TrySetResult(c));
                
                return t.Task;
            });
        }

        public Task<IList<FullSpeakerDto>> GetFeaturedSpeakers()
        {
            return Task.Run(() =>
            {
                var baseUrl = ConfigurationManager.AppSettings["BaseUrl"]; // TODO : IOC

                var repository = new RemoteDataRepository(baseUrl);

                var t = new TaskCompletionSource<IList<FullSpeakerDto>>();

                repository.GetFeaturedSpeakers(s => t.TrySetResult(s));

                return t.Task;
                
            });
        }
    }
}
