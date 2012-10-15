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

namespace TekConf.UI.Api.Services.v1
{
    public class ConferenceService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

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
                var entity = Mapper.Map<SpeakerEntity>(speaker);

                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var conference = collection.AsQueryable().FirstOrDefault(c => c.slug == speaker.conferenceSlug);
                if (conference == null)
                {
                    return new HttpError() {StatusCode = HttpStatusCode.BadRequest};
                }

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

        public object Post(CreateConference conference)
        {
            FullConferenceDto conferenceDto = null;
            try
            {
                var entity = Mapper.Map<ConferenceEntity>(conference);
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

        private object GetFullSingleConference(Conference request)
        {
            var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
                                    {
                                        var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                
                                        var conference = collection
                                            .AsQueryable()
                                            .Where(c => c.isLive)
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