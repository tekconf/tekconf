using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Web.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
        private readonly IConferenceRepository _conferenceRepository;

        public AdminController(IConferenceRepository conferenceRepository)
        {
            _conferenceRepository = conferenceRepository;
        }

		public async Task<ActionResult> Index()
		{
            IList<FullConferenceDto> newestConferences = null;
            var getNewestConferencesTask = Task.Factory.StartNew(() =>
            {
                var nconferences = _conferenceRepository.GetNewestConferences();

                newestConferences = Mapper.Map<List<FullConferenceDto>>(nconferences);
            });
            await getNewestConferencesTask;
            ViewBag.NewestConferences = newestConferences;

			return View();
		}
	}
}