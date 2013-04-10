using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using Elmah;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.Common.Entities;

	using ConferenceRepository = TekConf.Common.Entities.ConferenceRepository;
	using IConferenceRepository = TekConf.Common.Entities.IConferenceRepository;

	public class ConferencesFilter
	{
		public string sortBy { get; set; }
		public bool showPastConferences { get; set; }
		public bool showOnlyOpenCalls { get; set; }
		public bool showOnlyOnSale { get; set; }
		public string viewAs { get; set; }
		public string search { get; set; }

		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public double? latitude { get; set; }
		public double? longitude { get; set; }
		public double? distance { get; set; }

	}

	public class ConferencesController : Controller
	{
		private readonly RemoteDataRepositoryAsync _repository;
		private readonly IConferenceRepository _conferenceRepository;
		public ConferencesController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
			IEntityConfiguration entityConfiguration = new EntityConfiguration();
			_conferenceRepository = new ConferenceRepository(entityConfiguration);
		}

		[CompressFilter]
		public async Task<ActionResult> Index(string sortBy, bool? showPastConferences, bool? showOnlyOpenCalls, bool? showOnlyOnSale,
												string viewAs, string search, string city, string state, string country, double? latitude, double? longitude, double? distance)
		{
			if (!string.IsNullOrWhiteSpace(viewAs) && viewAs == "table")
			{
				ViewBag.ShowTable = true;
			}
			else
			{
				ViewBag.ShowTable = false;
			}

			IEnumerable<ConferenceEntity> conferences = new List<ConferenceEntity>();

			Task getConferencesTask = Task.Factory.StartNew(() =>
					{
						conferences = _conferenceRepository.GetConferences(search, sortBy, showPastConferences, showOnlyOpenCalls, showOnlyOnSale, false, longitude, latitude, distance, city, state, country);
					});

			await getConferencesTask;

			var filter = new ConferencesFilter()
				{
					showOnlyOnSale = showOnlyOnSale.HasValue && showOnlyOnSale.Value,
					showOnlyOpenCalls = showOnlyOpenCalls.HasValue && showOnlyOpenCalls.Value,
					showPastConferences = showPastConferences.HasValue && showPastConferences.Value,
					search = search,
					viewAs = viewAs,
					sortBy = sortBy,
					city = city,
					state = state,
					country = country,
					distance = distance,
					latitude = latitude,
					longitude = longitude
				};

			ViewBag.Filter = filter;

			var conferencesDtos = Mapper.Map<List<FullConferenceDto>>(conferences);

			return View(conferencesDtos);

		}

		[CompressFilter]
		public async Task<ActionResult> Detail(string conferenceSlug)
		{

			string userName = string.Empty;
			if (Request.IsAuthenticated)
			{
				userName = System.Web.HttpContext.Current.User.Identity.Name;
			}

			var conferenceTask = _repository.GetFullConference(conferenceSlug, userName);

			await conferenceTask;

			if (conferenceTask.Result == null)
			{
				Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(new Exception("Conference " + conferenceSlug + " not found")));
				return RedirectToAction("NotFound", "Error");
			}

			return View(conferenceTask.Result);
		}


	}
}
