using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class PresentationsController : Controller
	{
	    private readonly IRemoteDataRepositoryAsync _repository;
	   
		public PresentationsController(IRemoteDataRepositoryAsync repository)
		{
		    _repository = repository;
            //var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            //_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Detail(string slug)
		{
			return View();
		}

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Add(AddPresentationViewModel addPresentationViewModel)
		{
            
			var presentation = new CreatePresentation()
				{
                    UserName = this.ControllerContext.HttpContext.User.Identity.Name,
                    SpeakerSlug = this.ControllerContext.HttpContext.User.Identity.Name,
					Description = addPresentationViewModel.Description,
					Tags = addPresentationViewModel.Tags,
					Title = addPresentationViewModel.Title
				};
            var presentationTask = _repository.CreatePresentation(presentation);

			await Task.WhenAll(presentationTask);

			return RedirectToAction("AddHistory", new { presentationSlug = presentation.Slug });
		}


		public ActionResult AddHistory(string slug)
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> AddHistory(AddPresentationHistoryViewModel addPresentationHistoryViewModel)
		{
			return RedirectToRoute("PresentationDetail", new { slug = "test" });
		}



	}
}
