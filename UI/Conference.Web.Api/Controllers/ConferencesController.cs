using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Conference.Web.Api.Controllers
{
    public class ConferencesController : ApiController
    {
        // GET api/values
        public IEnumerable<Conference> Get()
        {
            var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263");
            var _database = _server.GetDatabase("app4727263");
            var conferences = _database.GetCollection<Conference>("conferences").AsQueryable().ToList();
            return conferences.AsEnumerable();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post(string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    [Serializable]
    public class Conference
    {
        public object _id { get; set; }
        public string description { get; set; }
        public string facebookUrl { get; set; }
        public string slug { get; set; }
        public string homepageUrl { get; set; }
        public string lanyrdUrl { get; set; }
        public string location { get; set; }
        public string meetupUrl { get; set; }
        public string name { get; set; }
        public object start { get; set; }
        public object end { get; set; }
        public string twitterHashTag { get; set; }
        public string twitterName { get; set; }
        public List<Session> sessions { get; set; }

    }

    [Serializable]
    public class Session
    {
        public string slug { get; set; }
        public string conferenceSlug { get; set; }
        public string title { get; set; }
        public object start { get; set; }
        public object end { get; set; }
        public string room { get; set; }
        public string difficulty { get; set; }
        public string description { get; set; }
        public string twitterHashTag { get; set; }
        public string sessionType { get; set; }
        public List<string> links { get; set; }
        public List<string> tags { get; set; }
        public List<string> subjects { get; set; }
        public List<Speaker> speakers { get; set; }
    }

    [Serializable]
    public class Speaker
    {
        public string slug { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}