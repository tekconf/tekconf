using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.Web.App_Start;

namespace TekConf.Web.Controllers
{
	public class ApiController : Controller
	{
        private readonly IConferenceRepository _conferenceRepository;

        public ApiController(IConferenceRepository conferenceRepository)
        {
            _conferenceRepository = conferenceRepository;
        }

		[CompressFilter]
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
