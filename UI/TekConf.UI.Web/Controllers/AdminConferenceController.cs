using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
    public class AdminConferenceController : AsyncController
    {
        private RemoteDataRepositoryAsync _repository;
        public AdminConferenceController()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            _repository = new RemoteDataRepositoryAsync(baseUrl);
        }

        #region Add Conference

        [HttpGet]
        [CompressFilter]
        public ActionResult CreateConference()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateConference(CreateConference conference, HttpPostedFileBase file)
        {
            string url = string.Empty;

            if (file != null)
            {
                url = "/img/conferences/" + conference.name.GenerateSlug() + Path.GetExtension(file.FileName);
                conference.imageUrl = url;
            }

            var imageTask = SaveConferenceImage(url, file);
            var conferenceTask = _repository.CreateConference(conference);

            await Task.WhenAll(imageTask, conferenceTask);

            return RedirectToAction("Detail", "Conferences", new { conferenceSlug = conference.slug });

        }

        public Task SaveConferenceImage(string url, HttpPostedFileBase file)
        {
            return Task.Factory.StartNew(() =>
            {
                if (file != null)
                {
                    var filename = Server.MapPath(url);
                    file.SaveAs(filename);
                }
            });
        }

        #endregion

        #region Edit Conference

        [HttpGet]
        public void EditConferenceAsync(string conferenceSlug)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

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
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

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

						repository.EditConference(conference, "user", "password", c =>
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

        public void EditConferencesIndexAsync(string sortBy, bool? showPastConferences, string search)
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();

            repository.GetConferences(sortBy: sortBy, showPastConferences: showPastConferences, search: search, callback: conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult EditConferencesIndexCompleted(List<ConferencesDto> conferences)
        {
            return View(conferences.OrderBy(c => c.name).ToList());
        }
    }
}