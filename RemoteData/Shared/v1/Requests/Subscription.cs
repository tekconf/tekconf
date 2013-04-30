using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/subscriptions/{emailAddress}", "GET")]
	public class Subscription : IReturn<SubscriptionDto>
	{
		[ApiMember(Name = "emailAddress", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string emailAddress { get; set; }
	}
}