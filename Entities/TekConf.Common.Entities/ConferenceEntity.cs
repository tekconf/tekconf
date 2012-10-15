using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using TekConf.Common.Entities;
using TinyMessenger;

namespace TekConf.UI.Api
{
    public class ConferenceEntity
    {
        [BsonIgnore]
        public ITinyMessengerHub Hub { get; set; }

        private IList<SessionEntity> _sessions = new List<SessionEntity>();
        
        public void Publish()
        {
            this.datePublished = DateTime.Now;
            this.isLive = true;

            this.Hub.Publish(new ConferencePublished());
        }

        public void AddSession(SessionEntity session)
        {
            if (_sessions == null)
            {
                _sessions = new List<SessionEntity>();
            }
            this.Hub.Publish(new SessionAdded());
            _sessions.Add(session);
        }

        public void Save(MongoCollection<ConferenceEntity> collection)
        {
            if (!this.isSaved)
            {
                if (_id == default(Guid))
                {
                    _id = Guid.NewGuid();                    
                }
                slug = name.GenerateSlug();
                dateAdded = DateTime.Now;
                isSaved = true;
            }
            collection.Save(this);
        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; private set; }
        public bool isLive { get; private set; }
        public string slug { get; private set; }
        public DateTime datePublished { get; private set; }
        public bool isSaved { get; private set; }

        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime callForSpeakersOpens { get; set; }
        public DateTime callForSpeakersCloses { get; set; }
        public DateTime registrationOpens { get; set; }
        public DateTime registrationCloses { get; set; }
        public DateTime dateAdded { get; set; }

        public string location { get; set; }
        public AddressEntity address { get; set; }
        public string description { get; set; }

        public string imageUrl { get; set; }
        public string tagLine { get; set; }

        public IEnumerable<SessionEntity> sessions
        {
            get { return _sessions.AsEnumerable(); }
            private set 
            { 
                _sessions = value.ToList(); 
            }
        }

        public string facebookUrl { get; set; }
        public string homepageUrl { get; set; }
        public string lanyrdUrl { get; set; }
        public string twitterHashTag { get; set; }
        public string twitterName { get; set; }
        public string meetupUrl { get; set; }
        public string googlePlusUrl { get; set; }
        public string vimeoUrl { get; set; }
        public string youtubeUrl { get; set; }
        public string githubUrl { get; set; }
        public string linkedInUrl { get; set; }
    }
}