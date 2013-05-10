using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/push/{userName}/wp", "POST")]
	public class WindowsPhonePushRequest : IReturn<bool>
	{
		[ApiMember(Name = "userName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string userName { get; set; }

		[ApiMember(Name = "endpointUri", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string endpointUri { get; set; }
	}
}