using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/schedule", "POST")]
	public class AddSessionToSchedule : IReturn<ScheduleDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "The unique slug to identify the conference.", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "userName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string userName { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string sessionSlug { get; set; }
	}

	[Route("/v1/conferences/{conferenceSlug}/schedule", "DELETE")]
	public class RemoveSessionFromSchedule : IReturn<ScheduleDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "userName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string userName { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string sessionSlug { get; set; }
	}
}
