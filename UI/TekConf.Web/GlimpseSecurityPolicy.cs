using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;

namespace TekConf.Web
{
    public class GlimpseSecurityPolicy : IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
						//var httpContext = policyContext.GetHttpContext();
						//if (httpContext.User == null
						//				|| httpContext.User.Identity.Name.ToLower() != "robgibbens"
						//				|| httpContext.User.Identity.Name.ToLower() != "qconf")
						//{
						//		return RuntimePolicy.Off;
						//}

            return RuntimePolicy.On;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }
    }
}