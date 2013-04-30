using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/subscriptions/{emailAddress}", "PUT")]
	[Route("/v1/subscriptions/{emailAddress}", "POST")]
	public class CreateSubscription : IReturn<SubscriptionDto>
	{
		public string EmailAddress { get; set; }
	}
}