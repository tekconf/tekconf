using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/users/{userName}", "GET")]
    [Route("/v1/users", "POST")]
    public class User : IReturn<UserDto>
    {
			[ApiMember(Name = "userName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = true)]
			public string userName { get; set; }
    }
}
