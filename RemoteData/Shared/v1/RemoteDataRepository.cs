using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using ServiceStack.ServiceClient.Web;
using TekConf.RemoteData.Dtos.v1;
using ServiceStack.Text;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.RemoteData.v1
{
    public class RemoteDataRepository
    {
        private string _baseUrl;
        public RemoteDataRepository(string baseUrl)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl + "/";
            }
            _baseUrl = baseUrl;
        }

        private JsonServiceClient ServiceClient
        {
            get
            {
                string baseUri = ConfigurationManager.AppSettings["BaseUrl"];

                var restClient = new JsonServiceClient(baseUri);
                return restClient;
            }
        }

        public void GetConferences(Action<IList<FullConferenceDto>> callback)
        {
            ServiceClient.GetAsync(new Conferences(), callback, (r, ex) => { throw ex; });
        }

        public void GetConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug}, callback, (r, ex) => { throw ex; });
        }


        public void GetFullConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) => { throw ex; });
        }


        public void GetFeaturedSpeakers(Action<List<FullSpeakerDto>> callback)
        {
            ServiceClient.GetAsync(new FeaturedSpeakers(), callback, (r, ex) => { throw ex; });
        }

        public void GetSessionSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback)
        {
            ServiceClient.GetAsync(new SessionSpeakers() { conferenceSlug = conferenceSlug, sessionSlug = sessionSlug }, callback, (r, ex) => { throw ex; });
        }

        public void GetSpeakers(string conferenceSlug, Action<IList<FullSpeakerDto>> callback)
        {
            ServiceClient.GetAsync(new Speakers() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { throw ex; });
        }

        public void GetSpeaker(string conferenceSlug, string speakerSlug, Action<FullSpeakerDto> callback)
        {
            ServiceClient.GetAsync(new Speaker() { conferenceSlug = conferenceSlug, speakerSlug = speakerSlug}, callback, (r, ex) => { throw ex; });
        }

        public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
        {
            ServiceClient.GetAsync(new Sessions() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { throw ex; });
        }

        public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
        {
            ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug }, callback, (r, ex) => { throw ex; });
        }


        public void CreateUser(string userName, Action<UserDto> callback)
        {
            ServiceClient.Post(new User() {userName = userName}, callback, (r, ex) => { throw ex; });
            string url = _baseUrl + "users/" + userName;

            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            
            client.DownloadStringCompleted += (sender, args) =>
            {
                var user = JsonSerializer.DeserializeFromString<UserDto>(args.Result);
                callback(user);
            };

            client.DownloadStringAsync(new Uri(url));
        }

        public void GetUser(string userName, Action<UserDto> callback)
        {
            //TODO : Auth against service
            var user = new UserDto() {userName = userName};
            callback(user);
        }
    }
}
