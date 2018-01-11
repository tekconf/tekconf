using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;

namespace tekconf.cli
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {

            // discover endpoints from metadata

            //var disco = await DiscoveryClient.GetAsync("http://localhost:5001");
            var disco = await DiscoveryClient.GetAsync("https://tekconfidentity.azurewebsites.net");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ExternalApiClient", "secret");

            // request token
            var tokenResponse =
                await tokenClient.RequestClientCredentialsAsync("tekconfApi tekconfApiPostAttendee");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            
            var response = await client.PostAsync("https://tekconfapi.azurewebsites.net/Attendee/Post/1/Roland", null);
            //var response = await client.PostAsync("http://localhost:54439/Attendee/Post/1/Roland", null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine("Attendee posted");
            }
            Console.ReadKey();
        }
    }
}
