using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/schedule", "GET")]
	public class Schedule : IReturn<ScheduleDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "userName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string userName { get; set; }
	}
}