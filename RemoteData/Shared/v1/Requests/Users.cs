using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/users/{userName}", "GET")]
    [Route("/v1/users", "POST")]
    public class User : IReturn<UserDto>
    {
        public string userName { get; set; }
    }
}