using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using ServiceStack.ServiceClient.Web;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.RemoteData.v1
{
    public class RemoteDataRepository
    {
        private JsonServiceClient _restClient;
        private JsonServiceClient ServiceClient
        {
            get
            {
                var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
                if (!baseUrl.EndsWith("/"))
                {
                    baseUrl = baseUrl + "/";
                }

                if (_restClient == null)
                {
                    _restClient = new JsonServiceClient(baseUrl);
                }

                return _restClient;
            }
        }


        public void GetConferences(Action<IList<FullConferenceDto>> callback, string sortBy = "end", bool? showPastConferences = false, string search = null)
        {
            var conferences = new Conferences() { sortBy = sortBy, showPastConferences = showPastConferences, search = search };
            ServiceClient.GetAsync(conferences, callback, (r, ex) => { throw ex; });
        }

        public void GetConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) => { throw ex; });
        }


        public void GetFullConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) =>
                                                                                             {
                                                                                                 callback(null);
                                                                                                 //throw ex;
                                                                                             });
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
            ServiceClient.GetAsync(new Speaker() { conferenceSlug = conferenceSlug, speakerSlug = speakerSlug }, callback, (r, ex) => { throw ex; });
        }

        public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
        {
            ServiceClient.GetAsync(new Sessions() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { throw ex; });
        }

        public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
        {
            ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug }, callback, (r, ex) => { throw ex; });
        }

        public void CreateConference(CreateConference conference, Action<FullConferenceDto> callback)
        {
            conference.slug = conference.name.GenerateSlug();
            ServiceClient.PostAsync(conference, callback, (r, ex) => { throw ex; });
        }

        public void CreateUser(string userName, Action<UserDto> callback)
        {
            //ServiceClient.PostAsync(new User() {userName = userName}, callback, (r, ex) => { throw ex; });
            var user = new UserDto() { userName = userName };
            callback(user);
        }

        public void GetUser(string userName, Action<UserDto> callback)
        {
            ServiceClient.GetAsync(new User() { userName = userName }, callback, (r, ex) => { throw ex; });
        }

        public void AddSessionToConference(AddSession session, Action<FullConferenceDto> callback)
        {
            session.slug = session.title.GenerateSlug();
            ServiceClient.PostAsync(session, callback, (r, ex) => { throw ex; });
        }
    }

    public static class Helpers
    {
        public static string GenerateSlug(this string phrase)
        {
            string slug = phrase.RemoveAccent().ToLower();
            // invalid chars           
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            // cut and trim 
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens   
            return slug;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
