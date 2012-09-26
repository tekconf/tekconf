using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/users/{userName}", "GET")]
    [Route("/v1/users", "POST")]
    public class User
    {
        public string userName { get; set; }
    }
}