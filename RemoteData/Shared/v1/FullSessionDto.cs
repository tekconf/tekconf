using System;
using System.Collections.Generic;

namespace ConferencesIO.RemoteData.Dtos.v1
{
    public class FullSessionDto
    {
        public string slug { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string room { get; set; }
        public string difficulty { get; set; }
        public string description { get; set; }
        public string twitterHashTag { get; set; }
        public string sessionType { get; set; }
        public List<string> links { get; set; }
        public List<string> tags { get; set; }
        public List<string> subjects { get; set; }
        public List<string> resources { get; set; }
        public List<string> prerequisites { get; set; }
        public List<FullSpeakerDto> speakers { get; set; }
    }
}