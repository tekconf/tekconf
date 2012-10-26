using System;
using System.Collections.Generic;
using System.Configuration;
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
                if (_restClient == null)
                {
                    _restClient = new JsonServiceClient(_baseUrl);
                }

                return _restClient;
            }
        }

		private string _baseUrl;
		public RemoteDataRepository (string baseUrl)
		{
//			if (!baseUrl.EndsWith("/"))
//			{
//				baseUrl = baseUrl + "/";
//			}

			_baseUrl = baseUrl;
		}

        public void GetConferences(Action<IList<FullConferenceDto>> callback, string sortBy = "end", bool? showPastConferences = false, string search = null)
        {
            var conferences = new Conferences() { sortBy = sortBy, showPastConferences = showPastConferences, search = search, showOnlyFeatured = false };
            ServiceClient.GetAsync(conferences, callback, (r, ex) => { callback(null); });
        }

        public void GetFeaturedConferences(Action<IList<FullConferenceDto>> callback)
        {
            var featured = new Conferences() { showOnlyFeatured = true };
            ServiceClient.GetAsync(featured, callback, (r, ex) => { callback(null); });
        }

        public void GetConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) => { callback(null); });
        }

        public void GetFullConference(string slug, Action<FullConferenceDto> callback)
        {
            ServiceClient.GetAsync(new Conference() { conferenceSlug = slug }, callback, (r, ex) =>
                                                                                             {
                                                                                                 callback(null);
                                                                                             });
        }

        public void GetFeaturedSpeakers(Action<List<FullSpeakerDto>> callback)
        {
            ServiceClient.GetAsync(new FeaturedSpeakers(), callback, (r, ex) => { callback(null); });
        }

        public void GetSessionSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback)
        {
            ServiceClient.GetAsync(new SessionSpeakers() { conferenceSlug = conferenceSlug, sessionSlug = sessionSlug }, callback, (r, ex) => { callback(null); });
        }

        public void GetSpeakers(string conferenceSlug, Action<IList<FullSpeakerDto>> callback)
        {
            ServiceClient.GetAsync(new Speakers() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { callback(null); });
        }

        public void GetSpeaker(string conferenceSlug, string speakerSlug, Action<FullSpeakerDto> callback)
        {
            ServiceClient.GetAsync(new Speaker() { conferenceSlug = conferenceSlug, speakerSlug = speakerSlug }, callback, (r, ex) =>
                                                        {
                                                            callback(null);
                                                        });
        }

        public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
        {
            ServiceClient.GetAsync(new Sessions() { conferenceSlug = conferenceSlug }, callback, (r, ex) => { 
				var x = r;
				throw ex; 
			});
        }

        public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
        {
            ServiceClient.GetAsync(new Session() { conferenceSlug = conferenceSlug, sessionSlug = slug }, callback, (r, ex) =>
                                            {
                                                callback(null);
                                            });
        }

        public void CreateConference(CreateConference conference, Action<FullConferenceDto> callback)
        {
            conference.slug = conference.name.GenerateSlug();
            ServiceClient.PostAsync(conference, callback, (r, ex) => { callback(null); });
        }

        public void EditConference(CreateConference conference, Action<FullConferenceDto> callback)
        {
            ServiceClient.PutAsync(conference, callback, (r, ex) => { callback(null); });
        }

        public void CreateUser(string userName, Action<UserDto> callback)
        {
            //ServiceClient.PostAsync(new User() {userName = userName}, callback, (r, ex) => { callback(null); });
            var user = new UserDto() { userName = userName };
            callback(user);
        }

        public void GetUser(string userName, Action<UserDto> callback)
        {
            ServiceClient.GetAsync(new User() { userName = userName }, callback, (r, ex) => { callback(null); });
        }

        public void AddSessionToConference(AddSession session, Action<SessionDto> callback)
        {
            session.slug = session.title.GenerateSlug();
            ServiceClient.PostAsync(session, callback, (r, ex) => { callback(null); });
        }

        public void EditSessionInConference(AddSession session, Action<SessionDto> callback)
        {
            ServiceClient.PutAsync(session, callback, (r, ex) => { callback(null); });
        }

        public void AddSpeakerToSession(CreateSpeaker speaker, Action<FullSpeakerDto> callback)
        {
            speaker.slug = (speaker.firstName.ToLower() + " " + speaker.lastName.ToLower()).GenerateSlug();
            ServiceClient.PostAsync(speaker, callback, (r, ex) =>
                                                           {
                                                               callback(null);
                                                           });
        }

        public void EditSpeaker(CreateSpeaker speaker, Action<FullSpeakerDto> callback)
        {
            ServiceClient.PutAsync(speaker, callback, (r, ex) =>
                                    {
                                        callback(null);
                                    });
        }

    }
}
