using System;
using System.Linq;
using System.Net;
using AutoMapper;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.ServiceHost;
using TinyMessenger;

namespace TekConf.UI.Api.Services.v1
{
    public class ConferenceService : MongoServiceBase
    {
        private readonly ITinyMessengerHub _hub;
        public ICacheClient CacheClient { get; set; }

        public ConferenceService(ITinyMessengerHub hub)
        {
            _hub = hub;
        }

        public object Get(Conference request)
        {
            var fullConferenceDto = GetFullSingleConference(request);
            return fullConferenceDto;
        }

        public object Post(CreateSpeaker speaker)
        {
            FullSpeakerDto speakerDto = null;
            try
            {
                if (string.IsNullOrWhiteSpace(speaker.profileImageUrl))
                {
                    if (!string.IsNullOrWhiteSpace(speaker.emailAddress))
                    {
                        var profileImage = new GravatarImage();

                        var profileImageUrl = profileImage.GetUrl(speaker.emailAddress, 100, "pg");

                        if (profileImage.GravatarExists(profileImageUrl))
                        {
                            speaker.profileImageUrl = profileImageUrl;
                        }
                        else
                        {
                            speaker.profileImageUrl = "/img/speakers/default.png";
                        }
                    }
                    else
                    {
                        speaker.profileImageUrl = "/img/speakers/default.png";                        
                    }
                }


                var entity = Mapper.Map<SpeakerEntity>(speaker);

                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var conference = collection.AsQueryable().FirstOrDefault(c => c.slug == speaker.conferenceSlug);
                
                if (conference == null)
                {
                    return new HttpError() {StatusCode = HttpStatusCode.BadRequest};
                }
                conference.Hub = _hub;

                var session = conference.sessions.FirstOrDefault(s => s.slug == speaker.sessionSlug);
                if (session == null)
                {
                    return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
                }

                session.speakers.Add(entity);
                conference.Save(collection);

                speakerDto = Mapper.Map<SpeakerEntity, FullSpeakerDto>(entity);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }

            return speakerDto;
        }

        public object Put(CreateSpeaker request)
        {
            var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            var conference = collection.AsQueryable()
                                    .Where(c => c.slug == request.conferenceSlug)
                                    .FirstOrDefault(c => c.sessions != null);

            
            if (conference != null && conference.sessions != null)
            {
                conference.Hub = _hub;

                var speakers = conference.sessions
                    .Where(session => session.speakers != null)
                    .SelectMany(session => session.speakers)
                    .Where(speaker => speaker.slug == request.slug)
                    .ToList();

                SpeakerEntity lastSpeakerEntity = null;
                foreach (var speakerEntity in speakers)
                {
                    Mapper.Map<CreateSpeaker, SpeakerEntity>(request, speakerEntity);
                    lastSpeakerEntity = speakerEntity;
                }


                conference.Save(collection);
                var speakerDto = Mapper.Map<SpeakerEntity, FullSpeakerDto>(lastSpeakerEntity);

                return speakerDto;
            }
            else
            {
                return new HttpError() {StatusCode = HttpStatusCode.BadRequest};
            }
        }

        public object Post(CreateConference conference)
        {
            FullConferenceDto conferenceDto = null;
            try
            {
                var entity = Mapper.Map<ConferenceEntity>(conference);
                entity.Hub = _hub;
                entity.dateAdded = DateTime.Now; // TODO : This logic should be encapsulated
                if (entity.isLive)
                {
                    entity.Publish();
                }
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                entity.Save(collection);

                conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(entity);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }

            return conferenceDto;
        }

        public object Put(CreateConference conference)
        {
            FullConferenceDto conferenceDto = null;
            try
            {
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var existingConference = collection.AsQueryable().FirstOrDefault(c => c.slug == conference.slug);
                existingConference.Hub = _hub;
                bool existingConferenceIsLive = existingConference.isLive;
                Mapper.Map<CreateConference, ConferenceEntity>(conference, existingConference);

                if (!existingConferenceIsLive && existingConference.isLive)
                {
                    existingConference.Publish();
                }
                existingConference.Save(collection);

                conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(existingConference);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }

            return conferenceDto;
        }

        private object GetFullSingleConference(Conference request)
        {
            var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
                                    {
                                        var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                
                                        var conference = collection
                                            .AsQueryable()
                                            //.Where(c => c.isLive)
                                            .SingleOrDefault(c => c.slug == request.conferenceSlug);

                                        if (conference == null)
                                        {
                                            throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
                                        }

                                        var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);

                                        return conferenceDto;
                                    });
        }
    
    }
}