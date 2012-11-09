using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
    public class SessionController : Controller
    {
        private RemoteDataRepositoryAsync _repository;

        public SessionController()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            _repository = new RemoteDataRepositoryAsync(baseUrl);
        }

        [CompressFilter]
        public async Task<ActionResult> Index(string conferenceSlug)
        {
            var sessionsTask = _repository.GetSessions(conferenceSlug);
            await sessionsTask;
            return View(sessionsTask.Result);
        }

        [CompressFilter]
        public async Task<ActionResult> Detail(string conferenceSlug, string sessionSlug)
        {
            var sessionDetailTask = _repository.GetSessionDetail(conferenceSlug, sessionSlug);
            
            await sessionDetailTask;

            if (sessionDetailTask.Result == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(sessionDetailTask.Result);
        }
    }
}