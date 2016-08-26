using TekConf.Api.Data;

namespace TekConf.Api.Infrastructure.DataAccess
{
    using System.Web.Mvc;
    using App_Start;

    public class MvcTransactionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Logger.Instance.Verbose("MvcTransactionFilter::OnActionExecuting");
            var context = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<TekConfContext>();
            context.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Logger.Instance.Verbose("MvcTransactionFilter::OnActionExecuted");
            var instance = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<TekConfContext>();
            instance.CloseTransaction(filterContext.Exception);
        }
    }
}