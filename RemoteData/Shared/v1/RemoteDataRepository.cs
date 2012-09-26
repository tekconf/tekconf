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
            if (slug == default(string))
            {
                slug = "ThatConference-2012";
            }
            string url = _baseUrl + "conferences/" + slug + "?detail=all";
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            client.DownloadStringCompleted += (sender, args) =>
            {
                var conference = JsonSerializer.DeserializeFromString<FullConferenceDto>(args.Result);
                callback(conference);
            };

            client.DownloadStringAsync(new Uri(url));
        }


        public void GetFeaturedSpeakers(Action<List<SpeakersDto>> callback)
        {
            string url = _baseUrl + "speakers/featured";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.Accept] = "application/json";

            client.DownloadStringCompleted += (sender, args) =>
            {
                var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(args.Result);
                callback(speakers);
            };

            client.DownloadStringAsync(new Uri(url));
        }

        public void GetSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback)
        {
            string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.Accept] = "application/json";

            client.DownloadStringCompleted += (sender, args) =>
            {
                var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(args.Result);
                callback(speakers);
            };

            client.DownloadStringAsync(new Uri(url));
        }

        public void GetSpeakers(string conferenceSlug, Action<IList<SpeakersDto>> callback)
        {
            string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.Accept] = "application/json";

            client.DownloadStringCompleted += (sender, args) =>
            {
                var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(args.Result);
                //callback(speakers.OrderBy(s => s.firstName).OrderBy(s => s.lastName).ToList());
                callback(speakers.OrderBy(s => s.lastName).OrderBy(s => s.firstName).ToList());
            };

            client.DownloadStringAsync(new Uri(url));

        }

        public void GetSpeaker(string conferenceSlug, string slug, Action<FullSpeakerDto> callback)
        {
            string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers/" + slug;

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.Accept] = "application/json";

            client.DownloadStringCompleted += (sender, args) =>
            {
                var speaker = JsonSerializer.DeserializeFromString<FullSpeakerDto>(args.Result);
                callback(speaker);
            };

            client.DownloadStringAsync(new Uri(url));
        }

        public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
        {
            string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions?format=json";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            //client.Headers[HttpRequestHeader.Accept] = "application/json";

            client.DownloadStringCompleted += (sender, args) =>
              {
                  var sessions = JsonSerializer.DeserializeFromString<List<SessionsDto>>(args.Result);
                  callback(sessions);
              };

            client.DownloadStringAsync(new Uri(url));

        }


        public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
        {
            ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug }, callback, (r, ex) => { throw ex; });
        }


        public void CreateUser(string userName, Action<UserDto> callback)
        {
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
