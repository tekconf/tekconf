namespace TekConf.Api.Features.Session
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;

    public class UiController : Controller
    {
        private readonly IMediator _mediator;

        public UiController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        public async Task<ActionResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Details.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return Json(model, JsonRequestBehavior.AllowGet);

        }

    }
}