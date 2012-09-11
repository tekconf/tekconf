using System;

namespace ConferencesIO.RemoteData.Dtos.v1
{
    public class ConferencesDto
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string location { get; set; }
        public string url { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
    }
}